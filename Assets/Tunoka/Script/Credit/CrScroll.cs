using UnityEngine;
using System.Collections;

public class CrScroll : MonoBehaviour {

    [SerializeField]
    private int scollTime = 8;
    [SerializeField]
    private GameObject Logo;
    [SerializeField]
    private GameObject Fade;
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
        Fade.GetComponent<FadeInOut>().HalfFadeIn();
        Logo.GetComponent<LogoIn>().enabled = true;
        Invoke("LogoIn", 0.5f);
    }
    void LogoIn()
    {
        Destroy(gameObject);
    }
}
