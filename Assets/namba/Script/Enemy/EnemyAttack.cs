using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {

    public GameObject ohuda;
    public GameObject bow;
    public GameObject punch1, punch2;
    private float cooltime_S ,cooltime_M, cooltime_L;
    private bool run = false;
    private bool flag = false;
    private float a;

    void Start()
    {
        cooltime_S = 0.5f;
        cooltime_M = 3.0f;
        cooltime_L = 3.0f;
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

        if (!flag)
        {
            Instantiate(punch1, transform.position + transform.forward, transform.rotation);
            Vector3 vec = transform.position + transform.forward * 2;
            iTween.MoveTo(gameObject, iTween.Hash("position", vec));
            flag = true;
            yield return new WaitForSeconds(cooltime_S);
        }

        // ランダムでパンチ
        a = Random.Range(0, 2);
        if (a == 0) {
            Instantiate(punch1, transform.position + transform.forward, transform.rotation);
        }else {
            Instantiate(punch2, transform.position + transform.forward, transform.rotation);
        }

        yield return new WaitForSeconds(cooltime_S);
        run = false;
    }

    private IEnumerator OhudaAttack()
    {
        if (run) { yield break; }
        run = true;
        //処理

        Vector3 vec = transform.up / 2;
        Instantiate(ohuda, transform.position + vec, transform.rotation);


        yield return new WaitForSeconds(cooltime_M);
        run = false;
    }

    private IEnumerator yumiAttack()
    {
        if (run) { yield break; }
        run = true;
        //処理

        Instantiate(bow, transform.position, this.transform.rotation);


        yield return new WaitForSeconds(cooltime_L);
        run = false;
    }
}
