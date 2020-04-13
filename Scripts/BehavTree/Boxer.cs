using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boxer : MonoBehaviour
{
    // Start is called before the first frame update
    Pause _test;

    public Door targetDoor;
    public GameObject drugs;

    Sequence _enterDoor;
    Task currentTask;
    bool bIsExecuting = false;
    
    void Start()
    {

        currentTask = BuildTask_GetPill();

        EventBus.StartListening(currentTask.TaskFinished, OnTaskFinished);
        currentTask.run();

        

    }

    void OnTaskFinished()
    {
        EventBus.StopListening(currentTask.TaskFinished, OnTaskFinished);
        bIsExecuting = false;
    }

    Task BuildTask_GetPill()
    {
        List<Task> taskList = new List<Task>();


        ///Open unlocked door
        Task isDoorNotLocked = new IsFalse(targetDoor.bIsLocked);
        Task catchBreath = new Pause(2f);
        Task openDoor = new OpenDoor(targetDoor);

        taskList.Add(isDoorNotLocked);
        taskList.Add(catchBreath);
        taskList.Add(openDoor);
        Sequence sqOpenUnlockedDoor = new Sequence(taskList);



        ///Barge door
        taskList = new List<Task>();
        Task isDoorClosed = new IsTrue(targetDoor.bIsClosed);
        Task stackUp = new StackUp(this.gameObject);
        Task bargeDoor = new BargeDoor(targetDoor.transform.GetChild(0).GetComponent<Rigidbody>());

        taskList.Add(isDoorClosed);
        taskList.Add(catchBreath);
        taskList.Add(stackUp);
        taskList.Add(catchBreath);
        taskList.Add(bargeDoor);
        Sequence sqBargeClosedDoor = new Sequence(taskList);


        ///Guarantee open closed door
        taskList = new List<Task>();

        taskList.Add(sqOpenUnlockedDoor);
        taskList.Add(sqBargeClosedDoor);
        Selector sqOpenTheDoor = new Selector(taskList);


        ///Get pill behind closed door
        taskList = new List<Task>();
        Task moveToDoor = new MoveKinematicToObject(this.gameObject.GetComponent<Kinematic>(), targetDoor.gameObject);
        Task moveToPill = new MoveKinematicToObject(this.gameObject.GetComponent<Kinematic>(), drugs.gameObject);

        taskList.Add(moveToDoor);
        taskList.Add(catchBreath);
        taskList.Add(sqOpenTheDoor);
        taskList.Add(catchBreath);
        taskList.Add(moveToPill);
        Sequence sqGetTreasureBehindClosedDoor = new Sequence(taskList);


        //Get pill if door is open
        taskList = new List<Task>();
        Task isDoorOpen = new IsFalse(targetDoor.bIsClosed);
        
        taskList.Add(isDoorOpen);
        taskList.Add(moveToPill);
        Sequence sqGetTreasureBehindOpenDoor = new Sequence(taskList);


        //Guarantee get pill
        taskList = new List<Task>();

        taskList.Add(sqGetTreasureBehindOpenDoor);
        taskList.Add(sqGetTreasureBehindClosedDoor);
        Selector sqGetTreasure = new Selector(taskList);



        return sqGetTreasure;

    }

}
