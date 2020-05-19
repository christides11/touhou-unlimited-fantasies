
namespace TAPI.Core
{
    public class BaseState
    {
        public virtual int StateDuration { get; }

        public virtual void OnStart()
        {

        }
        public virtual void OnUpdate()
        {
            if (CheckInterrupt())
            {
                return;
            }
        }
        public virtual bool CheckInterrupt()
        {
            return false;
        }
        public virtual void OnInterrupted()
        {

        }

        public virtual string GetName()
        {
            return "";
        }
    }
}