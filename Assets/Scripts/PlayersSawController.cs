using UnityEngine;

public class PlayersSawController : MonoBehaviour
{
    private PlayerMovement PM;
    private ProjectileShooting PS;
    [SerializeField] private Vector3 worldMousePosition;
    [SerializeField] private Vector3 sawDir;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject sawProj;
    [SerializeField] private Transform shootFromObj;
    private void Start()
    {
        PM = GetComponent<PlayerMovement>();
        PS = GameObject.FindFirstObjectByType<ProjectileShooting>().GetComponent<ProjectileShooting>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && PM.vulnerable)
        {
            worldMousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            worldMousePosition.z = 0;
            PS.Shoot(sawProj, shootFromObj.position, worldMousePosition);
        }
    }
}
