using UnityEngine;
using System.Collections;

public class Mp_Heel_Point : MonoBehaviour {

    private float mp;
    public float UpTime;
    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            mp += 0.1f;
            if (mp >= UpTime)
            {
                other.GetComponent<PlayerStatus>().currentMp += 1;
                mp = 0;
            }
        }
    }
}
