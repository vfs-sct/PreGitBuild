
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    public Counter[] counter;
    public PlayerController[] playerController;
    public bool NoCooking, RedirectCounter, CallBack, ExplodeOnWayForward, NoFuse;
    public Bomberang bomberangPrefab;
    [Header("After choosing the game modes, click this once")]
    public bool refreshMode;
    // Start is called before the first frame update
    void Start()
    {
        refreshMode = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (refreshMode)
        {
            if (NoCooking)
            {
                playerController[0].TurnCookingOn = false;
                playerController[1].TurnCookingOn = false;
                playerController[2].TurnCookingOn = false;
                playerController[3].TurnCookingOn = false;
               
            } else if (!NoCooking)
            {
                playerController[0].TurnCookingOn = true;
                playerController[1].TurnCookingOn = true;
                playerController[2].TurnCookingOn = true;
                playerController[3].TurnCookingOn = true;
            }

            if (RedirectCounter)
            {
                counter[0].turnRedirectModeOn = true;
                counter[1].turnRedirectModeOn = true;
                counter[2].turnRedirectModeOn = true;
                counter[3].turnRedirectModeOn = true;
            } else if (!RedirectCounter)
            {
                counter[0].turnRedirectModeOn = false;
                counter[1].turnRedirectModeOn = false;
                counter[2].turnRedirectModeOn = false;
                counter[3].turnRedirectModeOn = false;
            }

            if (CallBack)
            {
                playerController[0].CallBackGameMode = true;
                playerController[1].CallBackGameMode = true;
                playerController[2].CallBackGameMode = true;
                playerController[3].CallBackGameMode = true;
            } else if (!CallBack)
            {
                playerController[0].CallBackGameMode = false;
                playerController[1].CallBackGameMode = false;
                playerController[2].CallBackGameMode = false;
                playerController[3].CallBackGameMode = false;
            }

            if (ExplodeOnWayForward)
            {
                playerController[0].ExplodeOnForwardGameMode = true;
                playerController[1].ExplodeOnForwardGameMode = true;
                playerController[2].ExplodeOnForwardGameMode = true;
                playerController[3].ExplodeOnForwardGameMode = true;
            } else if (!ExplodeOnWayForward)
            {
                playerController[0].ExplodeOnForwardGameMode = false;
                playerController[1].ExplodeOnForwardGameMode = false;
                playerController[2].ExplodeOnForwardGameMode = false;
                playerController[3].ExplodeOnForwardGameMode = false;
            } 
            
            if (NoFuse)
            {
                bomberangPrefab.noFuseGameMode = true;
            } else if (!NoFuse)
            {
                bomberangPrefab.noFuseGameMode = false;
                bomberangPrefab.defaultFuse = true;
            }
        }

        refreshMode = false;
    }
}
