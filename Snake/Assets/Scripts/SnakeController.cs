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
            frames = 0;
        }
        rotateSnake();
    }

    private void rotateSnake()
    {
        int inputLR = Input.GetAxisRaw("Horizontal") >= 0 ? (int) Input.GetAxisRaw("Horizontal") : -1;
        int inputUD = Input.GetAxisRaw("Vertical") >= 0 ? (int) Input.GetAxisRaw("Vertical") : -1;
        if (canTurn == true && (inputLR != 0 || inputUD != 0))
        {
            int currentRotation = Mathf.RoundToInt(transform.rotation.eulerAngles.y);
            // rotation equals 90 or 180 -> dirFactor = -1 otherwise rotation equals 0 or 270 -> dirFactor = 1
            int dirFactor = currentRotation == 90 || currentRotation == 180 ? -1 : 1;

            // rotation equals 0 or 180 -> use LR input otherwise rotation equals 90 or 270 -> use UD input
            int input = currentRotation % 180 == 0 ? inputLR : inputUD;
            transform.eulerAngles += new Vector3(0, dirFactor * input * 90, 0);
            canTurn = false;
        }
    }

    private void moveSnake(int bodyNum)
    {
        if (bodyNum == 0)
        {
            transform.position += transform.forward;
            canTurn = true;
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

    private void eatFood(GameObject food)
    {
        extendBody();
        List<Vector3> takenPositions = new List<Vector3>();
        foreach (GameObject bodyPart in body)
        {
            Vector3 takenPos = new Vector3(Mathf.Round(bodyPart.transform.position.x), food.transform.position.y, Mathf.Round(bodyPart.transform.position.z));
            takenPositions.Add(takenPos);
        }
        Vector3 newPos;
        do
        {
            newPos = new Vector3(Mathf.Round(Random.Range(-8, 8)), food.transform.position.y, Mathf.Round(Random.Range(-4, 4)));
        }
        while (takenPositions.Contains(newPos));
        food.transform.position = newPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Food")
        {
            eatFood(other.gameObject);
        } 
        else if (other.gameObject.tag == "Body")
        {
            // Call Function in game controller similar to what is in here
            for (int i = 1; i < body.Count; i++)
            {
                GameObject.Destroy(body[i]);
            }
            body.RemoveRange(1, body.Count - 1);
            body[0].transform.position = Vector3.zero + new Vector3(0, 0.5f, 0);
            body[0].transform.rotation = new Quaternion(0, 0, 0, 0);
            setUpSnake(startingLength);
        }
    }
}