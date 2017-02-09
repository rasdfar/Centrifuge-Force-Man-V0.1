using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
public class PlayerScript : MonoBehaviour {

    public float speed;
    public float airtime=0;
    public float groundtimer =0.5f;
    public float climbtimer = 0;
    public Animator animat;
    public bool isrunning=false;
    public bool isfalling=false;
    public bool ishanging=false;
    public bool isgrounded=true;
    public bool isjumping = false;
    public bool isledge1 = false;
    public bool isledge2 = false;
    public bool isledge3 = false;
    private bool grounded;
    public bool facingRight = true;
    public bool firstClimb = true;
    public LayerMask Ledge1;
    public LayerMask Ledge2;
    public LayerMask Ledge3;
    public LayerMask groundIs;
    public Transform groundCheck;
    public Transform LedgeCheck1;
    public Transform LedgeCheck2;
    public Transform LedgeCheck3;
    public float ledgeradius = 0.08f;
    public float groundradius = 0.01f;
    public Vector2 jumpHeight;
    public Vector3 climbPosition;
    public Animator player;
    public Canvas InventoryCanvas;
    public Text itemText;
    public GameObject Door;
    public Transform startPoint;
    public Transform endPointRight;
    public Transform endPointLeft;
    //public Transform laserGunPoint;
    public bool rightTouchingWall;
    List<string> Inventory = new List<string>();
    private bool inventoryOpen;
    GameObject level;
    Rotate rotator;
    float rotZ;
    //arm and shooting variables\/
    //public Transform arm;
    public int armOfset = -90;      //-90 seems to be about right
    public LayerMask whatToHit;
    public Transform bulletTrail;
    public GameObject firstLazerDoor;
    public GameObject secondLazerDoor;
    private Transform barrel;
    public GameObject gunArm;
    public GameObject gunObject;
    bool gunPicked;
    bool gunActive;
    public float lazerOfset;
    public bool touchingMovableObject;
    public GameObject firstLazerTarget;
    public GameObject secondLazerTarget;
    public GameObject PressurePlateDoor;

    void Awake()
    {
        barrel = transform.FindChild("GunArm").FindChild("Barrel");
        if (barrel == null)
        {
            Debug.LogError("Barrel was not found");
        }
        gunArm.SetActive(false);
        gunPicked = false;
    }

    // Use this for initialization
    void Start()
    {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Confined;
        InventoryCanvas.gameObject.SetActive(false);
        inventoryOpen = false;
        animat = GetComponent<Animator>();
        level = GameObject.Find("demoLevelBase");
        rotator = level.GetComponent<Rotate>();  
    }

    void FixedUpdate()
    {
        hideWeapon();
        /* Physics2D.gravity = new Vector3(transform.position.x, transform.position.y, transform.position.z);
         Vector3 dir = GameObject.Find("Center").transform.position - GameObject.Find("Player").transform.position;
         float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
         GameObject.Find("Player").transform.localEulerAngles = new Vector3(0, 0, (angle - 90));*/
        isgrounded = Physics2D.OverlapCircle(groundCheck.position, groundradius, groundIs);
        isledge1 = Physics2D.OverlapCircle(LedgeCheck1.position, ledgeradius, Ledge1);
        isledge2 = Physics2D.OverlapCircle(LedgeCheck2.position, (ledgeradius-0.02f), Ledge2);
        isledge3 = Physics2D.OverlapCircle(LedgeCheck3.position, ledgeradius, Ledge3);
        animat.SetBool("Ground", isgrounded);
        animat.SetFloat("Speed", Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y));
        if (GetComponent<Rigidbody2D>().velocity.y < -1f && isgrounded == false)
        {
            isfalling = true;
            animat.SetBool("Jumping", false);
            animat.SetBool("Falling", isfalling);
        }
        if (GetComponent<Rigidbody2D>().velocity.y == 0)
        {
            isfalling = false;
            animat.SetBool("Falling", isfalling);

        }
        if (ishanging == true || isgrounded == true)
        {
            isfalling = false;
            isjumping = false;
        }

