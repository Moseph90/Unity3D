using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private new Camera camera;

    private void Start()
    {
        camera = Camera.main;
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + camera.transform.forward);
    }
}
