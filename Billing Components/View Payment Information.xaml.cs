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
using System.Collections.ObjectModel;

namespace Billing_Components
{
    /// <summary>
    /// Interaction logic for Payment.xaml
    /// </summary>
    public partial class Payment : Window
    {
        public ObservableCollection<DBConnection.BillPayment> BillPaymentInformation { get; set; }
        DBConnection _DBConnection;
        public Payment(DBConnection _DBConnection,DBConnection.Bill Bill)
        {
            this._DBConnection = _DBConnection;
            InitializeComponent();
            BillPaymentInformation = new ObservableCollection<DBConnection.BillPayment>(_DBConnection.DB_GetPaymentsForBill(Bill.Id));
            DataContext = this;
            TXT_TotalAmount.Text = Bill.amount;
        }
    }
}
