using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollAvoid : SteeringBehaviour
{
    public Kinematic character;
    float maxAcceleration = 80f;

    public Kinematic[] targets;

    float radius = 2f; //coll Rad

    public override SteeringOutput getSteering()
    {
        ///Any collisions pending
        ///
        float shortestTime = float.PositiveInfinity;

        Kinematic firstTarget = null;
        float firstMinSep;
        float firstDistance;
        Vector3 firstRelativePos;
        Vector3 firstRelativeVel;



        foreach (Kinematic target in targets)
        {
            ///Calc time to coll
            ///
            Vector3 relativePos = target.transform.position - character.transform.position;
            Vector3 relativeVel = character.linearVelocity - target.linearVelocity;
            float relativeSpeed = relativeVel.magnitude;
            float timeToColl = Vector3.Dot(relativePos, relativeVel) / (relativeSpeed * relativeSpeed);

            ///Is close?
            ///
            float distance = relativePos.magnitude;
            float minSeparation = distance - relativeSpeed * timeToColl;

            ///Never get close enough
            if(minSeparation > 2 * radius)
            {
                continue;
            }
            if(timeToColl > 0 && timeToColl < shortestTime)
            {
                shortestTime = timeToColl;
                firstTarget = target;
                firstMinSep = minSeparation;
                firstDistance = distance;
                firstRelativePos = relativePos;
                firstRelativeVel = relativeVel;
            }

        }

        if(firstTarget == null)
        { 
            //Debug.Log("No near colls");
            return null;
        }





        SteeringOutput result = new SteeringOutput();

        float dotResult = Vector3.Dot(character.linearVelocity.normalized, firstTarget.linearVelocity.normalized);
        if(dotResult < -0.9)
        {
            result.linear = -firstTarget.transform.right*5f;
        }
        else
        {
            result.linear = -firstTarget.linearVelocity;
        }

        result.linear.Normalize();
        result.linear *= maxAcceleration;
        result.angular = 0;
        //Debug.Log("Delta:" + result.linear.magnitude);
        return result;


    }
}
