using System;
using System.Windows;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

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

        public static Socket Socket;
        private static byte[] _buffer = new byte[1024];
        public static String Strbuffer;
        public static bool Except = false;
        private static byte[] GetBytes(String str)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);
            return bytes;
        }
        private static String GetString(byte[] bytes)
        {
            char[] chars = System.Text.Encoding.UTF8.GetChars(bytes);
            return new String(chars);
        }
        public static void SendThread(Object socket)
        {
            lock (socket)
            {
                try
                {
                    _buffer = GetBytes(Strbuffer);
                    ((Socket)socket).Send(_buffer);
                }
                catch (Exception)
                {
                    Except = true;
                    MessageBox.Show("Can't send data! {" + Strbuffer + "}", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        public static void RecieveThread(Object socket)
        {
            lock (socket)
            {
                try
                {
                    ((Socket)socket).Receive(_buffer);
                    Strbuffer = GetString(_buffer);
                }
                catch (Exception)
                {
                    Except = true;
                    MessageBox.Show("Can't recieve data!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private static byte[] PInvokeFill(byte[] value)
        {
            var arr = new byte[1024];
            GCHandle gch = GCHandle.Alloc(arr, GCHandleType.Pinned);
            MemSet(gch.AddrOfPinnedObject(), value[0], 1024);
            gch.Free();
            return arr;
        }
        public static void ClearBuff()
        {
            _buffer = PInvokeFill(GetBytes("\0"));
            Strbuffer = "\0";
        }

        [DllImport("msvcrt.dll",
            EntryPoint = "memset",
            CallingConvention = CallingConvention.Cdecl,
            SetLastError = false)]
        private static extern IntPtr MemSet(IntPtr dest, int value, int count);

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Socket != null)
            {
                ClearBuff();
                Strbuffer = "~~";
                Thread thread = new Thread(SendThread);
                thread.Start(Socket);
                thread.Join();
                Socket.Close();
            }
            Close();
        }
        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            int cnt = 3;
            try
            {
                Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                var ipAddress = IPAddress.Parse("127.0.0.1");
                var addr = new IPEndPoint(ipAddress, 6542);
                Socket.Connect(addr);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connetcion error!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            tryagainS:
            ClearBuff();
            Strbuffer = "Hello";
            Thread thread = new Thread(SendThread);
            thread.Start(Socket);
            thread.Join();

            if (Except)
            {
                if (cnt > 0)
                {
                    --cnt;
                    goto tryagainS;
                }
                MessageBox.Show("Connetcion error!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                
            }

            cnt = 3;

            tryagainR:
            ClearBuff();
            thread = new Thread(RecieveThread);
            thread.Start(Socket);
            thread.Join();

            if (Except)
            {
                if (cnt > 0)
                {
                    --cnt;
                    goto tryagainR;
                }
                MessageBox.Show("Connetcion error!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            cnt = 3;

            ClearBuff();
            Strbuffer = LoginTb.Text + ":" + PassTb.Password + "\0";
            // Login
            thread = new Thread(SendThread);
            thread.Start(Socket);
            thread.Join();

            if (Except)
            {
                MessageBox.Show("Connetcion error!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            ClearBuff();
            thread = new Thread(RecieveThread);
            thread.Start(Socket);
            thread.Join();

            if (Except)
            {
                MessageBox.Show("Connetcion error!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (Strbuffer.Substring(0, Strbuffer.IndexOf('\0')).Equals("TRUE"))
            {
                UserWnd userWnd = new UserWnd();
                userWnd.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Missing login or password!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
