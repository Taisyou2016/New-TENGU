using UnityEngine;
using System.Collections;

public enum EnemyState
{
    Wait,
    Pursuit,
    LostContact,
    Attack,
    Died,
    Hit
}

public class EnemyRoutine : EnemyBase<EnemyRoutine, EnemyState>
{
    public float SearchDistance = 10f;  // 透過視認範囲
    public int maxlife = 40;            // 最大ＨＰ
    public float speed;                 // スピード
    public int LengeType = 2;           // 攻撃タイプ(1=格闘 2=お札 3=弓)

    [SerializeField]
    private int life;
    public bool Pflag = false;          // プレイヤーを見つけたか
    public bool Gflag = false;          // 地面に足がついているか
    public bool Hflag = false;          // 攻撃を受けたか
    public string state;                // デバッグ用State確認
    private float rotateSmooth = 3.0f;  // 振り向きにかかる時間
    private float AttackDistance;       // 攻撃移行範囲
    private Vector3 StartPos;
    private Vector3 lostPos;
    private Transform player;
    private NavMeshAgent agent;
    private Rigidbody rd;
    private EnemyAttack attack;

    // Use this for initialization
    public void Start()
    {
        // Playerの座標を取得
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        rd = GetComponent<Rigidbody>();
        attack = GetComponent<EnemyAttack>();

        if (LengeType == 1) { AttackDistance = 1; }
        else if (LengeType == 2) { AttackDistance = 8; }
        else if (LengeType == 3) { AttackDistance = 10; }
        life = maxlife;
        StartPos = this.transform.position;
        lostPos = StartPos;
        Switch(0);

        // Stateの初期設定
        statelist.Add(new StateWait(this));
        statelist.Add(new StatePursuit(this));
        statelist.Add(new StateLostContact(this));
        statelist.Add(new StateAttack(this));
        statelist.Add(new StateDied(this));
        statelist.Add(new HitState(this));

        stateManager = new StateManager<EnemyRoutine>();

        ChangeState(EnemyState.Wait);
    }

    // プレイヤーを索敵
    private void PSeach()
    {
        // Playerとの距離
        float ToTargetDistance = Vector3.SqrMagnitude(this.transform.position - player.position);

        // 透過視認範囲外ならば
        if (ToTargetDistance > SearchDistance * 10.0f)
        {
            Pflag = false;
            // 以降の処理をスルー
            return;
        }

        //Targetとの間の障害物がなければ行動
        RaycastHit hit;
        Vector3 temp = player.transform.position - this.transform.position;
        temp = temp.normalized;
        int layerMask = ~LayerMask.GetMask(new string[] { "Enemy", "Bullet", "PlayerAttack"});
        if (Physics.Raycast(this.transform.position, temp, out hit, SearchDistance, layerMask))
        {
            Pflag = hit.collider.tag == "Player";
        }
    }

    // 接地検知
    private void GroundingDetection()
    {
        int mask = LayerMask.GetMask(new string[] { "Field" });
        RaycastHit hit;
        Gflag = Physics.Raycast(this.transform.position, transform.up * -1, out hit, 1, mask);
        if(Gflag)
        {
            Hflag = false;
        }
        if(!Gflag && !Hflag)
        {
            iTween.RotateTo(gameObject, iTween.Hash("x", 0, "z", 0));
        }
    }

    /// <summary>
    /// NavMeshとIsKinematicのON/OFF
    /// </summary>
    /// <param name="a">0でON 1でOFF</param>
    private void Switch(int a)
    {
        if (a == 0)
        {
            agent.enabled = false;
            rd.isKinematic = false;
        }
        else if (a == 1)
        {
            agent.enabled = true;
            rd.isKinematic = true;
        }
    }

    public void Damage(int dmg)
    {
        life -= dmg;
        if (life <= 0)
        {
            ChangeState(EnemyState.Died);
        }
        print("HP :" + life);
        ChangeState(EnemyState.Hit);
    }

    private IEnumerator Lost()
    {
        yield return new WaitForSeconds(2);
        agent.SetDestination(StartPos);
    }

    /*----------------------------------------------------/
                        ここからState処理
    /----------------------------------------------------*/



    /// <summary>
    /// 待機状態
    /// </summary>
    private class StateWait : IState<EnemyRoutine>
    {
        public StateWait(EnemyRoutine owner) : base(owner) { }

        public override void Initialize()
        {
            owner.state = "wait";
        }

        public override void Execute()
        {
            owner.PSeach();
            if(owner.Pflag)
            {
                owner.Switch(0);
                owner.ChangeState(EnemyState.Pursuit);
            }

        }

