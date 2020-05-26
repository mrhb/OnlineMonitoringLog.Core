using AlarmBase.DomainModel.Entities;
using AlarmBase.DomainModel.generics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using IwnTagType = System.Int32;

namespace AlarmBase.DomainModel.repository
{
    public class CentralConfigViewModel : INotifyPropertyChanged
    {
        AlarmableContext _ConfigContex;
        cultureType _cultureType = cultureType.en_US;
        public CentralConfigViewModel(AlarmableContext ctx, cultureType cultureType )
        {
             _cultureType = cultureType;
            _ConfigContex = ctx;
            FillCentralConfigs();
        }
        private void FillCentralConfigs()
        {
           // _centralConfigs
            CentralConfigs= _ConfigContex.occConfig
            .Where(c => c.Fk_AlarmableObjId == _selectedAlarmableObjId)
                .Include(c => c.OccCultureInfoes)
            .Select(P => new centralConfig()
            {
                OccCulture = P.OccCultureInfoes
                .FirstOrDefault(i => i.Culture == _cultureType.Value),
                OccConfig = P
            })
            .ToList();
        }
        private IwnTagType _selectedAlarmableObjId = 1;
        public IwnTagType SelectedAlarmableObjId
        {
            get
            {
                return _selectedAlarmableObjId;
            }
            set
            {
                _selectedAlarmableObjId = value;
                NotifyPropertyChanged();
                FillCentralConfigs();
            }
        }
        private List<centralConfig> _centralConfigs;
        public List<centralConfig> CentralConfigs
        {
            get
            {
                return _centralConfigs;
            }
            set
            {
                _centralConfigs = value;
                NotifyPropertyChanged();
            }
        }
        private centralConfig _selectedOccConfig;
        public centralConfig SelectedOccConfig
        {
            get
            {
                return _selectedOccConfig;
            }
            set
            {
                _selectedOccConfig = value;
                NotifyPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}
