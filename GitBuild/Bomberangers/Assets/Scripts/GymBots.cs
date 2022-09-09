using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GymBots : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private bool counterBot, throwBot;
    [SerializeField] private float throwCooldown, iniThrowCooldown;
   
    void Update()
    {
        if (throwBot && throwCooldown <= 0)
        {
            ThrowBomberang();
            throwCooldown = iniThrowCooldown;
        }
        if (counterBot)
        {
            Counter();
        }
    }
    private void FixedUpdate()
    {
        throwCooldown -= Time.deltaTime;
    }

    void ThrowBomberang()
    {
        playerController.BotThrowBomberang();
    }

    void Counter()
    {
        playerController.BotCounter();
    }
}
