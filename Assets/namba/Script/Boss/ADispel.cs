using UnityEngine;
using System.Collections;

public class ADispel : MonoBehaviour {

    public float radius;
    public float power;

    [SerializeField]
    private int dmg;
    private Rigidbody rd;

	// Use this for initialization
	void Start () {
        Collider[] cols = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider hit in cols)
        {
            rd = hit.GetComponent<Rigidbody>();
            if(rd != null)
            {
                rd.AddExplosionForce(power * 100, transform.position, radius, 2);
            }

            if(hit.gameObject.tag == "Player")
            {
                hit.GetComponent<PlayerMove>().SetVelocityY((int)power);
                hit.GetComponent<PlayerMove>().SetBlowPower(power);
                hit.GetComponent<PlayerStatus>().HpDamage(dmg);
            }

            if (hit.gameObject.tag == "Enemy")
            {
                hit.GetComponent<EnemyRoutine>().Damage(0);
            }
        }
    }

    void Update()
    {
        iTween.ScaleTo(gameObject, iTween.Hash("Scale", new Vector3(3, 3, 1.3f), "time", 2.0f)); 
    }
}
