﻿using UnityEngine;
using System.Collections;

public class ohuda : MonoBehaviour {

    private Rigidbody rd;
    public GameObject fire;
    public float speed = 10;
    public bool flag;
    public int damage = 1;


    // Use this for initialization
    void Start () {
        rd = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update () {
        if (!flag)
            rd.velocity = transform.forward * speed;
        else
            gameObject.layer = LayerMask.NameToLayer("PlayerAttack");
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerStatus>().HpDamage(damage);
        }

        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<EnemyRoutine>().Damage(damage * 10);
        }

        Died();

    }

    void Died()
    {
        Instantiate(fire, this.transform.position, transform.rotation);
        Destroy(this.gameObject);
    }

}
