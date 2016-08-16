using UnityEngine;
using System.Collections;

public class KamaitachiFieldCollision : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Field")
        {
            transform.parent.GetComponent<Kamaitachi>().Hit();
        }
    }
}
