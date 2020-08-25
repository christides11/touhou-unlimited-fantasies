using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using UnityEngine;

namespace TUF.Entities
{
    public class EntityStateManager : CAF.Entities.EntityStateManager
    {
        /// <summary>
        /// Changes the state to the given one.
        /// </summary>
        /// <param name="state">The state to change to.</param>
        /// <param name="stateFrame">What frame to start the state at.</param>
        /// <param name="callOnInterrupt">If OnInterrupt of the current state should be called.</param>
        public virtual void ChangeState(EntityState state, uint stateFrame = 0, bool callOnInterrupt = true)
        {
            currentStateFrame = stateFrame;
            if (callOnInterrupt)
            {
                currentState.OnInterrupted();
            }

            currentState = state;
            state.Manager = (EntityManager)manager;
            if (currentStateFrame == 0)
            {
                currentState.Initialize();
            }
        }
    }
}