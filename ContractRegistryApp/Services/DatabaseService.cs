using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using ContractRegistryApp.Models;

namespace ContractRegistryApp.Services
{
    public class DatabaseService
    {
        private const string DbName = "contracts_database.xml";
        private readonly string _filePath;

        public DatabaseService()
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DbName);
        }

        private List<Contract> LoadList()
        {
            if (!File.Exists(_filePath)) return new List<Contract>();
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Contract>));
                using (StreamReader reader = new StreamReader(_filePath))
                {
                    return (List<Contract>)serializer.Deserialize(reader);
                }
            }
            catch { return new List<Contract>(); }
        }

        private void SaveList(List<Contract> contracts)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Contract>));
            using (StreamWriter writer = new StreamWriter(_filePath))
            {
                serializer.Serialize(writer, contracts);
            }
        }

        public void AddContract(Contract c)
        {
            var list = LoadList();
            int newId = 1;
            if (list.Count > 0) newId = list.Max(x => x.Id) + 1;
            c.Id = newId;
            list.Add(c);
            SaveList(list);
        }

        public List<Contract> GetAllContracts()
        {
            var list = LoadList();
            list.Reverse(); // Legújabb elöl
            return list;
        }


        public void DeleteContract(int id)
        {
            var list = LoadList();
            var item = list.FirstOrDefault(c => c.Id == id);
            if (item != null)
            {
                list.Remove(item);
                SaveList(list);
            }
        }

        public void UpdateContract(Contract updated)
        {
            var list = LoadList();
            var index = list.FindIndex(c => c.Id == updated.Id);
            if (index != -1)
            {
                list[index] = updated;
                SaveList(list);
            }
        }
    }
}