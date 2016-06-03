using UnityEngine;
using System.Collections;

public class Deadline : MonoBehaviour {

    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerStatus>().currentHp = 0;
        }
    }
}
