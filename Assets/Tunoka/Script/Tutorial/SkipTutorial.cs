using UnityEngine;
using System.Collections;

public class SkipTutorial : MonoBehaviour {

    public GameObject NextPoint;
    
	// Update is called once per frame
	void Update ()
    {
        int count = transform.childCount;
        if (count <= 0)
        {
            Destroy(gameObject);
        }

    }
    public void OnTriggerEnter(Collider other)
    {
        print("スキップ");
        NextPoint.GetComponent<FadeInOut>().FadeIn();

    }
}
