using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SmartEnemyScript : MonoBehaviour
{
    [SerializeField] private ProjectileShooting ProjectileShooting;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;
    [SerializeField] private Transform dirArrow;
    [SerializeField] private GameObject Proj;
    [SerializeField] LayerMask bounceLayers;
    [SerializeField] LayerMask hideoutLayers;
    [SerializeField] LayerMask runLayers;
    private bool canShoot;

    private Vector3 lastPos;
    IEnumerator Shoot()
    {
        canShoot = false;
        ProjectileShooting.Shoot(Proj, dirArrow.position, lastPos);
        yield return new WaitForSeconds(1f);
        canShoot=true;
    }
    private void Start()
    {
        ProjectileShooting = GameObject.FindAnyObjectByType<ProjectileShooting>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        canShoot = true;
    }
    void FindBestHideout()
    {
        Vector2 enemyPos = transform.position;

        for (float angle = 0; angle < 360; angle += 5)
        {
            Vector2 testDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            RaycastHit2D hit = Physics2D.Raycast(enemyPos, testDirection, 50, hideoutLayers); 
            if (hit.collider != null && ((1 << hit.collider.gameObject.layer) & hideoutLayers) != 0)
            {
                agent.SetDestination(hit.transform.position + (hit.transform.position - transform.position).normalized);
            }
        }

    }

    void FindBestRicochet()
    {
        Vector2 enemyPos = transform.position;

        for (float angle = 0; angle < 360; angle += 5)
        {
            Vector2 testDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            RaycastHit2D hit = Physics2D.Raycast(enemyPos, testDirection, 50, bounceLayers);
            if (hit.collider != null && !hit.collider.CompareTag("Player"))
            {
                Vector2 normal = hit.normal;

                Vector2 reflectedDir = Vector2.Reflect(testDirection, normal);
                RaycastHit2D secondHit = Physics2D.Raycast(hit.point, reflectedDir, 50);
                if (secondHit.collider != null && secondHit.collider.CompareTag("Player"))
                {
                    Debug.DrawRay(enemyPos, testDirection * 50, Color.red, 1f);
                    Debug.DrawRay(hit.point, reflectedDir * 50, Color.blue, 1f);
                    lastPos = hit.point;
                    Vector3 sawDir = (lastPos - transform.position).normalized;
                    Vector3 rot = Quaternion.Euler(0, 0, 90) * sawDir;
                    transform.rotation = Quaternion.LookRotation(Vector3.forward, rot);
                    break;
                }
            }
        }
    }

    private void Update()
    {
        Vector2 dir = player.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir.normalized, dir.magnitude, runLayers);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            Debug.DrawRay(transform.position, dir.normalized * dir.magnitude, Color.cyan, 1f);
            FindBestHideout();
        }
        else
        {
            FindBestRicochet();
        }

        if(canShoot)
        {
            StartCoroutine(Shoot());
        }
    }

}
