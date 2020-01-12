using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public Spawner[] spawners;
    public int numOfEnemyesToStartSpawn = 3;
    public int round = 0;
    public bool nextRound = false;

    private float roundMultiplier = 1.5f;
    private int numOfEnemyesThisRound;
    private bool specialRound = false;
    private bool specialRoundDone = false;

    private void FixedUpdate()
    {
        if (round == 2)
        {
            specialRound = true;
        }
    }

    public void StartSpawners()
    {
        if(specialRound)
        {
            for (int i = 0; i < spawners.Length; i++)
            {
                spawners[i].SpawnSpecial();
            }
            specialRound = false;
            specialRoundDone = true;
            return;
        }

        numOfEnemyesThisRound = (int)(numOfEnemyesToStartSpawn + (round * roundMultiplier)) / spawners.Length;
        for (int i = 0; i < spawners.Length; i++)
        {
            for (int j = 0; j <= numOfEnemyesThisRound; j++)
            {
                spawners[i].SpawnEnemy();
            }

            if (specialRoundDone)
                spawners[i].SpawnSpecial();
        }
    }
}
