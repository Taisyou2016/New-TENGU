using UnityEngine;
using System.Collections;

public class SkipTutorial : MonoBehaviour {


    public GameObject ScennC;
    public GameObject NextPoint;
    private GameObject cursor;

    private int selectNom = 0;

    void Start()
    {
        selectNom = 0;
        //cursor = NextPoint.transform.FindChild("cursor").gameObject;
    }

    void Update ()
    {
       

    }
    public void OnTriggerEnter(Collider other)
    {
        print("スキップ");
        Destroy(gameObject);
        ScennC.GetComponent<FadeInOut>().FadeIn();

    }
    void Skipselect()
    {
        if (Input.GetKeyDown(KeyCode.A)) selectNom += 1;
        if (Input.GetKeyDown(KeyCode.D)) selectNom += 1;
        if (selectNom > 1) selectNom = 0;
        cursor.transform.eulerAngles = new Vector3(0, 0, 90+ (90 * selectNom));
    }
}
