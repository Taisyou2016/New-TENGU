using UnityEngine;
using System.Collections;

public class ParticleTornado : MonoBehaviour
{


    private ParticleSystem[] particles;
    public Vector3 direction;
    [SerializeField, Header("吹っ飛ばし力")]
    public float power;
    [SerializeField, Header("攻撃ダメージ")]
    public int damage = 20;

    [SerializeField, Header("前進するスピード")]
    private float AdvanceSpeed = 1;
    private bool bostr;

    void Start()
    {
        particles = this.gameObject.GetComponentsInChildren<ParticleSystem>();
        particles[0].startColor = new Color(0, 0, 0, 0);
        iTween.ValueTo(gameObject, iTween.Hash("from", 0f, "to", 1f, "time", 2f, "onupdate", "SetValue"));

    }
    void Update()
    {
        if (transform.root.gameObject.GetComponent<Tornado>().Free == true) return;
        transform.localPosition += new Vector3(0, 0, AdvanceSpeed * Time.deltaTime);
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
                other.GetComponent<Rigidbody>().AddTorque(new Vector3(10, 0, 10));
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyRoutine>().Damage(damage);
        }
        else if (other.tag == "Boss" && bostr == false)
        {
            other.GetComponent<BossRoutine>().Damage(damage);
            bostr = true;
        }
    }
    void SetValue(float alpha)
    {
        // iTweenで呼ばれたら、受け取った値をImageのアルファ値にセット
        if (transform.root.gameObject.GetComponent<Tornado>().Free == true)
        {
            particles[0].startColor = new Color(0, 255, 0, alpha);
            return;
        }
        particles[0].startColor = new Color(158, 158, 158, alpha);

    }


}
