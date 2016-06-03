using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Magic_Meter : MonoBehaviour
{

    public Image cooldown;
    private PlayerStatus player;
    void Start()
    {
        cooldown = GetComponent<Image>();
        //_slider = transform.GetComponent<Slider>();
        player = GameObject.Find("Player").GetComponent<PlayerStatus>();
    }

    void Update()
    {
        float _Mp = 1;
        _Mp =  (float)player.currentMp / (float)player.maxMp;
       
        // HPゲージに値を設定
        cooldown.fillAmount = _Mp;

    }
}