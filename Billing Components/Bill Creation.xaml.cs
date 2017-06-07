using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Billing_Components.DB;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Security.Cryptography;

namespace Billing_Components
{
    public enum BillCreationResult
    {
        Cancel,
        Create
    }
    /// <summary>
    /// Interaction logic for Bill_Overview.xaml
    /// </summary>
    public partial class Bill_Creation : Window
    {
        public BillCloseOption HasChanges { get; set; }

        public DBConnection.Bill Bill { get; set; }

        public BillCreationResult Result { get; set; }

        public DBConnection _DBConnection;

        public Bill_Creation(DBConnection _DBConnection)
        {
            this._DBConnection = _DBConnection;
            InitializeComponent();

            Bill = new DBConnection.Bill();

            Bill_Template BT = new Bill_Template(_DBConnection);
            BT.ShowDialog();
            if(BT.Key != null)
                if(BT.Key != "New")
                {
                    Bill.Payer = _DBConnection.BillTemplates[BT.Key].Payer;
                    Bill.Category = _DBConnection.BillTemplates[BT.Key].Category;
                    Bill.Recipient = _DBConnection.BillTemplates[BT.Key].Recipient;
                    Bill.Currency = _DBConnection.BillTemplates[BT.Key].Currency;
                    Bill.IBANOfRecipient = _DBConnection.BillTemplates[BT.Key].IBANOfRecipient;
                    Bill.Model = _DBConnection.BillTemplates[BT.Key].Model;
                }

            DataContext = Bill;

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Result = BillCreationResult.Create;
            Bill.GUID = _DBConnection.CreateMD5(Bill);
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Result = BillCreationResult.Cancel;
            this.Close();
        }

        private void Import_Bill(object sender, RoutedEventArgs e)
        {
            if(_DBConnection.BillTemplates.ContainsKey(Bill.Category))
            {
                DBConnection.Bill ImportedBill = (from item in _DBConnection.Bills
                                                  where item.Category == Bill.Category
                                                  select item).FirstOrDefault<DBConnection.Bill>();
                DataContext = null;

                Bill.Payer = ImportedBill.Payer.ToString();
                Bill.Category = ImportedBill.Category;
                Bill.Recipient = ImportedBill.Recipient;
                Bill.Currency = ImportedBill.Currency;
                Bill.Amount = Convert.ToDouble(ImportedBill.Amount);
                Bill.IBANOfRecipient = ImportedBill.IBANOfRecipient;
                Bill.Model = ImportedBill.Model;
                Bill.ReferenceNumber = ImportedBill.ReferenceNumber;
                Bill.Description = ImportedBill.Description;
                Bill.DueDate = ImportedBill.DueDate;
                Bill.ForMonth = ImportedBill.ForMonth;


                DataContext = Bill;
            }
            else
            {
                MessageBoxResult error = MessageBox.Show("No similar data based on given [Category] found to be imported.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
