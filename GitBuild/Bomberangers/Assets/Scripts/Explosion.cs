using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [HideInInspector]
    public float explosionForce, explosionR, explosionUpwards;
    public Material parentBomberangMaterial; //assign upon bomberang explode. will cache the bomberang material color.
    private void Start()
    {
        Destroy(gameObject, 0.2f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player1" || other.name == "Player2" || other.name == "Player3" || other.name == "Player4")
        {
           
            if(other.TryGetComponent(out Rigidbody rigidbody))
            {
                Vector3 vector = rigidbody.transform.position - transform.position;
                float distance = vector.magnitude;
                Vector3 direction = vector.normalized;
                float magnitude = 1f - Mathf.Pow(Mathf.Clamp01(distance / explosionR), 2f);
                Vector3 force = direction * magnitude * explosionForce + Vector3.up * explosionUpwards * magnitude;
                rigidbody.AddForce(force, ForceMode.Impulse);
            }

            PlayerController playerController = other.GetComponent<PlayerController>();
            playerController.grounded = false;
            if(parentBomberangMaterial.name == "Player1 (Instance) (Instance)")
            {
                playerController.hitByP1 = true;
                playerController.hitByP2 = false;
                playerController.hitByP3 = false;
                playerController.hitByP4 = false;
            } else if (parentBomberangMaterial.name == "Player2 (Instance) (Instance)")
            {
                playerController.hitByP2 = true;
                playerController.hitByP1 = false;
                playerController.hitByP3 = false;
                playerController.hitByP4 = false;
            }
            else if (parentBomberangMaterial.name == "Player3 (Instance) (Instance)")
            {
                playerController.hitByP3 = true;
                playerController.hitByP2 = false;
                playerController.hitByP1 = false;
                playerController.hitByP4 = false;
            }
            else if (parentBomberangMaterial.name == "Player4 (Instance) (Instance)")
            {
                playerController.hitByP4 = true;
                playerController.hitByP2 = false;
                playerController.hitByP3 = false;
                playerController.hitByP1 = false;
            }
        }
    }
}
