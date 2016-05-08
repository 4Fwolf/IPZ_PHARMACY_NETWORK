using System;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

namespace Pharmacy_server
{
    class Client
    {
        private static byte[] _buffer = new byte[1024];
        private static String _strbuffer;
        private static bool _exception = false;
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
        private static byte[] PInvokeFill(byte[] value)
        {
            var arr = new byte[1024];
            GCHandle gch = GCHandle.Alloc(arr, GCHandleType.Pinned);
            MemSet(gch.AddrOfPinnedObject(), value[0], 1024);
            gch.Free();
            return arr;
        }
        static void SendThread(Object client)
        {
            lock (client)
            {
                try
                {
                    _buffer = GetBytes(_strbuffer);
                    ((TcpClient) client).GetStream().Write(_buffer, 0, _buffer.Length);
                }
                catch (Exception)
                {
                    _exception = true;
                    Console.WriteLine(" [Error] Can't send data! {" + _strbuffer + "}");
                }
            }
        }
        static void RecieveThread(Object client)
        {
            lock (client)
            {
                try
                {
                    ((TcpClient) client).GetStream().Read(_buffer, 0, _buffer.Length);
                    _strbuffer = GetString(_buffer);
                }
                catch (Exception)
                {
                    _exception = true;
                    Console.WriteLine(" [Error] Can't recieve data!");
                }
            }
        }
        static void ClearBuff()
        {
            _buffer = PInvokeFill(GetBytes("\0"));
            _strbuffer = "\0";
        }

        [DllImport("msvcrt.dll",
            EntryPoint = "memset",
            CallingConvention = CallingConvention.Cdecl,
            SetLastError = false)]
        private static extern IntPtr MemSet(IntPtr dest, int value, int count);

