using UnityEngine;
using System.Collections;

public class Panch : MonoBehaviour {

    public int dmg = 2;
    private Rigidbody rd;

    void Start()
    {
        rd = GetComponent<Rigidbody>();
    }

    void Update()
    {
        rd.velocity = transform.forward * 5;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerStatus>().HpDamage(dmg);
            Destroy(this.gameObject);
        }
    }

}
