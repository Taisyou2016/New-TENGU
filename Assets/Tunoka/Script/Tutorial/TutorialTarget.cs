using UnityEngine;
using System.Collections;

public class TutorialTarget : MonoBehaviour {

    public void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
