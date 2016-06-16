using UnityEngine;
using System.Collections;

public enum BossState
{
    Idle,
    Move,
    Hexagram,
    Dispel,
    WindSlash,
    Tornado,
    Died,
    Hit
}

public class BossRoutine : EnemyBase<BossRoutine, BossState> {

    // ボスステータス
    public int life = 1000;                             // 最大HP
    public float speed = 4;                             // 通常時スピード
    public float madnessspeed = 8;                      // 発狂時スピード

    public Vector2 targetdis = new Vector2(2,5);
            
    public Vector2 tornadoDis = new Vector2(2,4);
    public Vector2 windSlashDis = new Vector2(1, 4); 
    public float displeDis = 1;

    [SerializeField]
    private string state;
    public int nowlife;

    private float rotateSmooth = 3.0f;  // 振り向きにかかる時間
    private float angle = 60.0f;
    private float dt;
    private bool Madness = false;
    private bool flag = false;

    private Transform player;
    private CharacterController charcon;
    private BossAttack attack;
    public Animator anima;
    

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        attack = GetComponent<BossAttack>();
        charcon = GetComponent<CharacterController>();

        nowlife = life;


        // Stateの初期設定
        statelist.Add(new IdleState(this));
        statelist.Add(new MoveState(this));
        statelist.Add(new HexagramAttack(this));
        statelist.Add(new DispelAttack(this));
        statelist.Add(new WindSlashAttack(this));
        statelist.Add(new TornadoAttack(this));
        statelist.Add(new DiedState(this));
        statelist.Add(new HitState(this));

