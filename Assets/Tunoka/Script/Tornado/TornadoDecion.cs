using UnityEngine;
using System.Collections;

public class TornadoDecion : MonoBehaviour {

    public Vector3 direction;
    public float power;

    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {          
            other.GetComponent<PlayerMove>().SetVelocityY((int)power);
        }
        else
        {
            if (other.GetComponent<Rigidbody>() != null)
            {
                other.GetComponent<Rigidbody>().AddForce(direction * 0.1f, ForceMode.Impulse);
            }
        }
    }
}
