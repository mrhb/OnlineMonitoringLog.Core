// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using AlarmBase.DomainModel;
using AlarmBase.DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Runtime.CompilerServices;

namespace OnlineMonitoringLog.UI_WPF.model
{
    public abstract class Variable :  IVariable
    {
        protected LoggableObj _loggableObj;
        protected ILoggRepository _repo;
        RegisteredVarConfig _varConfig;
        int _unitId;
        string _value = "Not assigned";
        DateTime _timeStamp = new DateTime();
        string _resource = "Not assigned";
        public Variable(int unitId,string resourceName, ILoggRepository repo) 
        {
            name = resourceName;
             _unitId= unitId;
            _repo = repo;
        }
       public void RecievedData(int val, DateTime dt)
        {
            _loggableObj.State = val;
            value = val.ToString();
            timeStamp = dt;
        }

        public string name
        {
            get
            {
                return Resource;
            }
            set
            {
                Resource = value;
                NotifyPropertyChanged("value");
            }
        }
        public string value
        {
            get
            {
                return Value;
            }
            set
            {
                Value = value;
                NotifyPropertyChanged("value");
            }
        }
        public DateTime timeStamp
        {
            get
            {
                return TimeStamp;
            }
            set
            {
                TimeStamp = value;
                NotifyPropertyChanged("timeStamp");
            }
        }

        public string Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
            }
        }

        public DateTime TimeStamp
        {
            get
            {
                return _timeStamp;
            }

            set
            {
                _timeStamp = value;
            }
        }

        public string Resource
        {
            get
            {
                return _resource;
            }

            set
            {
                _resource = value;
            }
        }

        public int UnitId
        {
            get
            {
                return _unitId;
            }
            protected set { _unitId = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        // This method is called by the Set accessor of each property.  
        // The CallerMemberName attribute that is applied to the optional propertyName  
        // parameter causes the property name of the caller to be substituted as an argument.  
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return value;
        }
        private void ChangedConfigEvent(object sender, PropertyChangedEventArgs e)
        {
            SetConfig((RegisteredVarConfig)sender);
        }
        public bool SetConfig(RegisteredVarConfig _OccConfig)
        {
            _varConfig = _OccConfig;
            _varConfig.ConfigChangeSaved += ChangedConfigEvent;
            Boolean result = true;
            try
            {
        //        Type t = Type.GetType(_OccConfig.SetPointType);
        //var sfsdf = Activator.CreateInstance(t);

        //        if (t == typeof(int))
        //        {
        //            setpointObj = Convert.ToInt32(_OccConfig.SerializedSetPoint);
        //        }
        //        else if (t == typeof(bool))
        //        {
        //            setpointObj = Convert.ToBoolean(_OccConfig.SerializedSetPoint);
        //        }

        //        else
        //            result = false;
            }
            catch (Exception c)
            {
                result = false;
            }
            return result;
            }

        public Boolean Initialization(RegisteredVarConfig varConfig)
        {
            _loggableObj = new LoggableObj(varConfig.VarConfigID, _repo);
            return SetConfig(varConfig);
        }
    }

}

