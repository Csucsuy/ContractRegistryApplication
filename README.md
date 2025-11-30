# Szerződésnyilvántartó Alkalmazás

Ez a projekt egy asztali alkalmazás, amely cégek számára teszi lehetővé szerződéseik nyilvántartását, kezelését és a kapcsolódó dokumentumok gyors elérését. A szoftver a **.NET Framework** környezetben, **WPF** technológiával és **MVVM** tervezési mintával készült.

## Funkciók

Az alkalmazás az alábbi fő funkciókat látja el:

* **Szerződések listázása:** Áttekinthető lista a rögzített elemekről.
* **Új szerződés rögzítése:** Részletes adatlap (Név, Felek, Dátumok, Összeg) kitöltése.
* **Dokumentum csatolása:** Fájl elérési út (pl. PDF) társítása a szerződéshez.
* **Módosítás és Törlés:** Meglévő adatok szerkesztése vagy eltávolítása az adatbázisból.
* **Fájl megnyitása:** A csatolt dokumentum közvetlen megnyitása az alapértelmezett megjelenítővel.
* **Perzisztencia:** Az adatok tartós tárolása XML formátumban.

## Technológiai Stack

* **Nyelv:** C#
* **Keretrendszer:** .NET Framework 4.8
* **UI Technológia:** Windows Presentation Foundation (WPF) + XAML
* **Architektúra:** Model-View-ViewModel (MVVM) 
* **Adattárolás:** XML Serializer
* **Tesztelés:** MSTest (Unit, Performance, Memory tests)

## Telepítés és Futtatás

1.  **Klónozás:** Töltsd le a forráskódot a számítógépedre.
2.  **Megnyitás:** Nyisd meg a `ContractRegistryApp.sln` fájlt Visual Studio 2019 vagy újabb verzióban.
3.  **Build:** Fordítsd le a projektet (`Build` -> `Build Solution`).
4.  **Futtatás:** Indítsd el az alkalmazást az `F5` billentyűvel.

*Megjegyzés: Az alkalmazás első indításakor automatikusan létrehozza a szükséges adatfájlt (`contracts_database.xml`) a kimeneti mappában.*

## Tesztelés

A projekt tartalmaz egy `ContractRegistryApp.Tests` nevű tesztprojektet, amely lefedi:
* A funkcionális működést (Hozzáadás, Törlés).
* A teljesítményt (Nagy mennyiségű adat rögzítése).
* A memóriahasználatot.

A tesztek futtatásához a Visual Studio-ban válaszd a `Test` -> `Run All Tests` menüpontot.

## Továbbfejlesztési lehetőségek

A projekt jelenlegi verziója egy stabil alapot biztosít, de a jövőben az alábbi fejlesztésekkel bővíthető:
* Áttérés relációs adatbázisra (SQL Server) a többfelhasználós működés érdekében.
* Keresési és szűrési funkciók beépítése.
* Felhasználói jogosultságok kezelése (Admin/User).
* Automatikus e-mail értesítés a szerződés lejárata előtt.