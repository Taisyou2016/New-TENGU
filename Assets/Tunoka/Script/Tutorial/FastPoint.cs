using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FastPoint : MonoBehaviour {

    public GameObject gate;
    public GameObject Offgate;
    public GameObject NextPoint;
    public Text text;
    public string talkText;

    private PlayerStatus player;

    void Start () {

        text.text = talkText;
        player = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>();
        player.maxMp = 0;
        player.currentMp = 0;

    }
    void Update()
    {
        int count = transform.childCount;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            return;
        }
        print("1　クリア");
        iTween.MoveTo(gate, iTween.Hash("y", -18, "time", 3));
        iTween.MoveTo(Offgate, iTween.Hash("y", 0, "time", 3));
        NextPoint.gameObject.SetActive(true);
        Destroy(gameObject);
    }
}
