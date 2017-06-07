using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.OleDb;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Billing_Components.DB;

namespace Billing_Components
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public ObservableCollection<DBConnection.Bill> Bills { get; set; }

        public ObservableCollection<DBConnection.Bill> MonthBills { get; set; }

        public ObservableCollection<BillReview> MonthReview { get; set; }

        public ObservableCollection<BillReview> TotalReview { get; set; }

        List<DBConnection.Member> Members { get; set; }

        public DBConnection _DBConnection;

        SortOptions _SortOptions;

        bool BillMenuOpened = false;

        public MainWindow()
        {
            InitializeComponent();
            _DBConnection = new DBConnection();
            _SortOptions = new SortOptions();

            DataContext = this;

            Bills = new ObservableCollection<DBConnection.Bill>(_DBConnection.Bills);
            MonthBills = new ObservableCollection<DBConnection.Bill>(_DBConnection.MonthBills);

            Members = _DBConnection.Members;

            CalculateReviews();

            TXT_Overview_Month.Text = DateTime.Today.Month.ToString();
            TXT_Overview_Year.Text = DateTime.Today.Year.ToString();
             
        }

        private void CalculateReviews()
        {
            #region Total
            if (TotalReview == null)
                TotalReview = new ObservableCollection<BillReview>();
            else
                TotalReview.Clear();

            foreach (DBConnection.Member member in Members)
            {
                TotalReview.Add(new BillReview(_DBConnection, member));
            }

            double Total = 0;
            double Remaining = 0;

            foreach (DBConnection.Bill bill in _DBConnection.Bills)
            {
                Total += bill.AmountHRK;
                if (!bill.Paid)
                    Remaining += bill.AmountHRK;
            }

            TXT_Total.Text = String.Format("{0:f2} kn", Total);
            TXT_Total_Remaining.Text = String.Format("{0:f2} kn", Remaining);
            TXT_Total_Share.Text = String.Format("{0:f2} kn", Total / _DBConnection.Members.Count);
            #endregion

            #region Monthly
            if (MonthReview == null)
                MonthReview = new ObservableCollection<BillReview>();
            else
                MonthReview.Clear();
            
            foreach (DBConnection.Member member in Members)
            {
                MonthReview.Add(new BillReview(_DBConnection, member, true));
            }

            Total=0;
            Remaining=0;

            foreach (DBConnection.Bill bill in _DBConnection.MonthBills)
            {
                Total += bill.AmountHRK;
                if (!bill.Paid)
                    Remaining += bill.AmountHRK;
            }

            TXT_Monthly_Total.Text = String.Format("{0:f2} kn", Total);
            TXT_Monthly_Remaining.Text = String.Format("{0:f2} kn", Remaining);
            TXT_Monthly_Share.Text = String.Format("{0:f2} kn", Total / _DBConnection.Members.Count);
            #endregion
        }

        private void Sort_List(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            List<DBConnection.Bill>  Sorter = new List<DBConnection.Bill>();

            if (btn.Tag.ToString() == _SortOptions.GetLastOption())
                _SortOptions.SetProperty(!_SortOptions.GetDescending());
            else
                _SortOptions.SetProperty(false);

            _SortOptions.SetProperty(btn.Tag.ToString());

            RefreshBillListItems();

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!BillMenuOpened)
            {
                ListBox list = (ListBox)sender;

                if (list.SelectedIndex > -1)
                {
                    BillMenuOpened = true;
                    DBConnection.Bill BillItem = (DBConnection.Bill)list.SelectedItem;
                    Bill_Overview BO = new Bill_Overview(_DBConnection, BillItem);
                    BO.Owner = this;
                    BO.ShowDialog();

                    switch(BO.HasChanges)
                    {
                        case BillCloseOption.Cancel:
                            break;
                        case BillCloseOption.Delete:
                            Bills.Remove(BillItem);
                            _DBConnection.DB_RemoveBill(BillItem.Id);
                            RefreshBillListItems();
                            CalculateReviews();
                            _DBConnection.RefreshTemplates();
                            break;
                        case BillCloseOption.Save:

                            _DBConnection.DB_UpdateBill(BO.Bill);

                            for (int i = 0; i < Bills.Count; i++)
                            {
                                if (Bills[i] == BillItem)
                                {
                                    Bills[i] = BO.Bill;
                                    break;
                                }
                            }
                            RefreshBillListItems();
                            CalculateReviews();
                            _DBConnection.RefreshTemplates();
                            break;
                    }

                    
                    list.SelectedIndex = -1;
                }
            }
            else
            {
                BillMenuOpened = !BillMenuOpened;
            }


        }

        private void RefreshBillListItems()
        {
            switch (_SortOptions.GetLastOption())
            {
                case "Id":
                    if (_SortOptions.GetDescending())
                        _DBConnection.Bills = Bills.OrderByDescending(x => x.Id).ToList<DBConnection.Bill>();
                    else
                        _DBConnection.Bills = Bills.OrderBy(x => x.Id).ToList<DBConnection.Bill>();
                    break;
                case "Category":
                    if (_SortOptions.GetDescending())
                        _DBConnection.Bills = Bills.OrderByDescending(x => x.Category).ToList<DBConnection.Bill>();
                    else
                        _DBConnection.Bills = Bills.OrderBy(x => x.Category).ToList<DBConnection.Bill>();
                    break;
                case "DueDate":
                    if (_SortOptions.GetDescending())
                        _DBConnection.Bills = Bills.OrderByDescending(x => x.DueDate).ToList<DBConnection.Bill>();
                    else
                        _DBConnection.Bills = Bills.OrderBy(x => x.DueDate).ToList<DBConnection.Bill>();
                    break;
                case "ForMonth":
                    if (_SortOptions.GetDescending())
                        _DBConnection.Bills = Bills.OrderByDescending(x => x.ForMonth).ToList<DBConnection.Bill>();
                    else
                        _DBConnection.Bills = Bills.OrderBy(x => x.ForMonth).ToList<DBConnection.Bill>();
                    break;
                case "Amount":
                    if (_SortOptions.GetDescending())
                        _DBConnection.Bills = Bills.OrderByDescending(x => x.Amount).ToList<DBConnection.Bill>();
                    else
                        _DBConnection.Bills = Bills.OrderBy(x => x.Amount).ToList<DBConnection.Bill>();
                    break;
                case "Paid":
                    if (_SortOptions.GetDescending())
                        _DBConnection.Bills = Bills.OrderByDescending(x => x.Paid).ToList<DBConnection.Bill>();
                    else
                        _DBConnection.Bills = Bills.OrderBy(x => x.Paid).ToList<DBConnection.Bill>();
                    break;
            }

            _DBConnection.ReOrderMonthBills();

            Bills.Clear();
            MonthBills.Clear();

            foreach(DBConnection.Bill bill in _DBConnection.Bills)
            {
                Bills.Add(bill);
            }

            foreach (DBConnection.Bill bill in _DBConnection.MonthBills)
            {
                MonthBills.Add(bill);
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox paid = (CheckBox)sender;

            int itemId = 0;

            for(int i=0;i<Bills.Count;i++)
            {
                if (Convert.ToInt32(paid.Tag.ToString()) == Bills[i].Id)
                {
                    itemId = i;
                    break;
                }
            }

            Bills[itemId].Paid = (bool)paid.IsChecked;

            if ((bool)paid.IsChecked)
            {
                Bills[itemId].DatePaid = DateTime.Now;
            }
            else
            {
                Bills[itemId].DatePaid = default(DateTime);
            }
        }

        private void BTN_OpenUsers(object sender, RoutedEventArgs e)
        {
            UserOverview BO = new UserOverview(_DBConnection);
            BO.Owner = this;
            BO.ShowDialog();
        }

        private void BTN_NewBill(object sender, RoutedEventArgs e)
        {
            Bill_Creation BC = new Bill_Creation(_DBConnection);
            BC.Owner = this;
            BC.ShowDialog();

            if(BC.Result == BillCreationResult.Create)
            {
                _DBConnection.DB_InsertBill(BC.Bill);
                Bills.Add(BC.Bill);
                RefreshBillListItems();
                CalculateReviews();
            }
        }

        private void BTN_GetOverview(object sender, RoutedEventArgs e)
        {
            _DBConnection.GetMonthOverview(Convert.ToInt32(TXT_Overview_Month.Text), Convert.ToInt32(TXT_Overview_Year.Text));

            MonthBills.Clear();

            foreach(DBConnection.Bill bill in _DBConnection.MonthBills)
            {
                MonthBills.Add(bill);
            }

            RefreshBillListItems();
            CalculateReviews();
        }
        
    }
}