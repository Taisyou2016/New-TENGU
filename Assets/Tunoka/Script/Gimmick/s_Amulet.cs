using UnityEngine;
using System.Collections;

public class s_Amulet : MonoBehaviour {

    private GameObject player;
    public GameObject Effect;
    public GameObject catchEnem;
    private bool setEnem;
    void Start()
    {
        player = GameObject.Find("Player");
        if (catchEnem != null)
        {
            setEnem = true;
            transform.GetComponent<MeshRenderer>().enabled = false;
            transform.FindChild("water2").gameObject.SetActive(false);
        }
    }
    void Update()
    {
       transform.FindChild("P_Amulet").transform.LookAt(player.transform.position);

        if (setEnem == true)
        {
            if (catchEnem == null)
            {
                transform.GetComponent<MeshRenderer>().enabled = true;
                transform.FindChild("water2").gameObject.SetActive(true);
                setEnem = false;
                return;
            }
            transform.position = catchEnem.transform.position;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && setEnem == false)
        {
            print("お札解除");
            Instantiate(Effect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