        public override void End()
        {
        }

    }

    /// <summary>
    /// 追跡処理
    /// </summary>
    private class StatePursuit : IState<EnemyRoutine>
    {
        public StatePursuit(EnemyRoutine owner) : base(owner) { }

        public override void Initialize()
        {
            owner.Switch(0);
            owner.state = "pursuit";

        }

        public override void Execute()
        {
            owner.PSeach();
            owner.GroundingDetection();
            if(!owner.Pflag && owner.Gflag)
            {
                owner.ChangeState(EnemyState.LostContact);
            }

            // Playerとの距離
            float ToAttackDistance = Vector3.SqrMagnitude(this.owner.transform.position - owner.player.position);
            // 攻撃範囲内
            if (ToAttackDistance < owner.AttackDistance * 10.0f)
            {
                //攻撃ステートに移行
                owner.ChangeState(EnemyState.Attack);
            }


            // Playerの方向を向く
            Vector3 vec = owner.player.position - owner.transform.position;
            vec.y = 0;
            Quaternion targetRotate = Quaternion.LookRotation(vec);
            owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, targetRotate, Time.deltaTime * owner.rotateSmooth);

            // 前に進む
            owner.transform.Translate(Vector3.forward * owner.speed * Time.deltaTime);

        }

        public override void End()
        {
        }
    }

    /// <summary>
    /// 見失った時の処理
    /// </summary>
    private class StateLostContact : IState<EnemyRoutine>
    {
        public StateLostContact(EnemyRoutine owner) : base(owner) { }

        public override void Initialize()
        {
            owner.lostPos = owner.player.position;
            owner.state = "lost";
        }

        public override void Execute()
        {
            owner.PSeach();
            if(owner.Pflag)
            {
                owner.ChangeState(EnemyState.Pursuit);
            }

            owner.Switch(1);
            owner.agent.SetDestination(owner.lostPos);
            if (Vector3.SqrMagnitude(owner.transform.position - owner.lostPos) <= 2)
            {
                owner.StartCoroutine(owner.Lost());
                owner.ChangeState(EnemyState.Wait);
            }

        }

        public override void End()
        {
        }

    }

    /// <summary>
    /// 攻撃処理
    /// </summary>
    private class StateAttack : IState<EnemyRoutine>
    {
        public StateAttack(EnemyRoutine owner) : base(owner) { }

        public override void Initialize()
        {
            owner.Switch(0);
            owner.state = "attack";

        }

        public override void Execute()
        {
            // Playerとの距離
            float ToAttackDistance = Vector3.SqrMagnitude(owner.transform.position - owner.player.position);
            // 攻撃範囲外
            if (ToAttackDistance > owner.AttackDistance * 10.0f)
            {
                //追跡ステートに移行
                owner.ChangeState(EnemyState.Pursuit);
            }
            owner.PSeach();
            owner.GroundingDetection();

            if (!owner.Pflag && owner.Gflag)
            {
                owner.ChangeState(EnemyState.LostContact);
            }

            // Playerの方向を向く
            Vector3 vec = owner.player.position - owner.transform.position;
            vec.y = 0;
            Quaternion targetRotate = Quaternion.LookRotation(vec);
            owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, targetRotate, Time.deltaTime * owner.rotateSmooth);


            // 攻撃処理
            owner.attack.Attack(owner.LengeType);
        }

        public override void End()
        {
        }

    }

    /// <summary>
    /// 死亡処理
    /// </summary>
    private class StateDied : IState<EnemyRoutine>
    {
        public StateDied(EnemyRoutine owner) : base(owner) { }

        public override void Initialize()
        {
            owner.Switch(0);
            owner.state = "died";
            Destroy(owner.gameObject, 1.0f);
        }

        public override void Execute()
        {
        }

        public override void End()
        {
        }

    }

    // 攻撃を受けている状態
    private class HitState : IState<EnemyRoutine>
    {
        public HitState(EnemyRoutine owner) : base(owner) { }

        public override void Initialize()
        {
            owner.state = "Hit";
            owner.Switch(0);
        }

        public override void Execute()
        {
            int mask = LayerMask.GetMask(new string[] { "Field" });
            if (Physics.CheckSphere(owner.transform.position, 0.8f, mask))
            {
                owner.StartCoroutine(move());
            }
        }

        public override void End()
        {
        }

        IEnumerator move()
        {
            iTween.RotateTo(owner.gameObject, iTween.Hash("x", 0, "z", 0));

            yield return new WaitForSeconds(1);
            owner.Switch(1);
            owner.StartCoroutine(owner.Lost());
            owner.ChangeState(EnemyState.Wait);
        }
    }

}
