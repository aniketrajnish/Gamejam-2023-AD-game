using UnityEngine;
using static Makra.MathUtils.MathUtils;

public class DFs : MonoBehaviour
{    
    public float sdHypercube(Vector4 p, Vector4 b)
    {
        Vector4 d = Abs(p) - b;
        float maxComponent = Mathf.Max(Mathf.Max(Mathf.Max(d.x, d.y), d.z), d.w);
        return Mathf.Min(maxComponent, 0.0f) + Max(d, Vector4.zero).magnitude;
    }
}
