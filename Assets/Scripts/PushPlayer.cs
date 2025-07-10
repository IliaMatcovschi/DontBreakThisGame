using UnityEngine;

public class PushPlayer : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    private void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        SawScript SS = other.GetComponent<SawScript>();
        if(other.gameObject.layer == 6 && SS != null)
        {
            playerMovement.Push(other.transform.right, SS.speed * 2);
        }
    }
}
