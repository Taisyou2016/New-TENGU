﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TalkText : MonoBehaviour {


    public GameObject NextPoint;
    public GameObject Textcanvas;
    private Text text;
    public string[] talkText;
    private int talkNum;
    private PlayerStatus player;
    // Use this for initialization
    void Start ()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>();
        player.maxMp = 0;
        player.GetComponent<PlayerMove>().walkSpeed = 0;
        talkNum = 0;
        text = Textcanvas.transform.FindChild("Text").gameObject.GetComponent<Text>();
        text.text = talkText[talkNum];
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1))
        {
            talkNum++;
            if (talkNum >= talkText.Length)
            {
                talkOff();
                return;
            }
            text.text = talkText[talkNum];
        }
	}
    void talkOff()
    {
        player.maxMp = 100;
        player.GetComponent<PlayerMove>().walkSpeed = 5;
        NextPoint.gameObject.SetActive(true);
        Destroy(gameObject);
    }
}