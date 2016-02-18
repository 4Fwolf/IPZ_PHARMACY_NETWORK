using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;

namespace Pharmacy_server
{
    // Класс-обработчик клиента
    class Client
    {
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
        private static byte[] PInvokeFill(byte[] value)
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

        // Конструктор класса. Ему нужно передавать принятого клиента от TcpListener
        public Client(TcpClient Client)
        {
            // Переменная для хранения количества байт, принятых от клиента
            int recv_bytes;
            bool flg = false;
            String tmp = "";

            // Читаем из потока клиента до тех пор, пока от него поступают данные
            recv_bytes = Client.GetStream().Read(_buffer, 0, _buffer.Length);
            _strbuffer = GetString(_buffer);

            //FileStream login = new FileStream("Login_db.txt", FileMode.Open);
            StreamReader login = new StreamReader("Login_db.txt");
            try
            {
                login.Peek();
            }
            catch (Exception ex)
            {
                Console.WriteLine("	Error, Login_db not found! \n");
                Client.Close();
            }
            while (!login.EndOfStream)
            {
                tmp = login.ReadLine();
                if (_strbuffer.Substring(0, _strbuffer.IndexOf('\0')).Equals(tmp))
                {
                    flg = true;
                    login.Close();
                    break;
                }
            }

            if (flg)
            {
                _buffer = GetBytes("TRUE\0");
		        Client.GetStream().Write(_buffer, 0, _buffer.Length);
                Console.WriteLine("	Logged!");
            }
	        else
	        {
                _buffer = GetBytes("FALSE\0");
                Client.GetStream().Write(_buffer, 0, _buffer.Length);
                Console.WriteLine("	Didn't logged!");
	        }
	        flg = false;

            _buffer = PInvokeFill(GetBytes("\0"));
            _strbuffer.Remove(0);

            Client.GetStream().Read(_buffer, 0, _buffer.Length);
            _strbuffer = GetString(_buffer);
            if (_strbuffer.Substring(0, _strbuffer.IndexOf('\0')).Equals("READY"))
            {
                StreamReader file = new StreamReader("Pharmacy_db.txt");
                try
                {
                    file.Peek();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("	Error, Pharmacy_db not found! \n");
                    Client.Close();
                }

                while (!file.EndOfStream)
                {
                    Thread.Sleep(5);
                    _buffer = PInvokeFill(GetBytes("\0"));
                    _strbuffer.Remove(0);

                    _strbuffer = file.ReadLine();
                    _buffer = GetBytes(_strbuffer);
                    Client.GetStream().Write(_buffer, 0, _buffer.Length);
                }

                _buffer = PInvokeFill(GetBytes("\0"));
                _strbuffer.Remove(0);

                _buffer = GetBytes("~~");
                Client.GetStream().Write(_buffer, 0, _buffer.Length);
                file.Close();
            }

            while (true)
            {
                _buffer = PInvokeFill(GetBytes("\0"));
                _strbuffer = "\0\0\0";

                Client.GetStream().Read(_buffer, 0, _buffer.Length);
                _strbuffer = GetString(_buffer);

                if (_strbuffer.Substring(0, _strbuffer.IndexOf('\0')).Equals("READY"))
                {
                    StreamWriter tmpfile = new StreamWriter("Cart.tmp", true);
                    //flg = true;
                    while (true)
                    {
                        _buffer = PInvokeFill(GetBytes("\0"));
                        _strbuffer = "\0\0\0";

                        Client.GetStream().Read(_buffer, 0, _buffer.Length);
                        _strbuffer = GetString(_buffer);
                        if (_strbuffer.Substring(0, _strbuffer.IndexOf('\0')).Equals("~~")) 
                            break;
                        
                        tmpfile.WriteLine(_strbuffer.Substring(0, _strbuffer.IndexOf('\0')));
                    }
                    tmpfile.Flush();
                    tmpfile.Close();
                    _buffer = PInvokeFill(GetBytes("\0"));
                    _strbuffer = "\0\0\0";
                }

                if (_strbuffer.Substring(0, _strbuffer.IndexOf('\0')).Equals("~~")) break;
            }

            Console.WriteLine("	Diconected!\n");
            Client.Close();
        }
    }

    class Server
    {
        TcpListener Listener; // Объект, принимающий TCP-клиентов

        // Запуск сервера
        public Server(int Port)
        {
            Listener = new TcpListener(IPAddress.Any, Port); // Создаем "слушателя" для указанного порта
            Listener.Start(); // Запускаем его
            Console.WriteLine("\nTCP SERVER STARTED");
            Console.WriteLine("Waiting for connections...\n");

            // В бесконечном цикле
            while (true)
            {
                // Принимаем нового клиента
                TcpClient Client = Listener.AcceptTcpClient();
                // Создаем поток
                Thread Thread = new Thread(new ParameterizedThreadStart(ClientThread));
                // И запускаем этот поток, передавая ему принятого клиента
                Thread.Start(Client);
                Console.Write(" " + DateTime.Now.TimeOfDay + " ");
                Console.WriteLine(" [" + Client.Client.LocalEndPoint + "]");
            }
        }

        static void ClientThread(Object StateInfo)
        {
            // Просто создаем новый экземпляр класса Client и передаем ему приведенный к классу TcpClient объект StateInfo
            new Client((TcpClient) StateInfo);
        }

        // Остановка сервера
        ~Server()
        {
            // Если "слушатель" был создан
            if (Listener != null)
            {
                // Остановим его
                Listener.Stop();
            }
        }

        class Program
        {
            static void Main(string[] args)
            {
                new Server(6542);
            }
        }
    }
}
