using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestLayoutGenerator : MonoBehaviour
{//Script meant to be attached to the LevelGenerator, generate the forest and all the others gameObject infinitely

    public ForestChunkData[] forestChunkData;
    public ForestChunkData firstChunk;
    public ForestChunkData nextChunk;  //Provisional, for the moment there is only one type of chunk

    private ForestChunkData previousChunk;

    public Vector3 spawnOrigin;

    private Vector3 spawnPosition;

    public int chunksToSpawn = 5;  //Number of chunks to spawn when the game starts

    public List<EnemyMotor> enemies;

    private System.Random rand = new System.Random();  //Useful to add randomness in the spawns

    private Vector3 thugDistance;
    private int numberOfSpiderOnThisChunk;

    void OnEnable()
    {
        TriggerExit.OnChunkExited += PickAndSpawnChunk;
    }

    private void OnDisable()
    {
        TriggerExit.OnChunkExited -= PickAndSpawnChunk;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))  //Useless for the game, useful for the developers to help edition and do tests when the game is running
        {
            PickAndSpawnChunk();
        }
    }

    void Start()
    {
        previousChunk = firstChunk;

        for (int i = 0; i < chunksToSpawn; i++)
        {
            PickAndSpawnChunk();
        }
    }

    ForestChunkData PickNextChunk()  //To be made more complex if you have several types of chunk and different cases
    {
        spawnPosition = spawnPosition + new Vector3(0f, 0f, previousChunk.chunkSize.y);  //  /!\ Here "y" coresponds to the coordinate "z" in the worldspace
        return nextChunk;
    }

    void PickAndSpawnChunk()
    {
        ForestChunkData chunkToSpawn = PickNextChunk();

        GameObject objectFromChunk = chunkToSpawn.forestChunks[Random.Range(0, chunkToSpawn.forestChunks.Length)]; //Select one of the versions
        previousChunk = chunkToSpawn;
        Instantiate(objectFromChunk, spawnPosition + spawnOrigin, Quaternion.identity);
        
        if (enemies.Count > 0) // enemies[0] = thug ; enemies[1] = spider ; enemies[2] = eagle
        {
            if (rand.Next(5) < 4)
            {
                thugDistance = rand.Next(30) * Vector3.forward - 15 * Vector3.forward; //This vector give a random place on the path of the new chunk spawned
                Instantiate(enemies[0], spawnPosition + spawnOrigin + thugDistance, Quaternion.Euler(0f, 180f, 0f)); //Instantiates a thug
            }            
            
            if (enemies[1])
            {
                numberOfSpiderOnThisChunk = rand.Next(3);

                for (int i = 0; i <= numberOfSpiderOnThisChunk; i++)
                {
                    if (rand.Next(2) == 1)
                    {
                        Instantiate(enemies[1], spawnPosition + spawnOrigin + 10 * Vector3.right + 5* i * Vector3.forward, Quaternion.Euler(0f, -90f, 0f)); //Instantiate a spider on the right       
                    }
                    else
                    {
                        Instantiate(enemies[1], spawnPosition + spawnOrigin + 10 * Vector3.left + 5 * i * Vector3.forward, Quaternion.Euler(0f, 90f, 0f)); //Instantiate a spider on the left
                    }
                }                
            }  
            
            if (enemies[2])
            {
                //if (rand.Next(2) < 1)
                //{
                    Instantiate(enemies[2], spawnPosition + spawnOrigin + 5 * Vector3.up + 30 * Vector3.forward, Quaternion.Euler(0f, 180f, 0f)); //Instantiate an eagle
                //}
            }
        }                
    }

    public void UpdateSpawnOrigin(Vector3 originDelta)  //Update needed because of the Floating Origin
    {
        spawnOrigin = spawnOrigin + originDelta;
    }

}
