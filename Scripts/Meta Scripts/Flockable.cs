using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flockable : Kinematic
{
    public GameObject flockCoMTarget;

    PrioritySteering _prioritySteering;
    BlendedSteering _generalFlockSteering;
    
    BlendedSteering _obstAvoidSteering;
    GameObject[] relevantBirds;


    void Start()
    {

        ///Setup behaviours and BlendSteer for general flocking
        #region General Flocking

        ///Start Separation
        //Separate from others and not self
        Separation _separate = new Separation();
        _separate.character = this;
        GameObject[] _goBirds = GameObject.FindGameObjectsWithTag("birds");
        relevantBirds = new GameObject[_goBirds.Length - 1];


        int _relevantBirdCount = 0;
        for(int _bird = 0; _bird < _goBirds.Length-1; _bird++)
        {
            if(_goBirds[_bird] == this)
            {
                //Ignore self for separation
                continue;
            }
            ///Else separate from them
            relevantBirds[_relevantBirdCount++] = _goBirds[_bird];
        }
        _separate.neighborhood = relevantBirds;
        ///End Separation

        ///Start Cohere
        Arrive _cohere = new Arrive();
        _cohere.character = this;
        _cohere.target = flockCoMTarget;
        ///End Cohere

        ///Start LWYG
        LookWhereGoing _lWYG = new LookWhereGoing();
        _lWYG.character = this;
        ///End LWYG



        ///Start Blending for general flocking
        _generalFlockSteering = new BlendedSteering();
        _generalFlockSteering.behaviours = new BehaviourAndWeight[3];

        //Separation
        _generalFlockSteering.behaviours[0] = new BehaviourAndWeight();
        _generalFlockSteering.behaviours[0].behaviour = _separate;
        _generalFlockSteering.behaviours[0].weight = 1f; //Weights are still not great, but functional

        //Cohere
        _generalFlockSteering.behaviours[1] = new BehaviourAndWeight();
        _generalFlockSteering.behaviours[1].behaviour = _cohere;
        _generalFlockSteering.behaviours[1].weight = .4f;

        //Look Where Going
        _generalFlockSteering.behaviours[2] = new BehaviourAndWeight();
        _generalFlockSteering.behaviours[2].behaviour = _lWYG;
        _generalFlockSteering.behaviours[2].weight = 20f; 
        ///End Blending

        #endregion General Flocking




        #region Obst Avoid
        ///Start ObstAvoid
        ObstAvoid _obstAvoid = new ObstAvoid();
        _obstAvoid.character = this;
        _obstAvoid.lookAheadDist = 5f;
        //_obstAvoid.target = flockCoMTarget;

        ///End ObstAvoid

        ///Start Blending for obstacle avoidance
        _obstAvoidSteering = new BlendedSteering();
        _obstAvoidSteering.behaviours = new BehaviourAndWeight[1];

        //ObstAvoid
        _obstAvoidSteering.behaviours[0] = new BehaviourAndWeight();
        _obstAvoidSteering.behaviours[0].behaviour = _obstAvoid;
        _obstAvoidSteering.behaviours[0].weight = 1f; //Priority handles this
        ///End Blending

        #endregion Obst Avoid





        #region Priority Blending
        ///Start Prioritization
        _prioritySteering = new PrioritySteering();
        _prioritySteering.blends = new BlendedSteering[2];

        //General Flocking
        _prioritySteering.blends[0] = new BlendedSteering();
        _prioritySteering.blends[0] = _obstAvoidSteering;

        //Obst Avoid
        _prioritySteering.blends[1] = new BlendedSteering();
        _prioritySteering.blends[1] = _generalFlockSteering;
        ///End Prioritization

        #endregion Priority Blending


    }

    protected override void Update()
    {
        controlledSteeringUpdate = new SteeringOutput();
        controlledSteeringUpdate = _prioritySteering.getSteering();
        base.Update();
    }
}
