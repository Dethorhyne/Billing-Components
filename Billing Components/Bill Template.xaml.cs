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
    /// Interaction logic for Bill_Template.xaml
    /// </summary>
    public partial class Bill_Template : Window
    {

        public string Key;
        public ObservableCollection<DBConnection.BillTemplate> TemplateKeys { get; set; }

        public Bill_Template(DBConnection _DBConnection)
        {
            InitializeComponent();

            DataContext = this;
            TemplateKeys = new ObservableCollection<DBConnection.BillTemplate>(_DBConnection.BillTemplates.Values);


        }

        private void BTN_Select_Click(object sender, RoutedEventArgs e)
        {
            Key = ((DBConnection.BillTemplate)TemplateListBox.SelectedItem).Category;
            this.Close();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = (ListBox)sender;
            if(lb.SelectedIndex>-1)
            {
                BTN_Select.IsEnabled = true;
            }
            else
            {
                BTN_Select.IsEnabled = false;
            }
        }
    }
}
