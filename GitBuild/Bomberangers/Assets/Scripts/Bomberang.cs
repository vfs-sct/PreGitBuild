
using UnityEngine;

public class Bomberang : MonoBehaviour
{
    // Start is called before the first frame update
    //Returns to player.
    //Has A Fuse. First, cooking decreases its timer.
    //Explodes and knockbacks. 
    public Transform playerToReturn;
    public float forceReturnSpeed;
    public float fuse;
    public GameObject explosion;
    public Vector3 explosionRadius;
    public float explosionForce, explosionR, explosionUpwards;
    private float halfFuse;
    private bool assignTarget;
    public bool defaultFuse;
    private Rigidbody rb;
    public bool dangerRang; //Means this bomberang is either returning or has been countered. On touch, dangeRangs explodes.
    public bool wasCountered;
    public Material redMaterial;
    public MeshRenderer[] bomberangMeshes;
    private bool assignedNewMat;
    public float timesCountered;
    public bool callBackGameMode, calledBack;
    public bool noFuseGameMode;
    public float timeToStartReturning; //Used time instead of distance because distance is affected by player movement, and could mislead their instincts. 
    public float iniTimeToStartReturning;
    public float speedCap;
    void Start()
    {
        if (noFuseGameMode)
        {
            defaultFuse = false;
        }
        explosionR = explosionRadius.x;
        halfFuse = fuse / 2;
        rb = GetComponent<Rigidbody>();
        timesCountered = 1;
        Physics.IgnoreLayerCollision(8, 6, true); //Ignores collision between bombereang and ground
        iniTimeToStartReturning = timeToStartReturning;
    }

    private void Update()
    {
        if (defaultFuse)
        {
            if (fuse <= halfFuse && !assignTarget)
            {
                assignTarget = true;
                dangerRang = true;
            }
        }
        if (dangerRang)
        {
            Physics.IgnoreLayerCollision(6, 7, false);
            if (!assignedNewMat)
            {
                foreach (MeshRenderer meshes in bomberangMeshes)
                {
                    meshes.material = redMaterial;
                    assignedNewMat = true;
                }
            }
        }
        
        if (!dangerRang)
        {
            Physics.IgnoreLayerCollision(6, 7, true); //Ignores between player and bombereang
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
      

            if (defaultFuse && !callBackGameMode)
            {
                fuse -= Time.deltaTime;
                if (fuse <= 0)
                {
                    Explode();
                }

                if (assignTarget && !wasCountered)
                {
                Vector3 dir = playerToReturn.transform.position - transform.position;
                dir = dir.normalized;
                rb.AddForce(dir * forceReturnSpeed);
            }
                if (wasCountered && fuse <= halfFuse)
                {
                Vector3 dir = playerToReturn.transform.position - transform.position;
                dir = dir.normalized;
                rb.AddForce(dir * forceReturnSpeed);
            }
            }

            if (callBackGameMode)
            {
            if (!calledBack)
            {
                fuse -= Time.deltaTime;
                if (fuse <= 0 && !noFuseGameMode)
                {
                    Explode();
                }

                if (assignTarget && !wasCountered)
                {
                    Vector3 dir = playerToReturn.transform.position - transform.position;
                    dir = dir.normalized;
                    rb.AddForce(dir * forceReturnSpeed);
                }
                if (wasCountered && fuse <= halfFuse)
                {
                    Vector3 dir = playerToReturn.transform.position - transform.position;
                    dir = dir.normalized;
                    rb.AddForce(dir * forceReturnSpeed);
                }
            }
            else if (calledBack && !noFuseGameMode)
            {
                fuse -= Time.deltaTime;
                if (fuse <= 0)
                {
                    Explode();
                }
                dangerRang = true;
                Vector3 dir = playerToReturn.transform.position - transform.position;
                dir = dir.normalized;
                rb.AddForce(dir * forceReturnSpeed);
            }
                
                if (noFuseGameMode && calledBack && !wasCountered)
                {
                dangerRang = true;
                if (rb.velocity.magnitude < speedCap)
                    {
                        Vector3 dir = playerToReturn.transform.position - transform.position;
                        dir = dir.normalized;
                        rb.AddForce(dir * forceReturnSpeed);
                    } else if (rb.velocity.magnitude >= speedCap)
                    {
                        Vector3 dir = playerToReturn.transform.position - transform.position;
                        dir = dir.normalized;
                        rb.AddForce(dir * 1);
                    }
                }
            else if (noFuseGameMode && calledBack && wasCountered)
            {
                dangerRang = true;
                if (rb.velocity.magnitude < speedCap)
                {
                    Vector3 dir = playerToReturn.transform.position - transform.position;
                    dir = dir.normalized;
                    rb.AddForce(dir * forceReturnSpeed);
                }
                else if (rb.velocity.magnitude >= speedCap)
                {
                    Vector3 dir = playerToReturn.transform.position - transform.position;
                    dir = dir.normalized;
                    rb.AddForce(dir * 1);
                }
            }

        }

        //No fuse game mode behavior
        if (noFuseGameMode)
        {
            timeToStartReturning -= Time.deltaTime;
            if (timeToStartReturning <= 0 && !assignTarget)
            {
                assignTarget = true;
                dangerRang = true;
                timeToStartReturning = iniTimeToStartReturning;
            }
            if (dangerRang && assignTarget)
            {
                if (!wasCountered)
                {
                    if (rb.velocity.magnitude < speedCap)
                    {
                        Vector3 dir = playerToReturn.transform.position - transform.position;
                        dir = dir.normalized;
                        rb.AddForce(dir * forceReturnSpeed);
                    }
                    else if (rb.velocity.magnitude >= speedCap)
                    {
                        Vector3 dir = playerToReturn.transform.position - transform.position;
                        dir = dir.normalized;
                        rb.AddForce(dir * 1);
                    }
                }

                if (wasCountered)
                {
                    if(timeToStartReturning <= 0)
                    {
                        if (rb.velocity.magnitude < speedCap)
                        {
                            Vector3 dir = playerToReturn.transform.position - transform.position;
                            dir = dir.normalized;
                            rb.AddForce(dir * forceReturnSpeed);
                        }
                        else if (rb.velocity.magnitude >= speedCap)
                        {
                            Vector3 dir = playerToReturn.transform.position - transform.position;
                            dir = dir.normalized;
                            rb.AddForce(dir * 1);
                        }
                    }
                }

            }
           
        }
        

    }

    void Explode()
    {
        GameObject explosionClone = Instantiate(explosion, transform.position, transform.rotation);
        Explosion explodeScript = explosionClone.GetComponent<Explosion>();
        explodeScript.explosionForce = explosionForce;
        explodeScript.explosionR = explosionR;
        explodeScript.explosionUpwards = explosionUpwards;
        explodeScript.parentBomberangMaterial = bomberangMeshes[0].material;
        explosionClone.transform.localScale = explosionRadius;
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if(collision.collider.tag == "Wall")
        {
            dangerRang = true;
            fuse = halfFuse * 2;
            assignTarget = false;
            calledBack = false;
        } if(collision.collider.tag == "ExplosiveWall")
        {
            Explode();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player") && dangerRang)
        {
            Explode();
        }
    }

    public void CounterRangColor()
    {
        foreach (MeshRenderer meshes in bomberangMeshes)
        {
            meshes.material = playerToReturn.GetComponent<MeshRenderer>().material;
            assignedNewMat = true;
        }
    }
}
