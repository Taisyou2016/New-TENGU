using UnityEngine;
using System.Collections;

public class Wind : MonoBehaviour
{
    public GameObject windMotion;
    public GameObject linePrefab;
    public GameObject particle;

    private Vector3 startPoint;
    private Vector3 particlePosition = Vector3.zero;
    private Quaternion particleRotation;
    public Vector3 point;
    public float lineLength = 1.0f;
    public float scaleY = 0.5f;
    public float scaleZ = 0.5f;

    public float playerPower;
    public float objectPower;
    public Vector3 direction;
    public int generationPattern;

    private float cost;

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
        particle.transform.localScale = new Vector3((startPoint - windMotion.transform.position).magnitude, scaleY, scaleZ / 2);
        particle.transform.position = Vector3.Lerp(startPoint, particlePosition, 5.0f);
        if (generationPattern == 3 || generationPattern == 4)
        { }
        else
            particle.transform.Rotate(new Vector3(1, 0, 0), particle.transform.eulerAngles.x * -1.0f);
        DrawLine();
    }

    public void Move(int pattern, Vector3 vector, float stopTime)
    {
        generationPattern = pattern;
        windMotion.GetComponent<Rigidbody>().velocity = vector;

        particle.transform.forward = direction;
        //particlePosition = direction + GameObject.Find("Player").transform.position;

        Transform playerTransform = GameObject.Find("Player").transform;
        //particle.transform.Rotate(new Vector3(0, 1, 0), GameObject.Find("Player").transform.eulerAngles.y);

        //position用　if文
        if (pattern == 1 || pattern == 2)
        {
            particlePosition = direction + playerTransform.position;
        }
        else if (pattern == 3 || pattern == 4)
        {
            particlePosition = direction + playerTransform.position + playerTransform.up * 2.5f;
        }
        else if (pattern == 5 || pattern == 6)
        {
            particlePosition = direction + playerTransform.position + playerTransform.up * 0.5f;
        }
        else if (pattern == 7 || pattern == 8)
        {
            particlePosition = direction + playerTransform.position + playerTransform.up;
        }

        //角度用　if文
        if (pattern == 1 || pattern == 6 || pattern == 7)
            particle.transform.right = vector.normalized;
        else if (pattern == 3 || pattern == 4)
        {
            particle.transform.right = vector.normalized;
            particle.transform.localRotation = Quaternion.Euler(particle.transform.localEulerAngles.x, 0, particle.transform.localEulerAngles.z);
        }
        else
            particle.transform.right = -vector.normalized;

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
            obj.transform.forward = player.transform.forward;
            if (generationPattern == 3 || generationPattern == 4) obj.transform.forward = player.transform.forward;

            obj.transform.Rotate(new Vector3(1, 0, 0), obj.transform.eulerAngles.x * -1.0f);

            if (generationPattern == 3 || generationPattern == 4)
                obj.transform.localScale = new Vector3(scaleY, (windMotion.transform.position - point).magnitude, scaleZ);
            else
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

        transform.forward = direction;
        //particlePosition = direction + GameObject.Find("Player").transform.position;
        //particle.transform.forward = direction;
    }
}
