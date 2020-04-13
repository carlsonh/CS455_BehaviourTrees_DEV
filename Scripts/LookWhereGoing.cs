using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookWhereGoing : Align
{

    public override float getTargetAngle()
    {
        Vector3 vel = character.linearVelocity;//This doesn't seem right

        if (vel.magnitude == 0)
        {
            return character.transform.eulerAngles.y;
        }
        return Mathf.Atan2(vel.x, vel.z) * Mathf.Rad2Deg;

    }

}
