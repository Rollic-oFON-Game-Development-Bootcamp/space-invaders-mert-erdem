using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHive : MonoBehaviour
{
    private List<Enemy> enemies = new List<Enemy>();
    [SerializeField] private CircleCollider2D enemyCollider;
    [Header("Specs")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float speedDelta = 0.001f;
    [SerializeField] private float fireDelta = 2f;
    private float fallingDelta;
    private bool canFire = true, directionChanging = false;


    void Awake()
    {
        fallingDelta = enemyCollider.radius / 2;
    }

    void Update()
    {
        Move();

        if (canFire && enemies.Count > 0)
            StartCoroutine(OrderFire());
    }

    private void Move()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private IEnumerator ChangeDirection()
    {
        directionChanging = true;
        speed *= -1;
        speedDelta *= -1;
        
        transform.transform.position -= new Vector3(0, fallingDelta, 0);

        yield return new WaitForEndOfFrame();

        directionChanging = false;
    }

    public IEnumerator OrderFire()
    {
        canFire = false;
        var selectedEnemy = enemies[Random.Range(0, enemies.Count - 1)];
        selectedEnemy.Fire();

        yield return new WaitForSeconds(fireDelta);

        canFire = true;
    }

    public void JoinHive(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    public void LeaveHive(Enemy enemy)
    {
        enemies.Remove(enemy);

        if(enemies.Count == 0)
        {
            HiveManager.Instance.SpawnHive();
            Destroy(gameObject);
        }

        speed += speedDelta;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Border") && !directionChanging)
        {
            StartCoroutine(ChangeDirection());
        }

        if(collision.CompareTag("Base"))
        {
            //game over
        }
    }
}
