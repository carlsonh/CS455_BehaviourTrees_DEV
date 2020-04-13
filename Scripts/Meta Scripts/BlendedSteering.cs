using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BehaviourAndWeight
{
    public SteeringBehaviour behaviour;
    public float weight = 0.0f;
}
public class BlendedSteering
{
    public BehaviourAndWeight[] behaviours;

    float _maxAccel = 100f;
    float _maxRotation = 5000f;

    public SteeringOutput getSteering()
    {
        SteeringOutput result = new SteeringOutput();


        //Collect weighted steering suggestions from active behaviours
        foreach (BehaviourAndWeight _b in behaviours)
        {
            SteeringOutput _s = _b.behaviour.getSteering();

            if(_s != null)
            {
                //If the Behaviour has a suggestion, add it
                result.angular += _s.angular * _b.weight;
                result.linear += _s.linear * _b.weight;
            }
        }
        
        
        //Clip lin accel
        if(result.linear.magnitude > _maxAccel)
        {
            result.linear = result.linear.normalized * _maxAccel;
        }

        ///Clip ang accel
        float _angAccel = Mathf.Abs(result.angular);
        if(_angAccel > _maxRotation)
        {
            result.angular /= _angAccel;
            result.angular *= _maxRotation;
        }
        return result;
    }
}