        if (isgrounded==true)
            groundtimer += Time.deltaTime;
        else
            groundtimer =0;


        /*If player presses and holds space, player can grab hold on ledge if necessary parameters are fufilled.*/
        if (Input.GetKey(KeyCode.Space))
        {
            if (isledge1 == false && isledge2 == true && isgrounded == false)
            {
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                climbPosition = GetComponent<Transform>().position;
                ishanging = true;
                animat.SetBool("Hanging", ishanging);
                animat.SetBool("Jumping", false);
            }
        }


        if (animat.GetBool("Climb Down") == true) {
            if (facingRight == true)
            {        
                GetComponent<Transform>().position = new Vector3(0, GetComponent<Transform>().position.y - 0.0034f, 0);
                rotator.transform.Rotate(Vector3.forward * +0.024f);
            }
            else
            {
                
                GetComponent<Transform>().position = new Vector3(0, GetComponent<Transform>().position.y - 0.0034f, 0);
                rotator.transform.Rotate(Vector3.forward * -0.024f);
            }
        }

        /*Moving up while climbing*/

        if (animat.GetBool("Climbing") == true || animat.GetBool("FirstClimb") == true)
        {
            if (animat.GetBool("FirstClimb") == true)
            {
                climbtimer += Time.deltaTime;
                
                    if (facingRight == true)
                    {
                    if (climbtimer < 2.9f)
                        GetComponent<Transform>().position = new Vector3(0, GetComponent<Transform>().position.y + 0.00097f, 0);
                    if ( climbtimer<2.9f )
                        rotator.transform.Rotate(Vector3.forward * -0.012f);
                    if (climbtimer >4.9f) {
                        GetComponent<Transform>().position = new Vector3(0, GetComponent<Transform>().position.y + 0.0017f, 0);
                        rotator.transform.Rotate(Vector3.forward * -0.014f);
                    }
                    }
                    else
                    {
                    if (climbtimer < 2.9f)
                        GetComponent<Transform>().position = new Vector3(0, GetComponent<Transform>().position.y + 0.00097f, 0);
                    if ( climbtimer<2.9f)                
                        rotator.transform.Rotate(Vector3.forward * +0.012f);
                    if (climbtimer >4.9f)
                    {
                        GetComponent<Transform>().position = new Vector3(0, GetComponent<Transform>().position.y + 0.0017f, 0);
                        rotator.transform.Rotate(Vector3.forward * +0.014f);
                    }
                    }
                
            }
            else
            {
                if (facingRight == true)
                {
                    GetComponent<Transform>().position = new Vector3(0, GetComponent<Transform>().position.y + 0.0028f, 0);
                    rotator.transform.Rotate(Vector3.forward * -0.025f);
                }
                else
                {
                    GetComponent<Transform>().position = new Vector3(0, GetComponent<Transform>().position.y + 0.0028f, 0);
                    rotator.transform.Rotate(Vector3.forward * +0.025f);
                }
            }
        }
        else
            climbtimer = 0;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (isgrounded == true)
        {
            isjumping = false;
            animat.SetBool("Jumping", false);
            isfalling = false;
            animat.SetBool("Falling", false);
        }
    }



   
    // Update is called once per frame
    void Update(){
        ArmRotation();

        if (isfalling==true)
            airtime += Time.deltaTime;
        else
            airtime = 0f;



        if (Input.GetKeyDown(KeyCode.I))
        {
            //open inventory
            ShowInventory();
        }
        if (Input.GetKey(KeyCode.A) && ishanging == false && animat.GetBool("Climbing") == false && animat.GetBool("FirstClimb") == false)
        {
            //if boolean from player script is true that player is touching left wall you cannot turn more to the left anymore.
            if (facingRight == true)
            {
                Debug.Log("Turning left");
                animat.SetBool("Turning", true);
            }

          
            if(facingRight==false && rightTouchingWall==false)
            {
                Debug.Log("Moving left");
                rotator.rotateLeft();
            }           
        }

        if (Input.GetKey(KeyCode.D) && ishanging == false && animat.GetBool("Climbing")==false && animat.GetBool("FirstClimb") == false)
        {
            //if boolean from player script is true that player is touching left wall you cannot turn more to the left anymore.
            if (facingRight == false)
            {
                Debug.Log("Turning right");
                animat.SetBool("Turning", true);
            }
           if(facingRight == true && rightTouchingWall == false)
            {
                Debug.Log("Moving right");
                rotator.rotateRight();
            }
        }

        if (Input.GetKey(KeyCode.W) && ishanging == true && animat.GetBool("Climbing") == false) {
            ishanging = false;
            animat.SetBool("Hanging", ishanging);
            if (firstClimb == true)
            {
                 Debug.Log("Climbing up");
                 animat.SetBool("FirstClimb", true);
                 firstClimb = false;
            }
            else{
                animat.SetBool("Climbing", true);
            }
           
        }

        if (Input.GetKey(KeyCode.S) && ishanging == true)
        {
            ishanging = false;
            animat.SetBool("Hanging", ishanging);
            GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePositionY;
            isfalling = true;
            animat.SetBool("Falling", isfalling);
           
        }

        if (Input.GetKeyDown(KeyCode.S) && isgrounded == true)
        {
            if (isledge3 == false)
            {
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                Debug.Log("Climb down");
                turnAround();
                animat.SetBool("Climb Down", true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && isgrounded==true && animat.GetBool("Falling")==false && groundtimer>0.5)
        {
            isjumping = true;
            if (GetComponent<Rigidbody2D>().velocity.y == 0)
                animat.SetBool("Jumping", true);
            else
                Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (isrunning == false)
            {
                isrunning = true;
                animat.SetFloat("Speed", speed);
                animat.SetBool("Running", isrunning);
            }
            else {
                isrunning = false;
                animat.SetFloat("Speed", speed);
                animat.SetBool("Running", isrunning);
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if(gunPicked == true && gunActive == false)
            {
                Debug.Log("gunDrawn");
                gunActive = true;
                gunArm.SetActive(true);
            }
            else
            {
                Debug.Log("gunHolstered");
                gunActive = false;
                gunArm.SetActive(false);
            }
            
        }

        if (Input.GetButton("Fire1") && gunActive == true)
        {
            ShootLazer();
        }
        Raycasting();
    }

    public void turnAround()
    {
        bool facing = facingRight;
        //if boolean from player script is true that player is touching left wall you cannot turn more to the left anymore.
        if (facing == true)
        {
            Vector3 newScale = GameObject.Find("Player").transform.localScale;
            newScale.x *= -1;
            GameObject.Find("Player").transform.localScale = newScale;
            facingRight = false;
        }

        if (facing == false)
        {
            Vector3 newScale = GameObject.Find("Player").transform.localScale;
            newScale.x *= -1;
            GameObject.Find("Player").transform.localScale = newScale;
            facingRight = true;
        }
        animat.SetBool("Turning", false);
    }


    void climbUp()
    {
        animat.SetBool("Climbing", false);
        animat.SetBool("FirstClimb", false);
    /*    if (facingRight == true)
        {
            Vector3 climbPosition = GetComponent<Transform>().position;
            GetComponent<Transform>().position = new Vector3(climbPosition.x, climbPosition.y + 0.45f, 0);
            rotator.transform.Rotate(Vector3.forward * -2.2f);
        }
        else
        {
            Vector3 climbPosition = GetComponent<Transform>().position;
            GetComponent<Transform>().position = new Vector3(climbPosition.x, climbPosition.y + 0.45f, 0);
            rotator.transform.Rotate(Vector3.forward * +2.2f);
        }
        */
        GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePositionY;
    }

    void climbDown()
    {
        animat.SetBool("Climb Down", false);
        Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        /*
        climbPosition = GetComponent<Transform>().position;
        if (facingRight == true)
        {
            rotator.rotateLeft();
            rotator.rotateLeft();
            GetComponent<Transform>().position = new Vector3(climbPosition.x, climbPosition.y - 0.5f, 0);
        }
        else
        {
            rotator.rotateRight();
            rotator.rotateRight();
            GetComponent<Transform>().position = new Vector3(climbPosition.x, climbPosition.y - 0.5f, 0);
        }*/
        GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePositionY;
        turnAround();
    }


    /*Method for jumping. Specific animations call this jumping during animation itself*/
    void Jump()
    {

        if (isrunning == false)
        {
            jumpHeight = new Vector3(0, 2.2f, 0);
            GetComponent<Rigidbody2D>().AddForce(jumpHeight, ForceMode2D.Impulse);
        }
        if (isrunning == true)
        {
            jumpHeight = new Vector3(0, 2.5f, 0);
            GetComponent<Rigidbody2D>().AddForce(jumpHeight, ForceMode2D.Impulse);
        }
        Debug.Log("Jump");
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
                if (item == "Plutonium stick")
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
        if (other.gameObject.CompareTag("LazerGun"))
        {
            //set gun arm so that it can be used now by pressing right mouse button
            Debug.Log("lasergun picked up");
            gunPicked = true;
            gunObject.SetActive(false);//hide pickup gun object
        }
        /*
        if (other.gameObject.CompareTag("PressurePlate"))
        {
            Debug.Log("Pressure plate is activated");
            PressurePlateDoor.SetActive(false);
        }*/

    }

    /*void OnTriggerExit2D(Collider other)
    {
        //if player pushes object outside of the pressureplate collider
        //->door closes again
        if (other.gameObject.CompareTag("PressurePlate"))
        {
            Debug.Log("Pressure plate is deactivated");
            PressurePlateDoor.SetActive(true);
        }
    }*/


    void Raycasting()
    {
        //this is for recognising if the player is touching a wall from the left side or right
        //used to disable level rotation once the player is "stuck"

        //Debug.DrawLine(startPoint.position, endPointLeft.position, Color.cyan);//leftline
        Debug.DrawLine(startPoint.position, endPointRight.position, Color.red);//rightline
        rightTouchingWall = Physics2D.Linecast(startPoint.position, endPointRight.position, 1 << LayerMask.NameToLayer("Level"));
        touchingMovableObject = /*Physics2D.Linecast(startPoint.position, endPointLeft.position, 1 << LayerMask.NameToLayer("MovableObject")) ||*/ Physics2D.Linecast(startPoint.position, endPointRight.position, 1 << LayerMask.NameToLayer("MovableObject"));
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

        Debug.DrawLine(barrelPositon, mousePosition, Color.red);
        Debug.DrawLine(barrel2D, (mouse2D - barrel2D) * 100, Color.blue);
        //Effect for the lazer
        Instantiate(bulletTrail, barrel2D, Quaternion.Euler(0f, 0f, rotZ + lazerOfset));// its not perfect. its shoots a bit to to the side compared to the actual raycast hit.
        if (hit.transform == firstLazerTarget.transform)
        {
            Debug.Log(hit.collider);
            firstLazerDoor.SetActive(false);
        }
        else if (hit.transform == secondLazerTarget.transform)
        {
            secondLazerDoor.SetActive(false);
        }

    }

    void ArmRotation()
    {
        //https://www.youtube.com/watch?v=m-J5sCRipA0&index=6&list=PLPV2KyIb3jR42oVBU6K2DIL6Y22Ry9J1c
        //getting difference is littlebit different combared to video but the funcktion is the same
        Vector3 difference = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)) - gunArm.transform.position;
        difference.Normalize();
        rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        gunArm.transform.rotation = Quaternion.Euler(0f, 0f, rotZ + armOfset);
    }

    void hideWeapon()
    {
        if (GetComponent<Rigidbody2D>().velocity.y != 0 || animat.GetBool("Turning") || ishanging || animat.GetBool("Climbing") || animat.GetBool("Climb Down") || animat.GetBool("FirstClimb"))
        {
            gunArm.SetActive(false);
        }
        else if(gunPicked && gunActive)
        {
            gunArm.SetActive(true);
        }
    }
}
