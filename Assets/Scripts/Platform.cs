using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{

    bool locked = false;
    public Material normalMat;
    public Material lockedMat;
    MeshRenderer meshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool isLocked()
    {
        return locked;
    }
     public void Lock()
    {
        locked = !locked;
        meshRenderer.material = locked ? lockedMat : normalMat;

    }
}