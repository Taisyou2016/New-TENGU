using UnityEngine;
using System.Collections;

public class DestroyTime : MonoBehaviour
{
    public float destroyTime = 2.0f;

    void Start()
    {

    }

    void Update()
    {
        Invoke("Destroy", destroyTime);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
