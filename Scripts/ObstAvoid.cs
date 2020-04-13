using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstAvoid : Seek
{
    ///Min Dist to wall, issues here.!-- Lower values aren't as effective.
    float _avoidDist = 75f;
    ///Distance to look out for colls
    public float lookAheadDist = 10f;


    public override SteeringOutput getSteering()
    {
        Vector3 _ray = character.linearVelocity;
        _ray.Normalize();

        RaycastHit _hit;
        SteeringOutput result = new SteeringOutput();


        if (Physics.Raycast(character.transform.position, _ray, out _hit, lookAheadDist))
        {
            Debug.DrawRay(character.transform.position, _ray * _hit.distance, Color.red, 0.05f);
            //Ray hit, going to collide
            
            if(Vector3.Dot(_hit.normal.normalized, character.linearVelocity) < -0.9f)
            {
                //Small angle collision pending, drift to side
                result.linear = character.transform.right * _avoidDist;
            }
            else
            {
                result.linear = _hit.point +  (_hit.normal * _avoidDist);
            }

            return result;
        }
        else
        {
            Debug.DrawRay(character.transform.position, _ray * lookAheadDist, Color.green, 0.05f);
            //Nothing to avoid for now
            return null;

        }


    }

}