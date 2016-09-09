using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Geme_Rule : MonoBehaviour {

    private int m_p_Hp = 10;
    private GameObject player;
    private PlayerStatus playerStatus;
    private BossRoutine boss;
    private bool Finish = false;

    public GameObject _GameOvera;
    public GameObject _GameClear;
    public GameObject FadeInOut;

    void Start () {
        Finish = false;
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
         if (Input.GetKeyDown(KeyCode.Escape))
         {
             SceneManager.LoadScene("title", LoadSceneMode.Single);
         }
        if (GameObject.Find("Player") == null) return;
        if (Finish == true) return;


        if (m_p_Hp <= 0 )
        {
            _GameOvera.transform.localPosition = Vector3.zero;
            Finish = true;
            Invoke("Gameovera", 3f);
        }
        if (boss != null)
        {
            if (boss.nowlife <= 0)
            {
                Finish = true;
                _GameClear.transform.localPosition = Vector3.zero;
                GameObject.FindGameObjectWithTag("CameraController").transform.GetComponent<BossCamera>().enabled = true;
                gameObject.GetComponent<ReturnToTitle>().enabled = false;
            }
        }
        
        m_p_Hp = playerStatus.currentHp;

    }
    void Gameovera()
    {
        FadeInOut.GetComponent<FadeInOut>().FadeIn("GameOver");
    }


    public void GameClear()
    {
        _GameClear.transform.localPosition = Vector3.zero;
        FadeInOut.GetComponent<FadeInOut>().FadeIn();
    }
}
