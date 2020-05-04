// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.ObjectModel;
using System.ComponentModel;
using System;
using RtuModbusTcpLib;
using System.Net;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace OnlineMonitoringLog.Core1
{

    public class Station : INotifyPropertyChanged
    {
        const string _name="CharmShahr";
        private ObservableCollection<Unit> _units = new ObservableCollection<Unit>();

       
        public ObservableCollection<Unit> Units
        { get { return _units; }
            set
            {
                _units = value;
                NotifyPropertyChanged("Units");
            }
        }



        public Station( )
        {
           
        }

        public void Start(List<Unit> UnitList)
        {    
          

        }

     
       public event PropertyChangedEventHandler PropertyChanged;
        // This method is called by the Set accessor of each property.  
        // The CallerMemberName attribute that is applied to the optional propertyName  
        // parameter causes the property name of the caller to be substituted as an argument.  
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



    }
}