using UnityEngine;
using System.Collections;

public class TurtorialIme : MonoBehaviour {

    public GameObject PointIme;

	// Update is called once per frame
	void Update ()
    {
        if (PointIme == null) { Destroy(gameObject); return; }
        if (GameObject.Find(PointIme.name) == null) return;
        

        int count = 0;
        foreach (Transform child in transform)
        {
            if (GameObject.Find(child.name) != null) return;
            child.gameObject.SetActive(true);
            count++;
        }
    }
}
