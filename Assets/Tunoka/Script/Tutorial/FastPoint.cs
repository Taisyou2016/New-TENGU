using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FastPoint : MonoBehaviour {

    public GameObject gate;
    public GameObject Offgate;
    public GameObject NextPoint;
    public Text text;

    private PlayerStatus player;

    void Start () {

        text.text = "移動の練習1\nターゲットの場所まで移動しよう\nスキップするには隣のゲートに";
        player = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>();
        player.maxMp = 0;
        player.currentMp = 0;

    }
    void Update()
    {
    }

    public void OnTriggerEnter(Collider other)
    {
        print("1　クリア");
        iTween.MoveTo(gate, iTween.Hash("y", -15, "time", 3));
        iTween.MoveTo(Offgate, iTween.Hash("y", 0, "time", 3));
        NextPoint.gameObject.SetActive(true);
        Destroy(gameObject);
    }
}
