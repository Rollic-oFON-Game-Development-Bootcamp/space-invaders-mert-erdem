using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyHive enemyHive;
    [SerializeField] private Transform cannon;
    [SerializeField] private GameObject bullet;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite explosionSprite;

    private void Start() => enemyHive.JoinHive(this);

    public void Fire()
    {
        Instantiate(bullet, cannon.position, Quaternion.Euler(0, 0, 180));
    }

    private void Die() => enemyHive.LeaveHive(this);

    private IEnumerator Explosion()
    {
        spriteRenderer.sprite = explosionSprite;

        yield return new WaitForSeconds(0.2f);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Bullet"))
        {
            Die();
            Destroy(collision.gameObject);
            GameManager.Instance.AddScore();
            AudioSource.PlayClipAtPoint(hitSound, Vector2.zero, 1);

            StartCoroutine(Explosion());                
        }

        if (collision.CompareTag("Border"))
            enemyHive.ChangeDirection();

        if(collision.CompareTag("Base") && !GameManager.Instance.GameOver)
        {
            GameManager.Instance.ActionGameOver?.Invoke();
        }
    }
}
