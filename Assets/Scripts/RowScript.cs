using UnityEngine;

public class RowScript : MonoBehaviour
{
    [SerializeField] StatsManager statsManager;
    [SerializeField] Vector3 GetUpPoint;
    [SerializeField] float offset;
    private void Start()
    {
        statsManager = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<StatsManager>();
    }
    private void Update()
    {
        float height = statsManager.transform.position.y;
        Mathf.Clamp(height, transform.position.y - transform.localScale.x, transform.position.y + transform.localScale.x);

        float loc = Mathf.Sign((statsManager.transform.position - transform.position).normalized.x);
        GetUpPoint = new Vector3(statsManager.transform.position.x + offset * loc, height, GetUpPoint.z);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.position = GetUpPoint;
            statsManager.HP = statsManager.HPdown(1);
            if (statsManager.HP <= 0) 
                statsManager.HP = statsManager.MaxHP;
        }
    }
}
