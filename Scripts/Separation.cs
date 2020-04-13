using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Separation : Seek
{
    public GameObject[] neighborhood;
    float neighborhoodSizeUnits = 5.0f;
    float decayCoeff = 10.0f;
    float maxAcceleration = 5.0f;


    public override SteeringOutput getSteering()
    {
        SteeringOutput result = new SteeringOutput();

        //Ungood
        //neighborhood = GameObject.FindGameObjectsWithTag("neighbor");
        //Removed to accomodate bird flocking


        foreach (GameObject neighbor in neighborhood)
        {
            Vector3 direction = neighbor.transform.position - character.transform.position;
            float distance = direction.magnitude;


            if(distance < neighborhoodSizeUnits)
            {
                float strength = Mathf.Min(decayCoeff / (distance * distance), maxAcceleration);

                direction.Normalize();
                //Added - to direction, probably not the right fix but now they go away from each other so
                result.linear += strength * -direction;
            }
        }

        return result;
    }

}
