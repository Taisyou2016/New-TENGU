﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Geme_Rule : MonoBehaviour {

    private int m_p_Hp = 10;
    private GameObject player;
    private PlayerStatus playerStatus;
    private BossRoutine boss;

    public GameObject _GameOvera;
    public GameObject _GameClear;
    public GameObject FadeInOut;
    // Use this for initialization
    void Start () {
        Cursor.visible = false;
        if (GameObject.Find("Player") == null) return;
        player = GameObject.Find("Player");
        if(GameObject.FindGameObjectWithTag("Boss") != null)
        {
            boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossRoutine>();
        }
        playerStatus = player.GetComponent<PlayerStatus>();
    }
    // Update is called once per frame
	void Update () {

        if (GameObject.Find("Player") == null) return;

        m_p_Hp = playerStatus.currentHp;
        if (m_p_Hp <= 0)
        {
            Gameovera();
        }
        if (boss != null)
        {
            if (boss.nowlife <= 0)
            {
                GameClear();
            }
        }

    }
    void Gameovera()
    {
        print("Gameovera");
        _GameOvera.transform.localPosition = Vector3.zero;
        FadeInOut.GetComponent<FadeInOut>().m_scenechange = "GameOver";
        FadeInOut.GetComponent<FadeInOut>().FadeIn();

    }
    public void GameClear()
    {
        print("GameClear");
        _GameClear.transform.localPosition = Vector3.zero;
        FadeInOut.GetComponent<FadeInOut>().FadeIn();
    }
}
