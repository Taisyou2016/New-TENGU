using UnityEngine;
using System.Collections;

public class ParticleTornado : MonoBehaviour {


    private ParticleSystem[] particles;
    public Vector3 direction;
    public float power;
    public int damage = 20;

    void Start () {
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
            other.GetComponent<PlayerMove>().SetVelocityY((int)power);
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
                other.GetComponent<Rigidbody>().AddTorque(new Vector3(10, 0 , 10));
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyRoutine>().Damage(damage);
        }
    }
        void SetValue(float alpha)
    {
        // iTweenで呼ばれたら、受け取った値をImageのアルファ値にセット
        particles[0].startColor = new Color(158, 158, 158, alpha);
      
    }


}
