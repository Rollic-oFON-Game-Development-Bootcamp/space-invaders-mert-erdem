using UnityEngine;

public class HiveManager : MonoBehaviour
{
    private static HiveManager _instance;
    public static HiveManager Instance => _instance ?? (_instance = FindObjectOfType<HiveManager>());

    [SerializeField] private GameObject hive;
    [SerializeField] private Transform spawnPoint;
    private Vector3 firstSpawnPoint;
    private short startPosChangeCount = 0;
    public bool canSpawn;


    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        canSpawn = true;
        firstSpawnPoint = spawnPoint.position;

        GameManager.Instance.ActionGameStart += SpawnHive;
        GameManager.Instance.ActionGameOver += StopSpawning;
    }

    public void SpawnHive()//tüm küme yok edildiğinde çağırılır
    {
        if (!canSpawn) return;

        if (startPosChangeCount == 3)
            spawnPoint.position = firstSpawnPoint;

        Instantiate(hive, spawnPoint.position, Quaternion.identity);

        spawnPoint.position = new Vector3(0, spawnPoint.position.y - 1, 0);
        startPosChangeCount++;
    }

    private void StopSpawning() => canSpawn = false;

    private void OnDestroy()
    {
        GameManager.Instance.ActionGameStart -= SpawnHive;
        GameManager.Instance.ActionGameOver -= StopSpawning;
    }
}
