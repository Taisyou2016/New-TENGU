using UnityEngine;
using System.Collections;

public class ReturnToTitle : MonoBehaviour {

    public float Counter = 0;
    public int LimitTime = 10;
    public FadeInOut m_fade;

	// Update is called once per frame
	void Update () {
        // 何も押されていなければカウント開始
        if(!Input.anyKey){
            Counter += 1 * Time.deltaTime;
        }else{
            Counter = 0;  // キーが押されたらカウントをリセット
        }


        // time時間経過したら
        if(Counter >= LimitTime)
        {
            // タイトルに戻る
            m_fade.GetComponent<FadeInOut>().m_scenechange = "title";
            m_fade.GetComponent<FadeInOut>().FadeIn();
            Counter = 0;
        }
	}
}
