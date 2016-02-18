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

        List<String[]> mProductLst = new List<String[]>();
        List<String[]> mCartLst = new List<String[]>();

        private void CountTb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9) return;
            else e.Handled = true;
        }
        private void Window_Initialized(object sender, EventArgs e)
        {
            #region ready
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
            #endregion
            #region Receiving DB
            // Creating the filestream and file
            StreamWriter tmpfile = new StreamWriter("Pharm_db.tmp", true);

            // Receiving the file from client
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
#endregion
            #region Reading DB
            StreamReader file = new StreamReader("Pharm_db.tmp");
            String[] strSpl;

            while (!file.EndOfStream)
            {
                strSpl = file.ReadLine().Split(':');
                mProductLst.Add(strSpl);
            }

            file.Close();
            File.Delete("Pharm_db.tmp");
            #endregion
            #region View DB
            for (int i = 0; i < mProductLst.Count; ++i)
                ProductsLbox.Items.Add(mProductLst[i][0]);

            ProductsLbox.SelectedItem = 0;
            ProductLb.Content = mProductLst[0][0];
            VendorLb.Content = mProductLst[0][1];
            PurchLb.Content = mProductLst[0][2];
            PriceLb.Content = mProductLst[0][3];
            YearLb.Content = mProductLst[0][4];
            DescrLb.Content = mProductLst[0][5];
            #endregion
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
            if ((CountTb.Text[0] != '0') && 
                ((int.Parse(PurchLb.Content.ToString())) >= (int.Parse(CountTb.Text))))
            {
                #region String item
                String[] tmp = new String[4];
                tmp.SetValue(ProductLb.Content.ToString(), 0);
                tmp.SetValue(PriceLb.Content.ToString(), 1);
                tmp.SetValue(CountTb.Text, 2);
                tmp.SetValue((int.Parse(PriceLb.Content.ToString())*int.Parse(CountTb.Text)).ToString(), 3);
                #endregion
                #region Add New Item
                mCartLst.Add(tmp);

                Row item = new Row() {ProductBnd = tmp[0], PriceBnd = tmp[1], CountBnd = tmp[2], SumBnd = tmp[3]};
                CartData.Items.Add(item);
                #region Union
                Row tmpitm = new Row();
                for (int i = 0; i < CartData.Items.Count; ++i)
                {
                    for (int j = 0; j < CartData.Items.Count; ++j)
                    {
                        if (i == j) continue;
                        item = CartData.Items[i] as Row;
                        tmpitm = CartData.Items[j] as Row;
                        if (item.ProductBnd == tmpitm.ProductBnd)
                        {
                            item.CountBnd = ((int.Parse(item.CountBnd)) + (int.Parse(tmpitm.CountBnd))).ToString();
                            item.SumBnd = ((int.Parse(item.SumBnd)) + (int.Parse(tmpitm.SumBnd))).ToString();
                            CartData.Items.Remove(CartData.Items[j]);
                            CartData.Items.Remove(CartData.Items[i]);
                            CartData.Items.Add(item);
                        }
                    }
                }
                #endregion
                #endregion

                TotSumLb.Content = ((int.Parse(TotSumLb.Content.ToString())) + (int.Parse(tmp[3]))).ToString();
                PurchLb.Content = (int.Parse(PurchLb.Content.ToString())) - (int.Parse(CountTb.Text));
            }
            else CountTb.Background = new SolidColorBrush(Colors.PaleVioletRed);
        }
        private void CountTb_MouseEnter(object sender, MouseEventArgs e)
        {
            CountTb.Background = new SolidColorBrush(Colors.White);
        }
        private void CartData_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            return;
        }
        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {
            int idx = -1;
            idx = CartData.SelectedIndex;

            TotSumLb.Content = ((int.Parse(TotSumLb.Content.ToString())) - (int.Parse(mCartLst[idx][3]))).ToString();

            CartData.Items.Remove(CartData.Items[idx]);
            mCartLst.Remove(mCartLst[idx]);
        }
        private void BuyBtn_Click(object sender, RoutedEventArgs e)
        {
            //return;
            #region Creating DB
            StreamWriter CartWr = new StreamWriter("Cart.tmp", true);
            String Str = "";

            for (int i = 0; i < mCartLst.Count; ++i)
            {
                Str = mCartLst[i][0] + ":" + mCartLst[i][1] + ":" + mCartLst[i][2] + ":" + mCartLst[i][3];
                CartWr.WriteLine(Str);
            }

            CartWr.Flush();
            CartWr.Close();
            #endregion
            #region ready
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
            #endregion
            #region Sending DB
            // Creating the filestream and file
            StreamReader tmpfile = new StreamReader("Cart.tmp");

            // Receiving the file from client
            while (!tmpfile.EndOfStream)
            {
                Thread.Sleep(5);
                LoginWnd._buffer = LoginWnd.PInvokeFill(LoginWnd.GetBytes("\0"));
                LoginWnd._strbuffer.Remove(0);

                LoginWnd._strbuffer = tmpfile.ReadLine();
                LoginWnd._buffer = LoginWnd.GetBytes(LoginWnd._strbuffer);

                LoginWnd._socket.Send(LoginWnd._buffer);
            }
            LoginWnd._buffer = LoginWnd.PInvokeFill(LoginWnd.GetBytes("\0"));
            LoginWnd._strbuffer.Remove(0);

            LoginWnd._buffer = LoginWnd.GetBytes("~~");
            LoginWnd._socket.Send(LoginWnd._buffer);

            LoginWnd._buffer = LoginWnd.PInvokeFill(LoginWnd.GetBytes("\0"));
            LoginWnd._strbuffer.Remove(0);

            tmpfile.Close();
            File.Delete("Cart.tmp");
            #endregion
            #region Clear
            for (int i = mCartLst.Count - 1; i >= 0; --i)
                mCartLst.Remove(mCartLst[i]);
            for (int i = CartData.Items.Count - 1; i >= 0; --i)
                CartData.Items.Remove(CartData.Items[i]);
            TotSumLb.Content = "0";
            #endregion
        }
    }
}
