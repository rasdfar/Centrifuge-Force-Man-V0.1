using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    public Vector2 jumpHeight;
    public Animator player;

   // public Animation Idle;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log("Jump");
            GetComponent<Rigidbody2D>().AddForce(jumpHeight, ForceMode2D.Impulse);
        }
        if (Input.GetKey(KeyCode.D))
        {
            player.SetTrigger("Left");
        }
        if (Input.GetKey(KeyCode.A))
        {
            player.SetTrigger("Right");
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            player.SetTrigger("LeftKeyUp");
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            player.SetTrigger("RightKeyUp");
        }

        //if collider trigger is "Finnish" -> load fucking last scene
        //ui elementti "you wake up in stange place.Whats that on the middle?"
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            Debug.Log("collision");
            SceneManager.LoadScene(2);
        }

    }
    }
