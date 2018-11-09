using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAt : MonoBehaviour
{
    public string Name;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Name == null)
        {
            return;
        }
        var chino = GameObject.Find(Name);
        if (chino == null)
        {
            return;
        }
        transform.LookAt(chino.transform.position + new Vector3(0, 1.0f, 0));
    }
}