using AlarmBase.DomainModel.generics;
using System;

namespace AlarmBase.DomainModel
{
    public abstract class IntThreshold : Alarm<int>
    {
        public IntThreshold(int _objId) : base(_objId)
        {
        }

        public TresholdKind tresholdKind { get; set; }
      
        public override AlarmState Check(int NewState, int PreState)
        {
          
            AlarmState _state = state;
            //DO ALL THE HEAVY LIFTING!!!
            if (NewState != PreState)
            {
                switch (tresholdKind)
                {
                    case TresholdKind.lower:
                        {
                            _state = NewState < setpoint ? AlarmState.set :
                                 NewState > setpoint * (1 + HysterisisOffset / 100) ? AlarmState.clear : state;
                            break;
                        }
                    case TresholdKind.upper:
                        {
                            _state = NewState > setpoint ? AlarmState.set :
                            NewState < setpoint * (1 - HysterisisOffset / 100) ? AlarmState.clear : state;
                            break;
                        }
                }
            }
      

            return _state;           
        }      

   
        public override string StateTypeToString(int st)
        {
           return st.ToString();
        }    

       

    }
    public  enum TresholdKind
    {upper,lower}
}