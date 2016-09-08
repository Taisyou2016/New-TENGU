using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossAttack : MonoBehaviour {

    public GameObject[] attacks;
    public List<AudioClip> sounds = new List<AudioClip>();

    private int curretNum;
    private AudioSource se;

    void Start()
    {
        se = gameObject.GetComponent<AudioSource>();
    }

    // 1:六芒星 2:吹き飛ばし 3:かまいたち 4:竜巻
    public void Attack(int index)
    {
        curretNum = Mathf.Clamp(index-1, 0, attacks.Length - 1);
        Instantiate(attacks[curretNum], transform.position + transform.forward, transform.rotation);
        se.PlayOneShot(sounds[curretNum]);
    }
}
