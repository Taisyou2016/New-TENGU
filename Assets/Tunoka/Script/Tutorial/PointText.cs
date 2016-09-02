using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class PointText : MonoBehaviour {

    void Start () {
	
	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            return;
        }
        Destroy(gameObject);
    }
}
