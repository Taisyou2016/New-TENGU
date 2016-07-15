using UnityEngine;
using System.Collections;

public class LogoIn : MonoBehaviour {

    [SerializeField]
    private int scollTime = 8;
    [SerializeField]
    private int PlayerGoTime = 0;
    [SerializeField]
    private GameObject ply ;
    void Start () {
        iTween.MoveTo(gameObject, iTween.Hash("y", 0,
                "delay", 4, "islocal", true,
                "time", scollTime,
                "easeType", iTween.EaseType.linear,
                "oncomplete", "EndAction",
                "oncompletetarget", gameObject
                ));
    }

    void EndAction()
    {
        Invoke("TENGU_Go", PlayerGoTime);
    }
    void TENGU_Go()
    {
        ply.SetActive(true);
    }
}
