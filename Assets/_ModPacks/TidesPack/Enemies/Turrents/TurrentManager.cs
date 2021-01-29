using CAF.Simulation;
using System;
using System.Collections;
using System.Collections.Generic;
using TUF;
using TUF.Combat;
using TUF.Combat.Danmaku;
using TUF.Entities;
using UnityEngine;

namespace TidesPack
{
    public class TurrentManager : SimObject
    {
        [SerializeField] private DetectionArea detectionArea;
        [SerializeField] private DanmakuManager danmakuManager;


        public EntityManager target;

        public DanmakuSequence danmaku;
        public DanmakuConfig danmakuConfig;
        
        public int countdown;
        private int timer = 100;

        public EntityTeams team;
        public HitInfo hitInfo;

        public override void SimStart()
        {
            base.SimStart();
            timer = countdown;
        }

        public override void SimUpdate()
        {
            if (timer > 0)
            {
                timer--;
            }

            if (detectionArea.entities.Count > 0)
            {
                target = GetClosestTarget();

                EntityPhysicsManager epm = (EntityPhysicsManager)target.PhysicsManager;

                //transform.LookAt(target.transform.position + Vector3.up + epm.forceMovement);
                transform.LookAt(PredictPosition(target.gameObject) + Vector3.up);

                if (timer <= 0)
                {
                    danmakuConfig.position = transform.position;
                    danmakuConfig.rotation = transform.eulerAngles;
                    danmakuManager.Fire(danmaku, danmakuConfig, team, hitInfo);
                    timer = countdown;
                }
            }

            danmakuManager.Tick();
        }

        private EntityManager GetClosestTarget()
        {
            EntityManager closestGO = detectionArea.entities[0];
            float closestDistance = Vector3.Distance(transform.position, closestGO.transform.position);

            for(int i = 0; i < detectionArea.entities.Count; i++)
            {
                float distance = Vector3.Distance(transform.position, detectionArea.entities[i].transform.position);
                if (distance < closestDistance)
                {
                    closestGO = detectionArea.entities[i];
                    closestDistance = distance;
                }
            }

            return closestGO;
        }

        public float tem = 0.2f;
        private Vector3 PredictPosition(GameObject target)
        {
            Vector3 velocity = target.GetComponent<EntityCharacterController>().Motor.Velocity;
            //float time = Vector3.Distance(transform.position, target.transform.position) / (danmakuConfig.speed.z.GetValue() * Time.smoothDeltaTime);
            //Vector3 coef = velocity * time;
            Vector3 newTarget = target.transform.position + (velocity * tem);
            return newTarget;
        }
    }
}