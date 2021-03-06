﻿using TUF.Inputs;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TUF.Core
{
    public static class Helpers
    {
        public static Vector2 RadianToVector2(float radian)
        {
            return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
        }

        public static Vector2 DegreeToVector2(float degree)
        {
            return RadianToVector2(degree * Mathf.Deg2Rad);
        }

        public static void RoundFloat(this float f0)
        {
            f0 = Mathf.Round(f0 * 100f) / 100f;
        }

        public static float Vector2ToDegree(this Vector2 v0)
        {
            return Mathf.Atan2(v0.y, v0.x) * (180.0f / Mathf.PI);
        }

        // https://answers.unity.com/questions/532297/rotate-a-vector-around-a-certain-point.html
        public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
        {
            return Quaternion.Euler(angles) * (point - pivot) + pivot;
        }

        public static Color ChangeHue(this Color co, float change)
        {
            float h, s, v;
            Color.RGBToHSV(co, out h, out s, out v);
            h += change;
            if (h > 1.0f)
            {
                h -= 1.0f;
            }
            return Color.HSVToRGB(h, s, v);
        }

        public static void AddListener(this EventTrigger trigger, EventTriggerType eventType, UnityAction<BaseEventData> call)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = eventType;
            entry.callback.AddListener(call);
            trigger.triggers.Add(entry);
        }

        public static void AddOnSelectedListeners(this EventTrigger trigger, UnityAction<BaseEventData> call)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Select;
            entry.callback.AddListener(call);
            trigger.triggers.Add(entry);

            EventTrigger.Entry entryT = new EventTrigger.Entry();
            entryT.eventID = EventTriggerType.PointerEnter;
            entryT.callback.AddListener(call);
            trigger.triggers.Add(entryT);
        }

        public static void AddOnSubmitListeners(this EventTrigger trigger, UnityAction<BaseEventData> call)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Submit;
            entry.callback.AddListener(call);
            trigger.triggers.Add(entry);

            EventTrigger.Entry entryT = new EventTrigger.Entry();
            entryT.eventID = EventTriggerType.PointerClick;
            entryT.callback.AddListener(call);
            trigger.triggers.Add(entryT);
        }

        public static void RemoveAllListeners(this EventTrigger trigger)
        {
            trigger.triggers.Clear();
        }

        public static void SelectDefaultSelection(GameObject defaultSelection)
        {
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                return;
            }

            if (GlobalInputManager.instance.GetAxis2D(0, "Horizontal", "Vertical").magnitude >= InputConstants.movementMagnitude)
            {
                EventSystem.current.SetSelectedGameObject(defaultSelection);
            }
        }

        public static bool Contains(this LayerMask mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }
    }
}