using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private int health = 3;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("BulletEnemy"))
        {
            health--;
            if (health == 0)
                Destroy(gameObject);
        }

        if (collision.CompareTag("Enemy"))
            Destroy(gameObject);
    }
}
