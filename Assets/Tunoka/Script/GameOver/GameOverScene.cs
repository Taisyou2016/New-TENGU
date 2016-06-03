using UnityEngine;
using System.Collections;

public class GameOverScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("FadeIn", 5);
    }

    void FadeIn()
    {
        GameObject.Find("FadeInOut").GetComponent<FadeInOut>().FadeIn();
    }
}
