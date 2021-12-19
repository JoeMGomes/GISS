using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{

    public GameObject cubePrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction * 20, out hit))
            {

                if (hit.transform.gameObject.CompareTag("Background"))
                {
                    Vector3 spawPosition = hit.point;
                    spawPosition.y = 6.5f;
                    Instantiate(cubePrefab, spawPosition, Quaternion.identity);
                }
            }
        }
    }
}
