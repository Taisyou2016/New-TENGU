using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Talkpos : MonoBehaviour {

    public GameObject talkText;
    public Text text;

    public bool ObjDelete = false;
    public bool loopTalk;
    public string talk;
    // Use this for initialization
    void Start () {
        text = talkText.transform.FindChild("Text").GetComponent<Text>();
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            textOn();
        }
    }
    void textOn()
    {
        talkText.gameObject.SetActive(true);
        text.text = talk;
        Invoke("textOff", 4f);
    }

    void textOff()
    {
        talkText.gameObject.SetActive(false);
        if (loopTalk == true)
        {
            return;
        }
        if (ObjDelete == true)
        {
            Destroy(gameObject);
        }
        Destroy(gameObject.GetComponent<Talkpos>());
    }
}
