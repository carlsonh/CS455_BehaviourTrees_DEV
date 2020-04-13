using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursue : Arrive
{
   private GameObject predictTarget = new GameObject();

   public override SteeringOutput getSteering()
   {
       Vector3 direction = target.transform.position - character.transform.position;
       float distance = direction.magnitude;
       float currSpeed = character.linearVelocity.magnitude;

       float maxPredict = 1.0f;
       float prediction = -1f;

       if(currSpeed <= distance / maxPredict)
       {
           prediction = maxPredict;
       }
       else
       {
           prediction = distance / currSpeed;
       }


        predictTarget.transform.position = target.transform.position + target.GetComponent<Kinematic>().linearVelocity * prediction;

        SteeringOutput result = new SteeringOutput();

        result.linear = predictTarget.transform.position - character.transform.position;

        //Give full accel
        result.linear.Normalize();
        result.linear *= maxAcceleration;

       return result;
   }
}
