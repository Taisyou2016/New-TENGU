using UnityEngine;
using System.Collections;

public class BossCreateEnemyP : MonoBehaviour {

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
                Instantiate(tornadoPrefabG, child.transform.position, transform.rotation);
            }
            else
            {
                Instantiate(tornadoPrefab, child.transform.position, transform.rotation);
            }
            tornadoPrefab.GetComponent<EnemyRoutine>().LengeType = i;
            tornadoPrefab.GetComponent<EnemyRoutine>().maxlife = 20;
            count++;
        }
        Invoke("DeleteObj", 3.5f);
    }
	
}
