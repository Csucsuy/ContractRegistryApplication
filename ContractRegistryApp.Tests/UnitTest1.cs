using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContractRegistryApp.Models;
using ContractRegistryApp.Services;

namespace ContractRegistryApp.Tests
{
    [TestClass]
    public class AllTests
    {
        private DatabaseService _dbService;
        private string _testFilePath;

        [TestInitialize]
        public void Setup()
        {
            _dbService = new DatabaseService();

            _testFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "contracts_database.xml");
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }

        // --- 1. UNIT TESZTEK (Funkcionális működés) ---

        [TestMethod]
        public void Unit_AddContract_ShouldIncreaseCount()
        {
            var contract = new Contract
            {
                Name = "Teszt Szerződés",
                Party1 = "Teszt Kft.",
                Amount = 10000
            };

            _dbService.AddContract(contract);
            var list = _dbService.GetAllContracts();

            Assert.AreEqual(1, list.Count, "A lista méretének 1-nek kell lennie hozzáadás után.");
            Assert.AreEqual("Teszt Szerződés", list[0].Name, "A név nem egyezik.");
        }

        [TestMethod]
        public void Unit_DeleteContract_ShouldReduceCount()
        {
            var c1 = new Contract { Name = "Törlendő", Party1 = "X" };
            _dbService.AddContract(c1);
            var listBefore = _dbService.GetAllContracts();
            int idToDelete = listBefore[0].Id;

            _dbService.DeleteContract(idToDelete);
            var listAfter = _dbService.GetAllContracts();

            Assert.AreEqual(0, listAfter.Count, "Törlés után a listának üresnek kell lennie.");
        }

        // --- 2. TELJESÍTMÉNY TESZT (Performance) ---

        [TestMethod]
        public void Performance_AddManyContracts()
        {
            // Cél: 1000 szerződés hozzáadása mennyi időbe telik?
            int count = 1000;
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            for (int i = 0; i < count; i++)
            {
                _dbService.AddContract(new Contract
                {
                    Name = $"Perf {i}",
                    Party1 = "Test",
                    Amount = i
                });
            }

            stopwatch.Stop();

            Console.WriteLine($"1000 elem hozzáadása: {stopwatch.ElapsedMilliseconds} ms");

            // Elvárjuk, hogy 1000 elem hozzáadása XML-be ne tartson tovább 5 másodpercnél (5000ms)
            // Megjegyzés: XML írás lassú lehet sok adatnál, mert mindig újraírja a fájlt.
            Assert.IsTrue(stopwatch.ElapsedMilliseconds < 8000, "A művelet túl lassú volt!");
        }

        // --- 3. MEMÓRIA FELHASZNÁLÁSI TESZT (Memory) ---

        [TestMethod]
        public void Memory_LoadLargeData()
        {
            // Generálunk sok adatot
            for (int i = 0; i < 2000; i++)
            {
                _dbService.AddContract(new Contract { Name = "MemTest", Party1 = "ABC" });
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();

            long memoryBefore = GC.GetTotalMemory(true);

            var list = _dbService.GetAllContracts();

            long memoryAfter = GC.GetTotalMemory(true);
            long difference = memoryAfter - memoryBefore;

            Console.WriteLine($"Memória használat a lista betöltésekor: {difference / 1024} KB");

            // A lista nem lehet üres, és nem fogyaszthat irreálisan sok memóriát (pl. 50 MB)
            Assert.IsTrue(list.Count == 2000);
            Assert.IsTrue(difference < 50 * 1024 * 1024, "Túl magas memória használat!");
        }
    }
}