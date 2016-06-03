using UnityEngine;
using System.Collections;

public class BossTornado : MonoBehaviour {

    [SerializeField]
    private int Time = 20;
    public Vector3 TornadoSize = new Vector3(2, 4, 2);
    public int damage = 5;

    // Use this for initialization
    void Start()
    {
        iTween.ScaleTo(gameObject, iTween.Hash("x", TornadoSize.x, "y", TornadoSize.y, "z", TornadoSize.z, "time", Time));

        transform.FindChild("Particle").gameObject.GetComponent<BossParticleTornado>().damage = damage;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * 0.1f;
        if (transform.localScale.x == TornadoSize.x)
        {
            iTween.ScaleTo(gameObject, iTween.Hash("x", 0, "z", 0, "time", 5));
            transform.FindChild("Particle").gameObject.GetComponent<BossParticleTornado>().FadeOut(1f);
        }
        else if (transform.localScale.x == 0)
        {
            Destroy(gameObject);
        }

    }
    private void TornadoMove(int Move, int MoveTime)
    {
        iTween.MoveTo(gameObject, transform.forward * Move, MoveTime);
    }
}
