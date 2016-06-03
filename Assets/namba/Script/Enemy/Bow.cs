using UnityEngine;
using System.Collections;

public class Bow : MonoBehaviour {

    private Rigidbody rd;
    private Transform player;
    public int damage = 1;

    public float angle = 60;
    private float gravity = 9.8f;
    private float dt;
    private float Vx, Vy;

    // Use this for initialization
    void Start () {
        //rd = GetComponent<Rigidbody>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        // 距離計算
        float subx = Vector3.Distance(this.transform.position, player.position);
        float velo = subx / (Mathf.Sin(2 * angle * Mathf.Deg2Rad) / gravity);
        Vx = Mathf.Sqrt(velo) * Mathf.Cos(angle * Mathf.Deg2Rad);
        Vy = Mathf.Sqrt(velo) * Mathf.Sin(angle * Mathf.Deg2Rad);

        //float flightDuration = subx / Vx;
        transform.rotation = Quaternion.LookRotation(player.position - transform.position);
    }

    // Update is called once per frame
    void Update () {
        transform.Translate(0, (Vy - (gravity * dt)) * Time.deltaTime, Vx * Time.deltaTime);
        dt += Time.deltaTime;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag != "Enemy")
        {
            if (col.gameObject.tag == "Player")
            {
                col.gameObject.GetComponent<PlayerStatus>().HpDamage(damage);
            }
            Destroy(this.gameObject);
        }
    }
}
