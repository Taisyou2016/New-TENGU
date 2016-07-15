using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayerMoveState;
using System;

public class CreditPlayer : MonoBehaviour {

    private Transform[] _cameraSetPoint;//位置配列
    private Vector3[] _cameraSetRot;//回転配列
    private PlayerMove playerMove;
    private float walkspeed;

    [SerializeField, Header("終わるまでの時間")]
    private float _moveTime;
    private float startTime;
    float counter;

    private PlayerStatus playerStatus;
    private Animator playerAnimator;
    public GameObject FadeInOut;
    void Start ()
    {
        GameObject _production_Camera_Pos = GameObject.Find("Production_Camera_Pos");
        _cameraSetPoint = new Transform[_production_Camera_Pos.transform.childCount];
        _cameraSetRot = new Vector3[_production_Camera_Pos.transform.childCount];

        for (int i = 1; i <= _production_Camera_Pos.transform.childCount; i++)
        {
            print(i);
            _cameraSetPoint[i - 1] = _production_Camera_Pos.transform.FindChild(i.ToString()).gameObject.transform;
            _cameraSetRot[i - 1] = _production_Camera_Pos.transform.FindChild(i.ToString()).gameObject.transform.eulerAngles;
        }
        if (_moveTime <= 0)
        {
            transform.position = _cameraSetPoint[_cameraSetPoint.Length - 1].position;
            transform.rotation = Quaternion.Euler(_cameraSetRot[_cameraSetPoint.Length - 1]);
            enabled = false;
            return;
        }

        transform.position = _cameraSetPoint[0].position;

        //startTime = Time.timeSinceLevelLoad;

        counter = 0;
        startTime = counter;


        playerStatus = gameObject.GetComponent<PlayerStatus>();
        playerAnimator = transform.FindChild("Tengu_Default").GetComponent<Animator>();

        playerAnimator.SetTrigger("Jump");
    }
	
	// Update is called once per frame
	void Update () {
        counter += 1.0f * Time.deltaTime;
        MoveCamera();
    }
    int tr = 0;
    public void MoveCamera()
    {
        if (tr != 0)return;
        //var diff = Time.timeSinceLevelLoad - startTime;
        var diff = counter - startTime;

        if (diff > _moveTime)
        {
            transform.position = _cameraSetPoint[_cameraSetPoint.Length - 1].position;
            transform.rotation = Quaternion.Euler(_cameraSetRot[_cameraSetPoint.Length - 1]);

            Fade();
            return;
        }

        var persent = diff / _moveTime;

        if (persent < 0.0f) persent = 0.0f;
        if (persent > 1.0f) persent = 1.0f;

        transform.position = iTween.PointOnPath(_cameraSetPoint, persent);
        transform.rotation = Quaternion.Euler(iTween.PointOnPath(_cameraSetRot, persent));
    }
    void Fade()
    {
        tr = 1;
        FadeInOut.GetComponent<FadeInOut>().FadeIn("Logo");
    }
}
