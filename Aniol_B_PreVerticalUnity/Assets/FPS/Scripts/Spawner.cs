using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpawnController))]
public class Spawner : MonoBehaviour
{
    //Public Variables
    public GameObject[] prefabToSpawn;
    public GameObject specialEnemy;
    public bool activeSpawner = false;

    public void SpawnEnemy()
    {
        Instantiate(prefabToSpawn[Random.Range(0, 3)], transform.position, Quaternion.identity);
        return;
    }

    public void SpawnSpecial()
    {
        Instantiate(specialEnemy, transform.position, Quaternion.identity);
        return;
    }
}
