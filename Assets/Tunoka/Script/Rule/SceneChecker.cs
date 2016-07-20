using UnityEngine;
using System.Collections;

public class SceneChecker : MonoBehaviour {


    [SerializeField, Header("シーンポイントを切り替えるかどうか")]
    private bool Load = true;
    [SerializeField, Header("今のシーン")]
    private int sceneNum = 0;

    public static int scenepoint = 0;

    // getter
    public static int getScenePoint()
    {
        return scenepoint;
    }

    void Start ()
    {
        if (Load == true)
        {
            scenepoint = sceneNum;
        }
	
	}
	
	void Update () {
	
	}
}
