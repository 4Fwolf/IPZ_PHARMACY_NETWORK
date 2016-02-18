using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pharmacy_client
{
    /// <summary>
    /// Interaction logic for LoginWnd.xaml
    /// </summary>
    public partial class LoginWnd : Window
    {
        public LoginWnd()
        {
            InitializeComponent();
        }

        public static Socket _socket;
        public static byte[] _buffer = new byte[1024];
        public static String _strbuffer;
        public static byte[] GetBytes(String str)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);
            //System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
        public static String GetString(byte[] bytes)
        {
            //char[] chars = new char[bytes.Length / sizeof(char)];
            //System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            char[] chars = System.Text.Encoding.UTF8.GetChars(bytes);
            return new String(chars);
        }
        public static byte[] PInvokeFill(byte[] value)
        {
            var arr = new byte[1024];
            GCHandle gch = GCHandle.Alloc(arr, GCHandleType.Pinned);
            MemSet(gch.AddrOfPinnedObject(), value[0], 1024);
            gch.Free();
            return arr;
        }

        [DllImport("msvcrt.dll",
            EntryPoint = "memset",
            CallingConvention = CallingConvention.Cdecl,
            SetLastError = false)]
        public static extern IntPtr MemSet(IntPtr dest, int value, int count);

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _buffer = GetBytes("~~");
                _socket.Send(_buffer);
                _socket.Close();
                Close();
            }
            catch (Exception ex)
            {
                Close();
            }
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Connect
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ipAddress = null;
                IPEndPoint addr = null;
                ipAddress = System.Net.IPAddress.Parse("127.0.0.1");
                addr = new IPEndPoint(ipAddress, 6542);
                _socket.Connect(addr);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connetcion error!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _strbuffer = LoginTb.Text + ":" + PassTb.Password + "\0";

            // Login
            _buffer = GetBytes(_strbuffer);
            _socket.Send(_buffer);

            _buffer = PInvokeFill(GetBytes("\0"));
            _strbuffer.Remove(0);

            _socket.Receive(_buffer);

            _strbuffer = GetString(_buffer);
            if (_strbuffer.Substring(0, _strbuffer.IndexOf('\0')).Equals("TRUE"))
            {
                _buffer = PInvokeFill(GetBytes("\0"));
                _strbuffer.Remove(0);

                UserWnd userWnd = new UserWnd();
                userWnd.Show();
                Close();
                //Hide();
            }
            else
            {
                _buffer = PInvokeFill(GetBytes("\0"));
                _strbuffer.Remove(0);
                MessageBox.Show("Missing login or password!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
