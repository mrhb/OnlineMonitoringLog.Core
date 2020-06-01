using System.ComponentModel;
using AlarmBase.DomainModel.Entities;
using IwnTagType = System.Int32;
namespace AlarmBase.DomainModel.generics
{
    public interface IOccurenceConfige
    {
        string AvailableParams { get; }
        int ConfigId { get; }
        IwnTagType ObjId { get; }
        string CultureTemplate { get; set; }
        int HysterisisOffset { get; set; }
        bool IsAlarm { get; }
        string ObjName { get; }
        int OccSeverity { get; set; }
        int OffDelay { get; set; }
        int OnDelay { get; set; }
        string SerializedSetPoint { get; set; }
        string SetPointType { get; set; }
        event PropertyChangedEventHandler PropertyChanged;
        bool SetConfig(RegisteredOccConfig OccConfig);
        bool Initialization(RegisteredOccConfig OccConfig, OccCultureInfo cultureInfo);
        bool SetPointSet(object setpoint);
    }
}