using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HpGauge : MonoBehaviour {

    RectTransform rectTransform = null;
    Transform target = null;
    Slider hp;

    [SerializeField] Vector3 offset;

    void Awake()
    {
        target = GameObject.Find("BossEnemy").transform;
        rectTransform = GetComponent<RectTransform>();
        hp = GetComponent<Slider>();
    }

    void Update()
    {
        rectTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, target.position + offset);
    }

    public void Damage(int dmg)
    {
        hp.value -= dmg;
    }
}
