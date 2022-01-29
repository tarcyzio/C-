using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    [Range(10, 1000)]
    public float environmentRange;
    public float randomRange = 10;

    public GameObject yellowCube;

    public GameObject blueCube;

    public GameObject greenCube;

    public GameObject redCube;

    public int yellowCubeCount;

    public int blueCubeCount;

    public int greenCubeCount;

    public int redCubeCount;

    private float redCubeSpawn = 0;

    public float redCubeSpawnRate = 3;

    private int reproductionGreenCube = 0;

    private int reproductionBlueCube = 0;

    private int reproductionYellowCube = 0;

    Vector3 spawnPosVec;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < redCubeCount; i++)
        {
            SpawnAnimal(redCube);
        }
        for (int i = 0; i < greenCubeCount; i++)
        {
            SpawnAnimal(greenCube);
        }
        for (int i = 0; i < blueCubeCount; i++)
        {
            SpawnAnimal(blueCube);
        }
        for (int i = 0; i < yellowCubeCount; i++)
        {
            SpawnAnimal(yellowCube);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(reproductionGreenCube > 0)
        {
            SpawnAnimalReproduction(greenCube, spawnPosVec);
            reproductionGreenCube--;
        }
        if(reproductionBlueCube > 0)
        {
            SpawnAnimalReproduction(blueCube, spawnPosVec);
            reproductionBlueCube--;
        }
        if(reproductionYellowCube > 0)
        {
            SpawnAnimalReproduction(yellowCube, spawnPosVec);
            reproductionYellowCube--;
        }
        redCubeSpawn += Time.deltaTime;
        if (redCubeSpawn > redCubeSpawnRate)
        {
            SpawnAnimal(redCube);
            redCubeSpawn += -redCubeSpawnRate;
        }
    }
/*
    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawCube(Vector3.zero, new Vector3(environmentRange * 2, 1, environmentRange * 2));
    }
*/
    public float getRandomNum()
    {
        float num = Random.Range(-randomRange, randomRange);
        return num; 
    }

    public Vector3 getRandomMovPosition(Vector3 targetPositionMov)
    {
        targetPositionMov.x += getRandomNum();
        targetPositionMov.z += getRandomNum();
        return targetPositionMov;
    }

    public Vector3 getRandomSpawnPosition()
    {
        Vector3 position = new Vector3(Random.Range(-environmentRange, environmentRange), 5, Random.Range(-environmentRange, environmentRange));

        if (!Physics.Raycast(position, Vector3.down, 10))
        {
            position = getRandomSpawnPosition();
        }

        return position;
    }

    public Vector3 getRunAwayPosition(Transform target, Vector3 targetPositionMov)
    {
        Vector3 position = Vector3.zero;

        float currentDistance = 1;

        for (int i = 0; i < 20; i++)
        {
            Vector3 tryPosition = getRandomMovPosition(targetPositionMov);
            float dist = Vector3.Distance(tryPosition, target.position);

            if (dist > currentDistance)
            {
                position = tryPosition;
                currentDistance = dist;
            }
        }
        return position;
    }

    public void SpawnAnimal(GameObject prefab)
    {
        Vector3 spawnPoint = getRandomSpawnPosition();

        Instantiate(prefab, spawnPoint, Quaternion.identity);
    }

    public void SpawnAnimalReproduction(GameObject prefab, Vector3 spawnPosVec)
    {
        Vector3 spawnPoint = spawnPosVec;

        Instantiate(prefab, spawnPoint, Quaternion.identity);
    }

    public void moreReproduction(string tag, Vector3 spawnPos)
    {
        string rep = "reproduction" + tag;
        spawnPosVec = spawnPos;
        if(rep == "reproductionGreenCube")
        {
            reproductionGreenCube++;
        }
        else if(rep == "reproductionBlueCube")
        {
            reproductionBlueCube++;
        }
        else if(rep == "reproductionYellowCube")
        {
            reproductionYellowCube++;
        }
    }

}
