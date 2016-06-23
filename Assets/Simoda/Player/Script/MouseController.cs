using UnityEngine;
using System.Collections;

public class MouseController : MonoBehaviour
{
    //public GameObject windAura;
    //public GameObject kamaitachiAura;
    //public GameObject tornadoAura;
    public float length = 200.0f;

    private Vector3 startPos = new Vector3(Screen.width / 2, Screen.height / 2, 0.0f);
    private Vector3 endPos;
    private Vector3 vector;
    private float radian;
    private float angle;
    private PlayerStatus playerStatus;

    //private bool doubleButtonDown = false;
    //private bool windGeneration = false;
    //private bool kamaitachiGeneration = false;
    //private bool tornadoGeneration = false;
    private bool generation = false;


    float time;

    void Start()
    {
        //Cursor.visible = false;
        playerStatus = GameObject.FindObjectOfType<PlayerStatus>();
        //windAura.SetActive(false);
        //kamaitachiAura.SetActive(false);
        //tornadoAura.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Cursor.visible = true;

        //if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
        //    tornadoAura.SetActive(true);
        //else
        //    tornadoAura.SetActive(false);

        //if (Input.GetMouseButton(0) && !Input.GetMouseButton(1))
        //    windAura.SetActive(true);
        //else
        //    windAura.SetActive(false);

        //if (Input.GetMouseButton(1) && !Input.GetMouseButton(0))
        //    kamaitachiAura.SetActive(true);
        //else
        //    kamaitachiAura.SetActive(false);

        //if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
        //{
        //    doubleButtonDown = true;
        //}
        //else
        //{
        //    doubleButtonDown = false;
        //}

        //if ((Input.GetMouseButtonDown(0) && Input.GetMouseButtonDown(1)) && doubleButtonDown == true)
        //{
        //    Cursor.lockState = CursorLockMode.Locked;
        //    Cursor.lockState = CursorLockMode.None;

        //    Invoke("TornadoDecision", 0.3f);
        //    return;
        //}

        //if (Input.GetMouseButtonDown(0))
        //{
        //    Cursor.lockState = CursorLockMode.Locked;
        //    Cursor.lockState = CursorLockMode.None;

        //    Invoke("WindAttackDecision", 0.3f);
        //    return;
        //}

        //if (Input.GetMouseButtonDown(1) && doubleButtonDown == false)
        //{
        //    Cursor.lockState = CursorLockMode.Locked;
        //    Cursor.lockState = CursorLockMode.None;

        //    Invoke("KamaitachiDecision", 0.3f);
        //    return;
        //}

        //竜巻を出した後、マウスを押してないのに気流またはかまいたちが出てしまうのをブロック
        time += Time.deltaTime;
        if (time >= 5.0f)
        {
            GenerationTrue();
            GenerationFalse();
            time = 0.0f;
        }

        if (Input.GetMouseButton(0) && Input.GetMouseButton(1) && generation == false)
        {
            GenerationTrue();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.lockState = CursorLockMode.None;

            Invoke("TornadoDecision", 0.5f);
            return;
        }

        if (Input.GetMouseButton(0) && !Input.GetMouseButton(1) && generation == false)
        {
            GenerationTrue();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.lockState = CursorLockMode.None;

            Invoke("WindAttackDecision", 0.3f);
            return;
        }

        if (Input.GetMouseButton(1) && !Input.GetMouseButton(0) && generation == false)
        {
            GenerationTrue();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.lockState = CursorLockMode.None;

            Invoke("KamaitachiDecision", 0.3f);
            return;
        }

        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    TornadoDecision();
        //}
    }

    public void WindAttackDecision()
    {
        endPos = Input.mousePosition;
        vector = endPos - startPos;
        radian = vector.y / vector.x;
        angle = Mathf.Atan(radian) * Mathf.Rad2Deg;

        //print(startPos);
        //print(endPos);
        //print(endPos - startPos);
        //print("ラジアン" + radian);
        //print("角度" + Mathf.Atan(radian) * Mathf.Rad2Deg);
        //print("長さ" + vector.magnitude);

        if (vector.magnitude > length)
        {
            if (playerStatus.MpCostDecision(playerStatus.windCost) && transform.parent.transform.GetComponent<PlayerMove>().knockBackState == false)
            {
                GameObject.FindObjectOfType<AttackPattern>().WindPatternDecision(angle, vector);
            }
        }

        Invoke("GenerationFalse", 0.3f);
    }

    public void KamaitachiDecision()
    {
        endPos = Input.mousePosition;
        vector = endPos - startPos;
        radian = vector.y / vector.x;
        angle = Mathf.Atan(radian) * Mathf.Rad2Deg;

        if (vector.magnitude > length)
        {
            //MPが一度ゼロになって回復中か、costが足りなかったら発生させない
            if (playerStatus.MpCostDecision(playerStatus.kamaitachiCost) && transform.parent.transform.GetComponent<PlayerMove>().knockBackState == false)
            {
                GameObject.FindObjectOfType<AttackPattern>().KamaitachiPatternDecision(angle, vector);
            }
        }

        Invoke("GenerationFalse", 0.3f);
    }

    public void TornadoDecision()
    {
        endPos = Input.mousePosition;
        vector = endPos - startPos;
        radian = vector.y / vector.x;
        angle = Mathf.Atan(radian) * Mathf.Rad2Deg;

        if (vector.magnitude > length)
        {
            if (playerStatus.MpCostDecision(playerStatus.tornadoCost) && transform.parent.transform.GetComponent<PlayerMove>().knockBackState == false)
                GameObject.FindObjectOfType<AttackPattern>().TornadoPatternDecision(angle, vector);
        }

        Invoke("GenerationFalse", 0.3f);
    }

    public void GenerationTrue()
    {
        //windGeneration = true;
        //kamaitachiGeneration = true;
        //tornadoGeneration = true;
        generation = true;
    }

    public void GenerationFalse()
    {
        //tornadoGeneration = false;
        //windGeneration = false;
        //kamaitachiGeneration = false;
        generation = false;
    }
}
