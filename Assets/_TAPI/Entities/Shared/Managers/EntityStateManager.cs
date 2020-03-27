using System.Collections;
using System.Collections.Generic;
using TAPI.Core;
using UnityEngine;

namespace TAPI.Entities
{
    public class EntityStateManager : MonoBehaviour
    {
        public BaseState CurrentState { get { return currentState; } }
        public uint CurrentStateFrame { get { return currentStateFrame; } }

        [SerializeField] private EntityController controller;
        protected Dictionary<int, BaseState> states = new Dictionary<int, BaseState>();
        protected BaseState currentState;
        [SerializeField] protected uint currentStateFrame = 0;
        [SerializeField] protected string currentStateName;

        public virtual void Tick()
        {
            if (currentState != null)
            {
                currentState.OnUpdate();
            }
        }

        /// <summary>
        /// Adds a state to the entity's state list.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="stateName">The number of the state.</param>
        public virtual void AddState(EntityState state, int stateNumber)
        {
            state.controller = controller;
            states.Add(stateNumber, state);
        }

        public virtual void ChangeState(int stateNumber, uint StateTime = 0, bool CallOnInterupt = true)
        {
            if (states.ContainsKey(stateNumber))
            {
                if (CallOnInterupt)
                {
                    if (currentState != null)
                    {
                        currentState.OnInterrupted();
                    }
                }
                currentStateFrame = StateTime;
                currentState = states[stateNumber];
                if (currentStateFrame == 0)
                {
                    currentState.OnStart();
                }
                currentStateName = currentState.GetName();
            }
        }

        public virtual void ChangeState(EntityState State, uint StateTime = 0, bool CallOnInterupt = true)
        {
            currentStateFrame = StateTime;
            if (CallOnInterupt)
            {
                currentState.OnInterrupted();
            }

            currentState = State;
            State.controller = controller;
            if (currentStateFrame == 0)
            {
                currentState.OnStart();
            }
        }

        public virtual void SetFrame(uint frame)
        {
            currentStateFrame = frame;
        }

        public virtual void IncrementFrame()
        {
            currentStateFrame++;
        }
    }
}