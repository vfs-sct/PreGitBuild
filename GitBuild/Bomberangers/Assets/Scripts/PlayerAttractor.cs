using UnityEngine;

public class PlayerAttractor : MonoBehaviour
{
    [SerializeField] private float attractorForce, speedCap;
    private bool isInRange;
    private Rigidbody bomberangRb;
    private Vector3 forceDirection;
    private Transform playerTransform;
    private float distance;

    private void Start()
    {
        playerTransform = GetComponentInParent<Transform>();
    }

    private void FixedUpdate()
    {
        if (isInRange && bomberangRb != null && bomberangRb.velocity.magnitude < speedCap)
        {
            distance = Vector3.Distance(playerTransform.position, bomberangRb.transform.position);
            bomberangRb.AddForce(forceDirection.normalized * attractorForce / Mathf.Pow(distance, 2));
        }
    }

    public void Attract()
    {
        bomberangRb.AddForce(forceDirection.normalized * attractorForce);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bomberang") && other.GetComponent<Bomberang>().dangerRang)
        {
            bomberangRb = other.GetComponent<Rigidbody>();
            forceDirection = playerTransform.position - bomberangRb.transform.position;
            isInRange = true;
            //Attract();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("bomberang") && other.GetComponent<Bomberang>().dangerRang)
        {
            isInRange = false;
        }
    }
}
