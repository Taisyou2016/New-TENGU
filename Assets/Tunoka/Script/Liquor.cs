using UnityEngine;
using System.Collections;

public class Liquor : MonoBehaviour {

    public int recovery;
    private PlayerStatus playerStatus;
    void Start()
    {
        playerStatus = GameObject.Find("Player").GetComponent<PlayerStatus>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (recovery <= playerStatus.maxHp - playerStatus.currentHp)
            {
                playerStatus.currentHp += recovery;
            }
            else
            {
                playerStatus.currentHp = playerStatus.maxHp;
            }
            Destroy(gameObject);
        }
    }
}
