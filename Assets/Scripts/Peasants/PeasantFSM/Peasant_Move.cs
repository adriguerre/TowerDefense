using UnityEngine;

namespace Peasants.PeasantFSM
{
    public class Peasant_Move : PeasantState
    {
        
        private Vector2 targetPosition;
        private bool isRunningFromBuilding;
        public float speed = 0.4f;
        
        public Peasant_Move(Peasant building, Vector2 location, bool isRunningFromBuilding) : base(building)
        {
            _peasant.isBusy = true;
            targetPosition = location;
            //Activate movement animation
            this._peasant.StartMovingAnimation();
            
            this.isRunningFromBuilding = isRunningFromBuilding;
            //just in case it comes from building
            if(isRunningFromBuilding)
                this._peasant.StopBuildingAnimation();
        }

        public override void Execute()
        {
            if (Vector2.Distance(this._peasant.gameObject.transform.position, targetPosition) <= 0.2f)
            {
                if (isRunningFromBuilding)
                {
                    _peasant.isBusy = false;
                    _peasant.TransitionToState(new Peasant_Idle(this._peasant));
                }
                else
                    _peasant.TransitionToState(new Peasant_Building(this._peasant));
                
                _peasant.StopMovingAnimation();
            }
            else
            {
                //Move To location
                _peasant.transform.position = Vector2.MoveTowards(_peasant.transform.position, targetPosition, speed * Time.deltaTime);
            }
        }
    }
}