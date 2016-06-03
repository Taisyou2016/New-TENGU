using UnityEngine;
using System.Collections;

public class WindBlock : MonoBehaviour
{
    public float playerPower;
    public float objectPower;
    public Vector3 direction;
    public GameObject particle;

    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        particle.transform.forward = direction;
        particle.transform.localScale = transform.localScale;
    }

    void Update()
    {
    }

    public void OnTriggerEnter(Collider other)
    {
        //transform.parent.GetComponent<Wind>().HitForce(other.gameObject);
        //print("当たった");
        //print(other.name);
        if (other.tag == "Player")
        {
            if (player.GetComponent<PlayerMove>().GetJampState() == true)
            {
                player.GetComponent<PlayerMove>().SetWindPower(playerPower, direction);
                player.GetComponent<PlayerMove>().SetVelocityY(10);
            }
        }
        else if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyRoutine>().Damage(0);
            other.GetComponent<Rigidbody>().AddForce(objectPower * direction, ForceMode.Impulse);
        }
        else if (other.tag == "EnemyBullet")
        {
            if (other.name.Contains("Bullet"))
            {
                other.GetComponent<ohuda>().flag = true;
            }
            other.GetComponent<Rigidbody>().velocity =
                objectPower * direction
                + other.transform.up * Random.Range(1.0f, 3.0f)
                + other.transform.right * Random.Range(-10.0f, 10.0f);
        }
    }

    public void SetForce(float playerPower, float objectPower, Vector3 direction)
    {
        this.playerPower = playerPower;
        this.objectPower = objectPower;
        this.direction = direction;
    }
}
