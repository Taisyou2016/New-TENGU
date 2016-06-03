using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{

    private Vector3 _mousePosition = Vector3.zero;
    private Vector3 _center = Vector3.zero;
    // Use this for initialization
    void Start()
    {
        _center = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    }

    // Update is called once per frame
    void Update()
    {

        Cursor.visible = true; //カーソル表示
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked; //中央にロック
            Cursor.lockState = CursorLockMode.None; //標準モード
            Invoke("CursorWind", 1);
        }
    }
    void CursorWind()
    {
        _mousePosition = Input.mousePosition;
        print("0"+_mousePosition);
        if (_mousePosition.x < _center.x - 10 && _mousePosition.y > _center.y + 10)
        {
            print(_mousePosition);
            print("OK");
            iTween.MoveTo(gameObject, iTween.Hash("x", -576, "y", 576, "time", 3f, "isLocal", true));
            Invoke("FadeIn", 1);
        }
        else
        {
            print(_mousePosition);
            print("No");
        }
    }
    void FadeIn()
    {
        GameObject.Find("FadeInOut").GetComponent<FadeInOut>().FadeIn();
    }
}