        public Client(TcpClient client)
        {
            bool flg = false;
            String tmp;
            String user = "";
            Logins_linqDataContext dataBase = new Logins_linqDataContext();

            // Hello message
            ClearBuff();
            var thread = new Thread(RecieveThread);
            thread.Start(client);
            thread.Join();

            if (_exception) goto disconn;

            ClearBuff();
            _strbuffer = "Hello!";
            thread = new Thread(SendThread);
            thread.Start(client);
            thread.Join();

            if (_exception) goto disconn;

            Relogin:
            ClearBuff();
            thread = new Thread(RecieveThread);
            thread.Start(client);
            thread.Join();

            if (_exception) goto disconn;
            dataBase.Connection.Open();
            //var loginTabl = dataBase.Logins.ToList();
            foreach (var login in dataBase.Logins.ToList())
            {
                tmp = login.Login1 + ":" + login.Password;
                if (!_strbuffer.Substring(0, _strbuffer.IndexOf('\0')).Equals(tmp)) continue;
                flg = true;
                break;
            }
            dataBase.Connection.Close();

            if (flg)
            {
                user = "<" + _strbuffer.Substring(0, _strbuffer.IndexOf(':')) + ">";

                ClearBuff();
                _strbuffer = "TRUE\0";
                thread = new Thread(SendThread);
                thread.Start(client);
                thread.Join();
                if (_exception) goto disconn;

                Console.WriteLine("	" + user + " Logged!");
            }
	        else
	        {
                ClearBuff();
                _strbuffer = "FALSE\0";
                thread = new Thread(SendThread);
                thread.Start(client);
                thread.Join();
                if (_exception) goto disconn;

                Console.WriteLine("	Didn't logged!");
                goto Relogin;
	        }
	        flg = false;

            Console.WriteLine("	" + user + " Waiting readiness ...");

            tryagain:
            ClearBuff();
            thread = new Thread(RecieveThread);
            thread.Start(client);
            thread.Join();
            if (_exception) goto disconn;
            //var pharmTabl = dataBase.Pharmacies.ToList();
            if (_strbuffer.Substring(0, _strbuffer.IndexOf('\0')).Equals("READY"))
            {
                Console.WriteLine("	" + user + " ready to receive data!");
                Console.WriteLine("	" + user + " Sending data ...");

                dataBase.Connection.Open();
                String tmp1 = dataBase.Pharmacies.ToList().Aggregate("", (current, pharm) => current + (pharm.Product + ":"));
                dataBase.Connection.Close();
                ClearBuff();
                _strbuffer = tmp1;
                thread = new Thread(SendThread);
                thread.Start(client);
                thread.Join();
                if (_exception) goto disconn;

                Console.WriteLine("	" + user + " Data sent!");
            }
            else goto tryagain;
            //var pharmTabl = dataBase.Pharmacies.ToList();
            while (true)
            {
                Console.WriteLine("	" + user + " Waiting readiness ...");

                ClearBuff();
                thread = new Thread(RecieveThread);
                thread.Start(client);
                thread.Join();
                if (_exception) goto disconn;

                tmp = _strbuffer.Substring(0, _strbuffer.IndexOf('\0'));
                //dataBase.Connection.Open();
                //var pharmTabl = dataBase.Pharmacies.ToList();
                //dataBase.Connection.Close();
                switch (tmp)
                {
                    case "getdata":
                        ClearBuff();
                        thread = new Thread(RecieveThread);
                        thread.Start(client);
                        thread.Join();
                        if (_exception) goto disconn;

                        Console.WriteLine("        " + user + " Get " + _strbuffer.Substring(0, _strbuffer.IndexOf('\0')) + " data.");
                        //var pharmTabl = dataBase.Pharmacies.ToList();
                        dataBase.Connection.Open();
                        foreach (var pharm in dataBase.Pharmacies.ToList().Where(pharm => pharm.Product == _strbuffer.Substring(0, _strbuffer.IndexOf('\0'))))
                        {
                            ClearBuff();
                            _strbuffer = pharm.Vendor + ":" +
                                         pharm.Count.ToString() + ":" +
                                         pharm.Price.ToString() + ":" +
                                         pharm.Year.ToString() + ":" +
                                         pharm.Description;
                            break;
                        }
                        dataBase.Connection.Close();
                        
                        thread = new Thread(SendThread);
                        thread.Start(client);
                        thread.Join();
                        if (_exception) goto disconn;
                        break;
                    case "buyproduct":
                        Console.WriteLine("        " + user + " Buy products.");

                        ClearBuff();
                        thread = new Thread(RecieveThread);
                        thread.Start(client);
                        thread.Join();

                        if (_exception) goto disconn;

                        String []splStr = _strbuffer.Substring(0, _strbuffer.LastIndexOf(';')).Split(';');

                        foreach (string t in splStr)
                        {
                            var cartItm = t.Split(':');

                            dataBase.Connection.Open();
                            var _itm = dataBase.Pharmacies.SingleOrDefault(p => p.Product.ToString() == cartItm[0]);
                            int cn = int.Parse(cartItm[2]);
                            
                            // TODO: LINQ Change
                            //dataBase.Pharmacies.Context.SubmitChanges();
                            //dataBase.Pharmacies.DeleteOnSubmit(_itm);
                            _itm.Count -= cn;
                            //dataBase.Pharmacies.InsertOnSubmit(_itm);
                            dataBase.Pharmacies.Context.SubmitChanges();
                            //dataBase.Connection.ChangeDatabase(dataBase.Connection.Database);
                            dataBase.Connection.Close();
                            
                            // TODO: LINQ Command
                            /*dataBase.Connection.Open();
                            string cm = string.Format("UPDATE dbo.Pharmacy SET Count = {0} WHERE Product LIKE '{1}';", cn, cartItm[0]);
                            int q = dataBase.ExecuteCommand(cm);
                            dataBase.SubmitChanges();
                            dataBase.Refresh(RefreshMode.KeepCurrentValues);
                            dataBase.Connection.Close();
                            Console.WriteLine(q);*/

                            // TODO: SQL Adapter
                            /*string cm = string.Format("UPDATE Pharmacy SET Count = {0} WHERE Product LIKE '{1}';", cn, cartItm[0]);
                            SqlConnection con = new SqlConnection(dataBase.Connection.ConnectionString);
                            con.Open();
                            DataTable tb = new DataTable("Pharmacy");
                            SqlDataAdapter dataAdpater = new SqlDataAdapter("Select * from Pharmacy", con);
                            dataAdpater.UpdateCommand = new SqlCommand(cm, con);
                            dataAdpater.Fill(tb);
                            dataAdpater.Update(tb);
                            con.Close();*/

                            // TODO: SQL Command
                            //string cm = string.Format("UPDATE Pharmacy SET Count = {0} WHERE Product LIKE '{1}';", cn, cartItm[0]);
                            /*SqlConnection con = new SqlConnection(dataBase.Connection.ConnectionString);
                            con.Open();
                            int conrez = new SqlCommand("UPDATE Pharmacy SET Count=" + cn + " where Product LIKE '" + cartItm[0] + "'", con).ExecuteNonQuery();
                            //SqlCommand cmd = new SqlCommand(cm, con);
                            //cmd.ExecuteNonQuery();
                            Console.WriteLine(conrez);
                            con.Close();*/

                            // TODO: Transaction
                            /*string cm = string.Format("UPDATE dbo.Pharmacy SET Count = {0} WHERE Product LIKE '{1}';", cn, cartItm[0]);
                            SqlConnection con = new SqlConnection(dataBase.Connection.ConnectionString);
                            con.Open();

                            SqlTransaction transaction;
                            transaction = con.BeginTransaction("SampleTransaction");
                            SqlCommand cmd = new SqlCommand(cm, con);
                            cmd.Transaction = transaction;

                            try
                            {
                                cmd.ExecuteNonQuery();
                                // Attempt to commit the transaction.
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                try { transaction.Rollback(); }
                                catch { }
                            }
                            con.Close();*/
                        }
                            break;
                    default:
                        goto disconn;
                }
            }

            disconn:
            Console.WriteLine("	" + user + " Diconected!\n");
            client.Close();
        }
    }

    class Server
    {
        readonly TcpListener _listener;

        public Server(int port)
        {
            _listener = new TcpListener(IPAddress.Any, port);
            _listener.Start();
            Console.WriteLine("\nTCP SERVER STARTED");
            Console.WriteLine("Waiting for connections...\n");

            while (true)
            {
                TcpClient client = _listener.AcceptTcpClient();
                Thread thread = new Thread(ClientThread);
                thread.Start(client);
                Console.Write(" " + DateTime.Now.TimeOfDay + " ");
                Console.WriteLine(" [" + client.Client.LocalEndPoint + "]");
            }
        }

        static void ClientThread(Object stateInfo)
        {
            new Client((TcpClient) stateInfo);
        }

        ~Server()
        {
            if (_listener != null)
            {
                _listener.Stop();
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
