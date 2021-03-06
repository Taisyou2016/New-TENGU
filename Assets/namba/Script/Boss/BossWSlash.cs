﻿using UnityEngine;
using System.Collections;

public class BossWSlash : MonoBehaviour {

    [SerializeField]
    private int dmg = 1;
    private float speed = 15;

	// Update is called once per frame
	void Update () {
        this.transform.position += transform.forward * speed * Time.deltaTime;
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerStatus>().HpDamage(dmg, gameObject);
            Destroy(gameObject);
        }
    }

}
