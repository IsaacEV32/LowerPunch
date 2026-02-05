using UnityEngine;
using System.Collections.Generic;
using UnityEditor.TerrainTools;

public class SpawnPointsEnemy : MonoBehaviour
{
    [SerializeField] int numberOfSpawnPoints = 3;
    [SerializeField] List<GameObject> spawnPointsActived = new List<GameObject>();
    [SerializeField] List<GameObject> normalEnemies = new List<GameObject>();
    [SerializeField] List<GameObject> heavyEnemies = new List<GameObject>();
    [SerializeField] GameObject normalEnemy;
    [SerializeField] GameObject heavyEnemy;

    public static SpawnPointsEnemy instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        GameObject tmpNE;
        GameObject tmpHE;
        for (int i = 0; i < numberOfSpawnPoints; i++)
        {
            spawnPointsActived[i].SetActive(false);
            tmpNE = Instantiate(normalEnemy);
            tmpHE = Instantiate(heavyEnemy);
            tmpNE.SetActive(false);
            tmpHE.SetActive(false);
            normalEnemies.Add(tmpNE);
            heavyEnemies.Add(tmpHE);
        }
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < numberOfSpawnPoints; i++)
        {
            if (!spawnPointsActived[i].activeInHierarchy)
            {
                spawnPointsActived[i].SetActive(true);
            }
        }
        for (int i = 0; i < numberOfSpawnPoints; i++)
        {
            if (!normalEnemies[i].activeInHierarchy)
            {
                normalEnemies[i].transform.position = spawnPointsActived[i].transform.position;
                normalEnemies[i].SetActive(true);
            }
           
        }
    }

    public void DeactivateSpawnPoint()
    {
        for (int i = 0; i < numberOfSpawnPoints; i++)
        {
            if (spawnPointsActived[i].activeInHierarchy)
            {
                spawnPointsActived[i].SetActive(false);
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        SpawnEnemies();
    }
}
