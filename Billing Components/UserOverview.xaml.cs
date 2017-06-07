using Billing_Components.DB;
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

namespace Billing_Components
{
    /// <summary>
    /// Interaction logic for UserOverview.xaml
    /// </summary>
    
    public partial class UserOverview : Window
    {

        public ObservableCollection<DBConnection.Member> Members { get; set; }
        public DBConnection _DBConnection;
        SortOptions _SortOptions;

        public UserOverview(DBConnection _DBConnection)
        {
            InitializeComponent();
            _DBConnection = new DBConnection();
            _SortOptions = new SortOptions();
            DataContext = this;

            Members = new ObservableCollection<DBConnection.Member>(_DBConnection.Members);

        }

        private void Sort_List(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            List<DBConnection.Member> Sorter = new List<DBConnection.Member>();

            if (btn.Tag.ToString() == _SortOptions.GetLastOption())
                _SortOptions.SetProperty(!_SortOptions.GetDescending());
            else
                _SortOptions.SetProperty(false);

            switch (btn.Tag.ToString())
            {

                case "Id":
                    if (_SortOptions.GetDescending())
                        Sorter = Members.OrderByDescending(x => x.Id).ToList<DBConnection.Member>();
                    else
                        Sorter = Members.OrderBy(x => x.Id).ToList<DBConnection.Member>();
                    break;
                case "FirstName":
                    if (_SortOptions.GetDescending())
                        Sorter = Members.OrderByDescending(x => x.FirstName).ToList<DBConnection.Member>();
                    else
                        Sorter = Members.OrderBy(x => x.FirstName).ToList<DBConnection.Member>();
                    break;
                case "LastName":
                    if (_SortOptions.GetDescending())
                        Sorter = Members.OrderByDescending(x => x.LastName).ToList<DBConnection.Member>();
                    else
                        Sorter = Members.OrderBy(x => x.LastName).ToList<DBConnection.Member>();
                    break;
                case "Contributes":
                    if (_SortOptions.GetDescending())
                        Sorter = Members.OrderByDescending(x => x.Contributes).ToList<DBConnection.Member>();
                    else
                        Sorter = Members.OrderBy(x => x.Contributes).ToList<DBConnection.Member>();
                    break;
                case "Active":
                    if (_SortOptions.GetDescending())
                        Sorter = Members.OrderByDescending(x => x.Active).ToList<DBConnection.Member>();
                    else
                        Sorter = Members.OrderBy(x => x.Active).ToList<DBConnection.Member>();
                    break;
            }

            _SortOptions.SetProperty(btn.Tag.ToString());

            Members.Clear();

            foreach (DBConnection.Member member in Sorter)
            {
                Members.Add(member);
            }

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
