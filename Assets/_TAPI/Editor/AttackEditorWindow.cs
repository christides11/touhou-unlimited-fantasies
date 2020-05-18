using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TAPI.Combat;
using System;

namespace TAPI.Combat
{
    public class AttackEditorWindow : EditorWindow
    {
        private int currentMenu;
        private AttackSO attack;

        public static void Init(AttackSO attack)
        {
            AttackEditorWindow window = (AttackEditorWindow)EditorWindow.GetWindow(typeof(AttackEditorWindow));
            window.attack = attack;
            window.Show();
        }

        void OnGUI()
        {
            attack = (AttackSO)EditorGUILayout.ObjectField("Attack", attack, typeof(AttackSO), false);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("General"))
            {
                currentMenu = 0;
            }
            if (GUILayout.Button("Meter"))
            {
                currentMenu = 1;
            }
            if (GUILayout.Button("Cancels"))
            {
                currentMenu = 2;
            }
            if (GUILayout.Button("Hitboxes"))
            {
                currentMenu = 3;
            }
            if (GUILayout.Button("Projectiles"))
            {
                currentMenu = 6;
            }
            if (GUILayout.Button("Events"))
            {
                currentMenu = 4;
            }
            if (GUILayout.Button("Hurtboxes"))
            {
                currentMenu = 5;
            }
            EditorGUILayout.EndHorizontal();

            if (!attack)
            {
                return;
            }

            switch (currentMenu)
            {
                case 0:
                    GeneralMenu();
                    break;
                case 1:
                    MeterMenu();
                    break;
                case 2:
                    CancelsMenu();
                    break;
                case 3:
                    HitboxesMenu();
                    break;
                case 4:
                    EventsMenu();
                    break;
                case 5:
                    HurtboxesMenu();
                    break;
                case 6:
                    ProjectilesMenu();
                    break;
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(attack);
            }
        }

        private void ProjectilesMenu()
        {

        }

