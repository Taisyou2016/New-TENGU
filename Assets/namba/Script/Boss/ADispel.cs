using UnityEngine;
using System.Collections;

public class DispelWave : MonoBehaviour {

    public float radius;
    public float power;
    private Rigidbody rd;

	// Use this for initialization
	void Start () {
        Collider[] cols = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider hit in cols)
        {
            rd = hit.GetComponent<Rigidbody>();
            if(rd != null)
            {
                rd.AddExplosionForce(power, transform.position, radius);
            }
        }
    }
}
