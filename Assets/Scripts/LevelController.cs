using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;

public class LevelController : MonoBehaviour
{
    public int health = 3;

    public GameObject cubePrefab;
    public GameObject longCubePrefab;
    public GameObject cylinderPrefab;

    //Manage Each Level
    public Level[] levelList;
    private Level currLevel;
    private int currLevelIndex = 0;

    //Manage Current level
    private int currentBlockIndex = 0;
    private int levelBlockSize;
    private List<GameObject> existingBlocks;


    //Manage UI
    public GameObject[] healthBar;
    public GameObject countdownText;
    public GameObject winMenu;
    public GameObject lostMenu;


    private static LevelController _instance;

    public static LevelController Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("LevelController");
                go.AddComponent<LevelController>();
            }

            return _instance;
        }
    }

    //Sound
    public SoundAudioClip[] audioClips; //This should be a map for faster access
    [System.Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }

    void Awake()
    {
        _instance = this;

        SceneManager.LoadScene(1, LoadSceneMode.Additive);

        if (levelList == null)
        {
            Debug.LogError("No levels assigned to play");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        if (healthBar == null)
        {
            Debug.LogError("No health bar");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        if (countdownText == null)
        {
            Debug.LogError("No countdown text");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        existingBlocks = new List<GameObject>();
        currLevel = levelList[currLevelIndex];
        countdownText.SetActive(false);
        foreach (GameObject a in healthBar)
        {
            a.SetActive(false);
        }
    }



    // Start is called before the first frame update
    void Start()
    {

        levelBlockSize = currLevel.levelBlocks.Count;
    }

    void StartLevel()
    {
        foreach (GameObject a in healthBar)
        {
            a.SetActive(true);
        }
        countdownText.SetActive(true);


        StartCoroutine(CountdownToStart());
    }

    IEnumerator CountdownToStart()
    {
        string[] numbers = { "3", "2", "1", "Go!" };

        TMP_Text countText = countdownText.GetComponent<TMP_Text>();

        for (int i = 0; i < numbers.Length; i++)
        {
            countText.text = numbers[i];
            yield return new WaitForSecondsRealtime(1);
        }

        countdownText.SetActive(false);

        StartCoroutine(PlayNextBlock());
    }

    IEnumerator PlayNextBlock()
    {
        Level.SpawnBlock currBlock = currLevel.levelBlocks[currentBlockIndex];
        float timeToWait = currBlock.timeToSpawn;
        yield return new WaitForSeconds(timeToWait);

        GameObject block = SpawnBlock(currBlock.block, currBlock.spawnPosition_X);
        existingBlocks.Add(block);
        currentBlockIndex++;
        if (currentBlockIndex < levelBlockSize)
            StartCoroutine(PlayNextBlock());
        else 
        {
            //Wait for all blocks to be destroyed
            while (existingBlocks.Count > 0)
            {
                yield return null;
            }

            SoundManager.Instance.PlaySound(SoundManager.Sound.GameWin);

            //Set win UI
            winMenu.SetActive(true);
            foreach (GameObject a in healthBar)
            {
                a.SetActive(false);
            }
        }
    }

    GameObject SpawnBlock(GameObject block, float x_position)
    {
        return Instantiate(block, new Vector3(x_position, 24, 0), Quaternion.identity);
    }

    //Debug only, used to spawn Obstacles on click
    void Spawn(GameObject g)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray.origin, ray.direction * 20, out hit))
        {

            if (hit.transform.gameObject.CompareTag("Background"))
            {
                Vector3 spawPosition = hit.point;
                spawPosition.y = 24f;
                Instantiate(g, spawPosition, Quaternion.identity);
            }
        }
    }

    public void loseLife()
    {
        health--;
        SoundManager.Instance.PlaySound(SoundManager.Sound.LoseLife);

        if (health > 0)
        {
            healthBar[health].SetActive(false);
        }
        else
        {
            //Stops the game loop
            StopAllCoroutines();

            //destroy and clear all existing blocks and the list
            foreach(GameObject g in existingBlocks)
            {
                Destroy(g);
            }
            existingBlocks.Clear();

            //Reset UI
            lostMenu.SetActive(true);
            foreach (GameObject a in healthBar)
            {
                a.SetActive(false);
            }
            SoundManager.Instance.PlaySound(SoundManager.Sound.GameLost);

        }

    }

    public void removeBlockFromLevel(GameObject b)
    {
        existingBlocks.Remove(b);
    }


    public void RestartLevel()
    {
        currentBlockIndex = 0;
        health = 3;

        foreach (GameObject a in healthBar)
        {
            a.SetActive(true);
        }
        lostMenu.SetActive(false);
        PlatformController.Instance.resetPlatforms();
        StartLevel();
    }


    public void LoadLevel(int level)
    {
        currentBlockIndex = level;
        currLevel = levelList[level];
        levelBlockSize = currLevel.levelBlocks.Count;
        RestartLevel();
        StartLevel();
    }

    public void NextLevel()
    {
        currLevelIndex++;
        LoadLevel(currLevelIndex);
    }


    public void QuitGame()
    {
        Debug.Log("QUIT GAME");
        Application.Quit();
    }
}
