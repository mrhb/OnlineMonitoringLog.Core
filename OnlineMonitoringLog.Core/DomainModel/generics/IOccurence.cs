using System.Threading;

namespace AlarmBase.DomainModel.generics
{
    public interface IOccurence: IOccurenceConfige
    {
        string GetSetPointType { get; }
        string ClearValue { get; }
        string SetPoint { get; }
        string SetValue { get; }

        /// <summary>
        /// return delay based on OffDelay or OnDelay
        /// </summary>
        /// <returns></returns>
        int delayTime { get; }
        AlarmState State { get; }
        CancellationToken Token { get; }
        string Message { get; }
        string DefaultMessage { get; }
    }
}