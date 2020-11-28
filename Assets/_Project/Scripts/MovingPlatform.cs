using CAF.Simulation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static iTween;

namespace TUF
{
    public class MovingPlatform : SimObject
    {
        [System.Serializable]
        public class PathPoint
        {
            public GameObject point;
            public int delay;
            public EaseType easeOutType;
            public float easeOutTime;
        }

        public enum WrapType
        {
            None, Loop, PingPong
        }

        public GameObject platform;

        public List<PathPoint> path = new List<PathPoint>();
        public WrapType wrapType;
        public bool autoActivate;

        private bool pathActivated;
        private int pathDirection = 1;
        private int currentPathPoint;
        private int currentPointWaitTimer;

        protected override void Start()
        {
            base.Start();
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
                // Wait at this position.
                if (currentPointWaitTimer < path[currentPathPoint].delay)
                {
                    currentPointWaitTimer++;
                    if(currentPointWaitTimer == path[currentPathPoint].delay)
                    {
                        // No point after this one.
                        int nextPoint = currentPathPoint + pathDirection;
                        if (nextPoint >= path.Count || nextPoint < 0)
                        {
                            PathFinished();
                            return;
                        }

                        iTween.MoveTo(platform, iTween.Hash("position", path[nextPoint].point.transform.position, 
                            "time", path[currentPathPoint].easeOutTime, 
                            "easetype", path[currentPathPoint].easeOutType));
                        StartCoroutine(SelectNextPoint(path[currentPathPoint].easeOutTime));
                    }
                    return;
                }
            }
        }

        public void StartPath(int startPoint = 0)
        {
            pathActivated = true;
            currentPathPoint = startPoint;
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
            switch (wrapType)
            {
                case WrapType.None:
                    break;
                case WrapType.Loop:
                    platform.transform.position = path[0].point.transform.position;
                    currentPathPoint = 0;
                    currentPointWaitTimer = 0;
                    break;
                case WrapType.PingPong:
                    pathDirection *= -1;
                    currentPointWaitTimer = 0;
                    break;
            }
        }

        protected IEnumerator SelectNextPoint(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            currentPathPoint += pathDirection;
            currentPointWaitTimer = 0;
        }
    }
}