        bool chargeFramesDropdown;
        bool followTargetDropdown;
        void GeneralMenu()
        {
            EditorGUI.BeginChangeCheck();
            string attackName = EditorGUILayout.TextField("Name", attack.attackName);
            EditorGUILayout.LabelField("Description");
            string description = EditorGUILayout.TextArea(attack.description);

            int stateOverride = EditorGUILayout.IntField("State Override", attack.stateOverride);
            int attackLength = EditorGUILayout.IntField("Length", attack.length);
            bool groundAble = EditorGUILayout.Toggle("Ground Able", attack.groundAble);
            bool airAble = EditorGUILayout.Toggle("Air Able", attack.airAble);
            AnimationClip animationGround = attack.animationGround;
            AnimationClip animationAir = attack.animationAir;
            float heightRestriction = attack.heightRestriction;
            UnityEngine.WrapMode wrapMode = attack.wrapMode;
            if (groundAble)
            {
                animationGround = (AnimationClip)EditorGUILayout.ObjectField("Animation (Ground)", attack.animationGround,
                    typeof(AnimationClip), false);
            }
            if (airAble) {
                animationAir = (AnimationClip)EditorGUILayout.ObjectField("Animation (Aerial)", attack.animationAir,
                    typeof(AnimationClip), false);
                heightRestriction = EditorGUILayout.FloatField("Height Restriction", attack.heightRestriction);
            }
            if(groundAble || airAble)
            {
                wrapMode = (UnityEngine.WrapMode)EditorGUILayout.EnumPopup("Wrap Mode", attack.wrapMode);
            }
            float gravityScale = EditorGUILayout.FloatField("Gravity Scale Added", attack.gravityScaleAdded);

            bool modifiesInertia = EditorGUILayout.Toggle("Modifies Inertia", attack.modifiesInertia);
            float inertiaModifier = attack.inertiaModifer;
            if (modifiesInertia)
            {
                inertiaModifier = EditorGUILayout.FloatField("Modifier", inertiaModifier);
            }
            bool carriesInertia = EditorGUILayout.Toggle("Carries Inertia", attack.carriesInertia);
            float carriedInertia = attack.carriedInertia;
            if (carriesInertia)
            {
                carriedInertia = EditorGUILayout.FloatField("Carried Inertia", carriedInertia);
            }

            int chargeLength = attack.chargeLength;
            chargeLength = EditorGUILayout.IntField("Charge Length", chargeLength);

            List<int> chargeFrames = attack.chargeFrames;
            EditorGUILayout.BeginHorizontal();
            chargeFramesDropdown = EditorGUILayout.Foldout(chargeFramesDropdown, "Charge Frames", true);
            if (GUILayout.Button("Add"))
            {
                chargeFrames.Add(0);
            }
            EditorGUILayout.EndHorizontal();
            if (chargeFramesDropdown)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < chargeFrames.Count; i++)
                {
                    chargeFrames[i] = EditorGUILayout.IntField("Frame: ", chargeFrames[i]);
                }
                EditorGUI.indentLevel--;
            }

            // Lockon Target Following
            List<AttackFaceLockonWindow> followWindows = new List<AttackFaceLockonWindow>(attack.faceLockonTargetWindows);
            EditorGUILayout.BeginHorizontal();
            followTargetDropdown = EditorGUILayout.Foldout(followTargetDropdown, "Follow Target Windows", true);
            if (GUILayout.Button("Add"))
            {
                followWindows.Add(new AttackFaceLockonWindow());
            }
            EditorGUILayout.EndHorizontal();
            if (followTargetDropdown)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < followWindows.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("X", GUILayout.Width(20)))
                    {
                        followWindows.RemoveAt(i);
                        continue;
                    }
                    EditorGUILayout.LabelField($"{i}.");
                    EditorGUILayout.EndHorizontal();
                    followWindows[i].startFrame = EditorGUILayout.IntField("Start: ", followWindows[i].startFrame);
                    followWindows[i].endFrame = EditorGUILayout.IntField("End: ", followWindows[i].endFrame);
                    followWindows[i].amount = EditorGUILayout.FloatField("Amount: ", followWindows[i].amount);
                    EditorGUILayout.Space();
                }
                EditorGUI.indentLevel--;
            }


            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(attack, "Changed General Property.");
                attack.attackName = attackName;
                attack.description = description;
                attack.stateOverride = stateOverride;
                attack.length = attackLength;
                attack.groundAble = groundAble;
                attack.airAble = airAble;
                attack.animationGround = animationGround;
                attack.animationAir = animationAir;
                attack.heightRestriction = heightRestriction;
                attack.wrapMode = wrapMode;
                attack.modifiesInertia = modifiesInertia;
                attack.inertiaModifer = inertiaModifier;
                attack.carriesInertia = carriesInertia;
                attack.carriedInertia = carriedInertia;
                attack.chargeFrames = chargeFrames;
                attack.chargeLength = chargeLength;
                attack.faceLockonTargetWindows = followWindows;
                attack.gravityScaleAdded = gravityScale;
                GUI.changed = false;
            }
        }

        private void MeterMenu()
        {

        }

        bool jumpCancelFoldout;
        bool landCancelFoldout;
        bool dashCancelFoldout;
        bool attackCancelFoldout;
        bool bulletCancelFoldout;
        bool specialCancelFoldout;
        private void CancelsMenu()
        {
            // Jump Cancel
            EditorGUILayout.BeginHorizontal(GUILayout.Width(300));
            jumpCancelFoldout = EditorGUILayout.Foldout(jumpCancelFoldout, "Jump Cancel Windows", true);
            if (GUILayout.Button("Add"))
            {
                attack.jumpCancelFrames.Add(new Vector2Int());
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5);
            if (jumpCancelFoldout)
            {
                CancelWindowFoldout(ref attack.jumpCancelFrames);
            }
            // Land Cancel
            EditorGUILayout.BeginHorizontal(GUILayout.Width(300));
            landCancelFoldout = EditorGUILayout.Foldout(landCancelFoldout, "Land Cancel Windows", true);
            if (GUILayout.Button("Add"))
            {
                attack.landCancelFrames.Add(new Vector2Int());
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5);
            if (landCancelFoldout)
            {
                CancelWindowFoldout(ref attack.landCancelFrames);
            }
            // Dash Cancel
            EditorGUILayout.BeginHorizontal(GUILayout.Width(300));
            dashCancelFoldout = EditorGUILayout.Foldout(dashCancelFoldout, "Dash Cancel Windows", true);
            if (GUILayout.Button("Add"))
            {
                attack.dashCancelableFrames.Add(new Vector2Int());
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5);
            if (dashCancelFoldout)
            {
                CancelWindowFoldout(ref attack.dashCancelableFrames);
            }
            // Attack Cancel
            EditorGUILayout.BeginHorizontal(GUILayout.Width(300));
            attackCancelFoldout = EditorGUILayout.Foldout(attackCancelFoldout, "Attack Cancel Windows", true);
            if (GUILayout.Button("Add"))
            {
                attack.attackCancelFrames.Add(new Vector2Int());
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5);
            if (attackCancelFoldout)
            {
                CancelWindowFoldout(ref attack.attackCancelFrames);
            }
            // Bullet Cancel
            EditorGUILayout.BeginHorizontal(GUILayout.Width(300));
            bulletCancelFoldout = EditorGUILayout.Foldout(bulletCancelFoldout, "Bullet Cancel Windows", true);
            if (GUILayout.Button("Add"))
            {
                attack.bulletCancelFrames.Add(new Vector2Int());
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5);
            if (bulletCancelFoldout)
            {
                CancelWindowFoldout(ref attack.bulletCancelFrames);
            }
            // Special Cancel
            EditorGUILayout.BeginHorizontal(GUILayout.Width(300));
            specialCancelFoldout = EditorGUILayout.Foldout(specialCancelFoldout, "Special Cancel Windows", true);
            if (GUILayout.Button("Add"))
            {
                attack.specialCancelFrames.Add(new Vector2Int());
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5);
            if (specialCancelFoldout)
            {
                CancelWindowFoldout(ref attack.specialCancelFrames);
            }
        }

        private void CancelWindowFoldout(ref List<Vector2Int> list)
        {
            EditorGUI.indentLevel++;
            for (int i = 0; i < list.Count; i++)
            {
                EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(400));
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    list.RemoveAt(i);
                    continue;
                }
                EditorGUILayout.LabelField($"Window {i}", GUILayout.MaxWidth(80));
                int start = EditorGUILayout.IntField(list[i].x, GUILayout.MaxWidth(50));
                int end = EditorGUILayout.IntField(list[i].y, GUILayout.MaxWidth(50));
                EditorGUILayout.EndHorizontal();
                list[i] = new Vector2Int(start, end);
            }
            EditorGUI.indentLevel--;
        }

        int currentHitboxGroupIndex;
        HitboxGroup copyCurrent = null;
        private void HitboxesMenu()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("<", GUILayout.Width(40)))
            {
                currentHitboxGroupIndex--;
                if(currentHitboxGroupIndex < 0)
                {
                    currentHitboxGroupIndex = Mathf.Clamp(attack.hitboxGroups.Count-1, 0, attack.hitboxGroups.Count);
                }
            }
            EditorGUILayout.LabelField($"{currentHitboxGroupIndex+1}/{attack.hitboxGroups.Count}", GUILayout.Width(50));
            if (GUILayout.Button(">", GUILayout.Width(40)))
            {
                currentHitboxGroupIndex++;
                if(currentHitboxGroupIndex >= attack.hitboxGroups.Count)
                {
                    currentHitboxGroupIndex = 0;
                }
            }
            GUILayout.Space(10);
            if (GUILayout.Button("Add", GUILayout.Width(50)))
            {
                attack.hitboxGroups.Add(new HitboxGroup());
            }
            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                attack.hitboxGroups.RemoveAt(currentHitboxGroupIndex);
                currentHitboxGroupIndex--;
            }
            if(GUILayout.Button("Copy", GUILayout.Width(60)))
            {
                copyCurrent = null;
                if (attack.hitboxGroups.Count > 0)
                {
                    copyCurrent = new HitboxGroup(attack.hitboxGroups[currentHitboxGroupIndex]);
                }
            }
            if(GUILayout.Button("Paste", GUILayout.Width(60)))
            {
                if(attack.hitboxGroups.Count > 0 && copyCurrent != null)
                {
                    attack.hitboxGroups[currentHitboxGroupIndex] = new HitboxGroup(copyCurrent);
                }
                copyCurrent = null;
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(30);
            if(attack.hitboxGroups.Count == 0)
            {
                return;
            }
            HitboxGroup currentGroup = attack.hitboxGroups[currentHitboxGroupIndex];
            DrawHitboxGroup(currentGroup);


        }

        bool hitboxesFoldout;
        private void DrawHitboxGroup(HitboxGroup currentGroup)
        {
            EditorGUILayout.LabelField("GENERAL");
            currentGroup.ID = EditorGUILayout.IntField("ID", currentGroup.ID);
            float activeFramesStart = currentGroup.activeFramesStart;
            float activeFramesEnd = currentGroup.activeFramesEnd;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(currentGroup.activeFramesStart.ToString(), GUILayout.Width(30));
            EditorGUILayout.MinMaxSlider(ref activeFramesStart,
                ref activeFramesEnd, 0, (float)attack.length);
            EditorGUILayout.LabelField(currentGroup.activeFramesEnd.ToString(), GUILayout.Width(30));
            EditorGUILayout.EndHorizontal();
            currentGroup.activeFramesStart = (int)activeFramesStart;
            currentGroup.activeFramesEnd = (int)activeFramesEnd;

            currentGroup.hitGroupType = (HitboxGroupType)EditorGUILayout.EnumPopup("Hit Type", currentGroup.hitGroupType);
            currentGroup.attachToEntity = EditorGUILayout.Toggle("Attach to Entity", currentGroup.attachToEntity);

            EditorGUILayout.BeginHorizontal(GUILayout.Width(300));
            hitboxesFoldout = EditorGUILayout.Foldout(hitboxesFoldout, "Hitboxes");
            if (GUILayout.Button("Add"))
            {
                currentGroup.hitboxes.Add(new HitboxDefinition());
            }
            EditorGUILayout.EndHorizontal();
            if (hitboxesFoldout)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < currentGroup.hitboxes.Count; i++)
                {
                    DrawHitboxOptions(currentGroup, i);
                    GUILayout.Space(5);
                }
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.Space(10);

            switch (currentGroup.hitGroupType)
            {
                case HitboxGroupType.HIT:
                    DrawHitboxGroupHitOptions(currentGroup);
                    break;
                case HitboxGroupType.GRAB:
                    DrawHitboxGroupGrabOptions(currentGroup);
                    break;
            }
        }

        private void DrawHitboxGroupGrabOptions(HitboxGroup currentGroup)
        {
            currentGroup.throwConfirm = (AttackSO)EditorGUILayout.ObjectField("Throw Confirm Move", currentGroup.throwConfirm,
                typeof(AttackSO), false);
        }

        private void DrawHitboxGroupHitOptions(HitboxGroup currentGroup)
        {
            EditorGUILayout.LabelField("EFFECT");
            currentGroup.hitInfo.groundOnly = EditorGUILayout.Toggle("Hit Ground Only?", currentGroup.hitInfo.groundOnly);
            currentGroup.hitInfo.airOnly = EditorGUILayout.Toggle("Hit Air Only?", currentGroup.hitInfo.airOnly);
            currentGroup.hitInfo.unblockable = EditorGUILayout.Toggle("Unblockable?", currentGroup.hitInfo.unblockable);
            currentGroup.hitInfo.breakArmor = EditorGUILayout.Toggle("Breaks Armor?", currentGroup.hitInfo.breakArmor);
            currentGroup.hitInfo.groundBounces = EditorGUILayout.Toggle("Ground Bounces?", currentGroup.hitInfo.groundBounces);
            currentGroup.hitInfo.wallBounces = EditorGUILayout.Toggle("Wall Bounces?", currentGroup.hitInfo.wallBounces);
            currentGroup.hitInfo.causesTumble = EditorGUILayout.Toggle("Causes Tumble?", currentGroup.hitInfo.causesTumble);
            currentGroup.hitInfo.knockdown = EditorGUILayout.Toggle("Causes Knockdown?", currentGroup.hitInfo.knockdown);
            currentGroup.hitInfo.continuousHit = EditorGUILayout.Toggle("Continuous Hit?", currentGroup.hitInfo.continuousHit);
            if (currentGroup.hitInfo.continuousHit)
            {
                currentGroup.hitInfo.spaceBetweenHits = EditorGUILayout.IntField("Space between hits", currentGroup.hitInfo.spaceBetweenHits);
            }
            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField("DAMAGE");
            currentGroup.hitInfo.damageOnBlock = EditorGUILayout.FloatField("Damage (Block)", currentGroup.hitInfo.damageOnBlock);
            currentGroup.hitInfo.damageOnHit = EditorGUILayout.FloatField("Damage (Hit)", currentGroup.hitInfo.damageOnHit);
            currentGroup.hitInfo.hitKills = EditorGUILayout.Toggle("Hit Kills", currentGroup.hitInfo.hitKills);
            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField("FORCES");
            currentGroup.hitInfo.opponentResetXForce = EditorGUILayout.Toggle("Reset X Force", currentGroup.hitInfo.opponentResetXForce);
            currentGroup.hitInfo.opponentResetYForce = EditorGUILayout.Toggle("Reset Y Force", currentGroup.hitInfo.opponentResetYForce);
            currentGroup.hitInfo.forceRelation = (HitForceRelation)EditorGUILayout.EnumPopup("Force Relation", currentGroup.hitInfo.forceRelation);
            currentGroup.hitInfo.forceType = (HitForceType)EditorGUILayout.EnumPopup("Force Type", currentGroup.hitInfo.forceType);
            switch (currentGroup.hitInfo.forceType)
            {
                case HitForceType.SET:
                    currentGroup.hitInfo.opponentForceDir = EditorGUILayout.Vector3Field("Force Direction", currentGroup.hitInfo.opponentForceDir);
                    if (GUILayout.Button("Normalize"))
                    {
                        currentGroup.hitInfo.opponentForceDir.Normalize();
                    }
                    currentGroup.hitInfo.opponentForceMagnitude = EditorGUILayout.FloatField("Force Magnitude", currentGroup.hitInfo.opponentForceMagnitude);
                    break;
                case HitForceType.PUSH:
                    currentGroup.hitInfo.forceIncludeYForce = EditorGUILayout.Toggle("Include Y Force", currentGroup.hitInfo.forceIncludeYForce);
                    currentGroup.hitInfo.opponentForceMagnitude
                        = EditorGUILayout.FloatField("Force Multiplier", currentGroup.hitInfo.opponentForceMagnitude);
                    break;
                case HitForceType.PULL:
                    currentGroup.hitInfo.forceIncludeYForce = EditorGUILayout.Toggle("Include Y Force", currentGroup.hitInfo.forceIncludeYForce);
                    currentGroup.hitInfo.opponentForceMagnitude 
                        = EditorGUILayout.FloatField("Force Multiplier", currentGroup.hitInfo.opponentForceMagnitude);
                    currentGroup.hitInfo.opponentMaxMagnitude
                        = EditorGUILayout.FloatField("Max Magnitude", currentGroup.hitInfo.opponentMaxMagnitude);
                    break;
            }

            if (currentGroup.hitInfo.wallBounces)
            {
                currentGroup.hitInfo.wallBounceForce = EditorGUILayout.FloatField("Wall Bounce Magnitude", currentGroup.hitInfo.wallBounceForce);
            }
            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField("STUN");
            currentGroup.hitInfo.attackerHitstop = (ushort)EditorGUILayout.IntField("Hitstop (Attacker)", 
                currentGroup.hitInfo.attackerHitstop);
            currentGroup.hitInfo.hitstop = (ushort)EditorGUILayout.IntField("Hitstop", currentGroup.hitInfo.hitstop);
            currentGroup.hitInfo.hitstun = (ushort)EditorGUILayout.IntField("Hitstun", currentGroup.hitInfo.hitstun);
        }

        private void DrawHitboxOptions(HitboxGroup currentGroup, int index)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                currentGroup.hitboxes.RemoveAt(index);
                return;
            }
            GUILayout.Label($"Group {index}");
            EditorGUILayout.EndHorizontal();
            currentGroup.hitboxes[index].shape = (ShapeType)EditorGUILayout.EnumPopup("Shape", currentGroup.hitboxes[index].shape);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Offset", GUILayout.Width(135));
            currentGroup.hitboxes[index].offset.x = EditorGUILayout.FloatField(currentGroup.hitboxes[index].offset.x, GUILayout.Width(40));
            currentGroup.hitboxes[index].offset.y = EditorGUILayout.FloatField(currentGroup.hitboxes[index].offset.y, GUILayout.Width(40));
            currentGroup.hitboxes[index].offset.z = EditorGUILayout.FloatField(currentGroup.hitboxes[index].offset.z, GUILayout.Width(40));
            EditorGUILayout.EndHorizontal();
            switch (currentGroup.hitboxes[index].shape)
            {
                case ShapeType.Rectangle:
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Size", GUILayout.Width(135));
                    currentGroup.hitboxes[index].size.x = EditorGUILayout.FloatField(currentGroup.hitboxes[index].size.x, GUILayout.Width(40));
                    currentGroup.hitboxes[index].size.y = EditorGUILayout.FloatField(currentGroup.hitboxes[index].size.y, GUILayout.Width(40));
                    currentGroup.hitboxes[index].size.z = EditorGUILayout.FloatField(currentGroup.hitboxes[index].size.z, GUILayout.Width(40));
                    EditorGUILayout.EndHorizontal();
                    break;
                case ShapeType.Circle:
                    currentGroup.hitboxes[index].radius
                        = EditorGUILayout.FloatField("Radius", currentGroup.hitboxes[index].radius);
                    break;
            }
        }

        private void HurtboxesMenu()
        {
        }

        int eventSelected = -1;
        private void EventsMenu()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add", GUILayout.Width(100)))
            {
                attack.events.Add(new AttackEventDefinition());
            }
            GUILayout.Space(20);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical(GUILayout.MaxWidth(110));
            for(int i = 0; i < attack.events.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                if(GUILayout.Button("X", GUILayout.Width(15)))
                {
                    attack.events.RemoveAt(i);
                    continue;
                }
                if (GUILayout.Button($"{attack.events[i].nickname}",
                    GUILayout.Height(25), GUILayout.Width(95)))
                {
                    eventSelected = eventSelected == i ? -1 : i;
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

            if(eventSelected == -1)
            {
                EditorGUILayout.BeginVertical();
                for(int i = 0; i < attack.events.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(attack.events[i].startFrame.ToString(), GUILayout.Width(20));
                    float eventStart = attack.events[i].startFrame;
                    float eventEnd = attack.events[i].endFrame;
                    EditorGUILayout.MinMaxSlider(ref eventStart, ref eventEnd, 1, attack.length, GUILayout.Height(25));
                    attack.events[i].startFrame = (uint)eventStart;
                    attack.events[i].endFrame = (uint)eventEnd;
                    EditorGUILayout.LabelField(attack.events[i].endFrame.ToString(), GUILayout.Width(20));
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
            }
            else
            {
                EditorGUILayout.BeginVertical();
                ShowEventInfo(eventSelected);
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }

        bool eventVariablesFoldout;
        private void ShowEventInfo(int eventSelected)
        {
            if(attack.events[eventSelected] == null)
            {
                return;
            }
            attack.events[eventSelected].nickname = EditorGUILayout.TextField("Name", attack.events[eventSelected].nickname);
            attack.events[eventSelected].active = EditorGUILayout.Toggle("Active", attack.events[eventSelected].active);
            attack.events[eventSelected].onHit = EditorGUILayout.Toggle("On Hit?", attack.events[eventSelected].onHit);
            if (attack.events[eventSelected].onHit)
            {
                attack.events[eventSelected].onHitHitboxGroup = EditorGUILayout.IntField("Hitbox Group",
                    attack.events[eventSelected].onHitHitboxGroup);
            }
            attack.events[eventSelected].onDetect = EditorGUILayout.Toggle("On Detect?", attack.events[eventSelected].onDetect);
            if (attack.events[eventSelected].onDetect)
            {
                attack.events[eventSelected].onDetectHitboxGroup = EditorGUILayout.IntField("Detect Group", 
                    attack.events[eventSelected].onDetectHitboxGroup);
            }
            attack.events[eventSelected].attackEvent = (AttackEvent)EditorGUILayout.ObjectField("Event", 
                attack.events[eventSelected].attackEvent,
                typeof(AttackEvent), false);
            eventVariablesFoldout = EditorGUILayout.Foldout(eventVariablesFoldout, "Variables");
            if (eventVariablesFoldout)
            {
                EditorGUI.indentLevel++;
                if (attack.events[eventSelected].attackEvent)
                {
                    attack.events[eventSelected].attackEvent.DrawEventVariables(attack.events[eventSelected]);
                }
                EditorGUI.indentLevel--;
            }
        }
    }
}