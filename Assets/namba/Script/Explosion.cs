using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

    [SerializeField]
    private float time;

	// Use this for initialization
	IEnumerator Start () {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
	}

}
