using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultObstacle : MonoBehaviour
{
    public float NORMAL_SPEED = -2.5f;
    private float currentSpeed;
    private bool locked = false;

    public float fillPerSec = 0.6f;
    private float filled = 0.0f;
    MeshRenderer meshRenderer;
    public Color normalColor, fullColor;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        currentSpeed = NORMAL_SPEED;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + currentSpeed * Time.deltaTime);
        if (locked)
        {
            filled += fillPerSec * Time.deltaTime;
            meshRenderer.material.color = Color.Lerp(normalColor, fullColor, filled);
        }

        if(filled >= 1.0)
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Floor"))
        {
            Debug.Log("Hit Floor");
            Destroy(gameObject);
        }

        currentSpeed = 0.0f;
        locked = true;
    }

    private void OnTriggerExit(Collider collision)
    {
        currentSpeed = NORMAL_SPEED;
        locked = false;
    }
}
