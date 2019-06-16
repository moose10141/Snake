using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour {

    private int speed = 40;
    private int frames = 0;
    public int startingLength = 3;
    private bool canTurn = true;
    public GameObject bodyPrefab;
    List<GameObject> body = new List<GameObject>();

	// Use this for initialization
	void Start ()
    {
        body.Add(this.gameObject);
        GameObject[] startBody = GameObject.FindGameObjectsWithTag("Body");
        foreach (GameObject bodyPart in startBody)
        {
            body.Add(bodyPart);
        }
        setUpSnake(startingLength);
    }

    void FixedUpdate()
    {
        frames++;
        if (frames % speed == 0)
        {
            moveSnake(body.Count - 1);
            transform.position += transform.forward;
            frames = 0;
            canTurn = true;
        }


        float input = Input.GetAxisRaw("Horizontal");
        if (canTurn == true && input != 0)
        {
            transform.eulerAngles += new Vector3(0, input * 90, 0);
            canTurn = false;
        }
    }

    private void moveSnake(int bodyNum)
    {
        if (bodyNum == 0)
        {
            return;
        }
        else
        {
            body[bodyNum].transform.position = body[bodyNum - 1].transform.position;
            moveSnake(bodyNum - 1);
        }
    }

    private void setUpSnake(int startingLength)
    {
        for (int i = 0; i < startingLength; i++)
        {
            body.Add(Instantiate(bodyPrefab, body[i].transform.position - body[i].transform.forward, body[i].transform.rotation));
        }

        for (int i = 1; i < body.Count; i++)
        {
            body[i].transform.position = body[i - 1].transform.position - body[i - 1].transform.forward;
        }
    }

    private void extendBody()
    {
        body.Add(Instantiate(bodyPrefab, body[body.Count - 1].transform.position, body[body.Count - 1].transform.rotation));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Food")
        {
            extendBody();
            List<Vector3> takenPositions = new List<Vector3>();
            foreach (GameObject bodyPart in body)
            {
                takenPositions.Add(bodyPart.transform.position);
            }
            Vector3 newPos;
            do
            {
                newPos = new Vector3(Mathf.Round(Random.Range(-8, 8)), other.transform.position.y, Mathf.Round(Random.Range(-4, 4)));
            }
            while (takenPositions.Contains(newPos));
            
            other.transform.position = newPos;
        } 
    }
}