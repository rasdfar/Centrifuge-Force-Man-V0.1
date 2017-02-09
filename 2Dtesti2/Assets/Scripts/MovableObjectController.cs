using UnityEngine;
using System.Collections;

public class MovableObjectController : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject Player;
    public PlayerScript playerScript;
    public GameObject PressurePlateDoor;
    //bugy as fuck atm
    void Awake()
    {
        Player = GameObject.Find("Player");
        playerScript = Player.GetComponent<PlayerScript>();
    }
    void Start()
    {
        rb.isKinematic = true;
    }
    void FixedUpdate()
    {

        if (playerScript.touchingMovableObject)
        {
            rb.isKinematic = false;
        }

        else if (!playerScript.touchingMovableObject)
        {
            rb.isKinematic = true;
        }

    }
    /*
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PressurePlate"))
        {
            //open door
            //add to player script too-> if player stands on pressure plate open the door
            Debug.Log("Pressure plate is activated");
            PressurePlateDoor.SetActive(false);
        }
    }
    void OnTriggerExit2Dd(Collider other)
    {
        //if player pushes object outside of the pressureplate collider
        //->door closes again
        if (other.gameObject.CompareTag("PressurePlate"))
        {
            Debug.Log("Pressure plate is deactivated");
            PressurePlateDoor.SetActive(true);
        }
    }
    */
}
