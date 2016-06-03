using UnityEngine;
using System.Collections;

public class Wind : MonoBehaviour
{
    public GameObject windMotion;
    public GameObject linePrefab;

    private Vector3 startPoint;
    public Vector3 point;
    public float lineLength = 1.0f;
    public float scaleY = 0.5f;
    public float scaleZ = 0.5f;

    public float playerPower;
    public float objectPower;
    public Vector3 direction;

    private int cost;

    private GameObject player;
    private PlayerStatus playerStatus;

    void Start()
    {
        player = GameObject.Find("Player");
        playerStatus = player.GetComponent<PlayerStatus>();
        cost = playerStatus.windCost;
        playerStatus.MpConsumption(cost);
        //cost = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>().windCost;
        //GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>().MpConsumption(cost);
        point = transform.position;
    }

    void Update()
    {
        startPoint = transform.position;

        DrawLine();
    }

    public void Move(Vector3 vector, float stopTime)
    {
        windMotion.GetComponent<Rigidbody>().velocity = vector;

        Invoke("WindStop", stopTime);
    }

    public void WindStop()
    {
        windMotion.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void DrawLine()
    {
        if ((windMotion.transform.position - point).magnitude > lineLength)
        {
            GameObject obj = Instantiate(linePrefab, startPoint, transform.rotation) as GameObject;
            obj.GetComponent<WindBlock>().SetForce(playerPower, objectPower, direction);
            obj.transform.position = windMotion.transform.position;
            obj.transform.right = (windMotion.transform.position - point).normalized;
            obj.transform.Rotate(new Vector3(1, 0, 0), obj.transform.eulerAngles.x * -1.0f);
            obj.transform.localScale = new Vector3((windMotion.transform.position - point).magnitude, scaleY, scaleZ);
            obj.transform.parent = this.transform;

            point = obj.transform.position;
        }
    }

    public void SetScale(float Y, float Z)
    {
        scaleY = Y;
        scaleZ = Z;
    }

    public void SetForce(float playerPower, float objectPower, Vector3 direction)
    {
        this.playerPower = playerPower;
        this.objectPower = objectPower;
        this.direction = direction;
    }
}
