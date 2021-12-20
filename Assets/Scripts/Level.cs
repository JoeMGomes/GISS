using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Level/LevelTimeline", order = 1)]
public class Level : ScriptableObject
{
    [System.Serializable]
    public struct SpawnBlock
    {
        public GameObject block;
        public float spawnPosition_X;
        public float timeToSpawn;
    }

    public string LevelName;
    public List<SpawnBlock> levelBlocks;
}
