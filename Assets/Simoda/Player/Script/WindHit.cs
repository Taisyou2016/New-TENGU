using UnityEngine;
using System.Collections;

public class WindHit : MonoBehaviour
{
    public GameObject enemy;

    public Vector3 hitVector = Vector3.zero;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //TEnemy.Hit(hitVector, 10);
            //collision.rigidbody.AddForce(hitVector, ForceMode.Impulse);
        }
    }

    void Start()
    {

    }

    void Update()
    {
    }

    public void SetVector(Vector3 vector)
    {
        hitVector = vector;
    }

    public Vector3 a()
    {
        return hitVector;
    }
}
