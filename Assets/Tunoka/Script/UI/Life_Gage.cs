using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Life_Gage : MonoBehaviour {

    public Image cooldown;
    private PlayerStatus player;
    public int Hp_No;
    void Start ()
    {
        cooldown = GetComponent<Image>();
        player = GameObject.Find("Player").GetComponent<PlayerStatus>();
        Hp_No = int.Parse(transform.name);
    }

    // Update is called once per frame
    void Update () {
        float Hp = 1;
        Hp = (float)player.currentHp;

        if (Hp == Hp_No * 2 - 1)
        {
            cooldown.fillAmount = 0.5f;
        }
        else if (Hp < Hp_No * 2 - 1)
        {
            cooldown.fillAmount = 0;
        }
        else
        {
            cooldown.fillAmount = 1;
        }
        

    }
}
