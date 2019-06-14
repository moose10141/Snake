using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour {

    private int speed = 30;
    private int frames = 0;
    private bool canTurn = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    void FixedUpdate()
    {
        frames++;
        if (frames % speed == 0)
        {
            transform.position += transform.forward;
            frames = 0;
            canTurn = true;
        }


        float input = Input.GetAxisRaw("Horizontal");
        if (input != 0)
        {
            transform.localEulerAngles += new Vector3(0, input * 90, 0);
            canTurn = false;
        }
    }
}
