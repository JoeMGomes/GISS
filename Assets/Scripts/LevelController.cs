using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{


    public int health = 3;


    public GameObject cubePrefab;
    public GameObject longCubePrefab;
    public GameObject cylinderPrefab;
    public Level level;

    private int currentBlockIndex = 0;
    private int levelBlockSize;

    public GameObject timerText;
    // Start is called before the first frame update
    void Start()
    {
        if(level == null)
        {
            Debug.LogError("No level assigned to play");
            UnityEditor.EditorApplication.isPlaying = false;
        }

        levelBlockSize = level.levelBlocks.Count;
    }

    void StartLevel()
    {
        timerText.GetComponent<TimerText>().startTimer();
        StartCoroutine(PlayNextBlock());
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartLevel();
        }

        /// DEBUG ONLY
        if (Input.GetKeyDown(KeyCode.R))
        {
            Spawn(cubePrefab);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Spawn(longCubePrefab);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Spawn(cylinderPrefab);
        }
    }

    IEnumerator PlayNextBlock()
    {
        Level.SpawnBlock currBlock = level.levelBlocks[currentBlockIndex];
        float timeToWait = currBlock.timeToSpawn;
        yield return new WaitForSeconds(timeToWait);
        SpawnBlock(currBlock.block, currBlock.spawnPosition_X);
        currentBlockIndex++;
        if(currentBlockIndex < levelBlockSize)
            StartCoroutine(PlayNextBlock());
    }

    void SpawnBlock(GameObject block, float x_position)
    {
        Instantiate(block, new Vector3(x_position,10,0), Quaternion.identity);
    }

    void Spawn(GameObject g)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray.origin, ray.direction * 20, out hit))
        {

            if (hit.transform.gameObject.CompareTag("Background"))
            {
                Vector3 spawPosition = hit.point;
                spawPosition.y = 6.5f;
                Instantiate(g, spawPosition, Quaternion.identity);
            }
        }
    }
}
