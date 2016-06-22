using UnityEngine;
using System.Collections;

public class UVScroller : MonoBehaviour {
    public Vector2 m_ScrollSpeed;

    Material m_Material;

	void Start () 
    {
        m_Material = GetComponent<Renderer>().material;
	}
	

	void Update () {
        m_Material.SetTextureOffset("_MainTex", m_ScrollSpeed * Time.time);
	}
}
