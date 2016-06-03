using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour {

    public GameObject target;
    private GameObject player;
    private GameObject MapPosition;

    void Start()
    {
        player = GameObject.Find("Player");
        MapPosition = transform.FindChild("Position").transform.gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
        }
        else
        {
            float rot = 180 - target.transform.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0, 0, rot);

            Vector3 Va = player.transform.position;
            Vector3 Vb = target.transform.position;
            Va.y = 0;
            Vb.y = 0;
            float _distance = Vector3.Distance(Va, Vb);
            if (_distance <= 10)
            {
                MapPosition.transform.localPosition = new Vector3(0, _distance * 3, 0);
            }
            else
            {
                MapPosition.transform.localPosition = new Vector3(0, 30, 0);
            }

        }
    }
}
