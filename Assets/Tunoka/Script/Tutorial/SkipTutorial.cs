using UnityEngine;
using System.Collections;

public class SkipTutorial : MonoBehaviour {

    public GameObject NextPoint;
    
	// Update is called once per frame
	void Update ()
    {
       

    }
    public void OnTriggerEnter(Collider other)
    {
        print("スキップ");
            Destroy(gameObject);
        NextPoint.GetComponent<FadeInOut>().FadeIn();

    }
}
