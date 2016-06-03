using UnityEngine;
using System.Collections;

public class BossCreateTornado : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {
        int count = transform.childCount;
        if (count <= 0)
        {
            Destroy(gameObject);
        }
    }
}
