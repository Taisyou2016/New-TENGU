using UnityEngine;
using System.Collections;

public class Liquor : MonoBehaviour {

    public int recovery;
    private PlayerStatus playerStatus;
    public bool Hp = false;
    public bool Mp = false;
    void Start()
    {
        playerStatus = GameObject.Find("Player").GetComponent<PlayerStatus>();
    }
    void Update()
    {
        transform.eulerAngles += new Vector3(0, 2, 0);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Hp == true)
            {
                Hp_recovery();
            }
            if (Mp == true)
            {
                Mp_recovery();
            }
            Destroy(gameObject);
        }
    }
    void Hp_recovery()
    {
        if (recovery <= playerStatus.maxHp - playerStatus.currentHp)
        {
            playerStatus.currentHp += recovery;
        }
        else
        {
            playerStatus.currentHp = playerStatus.maxHp;
        }
    }
    void Mp_recovery()
    {
        if (recovery <= playerStatus.maxMp - playerStatus.currentMp)
        {
            playerStatus.currentMp += recovery;
        }
        else
        {
            playerStatus.currentMp = playerStatus.maxMp;
        }
    }
}
