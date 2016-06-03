using UnityEngine;
using System.Collections;

public class BulletDestruction : MonoBehaviour {

    private int time;
    private Transform player;
    private Vector3 vec;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        vec = player.transform.position - this.transform.position;
        transform.rotation = Quaternion.LookRotation(vec);
    }

    // Update is called once per frame
    void Update () {
        time++;
        if(time > 120)
        {
            Destroy(gameObject);
        }
	}
}
