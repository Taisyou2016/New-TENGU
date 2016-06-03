using UnityEngine;
using System.Collections;

public class PressurePlate : MonoBehaviour {

    public GameObject Gimmick;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == ("Player"))
        {
            print("感圧版用ギミック作動");
            //ギミックを動かすものを入れる
            //Gimmick.GetComponent<Gimmick>().Gimmick();
            iTween.MoveTo(gameObject, new Vector3(
                  transform.position.x
                , transform.position.y - 0.5f
                , transform.position.z), 5);
        }
    }
	// Update is called once per frame
	void Update () {
	
	}
}
