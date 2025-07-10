using UnityEngine;

public class ThrowTreeTrigger : MonoBehaviour
{
    [SerializeField] private BossScript bossScript;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && bossScript.attack == attacks.TreeThrow && other.CompareTag("Tree"))
        {
            Debug.Log("1");
            StartCoroutine(bossScript.GetTree(other.gameObject));
        }
    }
}
