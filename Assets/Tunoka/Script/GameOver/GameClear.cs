﻿using UnityEngine;
using System.Collections;

public class GameClear : MonoBehaviour {

    [SerializeField]
    private int FadeTime = 5;

    void Start()
    {
        Invoke("FadeIn", FadeTime);
    }

    void FadeIn()
    {
        GameObject.Find("FadeInOut").GetComponent<FadeInOut>().FadeIn();
    }
}
