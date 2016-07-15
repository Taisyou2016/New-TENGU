using UnityEngine;
using System.Collections;

public class Liquor : MonoBehaviour {

    public int recovery;
    private PlayerStatus playerStatus;
    public bool Hp = false;
    public bool Mp = false;

    public GameObject particle;

    public GameObject catchEnem;
    private bool setEnem;

    void Start()
    {
        playerStatus = GameObject.Find("Player").GetComponent<PlayerStatus>();
        if (catchEnem != null)
        {
            setEnem = true;
            transform.GetComponent<CapsuleCollider>().enabled = false;
            transform.FindChild("Gourd_model").gameObject.SetActive(false);
        }
    }
    void Update()
    {
        if (setEnem == true)
        {
            if (catchEnem == null)
            {
                transform.GetComponent<CapsuleCollider>().enabled = true;
                transform.FindChild("Gourd_model").gameObject.SetActive(true);
                setEnem = false;
                return;
            }
            transform.position = catchEnem.transform.position;
            return;
        }

        transform.eulerAngles += new Vector3(0, 2, 0);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Hp == true)
            {
                Hp_recovery();
            }
            if (Mp == true)
            {
                Mp_recovery();
            }
            GameObject mParticle = (GameObject)Instantiate(particle, other.transform.position, other.transform.rotation);

            
            Destroy(gameObject);
        }
    }
    void Hp_recovery()
    {
        if (recovery <= playerStatus.maxHp - playerStatus.currentHp)
        {
            playerStatus.currentHp += recovery;
        }
        else
        {
            playerStatus.currentHp = playerStatus.maxHp;
        }
    }
    void Mp_recovery()
    {
        if (recovery <= playerStatus.maxMp - playerStatus.currentMp)
        {
            playerStatus.currentMp += (float)recovery;
        }
        else
        {
            playerStatus.currentMp = playerStatus.maxMp;
        }
    }
}
