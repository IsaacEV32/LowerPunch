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
    [SerializeField] int numberOfNormalEnemies = 5;
    [SerializeField] int numberOfHeavyEnemies = 2;

    internal int numberOfEnemiesInScene;

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
            spawnPointsActived[i].SetActive(true);
        }
        for (int i = 0; i < numberOfNormalEnemies; i++)
        {
            tmpNE = Instantiate(normalEnemy);
            tmpNE.SetActive(false);
            normalEnemies.Add(tmpNE);
        }
        for (int i = 0; i < numberOfHeavyEnemies; i++)
        {
            tmpHE = Instantiate(heavyEnemy);
            tmpHE.SetActive(false);
            heavyEnemies.Add(tmpHE);
        }
        SelectEnemyToSpawn();
    }
    void SelectEnemyToSpawn()
    {
        float probability = Random.value;
        Debug.Log(probability);
        if (probability > 0.25)
        {
            SpawnEnemies(TypeOfEnemy.NormalEnemy);
        }
        else if (probability < 0.25)
        {
            SpawnEnemies(TypeOfEnemy.HeavyEnemy);
        }
        if (numberOfEnemiesInScene < 3)
        {
            SelectEnemyToSpawn();
        }
    }
    void SpawnEnemies(TypeOfEnemy e)
    {
        if (e == TypeOfEnemy.NormalEnemy)
        {
            for (int i = 0; i < normalEnemies.Count; i++)
            {
                if (!normalEnemies[i].activeInHierarchy)
                {
                    normalEnemies[i].transform.position = spawnPointsActived[Random.Range(0, spawnPointsActived.Count)].transform.position;
                    normalEnemies[i].SetActive(true);

                    numberOfEnemiesInScene += 1;
                    break;
                }
            }
        }
        else if (e == TypeOfEnemy.HeavyEnemy)
        {
            for (int i = 0; i < heavyEnemies.Count; i++)
            {
                if (!heavyEnemies[i].activeInHierarchy)
                {
                    heavyEnemies[i].transform.position = spawnPointsActived[Random.Range(0, spawnPointsActived.Count)].transform.position;
                    heavyEnemies[i].SetActive(true);

                    numberOfEnemiesInScene += 1;
                    break;
                }
            }
        }
    }
    internal void EnemyKilled()
    {
        numberOfEnemiesInScene--;
        if (numberOfEnemiesInScene < 3)
        {
            SelectEnemyToSpawn();
        }
    }
}