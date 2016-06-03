using UnityEngine;
using System.Collections;

public class TornadoGeneration : MonoBehaviour
{
    public GameObject tornadoPrefab;
    public float tornadoPower = 10;

    private PlayerStatus playerStatus;

    void Start()
    {
        playerStatus = transform.GetComponent<PlayerStatus>();
    }

    void Update()
    {

    }

    public void TornadoGeneration1()
    {
        playerStatus.MpConsumption(playerStatus.tornadoCost);

        GameObject tornado = Instantiate(tornadoPrefab);
        tornado.transform.FindChild("Particle").gameObject.GetComponent<ParticleTornado>().power = tornadoPower;

        tornado.transform.position =
            transform.position
            + transform.forward * 5.0f;
    }
}
