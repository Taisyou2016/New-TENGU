using UnityEngine;
using System.Collections;

public class s_Amulet : MonoBehaviour {

    private GameObject player;
    void Start()
    {
        player = GameObject.Find("Player");
    }
    void Update()
    {
       transform.FindChild("P_Amulet").transform.LookAt(player.transform.position);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            print("お札解除");
            Destroy(gameObject);
        }
    }
}
