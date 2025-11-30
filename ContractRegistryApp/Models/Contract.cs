using System;

namespace ContractRegistryApp.Models
{
    public class Contract
    {
        public int Id { get; set; }
        public string Name { get; set; }        // Szerződés neve*
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double Amount { get; set; }
        public string Party1 { get; set; }      // Szerződő fél 1*
        public string Party2 { get; set; }
        public string FilePath { get; set; }    // Dokumentum

        // Megjelenítéshez segédproperty
        public string DisplayInfo
        {
            get { return $"{Name} - {Party1}"; }
        }

        public Contract() { }
    }
}