using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyHive enemyHive;
    [SerializeField] private Transform cannon;
    [SerializeField] private GameObject bullet;

    private void Awake()
    {
        enemyHive.JoinHive(this);
    }

    public void Fire()
    {
        Instantiate(bullet, cannon.position, Quaternion.Euler(0, 0, 180));
    }

    private void OnDestroy()
    {
        enemyHive.LeaveHive(this);
    }
}
