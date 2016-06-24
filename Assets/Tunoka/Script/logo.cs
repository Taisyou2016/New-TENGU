using UnityEngine;
using System.Collections;

public class logo : MonoBehaviour {

	// Use this for initialization
	void Start () {
        print("a");
        Invoke("FadeIn", 4);
    }

    void FadeIn()
    {
        GameObject.Find("FadeInOut").GetComponent<FadeInOut>().FadeIn();
    }
}
