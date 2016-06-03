using UnityEngine;
using System.Collections;

public class Kamaitachi : MonoBehaviour
{
    public GameObject kamaitachiCollision;
    public GameObject particlePosition;
    public Vector3 moveVector;

    private bool hit = false;
    private int cost;

    private GameObject player;
    private PlayerStatus playerStatus;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerStatus = player.GetComponent<PlayerStatus>();
        cost = playerStatus.kamaitachiCost;
        playerStatus.MpConsumption(cost);
        //cost = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>().kamaitachiCost;
        //GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>().MpConsumption(cost);

        //particle = transform.FindChild("Windcuter").gameObject;
    }

    void Update()
    {
        transform.GetComponent<Rigidbody>().velocity = moveVector;
        //particle.transform.right = moveVector.normalized;

        if (hit == true) //linePrefabがPlayer以外に当たったら自分を消す
            Destroy(gameObject);
    }

    public void Move(Vector3 direction, float speed) //direction方向にspeedの速度で移動
    {
        moveVector = direction * speed;
        //transform.forward = direction; //進行方向を前に
    }

    public void SetCollisionScale(Vector3 scale) //当たり判定のサイズ変更
    {
        kamaitachiCollision.transform.localScale = scale;
    }

    public void SetParticleScale(Vector3 scale) //パーティクルのサイズ変更
    {
        particlePosition.transform.localScale = new Vector3(scale.z, scale.y, scale.x);
    }

    public void Hit() //linePrefabがPlayer以外に当たったら使用する
    {
        hit = true;
    }
}
