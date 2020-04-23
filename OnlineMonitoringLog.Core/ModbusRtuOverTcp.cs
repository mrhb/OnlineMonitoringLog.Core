using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Timers;

namespace RtuModbusTcpLib
{
    class ModbusRtuOverTcp
    {



        public connection[] connectionList;
        int _num;

        public Socket accept_sockt;
        uint timoutValue ;//3*100ms =300ms
        bool sendBusy;
       // public int newData;

        Timer timer_timout;
        Socket GlobalSocket;
//********************************************************************************************************
        public ModbusRtuOverTcp(int NumOfDevice)
        {
            _num = NumOfDevice;
            connectionList = new connection[_num];
            for (int i = 0; i < _num; i++)
            {

                connectionList[i] = new connection();

            }

        }

        public void Connection_Start(int port, uint Timout)
        {
            AsyncStart(port);

            timoutValue = Timout / 100;
            timer_timout = new System.Timers.Timer(100);
            timer_timout.AutoReset = true;
            timer_timout.Elapsed += handeler_timout;
            timer_timout.Enabled = true;

        }

         bool AsyncStart(int port)
        {
            bool status = false;
            try {
                IPEndPoint lan_ip = new IPEndPoint(IPAddress.Any, port);
                Socket lan_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                lan_socket.Bind(lan_ip);
                lan_socket.Listen(_num);

                AsyncCallback callBackMethod = new AsyncCallback(Accept_Callback);
                IAsyncResult arc = lan_socket.BeginAccept(Accept_Callback, lan_socket);
                accept_sockt = (Socket)arc.AsyncState;//this socket keep information about socket is listen to port
                status = true;
            }
            catch
            {

            }
            return status;

        }

        public void AsyncStop()
        {

            for (int i = 0; i < _num; i++)
            {

                if (connectionList[i].Sockete.Connected == true)
                { try { connectionList[i].Sockete.Close(); } catch { } }
            }
            accept_sockt.Close();
        }

        private void Accept_Callback(IAsyncResult ar)
        {
            Socket lan_socket = (Socket)ar.AsyncState;
            // try
            {
                Socket accept = lan_socket.EndAccept(ar);
                IPEndPoint eip1 = accept.RemoteEndPoint as IPEndPoint;
                IPAddress ip1 = eip1.Address;


                for (int i = 0; i < _num; i++)
                {


                    if (ip1.Equals(connectionList[i].ip))//this socket empty
                    {
                        connectionList[i].Sockete = accept;
                        // eip2 = (IPEndPoint)connectionList[i].Sockete.RemoteEndPoint;
                        // connectionList[i].ip = eip2.Address;

                        AsyncCallback ReceiveMethod = new AsyncCallback(ReciveData_callback);
                        connectionList[i].Sockete.BeginReceive(connectionList[i].buffer_read, 0, connectionList[i].buffer_read.Length,
                                                         SocketFlags.None, ReciveData_callback, connectionList[i]);

                        break;
                    }
                }

                //***********************************************************************************
                lan_socket.Listen(_num);
                AsyncCallback callBackMethod = new AsyncCallback(Accept_Callback);

                IAsyncResult arc = lan_socket.BeginAccept(Accept_Callback, lan_socket);
                accept_sockt = (Socket)ar.AsyncState;
            }
            //catch
            { }


        }

        private void ReciveData_callback(IAsyncResult ar)
        {
            connection connection1 = (connection)ar.AsyncState;
            Socket sokect1 = (Socket)connection1.Sockete;


            try
            {
                connection1.BufferReadNum = sokect1.EndReceive(ar);

                if (connection1.BufferReadNum > 0)
                {
                    //******* Data recive
                    //*******Data saved in ConnectionList.Buffer_Read 
                    ReciveData(connection1, false);

                    //************************************************************             
                    AsyncCallback ReceiveMethod = new AsyncCallback(ReciveData_callback);
                    connection1.Sockete.BeginReceive(connection1.buffer_read, 0, _num,
                                                 SocketFlags.None, ReciveData_callback, connection1);

                }
                else// sockt disconnect 
                {
                    sokect1.Close();
                }
            }
            catch
            {
                sokect1.Close();
            }
        }

        public bool SendData(uint device, byte[] data, int size)
        {
            bool status = false;


            if (connectionList[device].Sockete.Connected)
            {
                connectionList[device].Sockete.BeginSend(data, 0, size,
                        SocketFlags.None, new AsyncCallback(SendData_Callback), connectionList[device].Sockete);
                status = true;
            }


            return status;
        }

