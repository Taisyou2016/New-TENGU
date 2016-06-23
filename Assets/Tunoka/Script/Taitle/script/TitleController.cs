using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{

    private Vector3 _mousePosition = Vector3.zero;
    private Vector3 _center = Vector3.zero;
    public GameObject _leaf;
    private ParticleSystem _particle;

    void Start()
    {
        _particle = _leaf.GetComponent<ParticleSystem>();
        _center = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    }

    // Update is called once per frame
    void Update()
    {
        var a = _particle.forceOverLifetime.x;
        Cursor.visible = true; //カーソル表示
        if (Input.GetMouseButtonDown(1))
        {
            Cursor.lockState = CursorLockMode.Locked; //中央にロック
            Cursor.lockState = CursorLockMode.None; //標準モード
            Invoke("CursorWind", 1);
        }
    }
    void CursorWind()
    {
        _mousePosition = Input.mousePosition;
        if (_mousePosition.x < _center.x - 10 && _mousePosition.y > _center.y + 10)
        {
            print(_mousePosition);
            print("OK");
            iTween.MoveTo(_leaf, iTween.Hash("x", -68.3, "y", 31, "time", 3f, "isLocal", true));

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