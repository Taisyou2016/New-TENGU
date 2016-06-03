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
    public int life = 1000;
    public float speed = 4;
    public float madnessspeed = 8;

    public Vector2 movedis = new Vector2(2,5);
    public Vector2 tornadoDis = new Vector2(2,4);
    public Vector2 windSlashDis = new Vector2(1, 4);
    public float dispelDies = 1;

    [SerializeField]
    private string state;
    [SerializeField]
    private int nowlife;

    private float rotateSmooth = 3.0f;  // 振り向きにかかる時間


    private Transform player;
    private Rigidbody rd;
    private BossAttack attack;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rd = GetComponent<Rigidbody>();
        attack = GetComponent<BossAttack>();

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
        if (life <= 0)
        {
            ChangeState(BossState.Died);
        }
        print("HP :" + life);
        ChangeState(BossState.Hit);

    }

    // プレイヤーとの距離
    public void PDistance()
    {
        float Distance = Vector3.SqrMagnitude(this.transform.position - player.position);
        Distance *= 0.1f;


        if(Distance >= dispelDies)
        {
            ChangeState(BossState.Dispel);
        }
        else if(Distance >= windSlashDis.y && nowlife / life >= 0.3){
            ChangeState(BossState.WindSlash);
        }
        else
        {
            ChangeState(BossState.Tornado);
        }
    }

    // 覚醒状態
    public bool Madness()
    {
        bool flag = nowlife / life <= 0.5f;

        return flag;
    }



    /*----------------------------------------------------/
                    ここからState処理
    /----------------------------------------------------*/

    // 待機状態
    private class IdleState : IState<BossRoutine>
    {
        public IdleState(BossRoutine owner) : base(owner) { }

        public override void Initialize()
        {
            owner.state = "Idle";
        }

        public override void Execute()
        {
            owner.StopCoroutine(move());
        }

        public override void End()
        {
        }

        IEnumerator move()
        {
            yield return new WaitForSeconds(2);
            owner.ChangeState(BossState.Move);
        }
    }

    // 移動状態
    private class MoveState : IState<BossRoutine>
    {
        float targetdis;

        public MoveState(BossRoutine owner) : base(owner) { }

        public override void Initialize()
        {
            owner.state = "Move";

            targetdis = Random.Range(owner.movedis.x, owner.movedis.y);
            float Distance = Vector3.SqrMagnitude(owner.transform.position - owner.player.position);
            Distance /= 10;
            targetdis = Distance - targetdis;

            owner.StopCoroutine(move());
        }

        public override void Execute()
        {
        }

        public override void End()
        {
        }

        IEnumerator move()
        {
            // Playerの方向を向く
            Vector3 vec = owner.player.position - owner.transform.position;
            vec.y = 0;
            Quaternion targetRotate = Quaternion.LookRotation(vec);

            owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, targetRotate, Time.deltaTime * owner.rotateSmooth);

            iTween.MoveTo(owner.gameObject, iTween.Hash("position", owner.transform.position + owner.transform.forward * targetdis));

            if (owner.Madness())
            {
            }

            yield return new WaitForSeconds(1);
            // 移動が終わったら
            owner.PDistance();
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
            }
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
            yield return new WaitForSeconds(1);

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
            yield return new WaitForSeconds(1);

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
            vec = owner.transform.position + owner.transform.forward * 4;
        }

        public override void Execute()
        {
            // 攻撃終了後移行
            owner.ChangeState(BossState.Hexagram);
        }

        public override void End()
        {
        }

        IEnumerator Attack()
        {
            iTween.MoveTo(owner.gameObject, iTween.Hash("position", vec));
            owner.attack.Attack(3);

            // 攻撃モーションが終わり次第
            yield return new WaitForSeconds(1);

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
            yield return new WaitForSeconds(1);

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
