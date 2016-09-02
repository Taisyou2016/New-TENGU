using UnityEngine;
using System.Collections;

public class GameOverScene : MonoBehaviour {


    [SerializeField]
    private int FadeTime = 5;

    [SerializeField]
    private int scene;
    private int cursorNom;
    private GameObject cursor;
    [SerializeField]
    private GameObject skipsel;
    void Start ()
    {
        cursor = skipsel.transform.FindChild("cursor").gameObject;
        cursorNom = 0;
        scene = SceneChecker.getScenePoint();
    }
    void Update()
    {
        Conténewlect();
    }
    void Conténewlect()
    {
        if (Input.GetKeyDown(KeyCode.A)) cursorNom += 1;
        if (Input.GetKeyDown(KeyCode.D)) cursorNom += 1;
        if (cursorNom > 1) cursorNom = 0;
        cursor.transform.eulerAngles = new Vector3(0, 0, (180 * cursorNom));
        if (Input.GetMouseButton(1))
        {
            //Skip
            if (cursorNom == 0)
            {
                print("コンテニュウ");
                string name = "title";
                if (scene == 2)
                {
                    name = "GamePlay";
                }
                else if(scene == 3)
                {
                    name = "Boss";
                }
                GameObject.Find("FadeInOut").GetComponent<FadeInOut>().FadeIn(name);
                return;
            }
            FadeIn();
        }
    }
    void FadeIn()
    {
        GameObject.Find("FadeInOut").GetComponent<FadeInOut>().FadeIn();
    }
    void FadeIn(string name)
    {
        GameObject.Find("FadeInOut").GetComponent<FadeInOut>().FadeIn(name);
    }
}
