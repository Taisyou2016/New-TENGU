using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ThirdPoint : MonoBehaviour {


    public GameObject NextPoint;

    public Text text;
    private GameObject player;
    private PlayerStatus pStatus;
    public string talkText;
    // Use this for initialization
    void Start ()
    {
        text.text = talkText;
        player = GameObject.FindWithTag("Player");
        player.GetComponent<PlayerMove>().walkSpeed = 5;
        pStatus = player.GetComponent<PlayerStatus>();
    }
	
	// Update is called once per frame
	void Update () {
        pStatus.currentHp =10;
        int count = transform.childCount;
        if (count <= 0)
        {
            print("4　クリア");
            NextPoint.gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
