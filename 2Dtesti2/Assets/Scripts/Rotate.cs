using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour
{
    float z;
    public float rotationRate;
    //public GameObject player;
    // Use this for initialization
    //if wall collader and player collider are not touching rotate
    //if wall collader and player collider are touching dont rotate
    void Start()
    {
        z = 0f;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
    }

    public void rotateLeft()
    {

        GameObject Player = GameObject.Find("Player");
        PlayerScript playerScript = Player.GetComponent<PlayerScript>();
        if (playerScript.isrunning == false)
        {
            transform.Rotate(Vector3.forward * + 0.8f);
            playerScript.animat.SetFloat("Speed", 0.8f);
        }
        else {
            transform.Rotate(Vector3.forward * +1.2f);
            playerScript.animat.SetFloat("Speed", 1.2f);
        }
    }


    public void rotateRight()
    {
        GameObject Player = GameObject.Find("Player");
        PlayerScript playerScript = Player.GetComponent<PlayerScript>();

        if (playerScript.isrunning == false)
        {
            transform.Rotate(Vector3.forward * -0.8f);
            playerScript.animat.SetFloat("Speed", 0.8f);
        }
        else {
            transform.Rotate(Vector3.forward * -1.2F);
            playerScript.animat.SetFloat("Speed", 1.2f);
        }
    }
}