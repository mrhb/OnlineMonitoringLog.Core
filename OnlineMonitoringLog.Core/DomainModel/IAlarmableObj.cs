namespace AlarmBase.DomainModel
{
    public interface IAlarmableObj
    {
        int ObjId { get; }
        string ObjName { get; }
    }
}