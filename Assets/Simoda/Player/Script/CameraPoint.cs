using UnityEngine;
using System.Collections;

public class CameraPoint : MonoBehaviour
{
    public GameObject target;
    public float height = 3.0f;
    public float back = 6.0f;

    void Start()
    {

    }

    void Update()
    {
        //transform.position = target.transform.position + target.transform.forward * -back + target.transform.up * height;

        transform.LookAt(target.transform);

        //print("カメラポイント" + transform.position);

        //if (Input.GetKey(KeyCode.Q))
        //{
        //    transform.Rotate(0, -120 * Time.deltaTime, 0);
        //}

        //if (Input.GetKey(KeyCode.E))
        //{
        //    transform.Rotate(0, 120 * Time.deltaTime, 0);
        //}
    }
}
