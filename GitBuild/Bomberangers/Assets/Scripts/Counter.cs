using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{
    public PlayerController playerController;
    public Transform playerTransform;
    private float counterDuration; //Set on player controller. Time to deactivate the counter trigger after it was turned on.
    public float counterFuse;
    public float counterSpeedMod;
    public float repeatedCounterMod; //Modifier for the speed after bomberang was countered, by this amount.
    [Header("Redirect counter section")]
    public bool turnRedirectModeOn;
    public GameObject bomberangInHandLocation; 
    private bool grabbedBomberang;
    public Bomberang cloneBomberangScript;
    private Rigidbody bomberangRb;
    // Start is called before the first frame update
    void Start()
    {
        counterDuration = playerController.counterDuration;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameObject.activeInHierarchy)
        {
            counterDuration -= Time.deltaTime;
            if (counterDuration <= 0)
            {
                counterDuration = playerController.counterDuration;
                playerController.ResetSpeed(); //Fixes a bug related to bomberangs speed 
                gameObject.SetActive(false);
                if(turnRedirectModeOn && grabbedBomberang)
                {
                    ThrowCounter();
                    grabbedBomberang = false;
                }
            }
            if (turnRedirectModeOn)
            {
                if (grabbedBomberang)
                {
                    bomberangRb.transform.position = bomberangInHandLocation.transform.position;
                }
            }
        }

    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bomberang"))
        {
            if (turnRedirectModeOn == false)
            {
                //Counters.
                cloneBomberangScript = other.GetComponent<Bomberang>();
                cloneBomberangScript.dangerRang = true;
                cloneBomberangScript.wasCountered = true; //Used to stop bomberang from keep tracking the player on the way back.
                cloneBomberangScript.timesCountered += repeatedCounterMod;
                bomberangRb = other.GetComponent<Rigidbody>();
                bomberangRb.velocity = Vector3.zero;
                bomberangRb.AddTorque(0, playerController.bomberangRotation, 0);
                cloneBomberangScript.fuse = counterFuse;
                cloneBomberangScript.timeToStartReturning = cloneBomberangScript.iniTimeToStartReturning;
                cloneBomberangScript.calledBack = false;
                cloneBomberangScript.playerToReturn = playerTransform;
                float finalCounterSpeed = cloneBomberangScript.timesCountered * counterSpeedMod;
                playerController.bomberangSpeed.x = playerController.bomberangSpeed.x * finalCounterSpeed;
                //playerController.bomberangSpeed.z = playerController.bomberangSpeed.z * counterSpeedMod * cloneBomberangScript.timesCountered;
                ThrowCounter();

            } 
            else if (turnRedirectModeOn)
            {
                cloneBomberangScript = other.GetComponent<Bomberang>();
                cloneBomberangScript.dangerRang = true;
                cloneBomberangScript.wasCountered = true; //Used to stop bomberang from keep tracking the player on the way back.
                cloneBomberangScript.timesCountered += repeatedCounterMod;
                bomberangRb = other.GetComponent<Rigidbody>();
                bomberangRb.velocity = Vector3.zero;
                bomberangRb.AddTorque(0, playerController.bomberangRotation, 0);
                cloneBomberangScript.fuse = counterFuse;
                cloneBomberangScript.playerToReturn = playerTransform;
                cloneBomberangScript.timeToStartReturning = cloneBomberangScript.iniTimeToStartReturning;
                cloneBomberangScript.calledBack = false;
                playerController.bomberangSpeed.x = playerController.bomberangSpeed.x * counterSpeedMod * cloneBomberangScript.timesCountered;
                //playerController.bomberangSpeed.z = playerController.bomberangSpeed.z * counterSpeedMod * cloneBomberangScript.timesCountered;
                grabbedBomberang = true;
            }
        }
    }

    void ThrowCounter()
    {
        if (bomberangRb != null)
        {
            bomberangRb.AddForce(transform.forward * playerController.bomberangSpeed.x);
          
            //Prevents your bomberang from keeping returning to you once you called it back once.
            if (cloneBomberangScript.bomberangMeshes[0].material.name == "Player1 (Instance) (Instance)" && transform.parent.name == "Player1")
            {
                cloneBomberangScript.calledBack = false;
            } else if (cloneBomberangScript.bomberangMeshes[0].material.name == "Player2 (Instance) (Instance)" && transform.parent.name == "Player2")
            {
                cloneBomberangScript.calledBack = false;
            }else if (cloneBomberangScript.bomberangMeshes[0].material.name == "Player3 (Instance) (Instance)" && transform.parent.name == "Player3")
            {
                cloneBomberangScript.calledBack = false;
            }else if (cloneBomberangScript.bomberangMeshes[0].material.name == "Player4 (Instance) (Instance)" && transform.parent.name == "Player4")
            {
                cloneBomberangScript.calledBack = false;
            }
        }
       playerController.counterCooldown = 0;
        cloneBomberangScript.CounterRangColor();

    }

}
