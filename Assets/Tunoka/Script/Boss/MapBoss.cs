using UnityEngine;
using System.Collections;

public class MapBoss : MonoBehaviour {

    private GameObject player;

    void Start ()
    {
        player = GameObject.Find("Player");
    }
	
	void Update () {
        transform.LookAt(player.transform.position);
    }
}
