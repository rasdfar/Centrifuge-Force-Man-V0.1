using UnityEngine;
using System.Collections;

public class MovableObjectController : MonoBehaviour {
    public Rigidbody2D rb;
    public GameObject Player;
    public Player playerScript;
    //can only be move while player is touching it
    //tba can drop eaven tho player is not touching it
    void Awake()
    {
        Player = GameObject.Find("Player");
        playerScript = Player.GetComponent<Player>();
    }
    void Start()
    {
         rb.isKinematic = true;
    }
	void FixedUpdate () {

        if (playerScript.touchingMovableObject)
        {
            rb.isKinematic = false;
        }

        else if (!playerScript.touchingMovableObject)
        {
            rb.isKinematic = true;
        }

    }
}
