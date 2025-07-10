using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float closeRange;
    private NavMeshAgent agent;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        float magnitude = (target.position - transform.position).magnitude;
        if(magnitude > closeRange)
            agent.SetDestination(target.position);
        else
            agent.SetDestination(transform.position);
        Rotate();
    }

    private void Rotate()
    {
        Vector3 sawDir = (transform.position - target.position).normalized;
        Vector3 rot = Quaternion.Euler(0, 0, 270) * sawDir;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, rot);
    }

}
