// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.ObjectModel;
using System.ComponentModel;
using System;

namespace OnlineMonitoringLog.Core
{
    public class Station : INotifyPropertyChanged
    {
        private string _name;
        private ObservableCollection<Unit> _units = new ObservableCollection<Unit>();

        public ObservableCollection<Unit> Units
        { get { return _units; }
            set
            {
                _units = value;
                OnPropertyChanged("Units");
            }
        }

        private void OnPropertyChanged(string info)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(info));
        }
              

        public event PropertyChangedEventHandler PropertyChanged;


        public void AddUint()
        {
            _units.Add(new Unit("Mohammad", "Hajjar", "Seattle"));
        }
        public Station()
        {
         }

      

        public override string ToString() => _name;




    }
}