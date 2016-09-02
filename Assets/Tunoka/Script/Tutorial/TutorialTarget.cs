using UnityEngine;
using System.Collections;

public class TutorialTarget : MonoBehaviour {

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Kamaitachi")
        {
            Destroy(gameObject);
        }
    }
}
