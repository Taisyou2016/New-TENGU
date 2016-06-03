using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ForthPoint : MonoBehaviour {

    public Text text;
    public GameObject NextPoint;
    // Use this for initialization
    void Start ()
    {
        text.text = "お疲れ様です\nそれでは本編をお楽しみください";

        Invoke("Scene", 2);
    }

    void Scene()
    {
        NextPoint.GetComponent<FadeInOut>().FadeIn();
    }
}
