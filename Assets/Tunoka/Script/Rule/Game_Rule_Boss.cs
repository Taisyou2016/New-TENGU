using UnityEngine;
using System.Collections;

public class Game_Rule_Boss : MonoBehaviour {


    private int m_p_Hp = 10;
    private GameObject player;
    private PlayerStatus playerStatus;
    private BossRoutine boss;
    public GameObject _GameOvera;
    public GameObject _GameClear;
    public GameObject FadeInOut;
    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");
        boss = GameObject.Find("Boss").GetComponent<BossRoutine>();
        playerStatus = player.GetComponent<PlayerStatus>();
    }

    // Update is called once per frame
    void Update()
    {

        m_p_Hp = playerStatus.currentHp;
        if (m_p_Hp <= 0)
        {
            Gameovera();
        }
        if (boss != null)
        {
            if (boss.life <= 0)
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