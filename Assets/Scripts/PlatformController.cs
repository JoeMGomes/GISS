using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{

    int currentPlat = 0;
    public GameObject platPrefab;
    List<Platform> platformPool;

    public int MAX_PLATFORMS = 3;


    private static PlatformController _instance;

    public static PlatformController Instance
    {
        get
        {   
            if (_instance == null)
            {
                GameObject go = new GameObject("PlatformCotnroller");
                go.AddComponent<PlatformController>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

        platformPool = new List<Platform>();

        if (platPrefab == null)
        {
            Debug.LogError("No platform preset present");
        }
        else
        {
            for (int i = 0; i < MAX_PLATFORMS; i++)
            {
                GameObject p = Instantiate(platPrefab, new Vector3(0, 0, -100), Quaternion.identity);
                Platform s = p.GetComponent<Platform>();
                platformPool.Add(s);

            }
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction * 20, out hit))
            {

                if (hit.transform.gameObject.CompareTag("Background"))
                {
                    onBackGroundHit(hit);
                }
                else if (hit.transform.gameObject.CompareTag("Platform"))
                {
                    hit.transform.gameObject.GetComponent<Platform>().Lock();
                    SoundManager.Instance.PlaySound(SoundManager.Sound.LockPlat);
                }
            }
        }
    }


    public void resetPlatforms()
    {
         foreach(Platform p in platformPool)
        {
            p.gameObject.transform.position = new Vector3(0, 0, -100);
            p.Lock(false);
        }
    }

    void onBackGroundHit(RaycastHit hit)
    {
        for (int i = 0; i < MAX_PLATFORMS; i++)
        {
            if (platformPool[currentPlat % MAX_PLATFORMS].isLocked())
            {
                currentPlat++;
                continue;
            }
            else
            {
                platformPool[currentPlat % MAX_PLATFORMS].transform.position = hit.point;
                currentPlat++;
                break;
            }
        }
    }

}

