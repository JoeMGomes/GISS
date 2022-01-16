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


    public GameObject _particles;

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
            Color newColor = Color.Lerp(normalColor, fullColor, filled);
            meshRenderer.material.color = newColor;
            meshRenderer.material.SetColor("_EmissionColor", newColor);
        }

        if(filled >= 1.0)
        {
            LevelController.Instance.removeBlockFromLevel(gameObject);
            SoundManager.Instance.PlaySound(SoundManager.Sound.DestroyBlock);
            GameObject p = Instantiate(_particles, transform.position, transform.rotation);
            p.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Floor"))
        {
            LevelController.Instance.loseLife();
            LevelController.Instance.removeBlockFromLevel(gameObject);
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
