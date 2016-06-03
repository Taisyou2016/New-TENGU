using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SeconPoint : MonoBehaviour {

    public GameObject gate;
    private GameObject player;
    public GameObject NextPoint;
    public Text text;

    void Start ()
    {
        text.text = "移動の練習2\nターゲットをかまいたちで攻撃しよう";
        player = GameObject.FindWithTag("Player");
        player.GetComponent<PlayerStatus>().maxMp = 100;
        player.GetComponent<PlayerStatus>().currentMp = 100;
        player.GetComponent<PlayerMove>().walkSpeed = 0;

    }

    // Update is called once per frame
    void Update()
    {
        int count = transform.childCount;
        if (count <= 0)
        {
            print("2　クリア");
            NextPoint.gameObject.SetActive(true);
            iTween.MoveTo(gate, iTween.Hash("y", -15, "time", 3));
            Destroy(gameObject);
        }
    }
}
