using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF
{
    public class Ledge : MonoBehaviour
    {
        public List<BezierSolution.BezierSpline> ledges = new List<BezierSolution.BezierSpline>();

        public Vector3 FindClosestLedge(Vector3 entityPosition)
        {
            Vector3 closest = ledges[0].FindNearestPointTo(entityPosition);

            Vector3 temp;
            for(int i = 0; i < ledges.Count; i++)
            {
                temp = ledges[i].FindNearestPointTo(entityPosition);
                if(Vector3.Distance(entityPosition, temp) <= Vector3.Distance(entityPosition, closest))
                {
                    closest = temp;
                }
            }

            return closest;
        }
    }
}