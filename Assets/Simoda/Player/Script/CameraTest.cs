using UnityEngine;
using System.Collections;

public class CameraTest : MonoBehaviour
{
    public GameObject target;
    public GameObject cameraPoint;
    public bool flag;
    public float cameraMoveSpeedDefault = 3.0f;
    public float cameraMoveSpeedWind = 10.0f;
    public float cameraRotateSpeed = 2.0f;

    private float cameraMoveSpeed;
    private Transform cameraTransform;
    private Transform targetTransform;

    void Start()
    {
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        targetTransform = target.transform;

        cameraMoveSpeed = cameraMoveSpeedDefault;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetTransform.position, cameraMoveSpeed * Time.deltaTime);

        if (target.GetComponent<PlayerMove>().GetLockOnInfo() == true) //ロックしてるとき
        {
            if (Input.GetAxis("Horizontal") != 0) flag = true;
            if (transform.rotation == target.transform.rotation) flag = false;

            Vector3 targetRotation = targetTransform.rotation.eulerAngles;
            if (flag == true) //フラグがtrueだったらプレイヤーの後ろに回る
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetTransform.rotation, cameraRotateSpeed * Time.deltaTime);
            }
        }
        else if (target.GetComponent<PlayerMove>().GetWindMove() == true)
        {
            Quaternion targetRotationZzoro = target.transform.rotation;
            targetRotationZzoro.z = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationZzoro, cameraRotateSpeed * Time.deltaTime);
        }
        else
        {
            if (Input.GetAxis("Horizontal") != 0) flag = true;
            if (transform.rotation == target.transform.rotation || (Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") == 0))
            {
                flag = false;
            }

            if (flag == true)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetTransform.rotation, cameraRotateSpeed * Time.deltaTime);
            }
        }

        if (target.GetComponent<PlayerMove>().GetWindMove() == true)
            cameraMoveSpeed = cameraMoveSpeedWind;
        else
            cameraMoveSpeed = cameraMoveSpeedDefault;

        //ターゲットからカメラポイントの間に障害物がなければカメラをカメラポイントに移動する
        //障害物があったら障害物より前に移動する
        Vector3 cameraPointDirection = cameraPoint.transform.position - targetTransform.position;
        Ray cameraPointRay = new Ray(targetTransform.position, cameraPointDirection);
        RaycastHit cameraPointRayHitInfo;
        Debug.DrawRay(cameraPointRay.origin, cameraPointRay.direction * 6.5f, Color.green);

        if (Physics.SphereCast(cameraPointRay, 0.5f, out cameraPointRayHitInfo, 6.5f, 1 << 8))
        {
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, cameraPointRayHitInfo.point, cameraMoveSpeed * Time.deltaTime);
        }
        else
        {
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, cameraPoint.transform.position, cameraMoveSpeed * Time.deltaTime);
        }
    }

    public void LockStart()
    {
        flag = true;
    }
}
