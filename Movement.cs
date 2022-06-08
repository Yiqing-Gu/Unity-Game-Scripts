using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    protected int interval = 0;
    protected string status = null;
    protected Player player;
    protected Vehicle vehicle;
    protected GameObject direction;
    protected GameObject load_opponent;
    protected Transform background;
    protected List<Vehicle> manager;

    // Use this for initialization
    void Start ()
    {
        manager = new List<Vehicle>();
        var load_target = Resources.Load<GameObject>("target");
        var load_direction = Resources.Load<GameObject>("direction");
        load_opponent = Resources.Load<GameObject>("opponent");
        background = this.transform.Find("background");
        var target = GameObject.Instantiate(load_target);
        target.transform.SetParent(background);
        player = target.transform.GetComponent<Player>();
        direction = GameObject.Instantiate(load_direction);
        direction.transform.SetParent(background);
        direction.transform.localPosition = new Vector3(0, -260);
        direction.transform.localScale = new Vector3(1, 1, 1);
    }
	
	// Update is called once per frame
	void Update ()
    {
        // control phase
        if (Input.anyKey)
        {
            if (Input.GetKey(KeyCode.W))
            {
                player.Y = player.Y + 8.5f;
                if (player.Y > 260)
                {
                    player.Y = 260;
                }
                player.New_position();
            }
            if (Input.GetKey(KeyCode.S))
            {
                player.Y = player.Y - 8.5f;
                if (player.Y < -185)
                {
                    player.Y = -185;
                }
                player.New_position();
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
            {
                if (Input.GetKey(KeyCode.D))
                {
                    if (player.Status == "backward")
                    {
                        player.Counter = 0;
                    }
                    player.Status = "forward";
                    player.Counter = player.Counter + 0.05f;
                    if (player.Counter > 1)
                    {
                        player.Counter = 1;
                    }
                    player.X = player.X - player.Counter * 2.5f * 15;
                    if (player.X < -440)
                    {
                        player.X = 440;
                    }
                }
                if (Input.GetKey(KeyCode.A))
                {
                    if (player.Status == "forward")
                    {
                        player.Counter = 0;
                    }
                    player.Status = "backward";
                    player.Counter = player.Counter + 0.015f;
                    if (player.Counter > 1)
                    {
                        player.Counter = 1;
                    }
                    player.X = player.X + player.Counter * 2.5f * 10;
                    if (player.X > 440)
                    {
                        player.X = -440;
                    }
                }
            }
        }
        else
        {
            if (player.Counter > 0)
            {
                if (player.Status == "forward")
                {
                    player.Counter = player.Counter - 0.015f;
                    player.X = player.X - player.Counter * 2.5f * 6;
                    if (player.X < -440)
                    {
                        player.X = 440;
                    }
                }
                if (player.Status == "backward")
                {
                    player.Counter = player.Counter - 0.015f;
                    player.X = player.X + player.Counter * 2.5f * 8;
                    if (player.X > 440)
                    {
                        player.X = -440;
                    }
                }
            }
        }
        direction.transform.localPosition = new Vector3(player.X, -260);
        //opponent phase
        if (interval == 50 && manager.Count < 11)
        {
            var opponent = GameObject.Instantiate(load_opponent);
            vehicle = opponent.transform.GetComponent<Vehicle>();
            opponent.transform.SetParent(background);
            manager.Add(vehicle);
            interval = 0;
        }
        if (interval > 70)
        {
            interval = 0;
        }
        else
        {
            interval = interval + 1;
        }
        int length = manager.Count;
        for (int i = 0; i < length; i++)
        {
            bool result = manager[i].Control_vehicle(player.Counter);
            if (result == false)
            {
                manager.Remove(manager[i]);
                length = length - 1;
                i = i - 1;
            }
        }
        //hitbox
        for (int i = 0; i < length; i++)
        {
            manager[i].Hit(player, manager);
        }
    }
}
