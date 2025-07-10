using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] GameObject[] gameObjects;
    [SerializeField] Transform[] transforms;
    private bool canSpawn = true;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other != null && other.CompareTag("Player") && canSpawn)
        {
            for(int i = 0; i < gameObjects.Length; i++)
            {
                Instantiate(gameObjects[i], transforms[i]);
            }
        }
        canSpawn = false;
    }

}
