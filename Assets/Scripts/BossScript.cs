using System.Collections;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public enum attacks
{
    TreeThrow = 0,
    AllTreesPush = 1,
    RUN = 2
}
public class BossScript : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private NavMeshPlus.Components.NavMeshSurface NavMesh;
    [SerializeField] private StatsManager bossesStats;
    [SerializeField] public StatsManager playersStats;
    [SerializeField] private PlayerMovement PM;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject fallenTree;
    [SerializeField] private LayerMask pushLayers;
    public attacks attack;
    [SerializeField] private float throwDelay;
    [SerializeField] private float pushDelay;
    [SerializeField] private float smashDelay;

    [SerializeField] private float damage;
    public float pushForce = 10f; 
    public float pushDistance = 2f;
    public Vector2 pushBoxSize = new Vector2(2f, 1f); 
    private bool canAttack;
    private bool canHit;

    IEnumerator Smash()
    {
        canHit = false;
        yield return new WaitForSeconds(smashDelay);
        playersStats.HP = playersStats.HPdown(damage);
        canHit = true;
    }
    public IEnumerator GetTree(GameObject tree)
    {
        Destroy(tree);
        RotateToPlayer();
        Debug.Log("2");
        yield return new WaitForSeconds(throwDelay);
        ThrowTree();
    }
    IEnumerator PushTree()
    {
        RotateToPlayer();
        yield return new WaitForSeconds(pushDelay);
        Vector2 pushOrigin = (Vector2)transform.position + (Vector2)transform.right * (pushDistance / 2);
        Collider2D[] cols = Physics2D.OverlapBoxAll(pushOrigin, pushBoxSize, transform.rotation.eulerAngles.z, pushLayers);
        foreach (Collider2D col in cols)
        {
            Vector2 Dir = col.transform.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Dir.normalized, Mathf.Infinity, 1 << 10);
            if (hit.collider != null)
            {
                if (!hit.collider.CompareTag("Player") || PM.vulnerable)
                {
                    col.transform.position = hit.point;
                }
            }
        }
        
        if(cols.Length > 0)
            cols[(cols.Length - 1) / 2].transform.position = player.position;
        if (PM.vulnerable)
        {
            playersStats.HP = playersStats.HPdown(damage);
        }
        canAttack = true;
    }
    private bool findigTree;
    private void Awake()
    {
    }
    private void Start()
    {
        playersStats = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<StatsManager>();
        PM = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        NavMesh = GameObject.FindAnyObjectByType<NavMeshPlus.Components.NavMeshSurface>().GetComponent<NavMeshPlus.Components.NavMeshSurface>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        bossesStats = GetComponent<StatsManager>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        canAttack = true;
        canHit = true;
    }
    void Attack()
    {
        canAttack = false;
        switch(attack)
        {
            case attacks.TreeThrow:
                findigTree = true;
                return;
            case attacks.AllTreesPush:
                StartCoroutine(PushTree());
                return;
            case attacks.RUN:
                Run();
                return;
        }

    }
    void FindTree()
    {
        findigTree = false;
        Vector2 enemyPos = transform.position;

        for (float angle = 0; angle < 360; angle += 1)
        {
            Vector2 testDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            RaycastHit2D hit = Physics2D.Raycast(enemyPos, testDirection, 25, 1 << 9);
            Debug.DrawRay(enemyPos, testDirection * 50f, Color.red, 3f);
            if (hit.collider != null && hit.collider.CompareTag("BossTree"))
            {
                agent.SetDestination(hit.point);
                Debug.DrawRay(enemyPos, testDirection * 50f, Color.blue, 3f);
                return;
            }
        }
        canAttack = true;
    }
    void ThrowTree()
    {
        Instantiate(fallenTree, player.position, player.rotation);
        if(PM.vulnerable)
        {
            playersStats.HP = playersStats.HPdown(damage);
        }
        NavMesh.BuildNavMesh();
        canAttack = true;
        Debug.Log("3");
    }

    void Run()
    {
        agent.SetDestination(player.position);
    }

    void RotateToPlayer()
    {
        Vector3 sawDir = (player.position - transform.position).normalized;
        Vector3 rot = Quaternion.Euler(0, 0, 90) * sawDir;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, rot);
    }

    private void Update()
    {
        if(canAttack)
        {
            if (bossesStats.HP < 10)
            {
                attack = attacks.RUN;
                Attack();
            }
            else
            {
                attack = (attacks)Random.Range(0, 2);
                Attack();
            }
        }
        if (findigTree)
        {
            FindTree();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (attack == attacks.TreeThrow && other.CompareTag("BossTree"))
        {
            Debug.Log("1");
            StartCoroutine(GetTree(other.gameObject));
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (attack == attacks.RUN && other.CompareTag("Player") && canHit)
        {
            StartCoroutine(Smash());
        }
    }
}
