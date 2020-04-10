using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Helper
{
    public class MyFloat 
    {
        public static float FloatLerp(float initialValue, float finalValue, float speed)
        {
            if (initialValue > finalValue) speed = -speed;

            float r = initialValue;
            r += speed;
            return Mathf.Clamp(r, Mathf.Min(initialValue, finalValue), Mathf.Max(initialValue, finalValue));
        }

        public static float Round01Tollerence(float f, float tolerance)
        {
            if (f > 0)
            {
                if (f > tolerance) return 1;
                else return 0;
            }
            else
            {
                if (f < -1f * tolerance) return -1;
                else return 0;
            }
        }
    }
}