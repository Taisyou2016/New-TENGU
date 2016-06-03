using UnityEngine;
using System.Collections;

public class BossAttack : MonoBehaviour {

    public GameObject[] attacks;
    private int curretNum;

    public void Attack(int index)
    {
        curretNum = Mathf.Clamp(index, 0, attacks.Length - 1);
        Instantiate(attacks[curretNum], transform.position + transform.forward, transform.rotation);
    }
}
