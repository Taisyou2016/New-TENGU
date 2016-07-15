using UnityEngine;
using System.Collections;

public class BossCamera : MonoBehaviour {

    [SerializeField]
    private GameObject Boss;
    private GameObject Player;

    [SerializeField]
    private GameObject[] Enemys;

    [SerializeField, Header("ボスが映るまでの時間")]
    private float scollTime = 1;

    // Use this for initialization
    void Start () {
        transform.GetComponent<CameraTest>().enabled = false;
        Player = GameObject.Find("Player");
        Player.GetComponent<PlayerStatus>().currentHp = 10;

        iTween.MoveTo(gameObject, iTween.Hash(
            "x", Boss.transform.position.x ,
            "y", Boss.transform.position.y,
            "z", Boss.transform.position.z,
            "islocal", true,
            "time", scollTime,
            "easeType", iTween.EaseType.linear,
            "oncomplete", "FastAction",
            "oncompletetarget", gameObject
            ));
        
    }
    float JumpTime = 0;
    void Update()
    {
        if (JumpTime == 0)
        {
            transform.LookAt(Boss.transform.root.gameObject.transform.position);
            return;
        }
        else
        {
            transform.LookAt(Player.transform.position);
        }
        if (JumpTime == 2)
        {
            Player.transform.LookAt(new Vector3(-158.37f,Player.transform.position.y, -3.28f));
            Player.GetComponent<SphereCollider>().isTrigger = false;
            Player.GetComponent<PlayerMove>().SetVelocityY(4);
            
        }

    }
	// Update is called once per frame

    public void FastAction()
    {
        Enemys = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < Enemys.Length; i++)
        {
            Enemys[i].GetComponent<EnemyRoutine>().Damage(10);
        }
        iTween.MoveTo(gameObject, iTween.Hash(
            "x", Player.transform.position.x - 5,
            "y", Player.transform.position.y +5,
            "z", Player.transform.position.z,
            "delay", 3 ,"islocal", true,
            "time", scollTime,
            "easeType", iTween.EaseType.linear,
            "oncomplete", "SecondAction",
            "oncompletetarget", gameObject
            ));
    }
    public void SecondAction()
    {
        Enemys = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < Enemys.Length; i++)
        {
            Enemys[i].GetComponent<EnemyRoutine>().Damage(10);
        }
        Player.GetComponent<PlayerMove>().walkSpeed = 0;
        Player.GetComponent<PlayerMove>().transform.FindChild("Tengu_Default").GetComponent<Animator>().SetTrigger("Jump");
        JumpTime = 2;

        Invoke("ThirdAction", 4f);
    }
    public void ThirdAction()
    {
        iTween.MoveTo(Player, iTween.Hash(
               "x", -158.37,
               "z", -3.28,
               "time", 3,
               "easeType", iTween.EaseType.linear,
               "oncompletetarget", gameObject
               ));
    }
}
