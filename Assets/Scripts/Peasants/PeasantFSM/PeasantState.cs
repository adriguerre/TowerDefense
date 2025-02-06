using UnityEngine;

namespace Peasants.PeasantFSM
{
    public abstract class PeasantState
    {
        protected Peasant _peasant;
        
        protected PeasantState(Peasant peasant)
        {
            this._peasant = peasant;
        }
        

        public void SetReference(Peasant newStatus)
        {
            this._peasant = newStatus;
        }

        public abstract void Execute();
    }
}