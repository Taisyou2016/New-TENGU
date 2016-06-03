using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Geme_Rule : MonoBehaviour {

    private int m_p_Hp = 10;
    private GameObject player;
    private PlayerStatus playerStatus;

    public GameObject _GameOvera;
    public GameObject _GameClear;
    public GameObject FadeInOut;
    // Use this for initialization
    void Start () {
        player = GameObject.Find("Player");
        playerStatus = player.GetComponent<PlayerStatus>();
    }

    // Update is called once per frame
	void Update () {

  
        m_p_Hp = playerStatus.currentHp;
        if (m_p_Hp <= 0)
        {
            Gameovera();
        }
        
    }
    void Gameovera()
    {
        print("Gameovera");
        _GameOvera.transform.localPosition = Vector3.zero;

    }
    public void GameClear()
    {
        print("GameClear");
        _GameClear.transform.localPosition = Vector3.zero;
        FadeInOut.GetComponent<FadeInOut>().FadeIn();
    }
}