        stateManager = new StateManager<BossRoutine>();
        ChangeState(BossState.Idle);
    }

    // ダメージ処理
    public void Damage(int dmg)
    {
        nowlife -= dmg;
        if(nowlife / life <= 0.5f)
        {
            Madness = false;
            speed = madnessspeed;
        }
        else if (nowlife >= 0)
        {
            print("HP :" + life);
            ChangeState(BossState.Hit);
        }
        else
        {
            if (flag) { return; }
            flag = true;
            ChangeState(BossState.Died);
        }


    }

    // プレイヤーとの距離
    public void PDistance()
    {
        float Distance = Vector3.Distance(this.transform.position,player.position);
        print(Distance);
        if(Distance <= displeDis)
        {
            ChangeState(BossState.Dispel);
        }
        else if(Distance <= windSlashDis.y && nowlife / life >= 0.3){
            ChangeState(BossState.WindSlash);
        }
        else
        {
            ChangeState(BossState.Tornado);
        }
    }

    // Playerの方向を向く
    public void PLookAt()
    {
        Vector3 vec = player.position - transform.position;
        vec.y = 0;
        Quaternion targetRotate = Quaternion.LookRotation(vec);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotate, Time.deltaTime * rotateSmooth);

    }

    // 移動処理
    public void Move(Vector3 velocity)
    {
        velocity.y += Physics.gravity.y;
        charcon.Move(velocity * speed * Time.deltaTime);
    }

    // ジャンプ移動処理
    public void JumpMove(float Vx, float Vy)
    {
        Vector3 vec = transform.forward * Vx + transform.up * (Vy - (9.8f * dt));
        vec = vec / 2;
        charcon.Move(vec * speed * Time.deltaTime);
        dt += Time.deltaTime;
    }

    /*----------------------------------------------------/
                    ここからState処理
    /----------------------------------------------------*/

    // 待機状態
    private class IdleState : IState<BossRoutine>
    {
        public IdleState(BossRoutine owner) : base(owner) { }
        private bool run;

        public override void Initialize()
        {
            owner.state = "Idle";
        }

        public override void Execute()
        {
            owner.StartCoroutine(move());
        }

        public override void End()
        {
        }

        IEnumerator move()
        {
            if (run) { yield break; }
            run = true;

            yield return new WaitForSeconds(2);
            owner.ChangeState(BossState.Move);

            run = false;
        }
    }

    // 移動状態
    private class MoveState : IState<BossRoutine>
    {
        private float targetdis, Distance;
        private Vector3 destination;
        private float Vx, Vy;

        public MoveState(BossRoutine owner) : base(owner) { }

        public override void Initialize()
        {
            owner.state = "Move";
            targetdis = Random.Range(owner.targetdis.x, owner.targetdis.y);
            owner.anima.SetTrigger("Move");

            if (owner.Madness)
            {
                owner.anima.SetTrigger("Jump");

                float subx = Vector3.Distance(owner.player.position, owner.transform.position);
                float suby = subx / (Mathf.Sin(2 * owner.angle * Mathf.Deg2Rad) / 9.8f);
                Vx = Mathf.Sqrt(suby) * Mathf.Cos(owner.angle * Mathf.Deg2Rad);
                Vy = Mathf.Sqrt(suby) * Mathf.Sin(owner.angle * Mathf.Deg2Rad);
                owner.transform.rotation = Quaternion.LookRotation(owner.player.position - owner.transform.position);
            }
        }

        public override void Execute()
        {
            owner.PLookAt();

            Distance = Vector3.Distance(owner.player.position, owner.transform.position);
            destination = (owner.player.position - owner.transform.position).normalized;

            if (Distance > targetdis)
            {
                destination.y = 0;
                if (owner.Madness)
                {
                    owner.JumpMove(Vx, Vy);
                    return;
                }
                owner.Move(destination);

                return;
            }

            // 移動が終わったら
            owner.PDistance();

        }

        public override void End()
        {
            owner.anima.SetTrigger("MoveDown");

            owner.dt = 0;
        }

    }

    // 六芒星攻撃
    private class HexagramAttack : IState<BossRoutine>
    {
        public HexagramAttack(BossRoutine owner) : base(owner) { }

        private GameObject[] enemys;

        public override void Initialize()
        {
            owner.state = "Hexagram";

            enemys = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemys.Length > 0)
            {
                owner.ChangeState(BossState.Move);
                return;
            }
            owner.anima.SetTrigger("Hexa");
            owner.StartCoroutine(Attack());
        }

        public override void Execute()
        {


        }

        public override void End()
        {
        }

        IEnumerator Attack()
        {
            // 六芒星生成
            owner.attack.Attack(1);

            // 攻撃モーションが終わり次第
            while (owner.anima.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {
                yield return null;
            }


            // 攻撃終了後移行
            owner.ChangeState(BossState.Move);
        }

    }

    // 吹き飛ばし攻撃
    private class DispelAttack : IState<BossRoutine>
    {
        public DispelAttack(BossRoutine owner) : base(owner) { }

        public override void Initialize()
        {
            owner.state = "Dispel";
            owner.anima.SetTrigger("Dispel");
            owner.StartCoroutine(Attack());
        }

        public override void Execute()
        {

        }

        public override void End()
        {
        }

        IEnumerator Attack()
        {
            owner.attack.Attack(2);

            // 攻撃モーションが終わり次第
            while (owner.anima.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {
                yield return null;
            }


            // 攻撃終了後移行
            owner.ChangeState(BossState.Hexagram);
        }

    }

    // かまいたち攻撃
    private class WindSlashAttack : IState<BossRoutine>
    {
        public WindSlashAttack(BossRoutine owner) : base(owner) { }

        Vector3 vec;

        public override void Initialize()
        {
            owner.state = "WindSlash";
            owner.anima.SetTrigger("Cutter");
            vec = owner.transform.position + owner.transform.forward * 4;
            owner.StartCoroutine(Attack());
        }

        public override void Execute()
        {
            // 攻撃終了後移行
            //owner.ChangeState(BossState.Hexagram);
        }

        public override void End()
        {
        }

        IEnumerator Attack()
        {
            iTween.MoveTo(owner.gameObject, iTween.Hash("position", vec));
            owner.attack.Attack(3);

            // 攻撃モーションが終わり次第
            while (owner.anima.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {
                yield return null;
            }


            // 攻撃終了後移行
            owner.ChangeState(BossState.Hexagram);
        }

    }

    // 竜巻攻撃
    private class TornadoAttack : IState<BossRoutine>
    {
        public TornadoAttack(BossRoutine owner) : base(owner) { }

        public override void Initialize()
        {
            owner.state = "Tornado";
            owner.anima.SetTrigger("Toranado");
            owner.StartCoroutine(Attack());
        }

        public override void Execute()
        {
        }

        public override void End()
        {
        }

        IEnumerator Attack()
        {
            owner.attack.Attack(4);

            // 攻撃モーションが終わり次第
            while (owner.anima.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {
                yield return null;
            }

            // 攻撃終了後移行
            owner.ChangeState(BossState.Hexagram);
        }

    }

    // 死亡状態
    private class DiedState : IState<BossRoutine>
    {
        public DiedState(BossRoutine owner) : base(owner) { }

        public override void Initialize()
        {
            owner.state = "Died";
            owner.anima.SetTrigger("Death");

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
    private class HitState : IState<BossRoutine>
    {
        public HitState(BossRoutine owner) : base(owner) { }

        public override void Initialize()
        {
            owner.state = "Hit";
            owner.anima.SetTrigger("Damage");
            owner.StartCoroutine(hit());
        }

        public override void Execute()
        {
        }

        public override void End()
        {
        }

        IEnumerator hit()
        {
            yield return new WaitForSeconds(1);
            owner.ChangeState(BossState.Move);
        }
    }

}
