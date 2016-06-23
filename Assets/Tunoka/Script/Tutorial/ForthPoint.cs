using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ForthPoint : MonoBehaviour {

    public Text text;
    public GameObject NextPoint;
    // Use this for initialization
    void Start ()
    {
       
        text.text = "";

        Invoke("Scene", 2);
    }

    void Scene()
    {
        NextPoint.GetComponent<FadeInOut>().FadeIn();
    }
}
