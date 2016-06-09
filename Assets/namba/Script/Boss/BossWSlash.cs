using UnityEngine;
using System.Collections;

public class BossWSlash : MonoBehaviour {

    [SerializeField]
    private int dmg;
    private Transform boss;


	// Use this for initialization
	void Start () {
        boss = GameObject.FindGameObjectWithTag("Boss").transform;
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = boss.position + boss.transform.forward * 2;
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag != "Enemy")
        {
            if (col.gameObject.tag == "Player")
            {
                col.gameObject.GetComponent<PlayerStatus>().HpDamage(dmg);
            }
            Destroy(this.gameObject);
        }
    }

}
