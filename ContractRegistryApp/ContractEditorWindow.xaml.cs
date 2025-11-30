using System;
using System.Windows;
using Microsoft.Win32;
using ContractRegistryApp.Models;

namespace ContractRegistryApp
{
    public partial class ContractEditorWindow : Window
    {
        public Contract ResultContract { get; private set; }
        private int _editingId = 0; // Ha 0, akkor új, ha >0, akkor módosítás

        // Módosított konstruktor: Opcionálisan vár egy szerkesztendő szerződést
        public ContractEditorWindow(Contract contractToEdit = null)
        {
            InitializeComponent();

            if (contractToEdit != null)
            {
                Title = "Szerződés módosítása";
                _editingId = contractToEdit.Id; // Megjegyezzük az ID-t

                // Mezők feltöltése
                txtNev.Text = contractToEdit.Name;
                txtFel1.Text = contractToEdit.Party1;
                txtFel2.Text = contractToEdit.Party2;
                txtOsszeg.Text = contractToEdit.Amount.ToString();
                txtFile.Text = contractToEdit.FilePath;
                dpStart.SelectedDate = contractToEdit.StartDate;
                dpEnd.SelectedDate = contractToEdit.EndDate;
            }
        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == true)
            {
                txtFile.Text = dlg.FileName;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNev.Text) || string.IsNullOrWhiteSpace(txtFel1.Text))
            {
                MessageBox.Show("A Név és az 1. fél kötelező!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ResultContract = new Contract
            {
                Id = _editingId, // Visszaadjuk az eredeti ID-t (vagy 0-t ha új)
                Name = txtNev.Text,
                Party1 = txtFel1.Text,
                Party2 = txtFel2.Text,
                StartDate = dpStart.SelectedDate,
                EndDate = dpEnd.SelectedDate,
                FilePath = txtFile.Text
            };

            if (double.TryParse(txtOsszeg.Text, out double osszeg))
                ResultContract.Amount = osszeg;

            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}