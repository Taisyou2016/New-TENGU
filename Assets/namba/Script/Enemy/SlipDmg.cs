using UnityEngine;
using System.Collections;

public class SlipDmg : MonoBehaviour {
    public int damage = 1;

    void OnParticleCollision(GameObject col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerStatus>().HpDamage(damage);
        }
    }

}
