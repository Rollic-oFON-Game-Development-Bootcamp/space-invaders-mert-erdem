using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveManager : MonoBehaviour
{
    private static HiveManager _instance;
    public static HiveManager Instance => _instance ?? (_instance = FindObjectOfType<HiveManager>());

    [SerializeField] private GameObject hive;
    [SerializeField] private Transform spawnPoint;
    private Vector3 firstSpawnPoint;
    private short startPosChangeCount = 0;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        firstSpawnPoint = spawnPoint.position;
    }

    public void SpawnHive()//tüm küme yok edildiğinde çağırılır
    {
        if (startPosChangeCount == 3)
            spawnPoint.position = firstSpawnPoint;

        spawnPoint.position = new Vector3(0, spawnPoint.position.y - 1, 0);
        print(spawnPoint.position);
        startPosChangeCount++;

        Instantiate(hive, spawnPoint.position, Quaternion.identity);        
    }
}
