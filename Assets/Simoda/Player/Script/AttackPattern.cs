using UnityEngine;
using System.Collections;

public class AttackPattern : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    public void WindPatternDecision(float angle, Vector3 vector) //気流用
    {
        //横方向 → ←
        if (angle >= -20.0f && angle <= 20)
        {
            if (vector.x > 0)
            {
                print("パターン1 →");
                GameObject.FindObjectOfType<WindGeneration>().WindAttack1();
            }
            if (vector.x < 0)
            {
                print("パターン2 ←");
                GameObject.FindObjectOfType<WindGeneration>().WindAttack2();
            }
        }

        //縦方向 ↓ ↑
        if ((angle <= -70 && angle >= -90) || (angle >= 70 && angle <= 90))
        {
            if (vector.y < 0)
            {
                print("パターン3 ↓");
                GameObject.FindObjectOfType<WindGeneration>().WindAttack3();
            }
            if (vector.y > 0)
            {
                print("パターン4 ↑");
                GameObject.FindObjectOfType<WindGeneration>().WindAttack4();
            }
        }

        //右斜め方向 ↙ ↗
        if (angle > 20 && angle < 70)
        {
            if (vector.x < 0)
            {
                print("パターン5 ↙");
                GameObject.FindObjectOfType<WindGeneration>().WindAttack5();
            }
            if (vector.x > 0)
            {
                print("パターン6 ↗");
                GameObject.FindObjectOfType<WindGeneration>().WindAttack6();
            }
        }

        //左斜め方向 ↘ ↖
        if (angle < -20 && angle > -70)
        {
            if (vector.x > 0)
            {
                print("パターン7 ↘");
                GameObject.FindObjectOfType<WindGeneration>().WindAttack7();
            }
            if (vector.x < 0)
            {
                print("パターン8 ↖");
                GameObject.FindObjectOfType<WindGeneration>().WindAttack8();
            }
        }
    }

    public void KamaitachiPatternDecision(float angle, Vector3 vector) //かまいたち用
    {
        //横方向 → ←
        if (angle >= -20.0f && angle <= 20)
        {
            print("パターン1 → ←");
            GameObject.FindObjectOfType<KamaitachiGeneration>().KamaitachiGeneration1();
        }

        //縦方向 ↓ ↑
        if ((angle <= -70 && angle >= -90) || (angle >= 70 && angle <= 90))
        {
            print("パターン2 ↓ ↑");
            GameObject.FindObjectOfType<KamaitachiGeneration>().KamaitachiGeneration2();
        }

        //右斜め方向 ↙ ↗
        if (angle > 20 && angle < 70)
        {
            print("パターン3 ↙ ↗");
            GameObject.FindObjectOfType<KamaitachiGeneration>().KamaitachiGeneration3();
        }

        //左斜め方向 ↘ ↖
        if (angle < -20 && angle > -70)
        {
            print("パターン4 ↘ ↖");
            GameObject.FindObjectOfType<KamaitachiGeneration>().KamaitachiGeneration4();
        }
    }

    public void TornadoPatternDecision(float angle, Vector3 vector) //竜巻用
    {
        GameObject.FindObjectOfType<TornadoGeneration>().TornadoGeneration1();
    }
}
