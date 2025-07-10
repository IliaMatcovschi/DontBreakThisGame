using System.Threading;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    [SerializeField] private DeadBehaviour DeadBehaviour;
    public float HP;
    public float MaxHP;
    public float Damage;
    public float Size;
    public int CountOfReflections;
    public int effect;

    private void Start()
    {
        DeadBehaviour = GetComponent<DeadBehaviour>();
    }
    public float HPdown(float damage)
    {
        float r = HP - damage;
        if (r > 0)
        {
            return r;
        }
        else
        {
            return 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        SawScript SS = other.GetComponent<SawScript>();
        if (other.gameObject.layer == 6 && other.gameObject != null)
        {
            HP = HPdown(other.gameObject.GetComponent<SawScript>().Damage);
            if (HP <= 0)
            {
                DeadBehaviour.Dead(Quaternion.Euler(0, 0, 90) * SS.transform.right.normalized);
            }
            SS.CountOfReflections--;
            if(SS.CountOfReflections < 0)
            {
                Destroy(SS.gameObject);
            }
        }
    }
}
