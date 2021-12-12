using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ForestChunkData")]
public class ForestChunkData : ScriptableObject
{

    public Vector2 chunkSize = new Vector2(10f, 10f);

    public GameObject[] forestChunks;  //Table of different versions of the same chunk (example : straight path in the forest, but different trees placement
}
