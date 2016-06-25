using UnityEngine;
using System.Collections;

public class BossCreateEnemyP : MonoBehaviour {

    public GameObject a;
    // Use this for initialization
    public GameObject tornadoPrefab;
    public GameObject tornadoPrefabG;
    void Start ()
    {
        Invoke("Create", 2.5f);

    }
    void DeleteObj()
    {
        Destroy(transform.root.gameObject);
    }
    void Create()
    {
        int count = 0;
        foreach (Transform child in transform)
        {
            int i = Random.Range(1, 4);
            if (i == 3)
            {
                print("G");
                GameObject clone = (GameObject)Instantiate(tornadoPrefabG, child.position, child.rotation);

                clone.GetComponent<EnemyRoutine>().LengeType = i;
                clone.GetComponent<EnemyRoutine>().maxlife = 10;
            }
            else
            {
                GameObject clone = (GameObject)Instantiate(tornadoPrefab, child.position, child.rotation);

                clone.GetComponent<EnemyRoutine>().LengeType = i;
                clone.GetComponent<EnemyRoutine>().maxlife = 10;
            }
            count++;
        }
        Invoke("DeleteObj", 3.5f);
    }
	
}
