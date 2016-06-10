using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SeconPoint : MonoBehaviour {

    public GameObject gate;
    private GameObject player;
    public GameObject NextPoint;
    public Text text;

    public string talkText;
    void Start ()
    {
        text.text = talkText;
        player = GameObject.FindWithTag("Player");
        player.GetComponent<PlayerStatus>().maxMp = 100;
        player.GetComponent<PlayerStatus>().currentMp = 100;

    }

    // Update is called once per frame
    void Update()
    {
        int count = transform.childCount;
        if (count <= 0)
        {
            print("2　クリア");
            NextPoint.gameObject.SetActive(true);
            iTween.MoveTo(gate, iTween.Hash("y", -18, "time", 3));
            Destroy(gameObject);
        }
    }
}
