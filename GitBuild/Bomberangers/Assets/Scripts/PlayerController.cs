
using UnityEngine;
using UnityEngine.InputSystem;
//[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Player Section")]
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f; 
    [SerializeField]
    private float gravityValue = -9.81f;
    public Transform player1, player2;
    [SerializeField] private GameObject cooldownEffect;
    //private CharacterController controller;
    private Rigidbody rb;
    public bool grounded;

    private Vector2 movementInput = Vector2.zero;
    private bool dashed, threw, reset, counter;
    [Header("Bomberang Section")]
    public GameObject bomberang;
    public Bomberang bomberangScript;
    public Vector3 bomberangSpeed;
    public float bomberangCooldown; //Time between throws
    private float iniBomberangCooldown;
    public float bomberangSpeedModifier; //For cooking speed changes. The higher this is, the faster the speed. Base value is 130. 
    public float returnSpeedModifier; //For return speed cook changes. Base is 1.3f. Higher, the faster it returns.
    private Vector3 baseBomberangSpeed;
    public float bomberangRotation;
    public Transform bomberangSpawnLocation;
    private bool isCooking;
    public GameObject cookingBomberang;
    public float cookFuseIncreaseAmount; //when cooking, increases the fuse by this amount every fixed update count
    public float maxFuseLimit; //Limits the max amount of seconds that the bomberang can be cooked
    private float newFuseTime; //Used to set the new fuse time on the bomberang to spawn
    public bool TurnCookingOn;// Used for playtesting. Turn this off to disable the cooking behavior.
    public bool ExplodeOnForwardGameMode;
    private Bomberang cloneBomberangScript;
    public bool CallBackGameMode;
    private bool callBack;
    [Header("Counter Section")]
    public GameObject counterEffect;
    public float counterDuration;
    public Counter counterScript;
    public float counterCooldown;
    public float iniCounterCooldown;
    private bool hasCountered;
    public GameObject counterCooldownEffect;
    [Header("Dash Section")]
    public Vector3 dashSpeed;
    public Vector3 dashKb;
    public float dashCooldown;
    public float iniDashCooldown;
    private bool dashing;
    public GameObject dashCooldownEffect;
    private bool canKb;
    [Header("Score Section")]
    public bool hitByP1, hitByP2, hitByP3, hitByP4;


    private void Start()
    {
        //controller = gameObject.GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        baseBomberangSpeed = bomberangSpeed;
        iniBomberangCooldown = bomberangCooldown;
        iniCounterCooldown = counterCooldown;
        iniDashCooldown = dashCooldown;
    }
    public void ResetSpeed()
    {
        //fixes multiple counter bug
        bomberangSpeed = baseBomberangSpeed;
    }
    public void OnMove(InputAction.CallbackContext context) //Will be called when pressing the assigned input
    {
        movementInput = context.ReadValue<Vector2>(); 
    }
    public void Throw (InputAction.CallbackContext context)
    {
        isCooking = context.action.IsPressed(); //Means player is hodling throw bomberang button
    }
    public void Dash(InputAction.CallbackContext context)
    {
        dashed = context.action.triggered;
    }
    public void Counter(InputAction.CallbackContext context)
    {
        counter = context.action.triggered;
    }
    public void CallBack(InputAction.CallbackContext context)
    {
        callBack = context.action.triggered;
    }

    public void Reset(InputAction.CallbackContext context) //Pressed enter to reset characters
    {
        reset = true;
       
    }

    void Update()
    {
        if (dashed && !dashing)
        {
            UseDash();
            dashed = false; //Input related
            dashing = true; //Cooldown related
        }
        if (counter && !hasCountered)
        {
            counterEffect.SetActive(true);
            hasCountered = true;
            counter = false;
        }
        if (reset)
        {
            player1.transform.position = new Vector3(-3, 1, -0.3f);
            player2.transform.position = new Vector3(2, 1.5f, -0.3f);
            reset = false;
        }
        if (!threw) 
        {
            if (isCooking) //is Holding throw button. 
            {
                if (TurnCookingOn) //For playtest purposes. Disable on inspector to disable cooking mechanic.
                {
                    if (cookingBomberang.activeInHierarchy == false)
                    {
                        cookingBomberang.SetActive(true);
                        newFuseTime = bomberangScript.fuse;
                    }


                    newFuseTime += cookFuseIncreaseAmount * Time.deltaTime;
                    if (newFuseTime >= maxFuseLimit)
                    {
                        newFuseTime = maxFuseLimit;
                    }
                }
                else if (!TurnCookingOn)
                {
                    newFuseTime = bomberangScript.fuse;
                    throwBomberang();
                }

            }
            if (!isCooking && cookingBomberang.activeInHierarchy) //means pressed throw button and then let it go.
            {
                throwBomberang();
            }
        }
        if (CallBackGameMode) //Allows input to call bomberang back to you.
        {
            if (callBack)
            {
                if (cloneBomberangScript != null)
                {
                    if (cloneBomberangScript.bomberangMeshes[0].material.name == "Player1 (Instance) (Instance)" | cloneBomberangScript.bomberangMeshes[0].material.name == "Default-Material (Instance)" && transform.name == "Player1")
                    {
                        cloneBomberangScript.callBackGameMode = true;
                        cloneBomberangScript.calledBack = true;
                    }
                    else if (cloneBomberangScript.bomberangMeshes[0].material.name == "Player2 (Instance) (Instance)" | cloneBomberangScript.bomberangMeshes[0].material.name == "Default-Material (Instance)" && transform.name == "Player2")
                    {
                        cloneBomberangScript.callBackGameMode = true;
                        cloneBomberangScript.calledBack = true;
                    } else if (cloneBomberangScript.bomberangMeshes[0].material.name == "Player3 (Instance) (Instance)" | cloneBomberangScript.bomberangMeshes[0].material.name == "Default-Material (Instance)" && transform.name == "Player3")
                    {
                        cloneBomberangScript.callBackGameMode = true;
                        cloneBomberangScript.calledBack = true;
                    } else if (cloneBomberangScript.bomberangMeshes[0].material.name == "Player4 (Instance) (Instance)" | cloneBomberangScript.bomberangMeshes[0].material.name == "Default-Material (Instance)" && transform.name == "Player4")
                    {
                        cloneBomberangScript.callBackGameMode = true;
                        cloneBomberangScript.calledBack = true;
                    }
                }
                if (counterScript.cloneBomberangScript != null)
                {
                    if (counterScript.cloneBomberangScript.bomberangMeshes[0].material.name == "Player1 (Instance) (Instance)" | counterScript.cloneBomberangScript.bomberangMeshes[0].material.name == "Default-Material (Instance)" && transform.name == "Player1")
                    {

                        counterScript.cloneBomberangScript.callBackGameMode = true;
                        counterScript.cloneBomberangScript.calledBack = true;
                    } else if (counterScript.cloneBomberangScript.bomberangMeshes[0].material.name == "Player2 (Instance) (Instance)" | counterScript.cloneBomberangScript.bomberangMeshes[0].material.name == "Default-Material (Instance)" && transform.name == "Player2")
                    {

                        counterScript.cloneBomberangScript.callBackGameMode = true;
                        counterScript.cloneBomberangScript.calledBack = true;
                    }else if (counterScript.cloneBomberangScript.bomberangMeshes[0].material.name == "Player3 (Instance) (Instance)" | counterScript.cloneBomberangScript.bomberangMeshes[0].material.name == "Default-Material (Instance)" && transform.name == "Player3")
                    {

                        counterScript.cloneBomberangScript.callBackGameMode = true;
                        counterScript.cloneBomberangScript.calledBack = true;
                    }else if (counterScript.cloneBomberangScript.bomberangMeshes[0].material.name == "Player4 (Instance) (Instance)" | counterScript.cloneBomberangScript.bomberangMeshes[0].material.name == "Default-Material (Instance)" && transform.name == "Player4")
                    {

                        counterScript.cloneBomberangScript.callBackGameMode = true;
                        counterScript.cloneBomberangScript.calledBack = true;
                    }
                }
            }
        }
      
        if (grounded)
        {
            Vector3 move = new Vector3(movementInput.x, 0, movementInput.y);
            rb.velocity = move * playerSpeed;
            if (move != Vector3.zero) //Rotates player towards the direction they are facing.
            {
                gameObject.transform.forward = move;
            }
            hitByP1 = false;
            hitByP2 = false;
            hitByP3 = false;
            hitByP4 = false;
        }
        //rb2d.Move(move * Time.deltaTime * playerSpeed); //Multiplies vertical and horizontal axis (X and Z) by player speed to move it.       
    }

    private void FixedUpdate()
    {
        if (hasCountered)
        {
            counterCooldown -= Time.deltaTime;
            if (counterCooldown <= 0)
            {
                counterCooldown = iniCounterCooldown;
                counterCooldownEffect.SetActive(true);
                Invoke("DeactivateCounterCooldownEffect", 0.3f);
                hasCountered = false;
            }
        }
        if (threw)
        {
            bomberangCooldown -= Time.deltaTime;
            if (bomberangCooldown <= 0)
            {
                threw = false; //Can throw bomberang again.
                bomberangCooldown = iniBomberangCooldown;
                cooldownEffect.SetActive(true);
                Invoke("DeactivateCooldownEffect", 0.3f);
            }
        }
        if (dashing)
        {
            dashCooldown -= Time.deltaTime;
            if (dashCooldown < 0)
            {
                dashCooldown = iniDashCooldown;
                dashCooldownEffect.SetActive(true);
                Invoke("DeactivateDashCooldownEffect", 0.3f);
                dashing = false; //Can dash again
            }
        }
    }
    private void DeactivateCooldownEffect() //For counterCooldown
    {
        cooldownEffect.SetActive(false);
    }
    private void DeactivateDashCooldownEffect() //ForDashCooldown
    {
        dashCooldownEffect.SetActive(false);
    }
    private void DeactivateCounterCooldownEffect() //ForDashCooldown
    {
        counterCooldownEffect.SetActive(false);
    }
    private void UseDash()
    {
        grounded = false;
        canKb = true;
        dashCooldown = iniDashCooldown;
        
        rb.AddForce(transform.forward * dashSpeed.x);
        rb.AddForce(0, dashSpeed.y, 0);

    }
    private void throwBomberang()
    {
        threw = true;
        cookingBomberang.SetActive(false);
        //Spanws bomberang according to location faced, grab its core components
        GameObject bomberangClone = Instantiate(bomberang, bomberangSpawnLocation);
        Rigidbody bomberangRb = bomberangClone.GetComponent<Rigidbody>();
        bomberangClone.transform.SetParent(null);
        bomberangClone.transform.localScale = new Vector3(0.32f, 0.23f, 0.23f);
        bomberangRb.AddTorque(0, bomberangRotation, 0);
        //Sets the cooking behaviors on the new spawned bomberangs, keeping the prefab intact (IMPORTANT)
        cloneBomberangScript = bomberangClone.GetComponent<Bomberang>();
        cloneBomberangScript.fuse = newFuseTime;
        cloneBomberangScript.playerToReturn = gameObject.transform;
        cloneBomberangScript.redMaterial = gameObject.GetComponent<MeshRenderer>().material;
        //Adds force to bombereangs according to fuse time
        float timeCooked = newFuseTime - bomberangScript.fuse;
        if (ExplodeOnForwardGameMode)
        {
            Invoke("MakeItDangerang", 0.15f); //Time before becoming dangerang, to avoid it exploding in your face.
        }
        if (timeCooked != 0) 
        {
            bomberangSpeed.x = baseBomberangSpeed.x + (bomberangSpeedModifier * timeCooked);
            //bomberangSpeed.z = baseBomberangSpeed.z + (bomberangSpeedModifier * timeCooked);
        }     
        bomberangRb.AddForce(transform.forward * bomberangSpeed.x);
        ResetSpeed();

    }
    void MakeItDangerang()
    {
        cloneBomberangScript.dangerRang = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("ground") | collision.collider.CompareTag("Wall") | collision.collider.CompareTag("ExplosiveWall"))
        {
            grounded = true;
            canKb = false;
        }
        if (collision.collider.CompareTag("player") && grounded == false && canKb) //Grounded is false means that the player is dashing
        {
            Rigidbody otherColRb = collision.collider.GetComponent<Rigidbody>();
            otherColRb.velocity = Vector3.zero;
            collision.collider.GetComponent<PlayerController>().grounded = false;
            rb.velocity = Vector3.zero;
            otherColRb.AddForce(transform.forward * dashSpeed.x);
            otherColRb.AddForce(0, dashSpeed.y, 0);

        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("ground"))
        {
            grounded = false;
        }
    }

  
}
