using UnityEngine;
using System.Collections;

public class Tornado : MonoBehaviour {


    [SerializeField]
    private int time = 20;
    public Vector3 TornadoSize = new Vector3(2,4,2);

    [SerializeField, Header("回転スピード")]
    private float SpinSpeed = 1;
    

    public bool Free = false;
    // Use this for initialization
    void Start()
    {
        iTween.ScaleTo(gameObject, iTween.Hash("x", TornadoSize.x, "y", TornadoSize.y, "z", TornadoSize.z, "time", time));

    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles += new Vector3(0f, SpinSpeed * 100 * Time.deltaTime, 0f);

        if (transform.localScale.x == TornadoSize.x)
        {
            if (Free == true) return;
            iTween.ScaleTo(gameObject, iTween.Hash("x", 0, "z", 0, "time", 10));
            transform.FindChild("Particle").gameObject.GetComponent<ParticleTornado>().FadeOut(5f);
        }
        else if (transform.localScale.x == 0)
        {
            Destroy(gameObject);
        }

    }
    private void TornadoMove(int Move,int MoveTime)
    {
        iTween.MoveTo(gameObject, transform.forward * Move, MoveTime);
    }
    void Delete()
    {
    }
}
