using UnityEngine;
using System.Collections;

public class SkipTutorial : MonoBehaviour {


    public GameObject ScennC;
    public GameObject NextPoint;
    public GameObject skipsel;
    private GameObject cursor;

    private int selectNom = 0;

    void Start()
    {
        PlayerStatus player = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>();
        player.currentMp = 0;
        player.maxMp = 0;
        player.GetComponent<PlayerMove>().walkSpeed = 0;
        selectNom = 0;
        cursor = skipsel.transform.FindChild("cursor").gameObject;
    }

    void Update ()
    {
        Skipselect();
    }
    public void OnTriggerEnter(Collider other)
    {
        print("スキップ");
        Destroy(gameObject);
        ScennC.GetComponent<FadeInOut>().FadeIn();

    }
    void Skipselect()
    {
        print(selectNom);
        if (Input.GetKeyDown(KeyCode.A)) selectNom += 1;
        if (Input.GetKeyDown(KeyCode.D)) selectNom += 1;
        if (selectNom > 1) selectNom = 0;
        cursor.transform.eulerAngles = new Vector3(0, 0,  (180 * selectNom));
        if (Input.GetMouseButton(0))
        {
            //Skip
            if (selectNom == 0)
            {
                print("スキップ");
                Destroy(gameObject);
                ScennC.GetComponent<FadeInOut>().FadeIn();
                return;
            }
            Destroy(skipsel);
            NextPoint.gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
