 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


////////////
/// Code copied from bslease
/// https://github.com/bslease/Behavior_Tree/blob/master/Behavior Tree Project/Assets/Scripts/EventBus.cs
///////////
public class Arriver : Kinematic
{
    Arrive myMoveType;

    // allow people who are interested to find out when we've arrived
    public delegate void Arrived();
    public event Arrived OnArrived;

    // Start is called before the first frame update
    void Start()
    {
        myMoveType = new Arrive();
        myMoveType.character = this;
        myMoveType.target = target;
    }

    // Update is called once per frame
    protected override void Update()
    {
        myMoveType.target = target;

        if (target != null)
        {
            if ((target.transform.position - transform.position).magnitude < 1.5f)
            {
                OnArrived?.Invoke();
            }
        }

        if (target != null)
        {

            //steeringUpdate = new SteeringOutput();
            controlledSteeringUpdate = myMoveType.getSteering();
        }
        base.Update();
    }
}