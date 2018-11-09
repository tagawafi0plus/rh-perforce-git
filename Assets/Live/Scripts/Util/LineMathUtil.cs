using UnityEngine;

namespace Live.Scripts.Util
{
    public class LineMathUtil
    {
        public float paramA = 0;
        public float paramB = 32;

        public float ratioY = 0.14f;

        public float ix = 0.0f;
        public float dx = 0.0f;

        public float iz = 775;
        public float dz = -17.5f;

        public LineMathUtil()
        {
        }

        public Vector3 GetFxPos(float ratio)
        {
            return GetFxPosToRef(ratio, new Vector3());
        }

        public Vector3 GetFxPosToRef(float ratio, Vector3 result)
        {
            var i = ratio * 100;
            var x = ix + i * dx;
            var z = iz + i * dz;
            var y = GetFx(i, paramA, paramB) * ratioY;

            result.x = x;
            result.y = y;
            result.z = z;
            return result;
        }

        public float GetFx(float x, float a, float b)
        {
            return -(x - a) * (x - b);
        }

        public LineMathUtil Clone()
        {
            var mathUtil = new LineMathUtil();

            mathUtil.paramA = paramA;
            mathUtil.paramB = paramB;

            mathUtil.ratioY = ratioY;

            mathUtil.ix = ix;
            mathUtil.dx = dx;
            mathUtil.ix = ix;
            mathUtil.dx = dx;

            mathUtil.iz = iz;
            mathUtil.dz = dz;
            
            return mathUtil;
        }
    }
}