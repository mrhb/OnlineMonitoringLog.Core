// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace OnlineMonitoringLog.Core1
{
   
    public class Unit : INotifyPropertyChanged
    {

        Socket lan_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public classTimout timout = new classTimout();

        private byte _Id;
        private IPAddress _ip;
        public byte lastFunction;
        public Socket Sockete = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public int BufferReadNum = 0;
        public byte[] buffer_read = new byte[2048];
        public byte[] buffer_write = new byte[250];


        private string _firstname;
        private string _hometown;
        private string _lastname;

        public Unit()
        {
        }

        public Unit(string first, string last, string town)
        {
            
            _firstname = first;
            _lastname = last;
            _hometown = town;
        }
        public Unit(IPAddress ip, byte Id)
        {
            
            _ip = ip;
            _Id = Id;
        }
        public IPAddress ip
        {
            get { return _ip; }
            set
            {
                _ip = value;
                NotifyPropertyChanged("ip");
            }
        }

        public byte Id
        {
            get { return _Id; }
            set
            {
                _Id = value;
                NotifyPropertyChanged("Id");
            }
        }
        public string LastName
        {
            get { return _lastname; }
            set
            {
                _lastname = value;
                NotifyPropertyChanged("LastName");
            }
        }

        public string HomeTown
        {
            get { return _hometown; }
            set
            {
                _hometown = value;
                NotifyPropertyChanged("HomeTown");
            }
        }


        public override string ToString() { return ip.ToString(); }

        public event PropertyChangedEventHandler PropertyChanged;
        // This method is called by the Set accessor of each property.  
        // The CallerMemberName attribute that is applied to the optional propertyName  
        // parameter causes the property name of the caller to be substituted as an argument.  
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
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
}
 