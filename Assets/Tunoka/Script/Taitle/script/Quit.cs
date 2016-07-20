using UnityEngine;
using System.Collections;

public class Quit : MonoBehaviour {

    public float Counter = 0;
    public int LimitTime = 10;


    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Counter += Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Counter = 0;
        }
        if (Counter >= LimitTime)
        {
            print("ゲーム終了");
            Application.Quit();
        }

    }
}
