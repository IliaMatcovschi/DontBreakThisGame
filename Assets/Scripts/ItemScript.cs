using UnityEngine;

public class ItemScript : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private int countOfReflections;
    [SerializeField] private int effect;
    [SerializeField] private StatsManager statsManager;
    private void Start()
    {
        statsManager = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<StatsManager>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && other.CompareTag("Player"))
        {
            statsManager.Damage += damage;
            statsManager.CountOfReflections += countOfReflections;
            statsManager.effect = effect;
            if(effect == 0)
            {
                statsManager.gameObject.GetComponent<DeadBehaviour>().chooseDead = deads.QuitDead; 
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
