using UnityEngine;
using System.Collections;

public class StageClearPoint : MonoBehaviour {

    [SerializeField]
    private GameObject rule;
	// Use this for initialization
	void Start () {
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(gameObject);
            rule.GetComponent<Geme_Rule>().GameClear();
        }
      
    }
}
