using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProducerText : MonoBehaviour {

    public string[] Producer;
    public Text text;

    void Start ()
    {
        foreach (string value in Producer)
        {
            text.text = text.text + "\n" + value;
        }
    }
	

	void Update ()
    {
        
    }
}
