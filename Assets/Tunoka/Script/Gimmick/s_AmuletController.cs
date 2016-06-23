using UnityEngine;
using System.Collections;

public class s_AmuletController : MonoBehaviour {

    [SerializeField]
    private int AmuletCount;
    bool One = true;

    public GameObject Center;
    public GameObject WindPoint;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


        AmuletCount = transform.childCount;
        if (AmuletCount <= 1)
        {
            Gimmick();
        }
        if (transform.localScale.x <= 0)
        {
            Destroy(gameObject);
        }
	}
    void Gimmick()
    {
        if (One)
        {
            GetComponent<SphereCollider>().enabled = false;
            iTween.ScaleTo(gameObject, iTween.Hash("x", 0, "y", 0, "z", 0, "time", 10));
            print("護符用ギミック作動");
            One = false;
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.LookAt(Center.transform.position);
            other.GetComponent<PlayerMove>().ChangeKnockBackLarge(10);
        }
    }
}
