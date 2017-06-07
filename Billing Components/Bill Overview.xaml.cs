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

namespace Billing_Components
{
    public enum BillCloseOption
    {
        Cancel,
        Save,
        Delete
    }
    /// <summary>
    /// Interaction logic for Bill_Overview.xaml
    /// </summary>
    public partial class Bill_Overview : Window
    {
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        public BillCloseOption HasChanges { get; set; }

        public DBConnection.Bill Bill { get; set; }
        public DBConnection.Bill vBill { get; set; }

        public DBConnection _DBConnection { get; set; }

        public Bill_Overview(DBConnection _DBConnection, DBConnection.Bill bill)
        {
            this._DBConnection = _DBConnection;
            InitializeComponent();

            if(bill.Id == 0)
            {
                bill.Id = _DBConnection.UpdateId(bill.GUID);
            }
            if(String.IsNullOrWhiteSpace(bill.GUID))
            {
                _DBConnection.CreateGUID(bill);
            }

            Bill = bill;
            vBill = CreateBillEnvironment();
            DataContext = vBill;

            CheckViews();
        }

        private void CheckViews()
        {
            if (vBill.Paid)
            {
                BTN_Pay_Bill.IsEnabled = false;
                BTN_View_Payment_Information.Visibility = Visibility.Visible;
            }
            else
            {
                BTN_View_Payment_Information.Visibility = Visibility.Collapsed;
            }
        }

        private DBConnection.Bill CreateBillEnvironment()
        {
            DBConnection.Bill vBill = new DBConnection.Bill() {
                
            Id = Bill.Id,
            GUID = Bill.GUID,
            Payer = Bill.Payer,
            Category = Bill.Category,
            Recipient = Bill.Recipient,
            Currency = Bill.Currency,
            Amount = Bill.Amount,
            IBANOfRecipient = Bill.IBANOfRecipient,
            Model = Bill.Model,
            ReferenceNumber = Bill.ReferenceNumber,
            Description = Bill.Description,
            DueDate = Bill.DueDate,
            ForMonth = Bill.ForMonth,
            Paid = Bill.Paid,
            DatePaid = Bill.DatePaid
            
            };


            return vBill;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HasChanges = BillCloseOption.Save;

            UpdateBillPointer(vBill);
            
            this.Close();
        }

        private void UpdateBillPointer(DBConnection.Bill vBill)
        {
            Bill.Id = vBill.Id;
            Bill.Payer = vBill.Payer;
            Bill.Category = vBill.Category;
            Bill.Recipient = vBill.Recipient;
            Bill.Currency = vBill.Currency;
            Bill.Amount = vBill.Amount;
            Bill.IBANOfRecipient = vBill.IBANOfRecipient;
            Bill.Model = vBill.Model;
            Bill.ReferenceNumber = vBill.ReferenceNumber;
            Bill.Description = vBill.Description;
            Bill.DueDate = vBill.DueDate;
            Bill.ForMonth = vBill.ForMonth;
            Bill.Paid = vBill.Paid;
            Bill.DatePaid = vBill.DatePaid;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            HasChanges = BillCloseOption.Cancel;
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        private void View_Payment_Information(object sender, RoutedEventArgs e)
        {
            Payment payment = new Payment(_DBConnection, Bill);
            payment.Owner = this;
            payment.ShowDialog();
        }

        private void BTN_DeleteBill(object sender, RoutedEventArgs e)
        {
            MessageBoxResult Confirmation = MessageBox.Show("Are you sure you want to delete this invoice?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);
            if (Confirmation == MessageBoxResult.Yes)
            {
                HasChanges = BillCloseOption.Delete;
                this.Close();
            }
        }

        private void BTN_Pay_Bill_Click(object sender, RoutedEventArgs e)
        {
            Bill_Payment BP = new Bill_Payment(_DBConnection, Bill);
            BP.ShowDialog();

            switch(BP.HasChanges)
            {
                case PaymentCloseOptions.Save:
                    HasChanges = BillCloseOption.Save;
                    this.Close();
                    break;
            }

        }
    }
}
