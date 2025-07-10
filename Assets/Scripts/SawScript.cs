using UnityEngine;

public class SawScript : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private StatsManager SM;
    public float Damage;
    public float Size;
    public float speed;
    public int CountOfReflections;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SM = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<StatsManager>();
        if (SM != null)
        {
            Damage = SM.Damage;
            Size = SM.Size;
            CountOfReflections = SM.CountOfReflections;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + transform.right * speed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal;
        Vector2 newDir = Vector2.Reflect(transform.right, normal);
        float angle = Mathf.Atan2(newDir.y, newDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        if (CountOfReflections <= 0)
        {
            Destroy(gameObject);
        }
        if (SM.effect == 1)
        {
            if ((Size += SM.Size) < 30)
            {
                Size += SM.Size;
                transform.localScale = new Vector3(Size, Size, Size);
            }
            else
            {
                Application.Quit();
            }
        }
        CountOfReflections--;
    }
}
