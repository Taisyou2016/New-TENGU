using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAttack : MonoBehaviour {

    public GameObject ohuda;
    public GameObject bow;
    public GameObject punch1, punch2;
    private float cooltime_S ,cooltime_M, cooltime_L;
    private bool run = false;
    private bool flag = false;
    private float a;

    private AudioSource se;
    private AnimatorStateInfo info;
    public Animator anima;
    public List<AudioClip> sounds = new List<AudioClip>();

    void Start()
    {
        cooltime_S = 1.0f;
        cooltime_M = 3.0f;
        cooltime_L = 3.0f;

        se = gameObject.AddComponent<AudioSource>();
        info = anima.GetCurrentAnimatorStateInfo(0);
    }

    /// <summary>
    /// Enemyの攻撃生成
    /// </summary>
    /// <param name="a">1=格闘 2=お札 3=弓</param>
    public void Attack(int a)
    {
        if(a == 1)
        {
            StartCoroutine(InFighting());
        }
        else if(a == 2)
        {
            StartCoroutine(OhudaAttack());
        }
        else if(a== 3)
        {
            StartCoroutine(yumiAttack());
        }
    }

    private IEnumerator InFighting()
    {
        if (run) { yield break; }
        run = true;
        // 処理

        Vector3 pos = transform.up / 2;
        if (!flag)
        {
            anima.SetTrigger("Panch1");
            Instantiate(punch1, transform.position + pos, transform.rotation);
            Vector3 vec = transform.position + transform.forward * 2;
            iTween.MoveTo(gameObject, iTween.Hash("position", vec));
            flag = true;
            yield return new WaitForSeconds(cooltime_S);
        }

        // ランダムでパンチ
        a = Random.Range(0, 2);
        if (a == 0) {
            anima.SetTrigger("Panch1");
            Instantiate(punch1, transform.position + pos, transform.rotation);
        }else {
            anima.SetTrigger("Panch2");
            Instantiate(punch2, transform.position + pos, transform.rotation);
        }
        se.PlayOneShot(sounds[0]);

        yield return new WaitForSeconds(cooltime_S);
        run = false;
    }

    private IEnumerator OhudaAttack()
    {
        if (run) { yield break; }
        run = true;
        //処理

        anima.SetTrigger("Attack");
        yield return new WaitForSeconds(1);
        while (anima.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.6f)
        {
            yield return null;
        }

        Vector3 vec = transform.up / 2;
        Instantiate(ohuda, transform.position + vec, transform.rotation);
        se.PlayOneShot(sounds[1]);

        yield return new WaitForSeconds(cooltime_M - 1);

        run = false;
    }

    private IEnumerator yumiAttack()
    {
        if (run) { yield break; }
        run = true;
        //処理

        anima.SetTrigger("Bow");
        yield return new WaitForSeconds(1);
        while (anima.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.6f)
        {
            yield return null;
        }

        Instantiate(bow, transform.position, this.transform.rotation);
        se.PlayOneShot(sounds[2]);

        yield return new WaitForSeconds(cooltime_L - 1);

        run = false;
    }
}
