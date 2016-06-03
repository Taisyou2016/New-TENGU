using UnityEngine;
using System.Collections;

public class BossParticleTornado : MonoBehaviour {


    private ParticleSystem[] particles;
    public Vector3 direction;
    public float power;
    public int damage = 5;

    private float rot = 0;

    void Start()
    {
        particles = this.gameObject.GetComponentsInChildren<ParticleSystem>();
        particles[0].startColor = new Color(0, 0, 0, 0);
        iTween.ValueTo(gameObject, iTween.Hash("from", 0f, "to", 1f, "time", 2f, "onupdate", "SetValue"));

    }
    public void FadeOut(float Time)
    {
        iTween.ValueTo(gameObject, iTween.Hash("from", 1f, "to", 0f, "time", Time, "onupdate", "SetValue"));
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            rot += 10;
            other.GetComponent<PlayerMove>().SetVelocityY((int)power+5);
            other.transform.rotation = Quaternion.Euler(0, rot, 0);
        }
        else if (other.tag == "EnemyBullet")
        {
            other.GetComponent<Rigidbody>().AddTorque(new Vector3(50, 10, 50));
        }
        else
        {
            if (other.GetComponent<Rigidbody>() != null)
            {
                other.GetComponent<Rigidbody>().AddForce(direction * 0.1f, ForceMode.Impulse);
                other.GetComponent<Rigidbody>().AddTorque(new Vector3(10, 0, 10));
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerStatus>().HpDamage(damage);
        }
    }
    void SetValue(float alpha)
    {
        // iTweenで呼ばれたら、受け取った値をImageのアルファ値にセット
        particles[0].startColor = new Color(255, 0, 0, alpha);
    }
}
