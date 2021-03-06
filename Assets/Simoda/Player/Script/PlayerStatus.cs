﻿using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour
{
    public int maxHp = 10; //最大HP
    public int currentHp; //現在のHP
    public float maxMp = 100; //最大妖力
    public float currentMp; //現在の妖力
    public float windCost = 5; //気流発生に必要なcost
    public float kamaitachiCost = 10; //かまいたち発生に必要なcost
    public float tornadoCost = 60; //竜巻発生に必要なcost;
    public float flightCost = 1; //滞空に必要なcost;

    public float mpAutoRecoveryCost = 1; //自動回復するときの妖力の量
    public float defaultMpAutoRecoveryTime = 1.0f; //自動回復の間隔
    public float overMpAutoRecoveryTime = 0.1f; //自動回復の間隔
    public bool invincible = false; //無敵化どうか
    public float knockBackSmallInvincibleTime = 1.0f; //ノックバック小の時の無敵時間
    public float knockBackLargeInvincibleTime = 3.0f; //ノックバック大の時の無敵時間
    public Material[] modelMaterial; //モデルのマテリアル

    public AudioClip smallDamage;
    public AudioClip largeDamage;
    public AudioClip dead;

    private float mpAutoRecoveryTime = 1.0f; //自動回復の間隔
    private float lastMpAutoRecoveryTime = 0.0f; //前に回復した時の時刻
    private bool mpOver = false; //妖力がOvarしたかどうか
    private float currentInvincibleTime = 0.0f; //現在の無敵時間
    private Color originColor; //モデルの元の色
    private bool alphaZero; //アルファが0以下になったらtreu　1以上になったらfalse
    private float flashingSecond = 0.0f;

    private Animator playerAnimator;
    private AudioSource audioSource;
    private GameObject mouseController;

    void Start()
    {
        currentHp = maxHp;
        currentMp = maxMp;
        mpAutoRecoveryTime = defaultMpAutoRecoveryTime;

        foreach (var material in modelMaterial)
        {
            originColor = new Color(material.color.r, material.color.g, material.color.b, 1.0f);
            material.color = originColor;
        }

        //originColor = new Color(modelMaterial.color.r, modelMaterial.color.g, modelMaterial.color.b, 1.0f);
        //modelMaterial.color = originColor;
        //Color alpha = new Color(0, 0, 0, 0.5f);
        //modelMesh.material.color -= alpha;

        playerAnimator = transform.FindChild("Tengu_Default").GetComponent<Animator>();
        audioSource = transform.GetComponent<AudioSource>();
        mouseController = transform.FindChild("MouseController").gameObject;
    }

    void Update()
    {
        //現在の時刻が（前の回復した時刻 + 自動回復の間隔）を過ぎている + 現在の妖力がmaxMpより少なければ妖力を回復する
        if ((Time.time >= lastMpAutoRecoveryTime + mpAutoRecoveryTime) && currentMp < maxMp)
        {
            lastMpAutoRecoveryTime = Time.time;
            currentMp += mpAutoRecoveryCost;
        }
        //現在のMPが最大値を超えていたら最大値にする
        if (currentMp > maxMp) currentMp = 100;

        if (currentMp < 10.0f) //妖力が10以下になったらmpOverをtrueに
        {
            mpOver = true;
            mpAutoRecoveryTime = overMpAutoRecoveryTime;
        }
        if (currentMp >= maxMp && mpOver == true) //妖力が100まで回復したらmpOverをfalseに
        {
            mpOver = false;
            mpAutoRecoveryTime = defaultMpAutoRecoveryTime;
        }

        if (invincible == true)
            currentInvincibleTime -= Time.deltaTime;
        if (currentInvincibleTime <= 0) //currentInvincibleTimeが0より小さくなったら無敵を解除
            invincible = false;

        //if (Input.GetKeyDown(KeyCode.K))
        //    HpDamage(1);
        //if (Input.GetKeyDown(KeyCode.L))
        //    HpDamage(3);
        //if (Input.GetKeyDown(KeyCode.J))
        //{
        //    playerAnimator.SetTrigger("Joy");
        //    gameObject.GetComponent<PlayerMove>().ChangeStop();
        //}

        flashingSecond -= Time.deltaTime;
        if (flashingSecond >= 0.0f)
        {
            Flashing();
        }
        else
        {
            //Color alphaReset = new Color(0, 0, 0, 1.0f - modelMaterial.color.a);
            //modelMaterial.color = originColor;
            foreach (var material in modelMaterial)
            {
                originColor = new Color(material.color.r, material.color.g, material.color.b, 1.0f);
                material.color = originColor;
            }
        }

    }

    public void HpRecovery(int cost) //HPをcost分回復　maxHPを超えていたらmaxHPを代入
    {
        if (currentHp + cost > maxHp)
            currentHp = maxHp;
        else
            currentHp += cost;
    }

    public void HpDamage(int damage) //HPをダメージ分減らす
    {
        if (invincible == true) return; //無敵だったら処理を行わない
        if (currentHp <= 0) return; //現在のHPが0以下だったら処理を行わない

        if (currentHp - damage <= 0) //現在のHPが0より小さかったら0に
        {
            currentHp = 0;
            playerAnimator.SetTrigger("Dead");
            mouseController.SetActive(false);
            gameObject.GetComponent<PlayerMove>().ChangeStop();
            audioSource.PlayOneShot(dead); // 対応SE再生
            print("体力がなくなり死亡しました");
            return;
        }
        else
        {
            currentHp -= damage;
        }

        if (damage == 1) //damageが1だったらknockBackSmallInvincibleTimeを代入
        {
            currentInvincibleTime = knockBackSmallInvincibleTime;
            playerAnimator.SetTrigger("KnockBackSmall");
            gameObject.GetComponent<PlayerMove>().ChangeKnockBackSmall();
            audioSource.PlayOneShot(smallDamage); // 対応SE再生
            SetFlashingSecond(1.0f);
        }
        else if (damage >= 2) //damageが2以上だったらknockBackLargeInvincibleTimeを代入
        {
            currentInvincibleTime = knockBackLargeInvincibleTime;
            playerAnimator.SetTrigger("KnockBackLarge");
            gameObject.GetComponent<PlayerMove>().ChangeKnockBackLarge(10.0f);
            audioSource.PlayOneShot(largeDamage); // 対応SE再生
            SetFlashingSecond(3.0f);
        }

        invincible = true; //無敵に
    }

    public void HpDamage(int damage, GameObject damageObject) //HPをダメージ分減らす
    {
        if (invincible == true) return; //無敵だったら処理を行わない
        if (currentHp <= 0) return; //現在のHPが0以下だったら処理を行わない

        if (currentHp - damage <= 0) //現在のHPが0より小さかったら0に
        {
            currentHp = 0;
            transform.LookAt(damageObject.transform.position); // gameObjectの方を向く
            gameObject.GetComponent<PlayerMove>().knockBackState = true;
            playerAnimator.SetTrigger("Dead");
            gameObject.GetComponent<PlayerMove>().ChangeStop();
            audioSource.PlayOneShot(dead); // 対応SE再生
            print("体力がなくなり死亡しました");
            return;
        }
        else
        {
            currentHp -= damage;
        }

        if (damage == 1) //damageが1だったらknockBackSmallInvincibleTimeを代入
        {
            currentInvincibleTime = knockBackSmallInvincibleTime;
            transform.LookAt(damageObject.transform.position); // gameObjectの方を向く
            playerAnimator.SetTrigger("KnockBackSmall");
            gameObject.GetComponent<PlayerMove>().ChangeKnockBackSmall();
            audioSource.PlayOneShot(smallDamage); // 対応SE再生
            SetFlashingSecond(1.0f);
        }
        else if (damage >= 2) //damageが2以上だったらknockBackLargeInvincibleTimeを代入
        {
            currentInvincibleTime = knockBackLargeInvincibleTime;
            transform.LookAt(damageObject.transform.position); // gameObjectの方を向く
            playerAnimator.SetTrigger("KnockBackLarge");
            gameObject.GetComponent<PlayerMove>().ChangeKnockBackLarge(10.0f);
            audioSource.PlayOneShot(largeDamage); // 対応SE再生
            SetFlashingSecond(3.0f);
        }

        invincible = true; //無敵に
    }

    public void MpConsumption(float cost) //cost分現在のMPを消費する
    {
        currentMp -= cost;
    }

    public bool MpCostDecision(float cost)
    {
        if (mpOver == true) //一度妖力が0になったら100になるまで使えない
        {
            print("妖力回復中");
            return false;
        }
        if (currentMp < cost)
        {
            print("妖力が足りない");
            return false;
        }
        return true;
    }

    public void SetFlashingSecond(float second)
    {
        flashingSecond = second;
    }

    public void Flashing()
    {
        //アルファが0以下になったらtreu　1以上になったらfalse
        if (modelMaterial[0].color.a <= 0.0f)
            alphaZero = true;
        else if (modelMaterial[0].color.a >= 1.0f)
            alphaZero = false;

        Color alpha = new Color(0, 0, 0, Time.deltaTime * 2.0f); //アルファを0.5秒で1変化させる

        if (alphaZero == false)
        {
            foreach (Material material in modelMaterial)
            {
                material.color -= alpha;
            }
        }
        else
        {
            foreach (Material material in modelMaterial)
            {
                material.color += alpha;
            }
        }
    }

    public void Avoidance()
    {
        invincible = true;
        currentInvincibleTime = 1.0f;
    }
}