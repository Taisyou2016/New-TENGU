using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Secon2Point : MonoBehaviour {

    public GameObject gate;
    public GameObject Offgate;
    public GameObject NextPoint;
    public Text text;

    public string talkText;

    private PlayerStatus player;
    // Use this for initialization
    void Start () {
        text.text = talkText;
        player = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>();

        player.GetComponent<PlayerMove>().walkSpeed = 5;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    public void OnTriggerEnter(Collider other)
    {
        print("3　クリア");
        iTween.MoveTo(gate, iTween.Hash("y", -18, "time", 3));
        iTween.MoveTo(Offgate, iTween.Hash("y", 0, "time", 3));
        NextPoint.gameObject.SetActive(true);
        Destroy(gameObject);
    }
}
