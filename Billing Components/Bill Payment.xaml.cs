using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Billing_Components
{
    public enum PaymentCloseOptions
    {
        Save,
        Cancel
    }
    /// <summary>
    /// Interaction logic for Bill_Payment.xaml
    /// </summary>
    public partial class Bill_Payment : Window
    {
        private DBConnection _DBConnection { get; set; }
        private DBConnection.Bill Bill { get; set; }

        public PaymentCloseOptions HasChanges { get; set; }

        public ObservableCollection<MemberPayment> MemberPayment { get; set; }

        double TotalAmount { get; set; }
        double MemberShare { get { return Math.Round(TotalAmount / MemberPayment.Count,2); } }

        public Bill_Payment(DB.DBConnection _DBConnection, DB.DBConnection.Bill Bill)
        {
            InitializeComponent();
            this._DBConnection = _DBConnection;
            this.Bill = Bill;

            MemberPayment = new ObservableCollection<MemberPayment>();
            DataContext = this;
            
            foreach(DBConnection.Member member in _DBConnection.Members)
            {
                MemberPayment.Add(new MemberPayment(_DBConnection, Bill, member));
            }

            TotalAmount = 0;


            foreach(DBConnection.Bill bill in _DBConnection.DB_GetPaidBills(Bill.DueDateMonth, Bill.DueDateYear))
            {
                TotalAmount += bill.AmountHRK;
            }
            TotalAmount += Bill.AmountHRK;

            switch(Bill.Currency)
            {
                case "EUR":
                    TotalAmount /= Properties.Settings.Default.EURRate;
                    break;
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataContext = null;
            double memberShare = TotalAmount / MemberPayment.Count;

            foreach (MemberPayment item in MemberPayment)
            {
                switch(Bill.Currency)
                {
                    case "EUR":
                        item.PaymentAmount = Math.Round(memberShare - (item.PaidAmount / Properties.Settings.Default.EURRate), 2);
                        break;
                    default:
                        item.PaymentAmount = Math.Round(memberShare - item.PaidAmount, 2);
                        break;

                }
            }

            DataContext = this;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            HasChanges = PaymentCloseOptions.Cancel;
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            double PayAmount=0;
            foreach(MemberPayment item in MemberPayment)
            {
                _DBConnection.DB_InsertPayment(item.PaymentAmount, Bill.Id, Bill.Currency, item.MemberId);
                PayAmount+=item.PaymentAmount;
            }

            switch (Bill.Currency)
            {
                case "EUR":
                    PayAmount *= Properties.Settings.Default.EURRate;
                    break;
            }


            if (Math.Round(Bill.AmountHRK,0) == Math.Round(PayAmount,0))
            {
                Bill.Paid = true;
                Bill.DatePaid = DateTime.Now;
                _DBConnection.DB_MarkBillPaid(Bill);
            }

            HasChanges = PaymentCloseOptions.Save;
            this.Close();
        }
    }
}
