using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {
    float z;
    public float rotationRate;
	// Use this for initialization
	void Start ()
    {
        z = 0f;
	}
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetKey(KeyCode.A))
        {
            rotateLeft();
            //Debug.Log("Left");

        }
        if (Input.GetKey(KeyCode.D))
        {
            rotateRight();
            //Debug.Log("Right");

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
