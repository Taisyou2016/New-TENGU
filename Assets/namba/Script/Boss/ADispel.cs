using UnityEngine;
using System.Collections;

public class ADispel : MonoBehaviour {

    public float power;

    [SerializeField]
    private int dmg;
    private Rigidbody rd;
    private GameObject child;

	// Use this for initialization
	void Start () {
        child = transform.FindChild("Wind").gameObject;
    }

    void Update()
    {
        iTween.ScaleTo(child, iTween.Hash("Scale", new Vector3(3, 3, 2), "time", 2.0f)); 
    }

    void OnTriggerEnter(Collider hit)
    {
        rd = hit.GetComponent<Rigidbody>();
        if (rd != null)
        {
            rd.AddExplosionForce(power * 100, transform.position, transform.lossyScale.x / 2, 2);
        }

        if (hit.gameObject.tag == "Player")
        {
            hit.GetComponent<PlayerMove>().SetVelocityY((int)power / 2);
            hit.GetComponent<PlayerMove>().SetBlowPower(power);
            hit.GetComponent<PlayerStatus>().HpDamage(dmg, gameObject);
        }

        if (hit.gameObject.tag == "Enemy")
        {
            hit.GetComponent<EnemyRoutine>().Damage(0);
        }

    }
}
