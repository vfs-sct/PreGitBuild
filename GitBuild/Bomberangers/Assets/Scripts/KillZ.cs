using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class KillZ : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player1, player2, player3, player4;
    public TextMeshProUGUI p1Score, p2Scsore, p3Score, p4Score, winnerText;
    private PlayerController pc1, pc2, pc3, pc4;
    private int p1Int, p2Int, p3Int, p4Int;
    public int TargetScore;
    private string winnerString;
    private Vector3 p1IniPos, p2IniPos, p3IniPos, p4IniPos;
    void Start()
    {
        pc1 = player1.GetComponent<PlayerController>();
        pc2 = player2.GetComponent<PlayerController>();
        pc3 = player3.GetComponent<PlayerController>();
        pc4 = player4.GetComponent<PlayerController>();
        p1IniPos = player1.transform.position;
        p2IniPos = player2.transform.position;
        p3IniPos = player3.transform.position;
        p4IniPos = player4.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (p1Int >= TargetScore)
        {
            winnerString = "Blue";
            EndGame();
        } else if (p2Int >= TargetScore)
        {
            winnerString = "Red";
            EndGame();
        }else if (p3Int >= TargetScore)
        {
            winnerString = "Green";
            EndGame();
        }else if (p4Int >= TargetScore)
        {
            winnerString = "Pink";
            EndGame();
        }
    }

    private void EndGame()
    {
        ResetPlayers();
        winnerText.enabled = true;
        Invoke("DisableWinText", 1f);
        winnerText.text = winnerString + " Wins!";
        p1Int = 0;
        p2Int = 0;
        p3Int = 0;
        p4Int = 0;
        p1Score.text = "P1: " + p1Int;
        p2Scsore.text = "P2: " + p2Int;
        p3Score.text = "P3: " + p3Int;
        p4Score.text = "P4: " + p4Int;
    }

    void DisableWinText()
    {
        winnerText.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player1")
        {
            player1.transform.position = p1IniPos;
            if (pc1.hitByP2)
            {
                p2Int++;
                p2Scsore.text = "P2: " + p2Int;
            } else if (pc1.hitByP3)
            {
                p3Int++;
                p3Score.text = "P3: " + p3Int;
            }else if (pc1.hitByP4)
            {
                p4Int++;
                p4Score.text = "P4: " + p4Int;
            }
        }
        else if (other.name == "Player2")
        {
            player2.transform.position = p2IniPos;
            if (pc2.hitByP1)
            {
                p1Int++;
                p1Score.text = "P1: " + p1Int;
            }
            else if (pc2.hitByP3)
            {
                p3Int++;
                p3Score.text = "P3: " + p3Int;
            }
            else if (pc2.hitByP4)
            {
                p4Int++;
                p4Score.text = "P4: " + p4Int;
            }
        } else if (other.name == "Player3")
        {
            player3.transform.position = p3IniPos;
            if (pc3.hitByP2)
            {
                p2Int++;
                p2Scsore.text = "P2: " + p2Int;
            }
            else if (pc3.hitByP1)
            {
                p1Int++;
                p1Score.text = "P1: " + p1Int;
            }
            else if (pc3.hitByP4)
            {
                p4Int++;
                p4Score.text = "P4: " + p4Int;
            }
        } else if (other.name == "Player4")
        {
            player4.transform.position = p4IniPos;
            if (pc4.hitByP2)
            {
                p2Int++;
                p2Scsore.text = "P2: " + p2Int;
            }
            else if (pc4.hitByP3)
            {
                p3Int++;
                p3Score.text = "P3: " + p3Int;
            }
            else if (pc4.hitByP1)
            {
                p1Int++;
                p1Score.text = "P1: " + p1Int;
            }
        }
    }

    public void ResetPlayers()
    {

        player1.transform.position = p1IniPos;


        player2.transform.position = p2IniPos;


        player3.transform.position = p3IniPos;

        player4.transform.position = p4IniPos;  
    }

    
}
