using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Pharmacy_client
{
    public class Row
    {
        public string ProductBnd { get; set; }
        public string PriceBnd { get; set; }
        public string CountBnd { get; set; }
        public string SumBnd { get; set; }
    }
    /// <summary>
    /// Interaction logic for UserWnd.xaml
    /// </summary>
    public partial class UserWnd : Window
    {
        public UserWnd()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                LoginWnd._buffer = LoginWnd.GetBytes("~~");
                LoginWnd._socket.Send(LoginWnd._buffer);
                LoginWnd._socket.Close();
                Close();
            }
            catch (Exception ex)
            {
                Close();
            }
        }

        List<String[]> mProductLst = new List<string[]>();
        List<String[]> mCartLst = new List<string[]>();

        private void CountTb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D1 && e.Key <= Key.D9) return;
            else e.Handled = true;
        }
        private void Window_Initialized(object sender, EventArgs e)
        {
            try
            {
                LoginWnd._buffer = LoginWnd.GetBytes("READY");
                LoginWnd._socket.Send(LoginWnd._buffer);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Server not ready!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Creating the filestream and file
            //FileStream tmpfile = new FileStream("Pharm_db.txt", FileMode.OpenOrCreate);
            StreamWriter tmpfile = new StreamWriter("Pharm_db.tmp", true);

            // Receiving the file from client
            //Thread.Sleep(100);
            while (true)
            {
                LoginWnd._buffer = LoginWnd.PInvokeFill(LoginWnd.GetBytes("\0"));
                LoginWnd._strbuffer.Remove(0);
                //LoginWnd._socket.Receive(LoginWnd._buffer, 0, LoginWnd._buffer.Length, SocketFlags.None);
                LoginWnd._socket.Receive(LoginWnd._buffer);
                LoginWnd._strbuffer = LoginWnd.GetString(LoginWnd._buffer);
                if (LoginWnd._strbuffer[0] == '~' && LoginWnd._strbuffer[1] == '~') break;

                //tmpfile.Write(LoginWnd._buffer, 0, LoginWnd._buffer.Length);
                tmpfile.WriteLine(LoginWnd._strbuffer.Substring(0, LoginWnd._strbuffer.IndexOf('\0')));
            }
            tmpfile.Flush();
            tmpfile.Close();

            StreamReader file = new StreamReader("Pharm_db.tmp");
            String[] strSpl;

            while (!file.EndOfStream)
            {
                strSpl = file.ReadLine().Split(':');
                mProductLst.Add(strSpl);
            }

            file.Close();
            File.Delete("Pharm_db.tmp");

            for(int i = 0; i < mProductLst.Count; ++i)
                ProductsLbox.Items.Add(mProductLst[i][0]);

            ProductsLbox.SelectedItem = 0;
            ProductLb.Content = mProductLst[0][0];
            VendorLb.Content = mProductLst[0][1];
            PurchLb.Content = mProductLst[0][2];
            PriceLb.Content = mProductLst[0][3];
            YearLb.Content = mProductLst[0][4];
            DescrLb.Content = mProductLst[0][5];



           /* GridView myGridView = new GridView();
            myGridView.AllowsColumnReorder = true;
            myGridView.ColumnHeaderToolTip = "Product info";
            GridViewColumn gvc1 = new GridViewColumn();
            gvc1.DisplayMemberBinding = new Binding("Product");
            gvc1.Header = "Product";
            gvc1.Width = 204;
            myGridView.Columns.Add(gvc1);
            GridViewColumn gvc2 = new GridViewColumn();
            gvc2.DisplayMemberBinding = new Binding("Price");
            gvc2.Header = "Price";
            gvc2.Width = 70;
            myGridView.Columns.Add(gvc2);
            GridViewColumn gvc3 = new GridViewColumn();
            gvc3.DisplayMemberBinding = new Binding("Count");
            gvc3.Header = "Count";
            gvc3.Width = 70;
            myGridView.Columns.Add(gvc3);
            GridViewColumn gvc4 = new GridViewColumn();
            gvc4.DisplayMemberBinding = new Binding("Sum");
            gvc4.Header = "Sum";
            gvc4.Width = 72;
            myGridView.Columns.Add(gvc4);

            CartData.View = myGridView;*/
        }

        private void ProductsLbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProductLb.Content = mProductLst[ProductsLbox.SelectedIndex][0];
            VendorLb.Content = mProductLst[ProductsLbox.SelectedIndex][1];
            PurchLb.Content = mProductLst[ProductsLbox.SelectedIndex][2];
            PriceLb.Content = mProductLst[ProductsLbox.SelectedIndex][3];
            YearLb.Content = mProductLst[ProductsLbox.SelectedIndex][4];
            DescrLb.Content = mProductLst[ProductsLbox.SelectedIndex][5];
        }

        private void CartBtn_Click(object sender, RoutedEventArgs e)
        {
            String[] tmp = new String[4];
            tmp.SetValue(ProductLb.Content.ToString(), 0);
            tmp.SetValue(PriceLb.Content.ToString(), 1);
            tmp.SetValue(CountTb.Text, 2);
            tmp.SetValue((int.Parse(PriceLb.Content.ToString()) * int.Parse(CountTb.Text)).ToString(), 3);

            mCartLst.Add(tmp);

            Row item = new Row { ProductBnd = tmp[0], PriceBnd = tmp[1], CountBnd = tmp[2], SumBnd = tmp[3]};
            ((ArrayList)CartData.Resources["items"]).Add(item);

            CartData.UpdateLayout();
        }
    }
}
