using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool bIsClosed   = false;
    public bool bIsLocked   = false;

    Vector3 closedRotation  = new Vector3(0, 0, 0);
    Vector3 openRotation    = new Vector3(0, -135, 0);

    // Start is called before the first frame update
    void Start()
    {
        if (bIsClosed)
        {
            transform.eulerAngles = closedRotation;
        } else
        {
            transform.eulerAngles = openRotation;
        }


        ///Debugging

    }

    public bool Open()
    {
        if(bIsClosed && !bIsLocked)
        {
            Debug.Log("-locked +closed, Door opened");
            bIsClosed = false;
            transform.eulerAngles = openRotation;
            return true;
        }
        Debug.Log("Door open or locked");
        return false;
    }

    public bool Close()
    {
        if (!bIsClosed)
        {
            Debug.Log("Door closed");
            transform.eulerAngles = closedRotation;
            bIsClosed = !bIsClosed;
        }
        return true;  /////
    }
}
