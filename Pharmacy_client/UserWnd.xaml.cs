using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MessageBox = System.Windows.MessageBox;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;

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
        public string login;
        public string password;
        public UserWnd()
        {
            InitializeComponent();
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            return;
            if (LoginWnd.Socket.Connected)
            {
                LoginWnd.ClearBuff();
                LoginWnd.Strbuffer = "~~";
                Thread thread = new Thread(LoginWnd.SendThread);
                thread.Start(LoginWnd.Socket);
                thread.Join();
                LoginWnd.Socket.Close();
            }
            Close();
        }

        private readonly List<String> _mProductLst = new List<String>();
        private readonly List<String[]> _mCartLst = new List<String[]>();
        private object Locker = new object();
        //public Socket Socket;
        public IPEndPoint addr;

        private bool recond;

        private void Reconnect()
        {
            lock (Locker)
            {
                //Dispatcher.Invoke(new MethodInvoker(() => { IsEnabled = false; }), null);
                //int cnt = 3;
                try
                {
                    LoginWnd.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    //var ipAddress = IPAddress.Parse("127.0.0.1");
                    //var addr = new IPEndPoint(ipAddress, 6542);
                    LoginWnd.Socket.Connect(addr);
                }
                catch
                {
                    return;
                }

                LoginWnd.Except = false;
                LoginWnd.ClearBuff();
                LoginWnd.Strbuffer = "Hello";
                Thread thread = new Thread(LoginWnd.SendThread);
                thread.Start(LoginWnd.Socket);
                thread.Join();

                if (LoginWnd.Except)
                {
                    return;
                }

                LoginWnd.ClearBuff();
                thread = new Thread(LoginWnd.RecieveThread);
                thread.Start(LoginWnd.Socket);
                thread.Join();

                if (LoginWnd.Except)
                {
                    return;
                }

                LoginWnd.ClearBuff();
                LoginWnd.Strbuffer = login + ":" + password + "\0";
                // Login
                thread = new Thread(LoginWnd.SendThread);
                thread.Start(LoginWnd.Socket);
                thread.Join();

                if (LoginWnd.Except)
                {
                    return;
                }

                LoginWnd.ClearBuff();
                thread = new Thread(LoginWnd.RecieveThread);
                thread.Start(LoginWnd.Socket);
                thread.Join();

                if (LoginWnd.Except)
                {
                    return;
                }

                if (LoginWnd.Strbuffer.Substring(0, LoginWnd.Strbuffer.IndexOf('\0')).Equals("TRUE"))
                {
                    recond = true;
                    Dispatcher.Invoke(new MethodInvoker(() =>
                    {
                        ProductsLbox.SelectedIndex = -1; 
                        _mProductLst.Clear(); 
                        ProductsLbox.Items.Clear(); 
                        Init(); 
                        TabCtrl.IsEnabled = true; 
                    }), null);
                }
                else
                {
                    MessageBox.Show("Missing login or password!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void reconn()
        {
            recond = false;
            for (int i = 0; i < 10; ++i)
            {
                Thread recon = new Thread(Reconnect);
                recon.Start();
                recon.Join();
                if (recond) return;
                Thread.Sleep(2000);
            }
        }
        private void CountTb_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || 
                (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)) return;
            else e.Handled = true;
        }
        private void Init()
        {
            #region ready
            LoginWnd.ClearBuff();
            LoginWnd.Strbuffer = "READY";
            var thread = new Thread(LoginWnd.SendThread);
            thread.Start(LoginWnd.Socket);
            thread.Join();
            if (LoginWnd.Except)
            {
                TabCtrl.IsEnabled = false;
                Thread recon = new Thread(reconn);
                recon.Start();
                MessageBox.Show("Connetcion error!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            #endregion
            #region Receiving DB
            LoginWnd.ClearBuff();
            thread = new Thread(LoginWnd.RecieveThread);
            thread.Start(LoginWnd.Socket);
            thread.Join();
            if (LoginWnd.Except)
            {
                TabCtrl.IsEnabled = false;
                Thread recon = new Thread(reconn);
                recon.Start();
                MessageBox.Show("Connetcion error!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            String[] tmpStr = LoginWnd.Strbuffer.Substring(0, LoginWnd.Strbuffer.LastIndexOf(":", StringComparison.Ordinal)).Split(':');

            foreach (string t in tmpStr)
            {
                _mProductLst.Add(t);
                ProductsLbox.Items.Add(t);
            }

            #endregion
        }
        private void Window_Initialized(object sender, EventArgs e)
        {
            Init();
        }
        private void ProductsLbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductsLbox.SelectedIndex < 0) return;
            ProductLb.Content = _mProductLst[ProductsLbox.SelectedIndex];

            LoginWnd.ClearBuff();
            LoginWnd.Strbuffer = "getdata";
            Thread thread = new Thread(LoginWnd.SendThread);
            thread.Start(LoginWnd.Socket);
            thread.Join();

            if (LoginWnd.Except)
            {
                TabCtrl.IsEnabled = false;
                Thread recon = new Thread(reconn);
                recon.Start();
                MessageBox.Show("Connetcion error!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            LoginWnd.ClearBuff();
            LoginWnd.Strbuffer = _mProductLst[ProductsLbox.SelectedIndex];
            thread = new Thread(LoginWnd.SendThread);
            thread.Start(LoginWnd.Socket);
            thread.Join();

            if (LoginWnd.Except)
            {
                TabCtrl.IsEnabled = false;
                Thread recon = new Thread(reconn);
                recon.Start();
                MessageBox.Show("Connetcion error!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            LoginWnd.ClearBuff();
            thread = new Thread(LoginWnd.RecieveThread);
            thread.Start(LoginWnd.Socket);
            thread.Join();

            if (LoginWnd.Except)
            {
                TabCtrl.IsEnabled = false;
                Thread recon = new Thread(reconn);
                recon.Start();
                MessageBox.Show("Connetcion error!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            String[] splStr = LoginWnd.Strbuffer.Split(':');

            VendorLb.Content = splStr[0];
            PurchLb.Content = splStr[1];
            PriceLb.Content = splStr[2];
            YearLb.Content = splStr[3];
            DescrLb.Content = splStr[4].Substring(0, splStr[4].IndexOf('\0'));
        }
        private void CartBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int.Parse(CountTb.Text);
            }
            catch (Exception)
            {
                CountTb.Background = new SolidColorBrush(Colors.PaleVioletRed);
                return;
            }

            if (ProductsLbox.SelectedIndex < 0 || (string) VendorLb.Content == " ")
            {
                MessageBox.Show("Please, select product!");
                return;
            }

            if ((CountTb.Text[0] != '0') && 
                ((int.Parse(PurchLb.Content.ToString())) >= (int.Parse(CountTb.Text))))
            {
                #region String item
                String[] tmp = new String[4];
                tmp.SetValue(ProductLb.Content.ToString(), 0);
                tmp.SetValue(PriceLb.Content.ToString(), 1);
                tmp.SetValue(CountTb.Text, 2);
                tmp.SetValue((double.Parse(PriceLb.Content.ToString())*int.Parse(CountTb.Text)).ToString(), 3);
                #endregion
                #region Add New Item
                _mCartLst.Add(tmp);

                Row item = new Row() {ProductBnd = tmp[0], 
                                      PriceBnd = tmp[1], 
                                      CountBnd = tmp[2], 
                                      SumBnd = tmp[3]};
                CartData.Items.Add(item);
                #region Union
                for (int i = 0; i < CartData.Items.Count; ++i)
                {
                    for (int j = 0; j < CartData.Items.Count; ++j)
                    {
                        if (i == j) continue;
                        item = CartData.Items[i] as Row;
                        var tmpitm = CartData.Items[j] as Row;
                        if (item.ProductBnd != tmpitm.ProductBnd) continue;
                        item.CountBnd = ((int.Parse(item.CountBnd)) + (int.Parse(tmpitm.CountBnd))).ToString();
                        item.SumBnd = ((double.Parse(item.SumBnd)) + (double.Parse(tmpitm.SumBnd))).ToString(CultureInfo.InvariantCulture);
                        CartData.Items.Remove(CartData.Items[j]);
                        CartData.Items.Remove(CartData.Items[i]);
                        CartData.Items.Add(item);
                    }
                }
                #endregion
                #endregion

                TotSumLb.Content = ((double.Parse(TotSumLb.Content.ToString())) + (double.Parse(tmp[3]))).ToString();
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
            if (CartData.Items.IsEmpty)
            {
                MessageBox.Show("Cart is empty!");
                return;
            }
            var idx = CartData.SelectedIndex;

            if (idx < 0)
            {
                MessageBox.Show("Please, select item!");
                return;
            }

            TotSumLb.Content = ((double.Parse(TotSumLb.Content.ToString())) - (double.Parse(_mCartLst[idx][3]))).ToString(CultureInfo.InvariantCulture);

            CartData.Items.Remove(CartData.Items[idx]);
            _mCartLst.Remove(_mCartLst[idx]);
        }
        private void BuyBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CartData.Items.IsEmpty)
            {
                MessageBox.Show("Cart is empty!");
                return;
            }
            #region Creating DB

            StreamWriter cartWr = new StreamWriter("Cart.tmp", true);
            foreach (var str in _mCartLst.Select(t => t[0] + ":" + t[1] + ":" + t[2] + ":" + t[3]))
            {
                cartWr.WriteLine(str);
            }

            cartWr.Flush();
            cartWr.Close();
            #endregion
            #region ready
            LoginWnd.ClearBuff();
            LoginWnd.Strbuffer = "buyproduct";
            Thread thread = new Thread(LoginWnd.SendThread);
            thread.Start(LoginWnd.Socket);
            thread.Join();

            if (LoginWnd.Except)
            {
                IsEnabled = false;
                Thread recon = new Thread(reconn);
                recon.Start();
                MessageBox.Show("Connetcion error!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            #endregion
            #region Sending DB
            StreamReader tmpfile = new StreamReader("Cart.tmp");

            String tmp = "";
            while (!tmpfile.EndOfStream)
            {
                tmp += tmpfile.ReadLine() + ":";
            }

            LoginWnd.ClearBuff();
            LoginWnd.Strbuffer = tmp;

            thread = new Thread(LoginWnd.SendThread);
            thread.Start(LoginWnd.Socket);
            thread.Join();
            tmpfile.Close();
            if (LoginWnd.Except)
            {
                IsEnabled = false;
                Thread recon = new Thread(reconn);
                recon.Start();
                MessageBox.Show("Connetcion error!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //File.Delete("Cart.tmp");
            #endregion
            #region Clear
            for (int i = _mCartLst.Count - 1; i >= 0; --i)
                _mCartLst.Remove(_mCartLst[i]);
            for (int i = CartData.Items.Count - 1; i >= 0; --i)
                CartData.Items.Remove(CartData.Items[i]);
            TotSumLb.Content = "0";
            #endregion
        }
        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (LoginWnd.Socket.Connected)
            {
                LoginWnd.ClearBuff();
                LoginWnd.Strbuffer = "~~";
                Thread thread = new Thread(LoginWnd.SendThread);
                thread.Start(LoginWnd.Socket);
                thread.Join();
                LoginWnd.Socket.Close();
            }
            Close();
        }
        private void MinButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
