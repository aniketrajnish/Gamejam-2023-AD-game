using UnityEngine;

namespace Makra.MathUtils
{
    public static class MathUtils
    {
        public static Vector3 Abs(Vector3 v)
        {
            return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
        }
        public static Vector4 Abs(Vector4 v)
        {
            return new Vector4(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z), Mathf.Abs(v.w));
        }
        public static Vector3 Max(Vector3 vec1, Vector3 vec2)
        {
            return new Vector3(Mathf.Max(vec1.x, vec2.x), Mathf.Max(vec1.y, vec2.y), Mathf.Max(vec1.z, vec2.z));
        }
        public static Vector3 Min(Vector3 vec1, Vector3 vec2)
        {
            return new Vector3(Mathf.Min(vec1.x, vec2.x), Mathf.Min(vec1.y, vec2.y), Mathf.Min(vec1.z, vec2.z));
        }
        public static Vector4 Max(Vector4 vec1, Vector4 vec2)
        {
            return new Vector4(Mathf.Max(vec1.x, vec2.x), Mathf.Max(vec1.y, vec2.y), Mathf.Max(vec1.z, vec2.z), Mathf.Max(vec1.w, vec2.w));
        }
        public static Vector4 Min(Vector4 vec1, Vector4 vec2)
        {
            return new Vector4(Mathf.Min(vec1.x, vec2.x), Mathf.Min(vec1.y, vec2.y), Mathf.Min(vec1.z, vec2.z), Mathf.Min(vec1.w, vec2.w));
        }
        public static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * Mathf.Clamp01(t);
        }
    }
}
