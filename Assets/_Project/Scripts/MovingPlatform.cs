using CAF.Simulation;
using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using static iTween;

namespace TUF
{
    public class MovingPlatform : SimObject, IMoverController
    {
        public PhysicsMover Mover;
        public PlayableDirector Director;
        public bool autoActivate;

        private bool pathActivated;
        private int timer = 0;

        protected override void Start()
        {
            base.Start();
            Mover.MoverController = this;

            if (autoActivate)
            {
                StartPath();
            }

        }

        public override void SimUpdate()
        {
            base.SimUpdate();

            if (pathActivated)
            {
                timer++;
            }
        }

        public void StartPath()
        {
            pathActivated = true;
        }

        public void PausePath()
        {
            pathActivated = false;
        }

        public void ResumePath()
        {
            pathActivated = true;
        }

        protected void PathFinished()
        {

        }

        public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime)
        {
            // Remember pose before animation
            Vector3 _positionBeforeAnim = transform.position;
            Quaternion _rotationBeforeAnim = transform.rotation;

            // Update animation
            EvaluateAtTime(timer * Time.fixedDeltaTime);

            // Set our platform's goal pose to the animation's
            goalPosition = transform.position;
            goalRotation = transform.rotation;

            // Reset the actual transform pose to where it was before evaluating. 
            // This is so that the real movement can be handled by the physics mover; not the animation
            transform.position = _positionBeforeAnim;
            transform.rotation = _rotationBeforeAnim;
        }

        public void EvaluateAtTime(double time)
        {
            Director.time = time % Director.duration;
            Director.Evaluate();
        }
    }
}