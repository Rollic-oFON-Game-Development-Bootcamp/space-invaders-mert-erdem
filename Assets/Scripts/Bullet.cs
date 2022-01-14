using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private float speed;


    void Start()
    {
        rigidBody.velocity = transform.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Border") || collision.CompareTag("Base"))
            Destroy(gameObject);

        if ((collision.CompareTag("Enemy") && gameObject.CompareTag("Bullet")) ||
            collision.CompareTag("Bullet") || collision.CompareTag("BulletEnemy"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
