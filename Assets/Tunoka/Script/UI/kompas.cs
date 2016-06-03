using UnityEngine;
using System.Collections;

public class kompas : MonoBehaviour {

    public GameObject Map_Amu;
    public GameObject Map_Boss;
    public GameObject Boss;
    private GameObject player;

    void Start ()
    {
        player = GameObject.Find("Player");
        AmuletSet();
        if (Boss != null)
        {
            BossSet();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
       transform.rotation = Quaternion.Euler(0, 0, -player.transform.eulerAngles.y);
    }
    void AmuletSet()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Amulet");

        for (int i = 0; i < gos.Length ; i++)
        {
            GameObject clone = (GameObject)Instantiate(Map_Amu, transform.position, transform.rotation);
            clone.transform.parent = transform;
            clone.GetComponent<Map>().target = gos[i];
        }
    }
    void BossSet()
    {
        GameObject clone = (GameObject)Instantiate(Map_Boss, transform.position, transform.rotation);
        clone.transform.parent = transform;
        clone.GetComponent<Map>().target = Boss;
    }
}
