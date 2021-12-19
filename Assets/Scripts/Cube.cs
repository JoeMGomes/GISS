using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public float NORMAL_SPEED = -2.5f;
    private float currentSpeed;
    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = NORMAL_SPEED;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + currentSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        currentSpeed = 0.0f;
    }

    private void OnCollisionExit(Collision collision)
    {
        currentSpeed = NORMAL_SPEED; 
    }
}
