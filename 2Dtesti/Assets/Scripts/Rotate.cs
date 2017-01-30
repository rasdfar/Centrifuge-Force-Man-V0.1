using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {
    float z;
    public float rotationRate;
    //public GameObject player;
    // Use this for initialization
    //if wall collader and player collider are not touching rotate
    //if wall collader and player collider are touching dont rotate
    void Start ()
    {
        z = 0f;
    }
	
	// Update is called once per frame
	void Update () {

    }
    void FixedUpdate()
    {
        GameObject Player = GameObject.Find("Player");
        Player playerScript = Player.GetComponent<Player>();

        if (Input.GetKey(KeyCode.A))
        {
            //if boolean from player script is true that player is touching left wall you cannot turn more to the left anymore.
            if (!playerScript.leftTouchingWall)
            {
                rotateLeft();
                //Debug.Log("Left");
            }

        }
        if (Input.GetKey(KeyCode.D))
        {
            if (!playerScript.rightTouchingWall)
            {
                rotateRight();
                //Debug.Log("Right");
            }

        }
    }

    void rotateLeft()
    {
        transform.Rotate(Vector3.forward * + rotationRate);
    }


    void rotateRight()
    {
        transform.Rotate(Vector3.forward * - rotationRate);
    }

}
