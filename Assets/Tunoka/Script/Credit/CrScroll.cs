using UnityEngine;
using System.Collections;

public class CrScroll : MonoBehaviour {

    [SerializeField]
    private int scollTime = 8;
    public GameObject fade;
    void Start () {
        iTween.MoveTo(gameObject, iTween.Hash("y", 1301, 
            "delay", 4 ,"islocal" , true ,
            "time", scollTime, 
            "easeType", iTween.EaseType.linear,
            "oncomplete", "EndAction",
            "oncompletetarget", gameObject
            ));
    }
    void EndAction()
    {
        Invoke("FadeIn", 2);
    }
    void FadeIn()
    {
        GameObject.Find("FadeInOut").GetComponent<FadeInOut>().FadeIn();
    }
}
