using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ThirdPoint : MonoBehaviour {


    public GameObject NextPoint;

    public Text text;
    private GameObject player;

    // Use this for initialization
    void Start ()
    {
        text.text = "移動の練習4\n実際に敵を倒してみよう";
        player = GameObject.FindWithTag("Player");
        player.GetComponent<PlayerMove>().walkSpeed = 5;
    }
	
	// Update is called once per frame
	void Update () {
        int count = transform.childCount;
        if (count <= 0)
        {
            print("4　クリア");
            NextPoint.gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
