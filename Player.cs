using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    protected float x = 0f;
    protected float y = 0f;
    protected float counter = 0f;
    protected string status = null;

    // Use this for initialization
    void Start()
    {
        this.transform.localPosition = new Vector3(-180, 0f);
        this.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    void Update()
    {
        if (counter < 0)
        {
            counter = 0;
        }
    }

    public void New_position()
    {
        this.transform.localPosition = new Vector3 (-180, y);
    }

    public float X
    {
        get
        {
            return x;
        }
        set
        {
            x = value;
        }
    }

    public float Y
    {
        get
        {
            return y;
        }
        set
        {
            y = value;
        }
    }

    public float Counter
    {
        get
        {
            return counter;
        }
        set
        {
            counter = value;
        }
    }

    public string Status
    {
        get
        {
            return status;
        }
        set
        {
            status = value;
        }
    }
}