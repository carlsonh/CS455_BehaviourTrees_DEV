using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Task//For corots, likely overdoing it
{
    public abstract void run();
    public bool bDidTaskSucceed;

    protected int eventID;

    public string TaskFinished
    {
        get
        {
            return "FinishedTask" + eventID;
        }
    }
    public Task()
    {
        eventID = EventBus.GetEventID();
    }
}

public class IsTrue : Task
{
    bool bToTest;

    public IsTrue(bool bIncomingBool)
    {
        bToTest = bIncomingBool;
    }

    public override void run()
    {
        bDidTaskSucceed = bToTest;
        EventBus.TriggerEvent(TaskFinished);
    }
}

public class IsFalse : Task
{
    bool bToTest;

    public IsFalse(bool bIncomingBool)
    {
        bToTest = bIncomingBool;
    }

    public override void run()
    {
        bDidTaskSucceed = !bToTest;
        EventBus.TriggerEvent(TaskFinished);
    }
}

public class Pause : Task
{
    float fTimeToWait = 1f;

    public Pause(float fIncomingTime)
    {
        fTimeToWait = fIncomingTime;
    }

    public override void run()
    {

        //StartCoroutine("Wait");
        bDidTaskSucceed = true;
        EventBus.ScheduleTrigger(TaskFinished, fTimeToWait);

    }
    private IEnumerator Wait()
    {
        
        //Debug.Log("Waiting");
        yield return new WaitForSeconds(fTimeToWait);
        //Debug.Log("waited "+ fTimeToWait +" seconds"); //This doesn't wait correctly still

        bDidTaskSucceed = true;
        EventBus.ScheduleTrigger(TaskFinished, fTimeToWait);
    }
}


public class OpenDoor : Task
{
    Door targetDoor;

    public OpenDoor(Door drIncomingDoor)
    {
        targetDoor = drIncomingDoor;
    }
    public override void run()
    {
        bDidTaskSucceed = targetDoor.Open();
        EventBus.TriggerEvent(TaskFinished);
    }
}

public class BargeDoor : Task
{
    Rigidbody targetDoor;

    public BargeDoor(Rigidbody rbIncomingDoor)
    {
        targetDoor = rbIncomingDoor;
    }

    public override void run()
    {
        targetDoor.AddForce(-40f,0,0, ForceMode.VelocityChange);
        bDidTaskSucceed = true;
        EventBus.TriggerEvent(TaskFinished);
    }
}

public class StackUp : Task
{
    GameObject unit;

    public StackUp(GameObject goIncomingUnit)
    {
        unit = goIncomingUnit;
    }

    public override void run()
    {
        Debug.Log("Stacking");
        unit.transform.localPosition += new Vector3(0,0,5);
        unit.transform.Rotate(new Vector3(0,40,0));
        bDidTaskSucceed = true;
        EventBus.TriggerEvent(TaskFinished);
    }

}



/////Code from bslease
/////    https://github.com/bslease/Behavior_Tree/blob/0ed22763b0bb4edbbf5f743d5dcb1f97094b9b4e/Behavior%20Tree%20Project/Assets/Scripts/Task.cs#L160
/////    Lightly modified to fit variable names


public class MoveKinematicToObject : Task
{
    Arriver mMover;
    GameObject mTarget;

    public MoveKinematicToObject(Kinematic mover, GameObject target)
    {
        mMover = mover as Arriver;
        mTarget = target;
    }

    public override void run()
    {
        Debug.Log("Moving to target position: " + mTarget);
        mMover.OnArrived += MoverArrived;
        mMover.target = mTarget;
    }

    public void MoverArrived()
    {
        Debug.Log("arrived at " + mTarget);
        mMover.OnArrived -= MoverArrived;
        bDidTaskSucceed = true;
        EventBus.TriggerEvent(TaskFinished);
    }
}

public class Sequence : Task
{
    List<Task> children;
    Task currentTask;
    int currentTaskIndex = 0;

    public Sequence(List<Task> taskList)
    {
        children = taskList;
    }

    // Sequence wants all tasks to succeed
    // try all tasks in order
    // stop and return false on the first task that fails
    // return true if all tasks succeed
    public override void run()
    {
        //Debug.Log("sequence running child task #" + currentTaskIndex);
        currentTask = children[currentTaskIndex];
        EventBus.StartListening(currentTask.TaskFinished, OnChildTaskFinished);
        currentTask.run();
    }

    void OnChildTaskFinished()
    {
        //Debug.Log("Behavior complete! Success = " + currentTask.bDidTaskSucceed);
        if (currentTask.bDidTaskSucceed)
        {
            EventBus.StopListening(currentTask.TaskFinished, OnChildTaskFinished);
            currentTaskIndex++;
            if (currentTaskIndex < children.Count)
            {
                this.run();
            }
            else
            {
                // we've reached the end of our children and all have bDidTaskSucceed!
                bDidTaskSucceed = true;
                EventBus.TriggerEvent(TaskFinished);
            }

        }
        else
        {
            // sequence needs all children to succeed
            // a child task failed, so we're done
            bDidTaskSucceed = false;
            EventBus.TriggerEvent(TaskFinished);
        }
    }
}

public class Selector : Task
{
    List<Task> children;
    Task currentTask;
    int currentTaskIndex = 0;

    public Selector(List<Task> taskList)
    {
        children = taskList;
    }

    // Selector wants only the first task that succeeds
    // try all tasks in order
    // stop and return true on the first task that succeeds
    // return false if all tasks fail
    public override void run()
    {
        //Debug.Log("selector running child task #" + currentTaskIndex);
        currentTask = children[currentTaskIndex];
        EventBus.StartListening(currentTask.TaskFinished, OnChildTaskFinished);
        currentTask.run();
    }

    void OnChildTaskFinished()
    {
        //Debug.Log("Behavior complete! Success = " + currentTask.bDidTaskSucceed);
        if (currentTask.bDidTaskSucceed)
        {
            bDidTaskSucceed = true;
            EventBus.TriggerEvent(TaskFinished);
        }
        else
        {
            EventBus.StopListening(currentTask.TaskFinished, OnChildTaskFinished);
            currentTaskIndex++;
            if (currentTaskIndex < children.Count)
            {
                this.run();
            }
            else
            {
                // we've reached the end of our children and none have bDidTaskSucceed!
                bDidTaskSucceed = false;
                EventBus.TriggerEvent(TaskFinished);
            }
        }
    }
}