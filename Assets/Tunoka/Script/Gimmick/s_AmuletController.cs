using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class s_AmuletController : MonoBehaviour {

    [SerializeField]
    private int AmuletCount;
    bool One = true;

    public GameObject BreakAmu;
    private GameObject Center;
    private GameObject openGate;
    private Text AmuletCountText;
    private int Maxcount;
    // Use this for initialization
    void Start () {
        Center = transform.FindChild("CenterBarrier").gameObject;

        if (GameObject.Find("Gate") != null)
        {
            openGate = GameObject.Find("Gate");
        }
        if (GameObject.Find("AmuletCountText") != null)
        {
            AmuletCountText = GameObject.Find("AmuletCountText").GetComponent<Text>();
        }

        Maxcount = transform.childCount - 1;
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
        AmuletCountText.text =  (-1*(AmuletCount - 1 - Maxcount)).ToString() +  "/" + Maxcount.ToString();

    }
    void Gimmick()
    {
        if (One)
        {
            BreakAmu.GetComponent<Talkpos>().textOn();
            GetComponent<SphereCollider>().enabled = false;
            iTween.ScaleTo(gameObject, iTween.Hash("x", 0, "y", 0, "z", 0, "time", 10));
            openGate.GetComponent<BoxCollider>().enabled = true;
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