        private void SendData_Callback(IAsyncResult ar)
        {

            Socket client = (Socket)ar.AsyncState;

            // Complete sending the data to the remote device.  
            int bytesSent = client.EndSend(ar);


        }
//------------------------------------------------------------------------------------------------------------
        private void handeler_timout(Object source, ElapsedEventArgs e)
        {

            for (uint i = 0; i <_num; i++)
            {
                if (connectionList[i].timout.enable == true)
                {
                    if (connectionList[i].timout.Cnt >= timoutValue)
                    {
                        ReciveData(connectionList[i],true);
                        connectionList[i].timout.enable = false;
                        connectionList[i].timout.TimOut = true;
                    }
                    connectionList[i].timout.Cnt++;
                }
            }

        }

        public static void ReciveData(connection connection1, bool TimedOut)
        {
            if (!TimedOut)
            {
                //crc check
                if (BitConverter.ToUInt16(connection1.buffer_read, connection1.BufferReadNum - 2) == CRC_calculate(connection1.buffer_read, connection1.BufferReadNum - 2))
                {
                    connection1.timout.TimOutDisable();
                    if (connection1.Id == connection1.buffer_read[0])//Id check
                    {
                        if (connection1.lastFunction == connection1.buffer_read[1])//function check
                        {
                            //Form1.recivedData();
                            //ModbusDataPassToMotor(client.buffer_read.Skip(2).ToArray(), len_data - 4, id);
                        }
                    }

                }
            }
            else//timeOut 
            {
               // Form1.TimeOut();
                //*******************************
            }
            connection1.lastFunction = 0;

        }

        public bool WriteMultiRegister_F16(uint device, byte[] buffer, int Address, int lengh)
        {
            bool status = false;
            int i = 0;
            byte[] data = new byte[8 + lengh];

            data[i] = connectionList[device].Id; i++;

            data[i] = 16; i++;

            data[i] = (byte)(Address >> 8); i++;//hi
            data[i] = (byte)Address; i++;//low

            data[i] = (byte)(lengh >> 8); i++;//hi
            data[i] = (byte)lengh; i++;//low

            data[i] = (byte)(lengh * 2); i++;//byte cont

            for (int j = 0; j < (lengh * 2); j++)
            {
                data[i] = buffer[j]; i++;//

            }

            ushort crc = CRC_calculate(data, 6);
            data[i] = (byte)(crc); i++;
            data[i] = (byte)(crc >> 8); i++;

           status= SendData(device, data, i);

            if (status)
            {
                connectionList[device].lastFunction = 16;
                connectionList[device].timout.TimOutEnable();
            }
            return status;
        }

        public bool ReadMultiRegister_F3(uint device, int Address, int lengh)//lengh=number word device 0 to numberOfDevice
        {
            int i = 0;
            byte[] data = new byte[8];
            bool status=false;

            data[i] = connectionList[device].Id; i++;

            data[i] = 3; i++;

            data[i] = (byte)(Address >> 8); i++;//hi
            data[i] = (byte)Address; i++;//low

            data[i] = (byte)(lengh >> 8); i++;//hi
            data[i] = (byte)lengh; i++;//low

            ushort crc = CRC_calculate(data, 6);
            data[i] = (byte)(crc); i++;
            data[i] = (byte)(crc >> 8); i++;


            status = SendData(device, data, i);
            if (status)
            {
                connectionList[device].lastFunction = 3;
                connectionList[device].timout.TimOutEnable();
            }

            return status;

        }

        private static ushort CRC_calculate(byte[] data, int len)
        {
            ushort crc = 0xFFFF;
            int i, num = len, j = 0;
            while ((num--) > 0)
            {
                crc ^= data[j]; j++;
                for (i = 0; i < 8; i++)
                {
                    if ((crc & 1) > 0)
                    {
                        crc >>= 1;
                        crc ^= 0xA001;
                    }
                    else
                    {
                        crc >>= 1;
                    }
                }
            }
            return crc;
        }


        //*******************************************************************************************
        public class connection
        {
            public class classTimout
            {
                public void TimOutEnable()
                {
                    Cnt = 0;
                    TimOut = false;
                    enable = true;
                }
                public void TimOutDisable()
                {
                    enable = false;
                }
                public bool enable;
                public uint Cnt;
                public bool TimOut;
            }

            public classTimout timout = new classTimout();

            public byte Id;
            public IPAddress ip;
            public byte lastFunction;
            public Socket Sockete = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            public int BufferReadNum = 0;
            public byte[] buffer_read = new byte[2048];
            public byte[] buffer_write = new byte[250];

        }









    }
}
