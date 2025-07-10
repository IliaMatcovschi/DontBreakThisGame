using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class EnemyProjectileShooting : MonoBehaviour
{
    [SerializeField] private GameObject Proj;
    [SerializeField] private Transform dirArrow;
    [SerializeField] private ProjectileShooting ProjectileShooting;
    private Transform g;
    private Vector3 shootDir;
    private Vector3 rot;
    private bool canShoot;
    IEnumerator e()
    {
        canShoot = false;
        ProjectileShooting.Shoot(Proj, dirArrow.position, g.position);
        yield return new WaitForSeconds(3);
        canShoot = true;

    }
    private void Start()
    {
        g = GameObject.FindGameObjectWithTag("Player").transform;
        ProjectileShooting = GameObject.FindFirstObjectByType<ProjectileShooting>().GetComponent<ProjectileShooting>();
        canShoot = true;
    }
    void Update()
    {
        if (canShoot)
        {
            StartCoroutine(e());
        }
    }
}
