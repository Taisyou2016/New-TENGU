using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Map : MonoBehaviour {

    public GameObject target;
    private GameObject player;
    private GameObject MapPosition;

    private AnimationState AmuletPosition;

    private Animator anim;
    [SerializeField]
    private float fastSearchRange = 10;
    [SerializeField]
    private float secondSearchRange = 40;
    void Start()
    {
        anim.Play("Amulet", 0, 0.0f);
        player = GameObject.Find("Player");
        MapPosition = transform.FindChild("Position").transform.gameObject;
        anim = MapPosition.GetComponent<Animator>();
       
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
            Vector3 Va = player.transform.position;
            Vector3 Vb = target.transform.position;
            Va.y = 0;
            Vb.y = 0;
            float _distance = Vector3.Distance(Va, Vb);
            
            float rot = 180 - target.transform.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0, 0, rot);
            print(anim.speed);
            if (_distance <= fastSearchRange)
            {
                anim.Play("Amulet", 0, 0.0f);
                MapPosition.transform.localPosition = new Vector3(0, _distance / fastSearchRange * 30, 0);
            }
            else if (_distance >= secondSearchRange)
            {
                print("a");
                anim.speed = 0.5f;
            }
            else
            {
                print("b");
                anim.speed = 1f;
            }

        }
    }


}
