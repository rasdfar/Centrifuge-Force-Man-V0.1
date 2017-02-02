using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public Vector2 jumpHeight;
    public Animator player;
    public Canvas InventoryCanvas;
    public Text itemText;
    public GameObject Door;
    public Transform startPoint;
    public Transform endPointRight;
    public Transform endPointLeft;
    public Transform arm;
    public bool rightTouchingWall;
    public bool leftTouchingWall;
    public bool touchingMovableObject;
    public int armOfset = -90;      //-90 seems to be about right
    public LayerMask whatToHit;
    //public Transform bulletTrail;
    public GameObject lazerDoor;

    private Transform barrel;
    List<string> Inventory = new List<string>();
    private bool inventoryOpen;

    /*
    todo/notes:
    -Make public arrays of consoles and door
    so that when player interracts with console1-> door1 opens
    console2->door2 opens and so on....

    -remove plutonium stick from the list when player opens a door//done??

    -flip the character depending on movement direction. flipt the arm aiming too at the same time
    -finish shooting laser
    -make test object-> shoot it with laser ->door opens----done but bugy the cursoir point is not right you have to aim off the target to shoot it...
    -make arm rotation better
        */

    void Awake()
    {
        barrel = transform.FindChild("GunArm").FindChild("Barrel");
        if(barrel == null)
        {
            Debug.LogError("Barrel was not found");
        }
    }
    void Start () {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Confined;
        InventoryCanvas.gameObject.SetActive(false);
        inventoryOpen = false;
        //spawnPoint = gameObject.transform.position;
    }
	
	void Update () {
    
        ArmRotation();
      
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
        if (Input.GetKeyDown(KeyCode.I))
        {
            //open inventory
            ShowInventory();

        }
        if (Input.GetButton("Fire1"))
        {
            ShootLazer();
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            player.SetTrigger("LeftKeyUp");
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            player.SetTrigger("RightKeyUp");
        }
        Raycasting();
    }

    void Raycasting() //----RENAME THIS FUNCKTION AT SOME POINT-----
    {
        //this is for recognising if the player is touching a wall from the left side or right
        //used to disable level rotation once the player is "stuck"
       
        Debug.DrawLine(startPoint.position,endPointLeft.position,Color.cyan);//leftline

        Debug.DrawLine(startPoint.position, endPointRight.position, Color.red);//rightline

        leftTouchingWall = Physics2D.Linecast(startPoint.position, endPointLeft.position, 1 << LayerMask.NameToLayer("Level"));
        rightTouchingWall = Physics2D.Linecast(startPoint.position, endPointRight.position, 1 << LayerMask.NameToLayer("Level"));
        touchingMovableObject = Physics2D.Linecast(startPoint.position, endPointLeft.position, 1 << LayerMask.NameToLayer("MovableObject"))|| Physics2D.Linecast(startPoint.position, endPointRight.position, 1 << LayerMask.NameToLayer("MovableObject"));

    }
    void ShootLazer()
    {
        //https://www.youtube.com/watch?v=4ivFemmpYus&list=PLPV2KyIb3jR42oVBU6K2DIL6Y22Ry9J1c&index=10
        //using different names than video

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        Vector3 barrelPositon = barrel.transform.position;
        Vector2 mouse2D = new Vector2(mousePosition.x, mousePosition.y);
        Vector2 barrel2D = new Vector2(barrelPositon.x, barrelPositon.y);
        RaycastHit2D hit = Physics2D.Raycast(barrel2D, mouse2D - barrel2D, 100, whatToHit);

        Debug.DrawLine(barrelPositon, mousePosition,Color.red);
        Debug.DrawLine(barrel2D, (mouse2D - barrel2D) * 100,Color.blue);
        //Effect();

        if (hit)
        {
            Debug.Log("hit");
            lazerDoor.SetActive(false);
        }

    }
    void Effect()
    {
        //Instantiate(bulletTrail,lazerGunPoint.position,lazerGunPoint.rotation);
    }

    void ArmRotation()
    {
        //https://www.youtube.com/watch?v=m-J5sCRipA0&index=6&list=PLPV2KyIb3jR42oVBU6K2DIL6Y22Ry9J1c
        //getting difference is littlebit different combared to video but the funcktion is the same
        Vector3 difference = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane))- arm.transform.position;
        difference.Normalize();
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        arm.transform.rotation = Quaternion.Euler(0f, 0f, rotZ + armOfset);
    }

    void ShowInventory()
    {
        //open inventory list element
        //make ui element of the list
        if (!inventoryOpen)
        {
            inventoryOpen = true;
            InventoryCanvas.gameObject.SetActive(true);

            foreach (string item in Inventory)
            {
                Debug.Log(item);
                itemText.text = item;
            }
        }
        else if (inventoryOpen)
        {
            inventoryOpen = false;
            InventoryCanvas.gameObject.SetActive(false);
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            Debug.Log("collision");
            SceneManager.LoadScene(2);
        }
        if (other.gameObject.CompareTag("Plutonium"))
        {
            //pick up the plutonium stick
            other.gameObject.SetActive(false);

            //add plutonium to the inventory
            Inventory.Add("Plutonium stick");

        }
        if (other.gameObject.CompareTag("Console"))
        {
            foreach (string item in Inventory)
            {
                itemText.text = item;
                if(item == "Plutonium stick")
                {
                    //open the door
                    Door.gameObject.SetActive(false);
                    Inventory.Remove("Plutonium stick");
                    itemText.text = "";
                }
            }
        }
        if (other.gameObject.CompareTag("Hazard"))
        {
            //Destroy(gameObject);
            gameObject.SetActive(false);
            //gameObject.transform.position = spawnPoint;
        }

    }
    }
