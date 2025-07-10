using UnityEngine;

public class ProjectileShooting : MonoBehaviour
{
    public void Shoot(GameObject prefab, Vector3 from, Vector3 target)
    {
        Vector3 sawDir = (target - from).normalized;
        Vector3 rot = Quaternion.Euler(0, 0, 90) * sawDir;
        Instantiate(prefab, from, Quaternion.LookRotation(Vector3.forward, rot));
    }
}
