using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrioritySteering
{

    public BlendedSteering[] blends;

    public SteeringOutput getSteering()
    {
        foreach (BlendedSteering _steer in blends)
        {
            SteeringOutput steering = _steer.getSteering();


            if(steering.linear.magnitude > Mathf.Epsilon || Mathf.Abs(steering.angular) > Mathf.Epsilon)
            {
                return steering;
            }
        }
        ///In Mill this returns a < Epsilon steering of the last blended checked
        return null;
    }
}
