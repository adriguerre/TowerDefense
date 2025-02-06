using System;
using Peasants.PeasantFSM;
using UnityEngine;

namespace Peasants
{
    public class Peasant : MonoBehaviour
    {
        private PeasantState _peasantState;
        private Animator _animator;

        private void Start()
        {
            TransitionToState(new Peasant_Idle(this));
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if(_peasantState != null)
                _peasantState.Execute();
        }
        
        public void TransitionToState(PeasantState newState)
        {
            if(_peasantState != null)
                Debug.Log("PEASANT FSM: TRANSITION FROM STATE: " + _peasantState.ToString() + " TO " + newState.ToString());
            this._peasantState = newState;
            this._peasantState.SetReference(this);
        }

        public void StartMovingAnimation() => _animator.SetBool("IsMoving", true);
        public void StopMovingAnimation() => _animator.SetBool("IsMoving", false);
        public void StartBuildingAnimation() => _animator.SetBool("IsBuilding", true);
        public void StopBuildingAnimation() => _animator.SetBool("IsBuilding", false);
        
       
    }
}