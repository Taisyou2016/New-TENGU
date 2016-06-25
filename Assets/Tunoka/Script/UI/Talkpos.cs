using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Talkpos : MonoBehaviour {

    public GameObject talkText;
    private Text text;

    [SerializeField, Header("一回で終わるオブジェを壊す")]
    private bool ObjDelete = false;
    [SerializeField, Header("何回も実行する")]
    private bool loopTalk;
    [SerializeField, Header("Hp以下の時出る -は何もなし")]
    private float PlayerHp = -1;
    [SerializeField, Header("Hp以下の時出る -は何もなし")]
    private float PlayerMp= -1;
    [SerializeField, Header("話内容")]
    private string talk;

    private PlayerStatus playerstatus;

    // Use this for initialization
    void Start () {
        text = talkText.transform.FindChild("Text").GetComponent<Text>();
        playerstatus = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>();

    }
	
	// Update is called once per frame
	void Update () {
        if (playerstatus.currentMp < PlayerMp && PlayerMp > 0)
        {
            textOn();
        }
        if (playerstatus.currentHp < PlayerHp && PlayerHp > 0)
        {
            textOn();
        }

    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            textOn();
        }
    }
    public void textOn()
    {
        talkText.gameObject.SetActive(true);
        text.text = talk;
        Invoke("textOff", 4f);
    }
    void textOn(string _text)
    {
        talkText.gameObject.SetActive(true);
        text.text = _text;
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
