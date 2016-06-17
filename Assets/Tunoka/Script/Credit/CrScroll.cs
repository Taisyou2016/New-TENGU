using UnityEngine;
using System.Collections;

public class CrScroll : MonoBehaviour {

    [SerializeField]
    private int scollTime = 8;
    // Use this for initialization
    void Start () {
        iTween.MoveTo(gameObject, iTween.Hash("y", 884, "time", scollTime));
    }

    // Update is called once per frame
    void Update () {
	
	}
}
