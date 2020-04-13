using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringOutput
{
    public Vector3 linear;
    public float angular;

    /* public static SteeringOutput operator *(SteeringOutput a, SteeringOutput b)
    {
        SteeringOutput result = new SteeringOutput();
        result.linear = a.linear + b.linear;
        result.angular = a.angular * b.angular;

        return result;
    } */
}
