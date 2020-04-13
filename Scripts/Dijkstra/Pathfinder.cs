using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : Kinematic
{
    public Node start;
    public Node goal;
    Graph myGraph;

    FollowPathDemo myMoveType;
    LookWhereGoing myRotateType;

    //public GameObject[] myPath = new GameObject[4];
    GameObject[] myPath;

    // Start is called before the first frame update
    void Start()
    {

        if(start == null)
        {
            start = GameObject.Find("node__Instance1").GetComponent<Node>();
        }
        if(goal == null)
        {
            goal = GameObject.Find("node__Instance100").GetComponent<Node>();
        }

        myRotateType = new LookWhereGoing();
        myRotateType.character = this;
        myRotateType.target = target;

        Graph myGraph = new Graph();
        myGraph.Build();
        List<Connection> path = Dijkstra.pathfind(myGraph, start, goal);
        // path is a list of connections - convert this to gameobjects for the FollowPath steering behavior

        myPath = new GameObject[path.Count + 1];
        int i = 0;
        foreach (Connection c in path)
        {
            Debug.Log("from " + c.getFromNode() + " to " + c.getToNode() + " @" + c.getCost());
            myPath[i] = c.getFromNode().gameObject;
            i++;
        }
        myPath[i] = goal.gameObject;

        myMoveType = new FollowPathDemo();
        myMoveType.character = this;
        myMoveType.path = myPath;
    }

    // Update is called once per frame
    protected override void Update()
    {
        controlledSteeringUpdate = new SteeringOutput();
        //controlledSteeringUpdate.angular = myRotateType.getSteering().angular;
        controlledSteeringUpdate.linear = myMoveType.getSteering().linear;
        base.Update();
    }

}
