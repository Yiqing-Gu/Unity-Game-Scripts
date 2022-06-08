using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    protected float hori = 0;
    protected float vert = 0;
    protected float speed = 0;
    protected float time = 0;
    protected float distance = 0;

    private void Start()
    {
        hori = new List<int> { -510, 510 }[UnityEngine.Random.Range(0, 2)];
        if (hori == -510)
        {
            speed = new List<float> { 2.55f, 2.6f, 2.65f, 10f }[UnityEngine.Random.Range(0, 4)];
        }
        else
        {
            speed = new List<float> { 2f, 2.1f, 2.2f, 1.8f, 1.7f}[UnityEngine.Random.Range(0, 5)];
        }
        vert = UnityEngine.Random.Range(-240, 270);
        this.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);
        this.transform.localPosition = new Vector3(hori, vert);
    }

    void Update()
    {
        if(speed <= 0)
        {
            speed = 1f;
        }
    }

    public void New_position()
    {
        this.transform.localPosition = new Vector3(hori, vert);
    }

    public void Hit(Player player, List<Vehicle> manager)
    {
        if ((Mathf.Abs(-180 - hori) < 62 && Mathf.Abs(player.Y - vert) < 53))
        {
            if (hori < -180)
            {
                player.Counter = player.Counter + 0.5f;
                speed = speed - 1f;
                player.Status = "forward";
            }
            else
            {
                player.Counter = player.Counter - 0.5f;
                speed = speed + 1f;
                player.Status = "backward";
            }
        }
        for(int i = 0; i < manager.Count; i++)
        {
            if(manager[i] != this)
            {
                if ((Mathf.Abs(manager[i].Hori - hori) < 62 && Mathf.Abs(manager[i].Vert - vert) < 53))
                {
                    if (hori < manager[i].Hori)
                    {
                        manager[i].Hori = manager[i].Hori + 1f;
                        speed = speed - 1f;
                    }
                    else
                    {
                        manager[i].Hori = manager[i].Hori - 1f;
                        speed = speed + 1f;
                    }
                }
            }
        }
    }

    public bool Control_vehicle(float counter)
    {
        time = time + 1;
        if (time % 500 == 0 && speed > 1)
        {
            int ran = UnityEngine.Random.Range(0, 10);
            if (ran == 0 || ran == 1 || ran == 2)
            {
                speed = speed - 1f;
            }
            if (ran == 4 || ran == 9 || ran == 7)
            {
                speed = speed + 1f;
            }
            if (ran == 5)
            {
                speed = speed + 5f;
            }
            if (ran == 8)
            {
                speed = speed - 5f;
            }
        }
        if (speed > counter * 2.5f)
        {
            hori = hori + (speed - counter * 2.5f) * 3;
        }
        else
        {
            hori = hori - (counter * 2.5f - speed) * 3;
        }
        distance = Mathf.Abs(Hori - -180);
        this.New_position();
        if (distance > 1500)
        {
            Destroy(this.gameObject);
            return false;
        }
        else if (distance > 690)
        {
            this.gameObject.SetActive(false);
            return true;
        }
        else
        {
            this.gameObject.SetActive(true);
            return true;
        }
    }

    public float Vert
    {

        get
        {
            return vert;
        }
        set
        {
            vert = value;
        }
    }

    public float Hori
    {
        get
        {
            return hori;
        }
        set
        {
            hori = value;
        }
    }

    public float Speed
    {
        get
        {
            return speed;
        }
        set
        {
            speed = value;
        }
    }
}