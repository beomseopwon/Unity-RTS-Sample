using UnityEngine;

public class MathUtility
{
    public static float distanceXZ(Vector3 from, Vector3 to)
    {
        return Mathf.Sqrt((from.x - to.x) * (from.x - to.x) + (from.z - to.z) * (from.z - to.z));
    }

    public static float distanceXZ2(Vector3 from, Vector3 to)
    {
        return ((from.x - to.x) * (from.x - to.x) + (from.z - to.z) * (from.z - to.z));
    }

}
