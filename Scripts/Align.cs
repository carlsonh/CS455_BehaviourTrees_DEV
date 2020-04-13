using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Align : SteeringBehaviour
{
    public Kinematic character;
    public GameObject target;


    float maxAngularAccel = 5000f;
    float maxRotation = 3000f;
    float targetRadius = 1f;
    float slowRadius = 20f;

    float timeToTarget = 0.01f;

    public virtual float getTargetAngle()
    {
        return target.transform.eulerAngles.y;
    }

    public override SteeringOutput getSteering()
    {
      SteeringOutput result = new SteeringOutput();


        ///This isn't fully working in this implementation, issues with negative rots sometimes
        float _rotation = Mathf.DeltaAngle(character.transform.eulerAngles.y, getTargetAngle());
        float _rotationSize = Mathf.Abs(_rotation);



        if(_rotationSize < targetRadius)
        {
            return null;
        }
        float targetRotation = 0.0f;

        if (_rotationSize > slowRadius)
        {
            targetRotation = maxRotation;
        } 
        else
        {
            targetRotation = maxRotation * _rotationSize / slowRadius;
        }

    
        targetRotation *= _rotation / _rotationSize;

        result.angular = targetRotation - character.angularVelocity*5.0f;//5.0f: Fixes slamming into target direction and abruptly stopping //Comparing to an euler.y isn't right, who could've guessed
        result.angular /= timeToTarget;

        float _angAccel = Mathf.Abs(result.angular);
        if(_angAccel > maxAngularAccel)
        {
            result.angular /= _angAccel;
            result.angular *= maxAngularAccel;
        }

      //Final target rot comb speed & dir
      

      result.linear = Vector3.zero;

      return result;
    }
}
