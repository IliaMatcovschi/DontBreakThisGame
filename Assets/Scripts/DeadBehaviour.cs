using NavMeshPlus.Components;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
public enum deads
{
    PlayerDead = 0,
    TreeDead = 1,
    EnemyDead = 2,
    QuitDead = 3,
}

public class DeadBehaviour : MonoBehaviour
{
    [SerializeField] public deads chooseDead;
    [SerializeField] private GameObject DeadPrefab;
    [SerializeField] private Transform RespawnPoint;
    [SerializeField] public NavMeshPlus.Components.NavMeshSurface NavMesh;
    private void Start()
    {
        NavMesh = GameObject.FindAnyObjectByType<NavMeshPlus.Components.NavMeshSurface>().GetComponent<NavMeshPlus.Components.NavMeshSurface>();
    }
    public void Dead(Vector3 prefabDir)
    {
        switch(chooseDead)
        {
            case deads.PlayerDead:
                GameObject.FindGameObjectWithTag("Player").transform.position = RespawnPoint.position;
                GetComponent<StatsManager>().HP = GetComponent<StatsManager>().MaxHP;
                return;
            case deads.TreeDead:
                Instantiate(DeadPrefab, transform.position, Quaternion.LookRotation(Vector3.forward, prefabDir));
                NavMesh.BuildNavMesh();
                Destroy(gameObject);
                return;
            case deads.EnemyDead:
                Destroy(gameObject);
                return;
            case deads.QuitDead:
                Application.Quit();
                return;
        }
    }

}
