using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayerMoveState;
using System;

public class PlayerMove : MonoBehaviour
{
    public GameObject lockPosition; //ロック時左右移動の際の回転位置
    public float walkSpeed = 4.0f; //歩くスピード（メートル/秒）
    public float lockOnRotateSpeed = 45.0f; //ロックオンしているときの横移動
    public float lockDistanceY = 10.0f;
    public float gravity = 10.0f; //重力加速度
    public float flightGravity = 10.0f; //滞空中の重力加速度
    public float flightGravityDeltaTimeMagnification = 0.1f; //滞空中の重力の計算に使うdeltaTimeに掛ける倍率
    public float flightGroundDistance = 5.0f; //滞空可能な地面との距離
    public float jampPower = 10.0f; //ジャンプするパワー
    public float knockBackPower = 0.0f; //KnockBackLargeの時吹き飛ぶパワー
    public bool knockBackState = false;
    public float windPower = 0.0f; //風のパワー
    public Vector3 windDirection; //風の方向
    public bool previosGroundHit = false; //ひとつ前の地面に当たっているかどうかの判定
    public bool currentGroundHit = false; //現在の地面に当たっているかどうかの判定
    public float WaitMotinChangeTime = 10.0f; // この秒放置されたら放置アニメーションへ遷移
    public float blowPower = 0.0f;
    public GameObject lockEnemy;
    public AnimationCurve blowCurve;
    public float blowStartTime;
    public AudioClip jump;
    public AudioClip glide;
    public bool avoidanceDecision = false;
    public bool inAvoidance = false;
    public float avoidanceTime = 0.0f;
    public float inAvoidanceTime = 0.0f;
    public float avoidanceMousePositionY = 0.0f;

    private CharacterController controller;
    private GameObject cameraController;
    private Vector3 velocity;
    private float velocityY = 0;
    private float gravityDeltaTimeMagnification = 1.0f; //重力の計算に使うdeltaTimeに掛ける倍率
    private float groundDistance; //プレイヤーの地面までの距離
    private bool jampState = false;
    private bool flightState = false;
    private bool windMove = false;
    private bool stop = false;
    private List<GameObject> lockEnemyList = new List<GameObject>();
    private bool lockOn = false;
    private bool lockOnBoss = false;
    private GameObject bossEnemy;
    private PlayerStatus playerStatus;
    private Animator playerAnimator;
    private float WaitTime;
    private AudioSource audioSource;

    public StateProcessor stateProcessor = new StateProcessor();
    public PlayerMoveStateDefault stateDefault = new PlayerMoveStateDefault();
    public PlayerMoveStateLockOn stateLockOn = new PlayerMoveStateLockOn();
    public PlayerMoveStateWind stateWind = new PlayerMoveStateWind();
    public PlayerMoveStateKnockBackSmall stateKnockBackSmall = new PlayerMoveStateKnockBackSmall();
    public PlayerMoveStateKnockBackLarge stateKnockBackLarge = new PlayerMoveStateKnockBackLarge();
    public PlayerMoveStateStop stateStop = new PlayerMoveStateStop();
    public PlayerMoveStateBlow stateBlow = new PlayerMoveStateBlow();

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraController = GameObject.FindGameObjectWithTag("CameraController");
        playerStatus = gameObject.GetComponent<PlayerStatus>();
        playerAnimator = transform.FindChild("Tengu_Default").GetComponent<Animator>();
        audioSource = transform.GetComponent<AudioSource>();

        bossEnemy = GameObject.FindGameObjectWithTag("Boss");

        stateProcessor.State = stateDefault;
        stateDefault.exeDelegate = Default;
        stateLockOn.exeDelegate = LockOn;
        stateWind.exeDelegate = Wind;
        stateKnockBackSmall.exeDelegate = KnockBackSmall;
        stateKnockBackLarge.exeDelegate = KnockBackLarge;
        stateStop.exeDelegate = Stop;
        stateBlow.exeDelegate = Blow;
    }

    void Update()
    {
        stateProcessor.Execute(); //設定されている移動状態を実行

        //if (currentGroundHit == false && flightState == false) velocityY -= gravity * Time.deltaTime;
        //velocity.y = velocityY;

        if (currentGroundHit == false && flightState == false) velocityY -= gravity * Time.deltaTime * gravityDeltaTimeMagnification;
        velocity.y = velocityY;

        if (knockBackState == false
            && windMove == false
            && jampState == true
            && velocityY <= -1.0f
            && playerStatus.MpCostDecision(playerStatus.flightCost)
            && groundDistance >= flightGroundDistance
            && Input.GetKey(KeyCode.Space)) //滞空処理
        {
            playerStatus.MpConsumption(playerStatus.flightCost);
            flightState = true;
            gravityDeltaTimeMagnification = 0.1f;
            gravity = flightGravity;
            //velocity.y = 0;
        }
        if ((flightState == true && Input.GetKeyUp(KeyCode.Space))
            || knockBackState == true
            || windMove == true
            || velocityY > -1.0f
            || !playerStatus.MpCostDecision(playerStatus.flightCost)
            || groundDistance < flightGroundDistance)
        {
            flightState = false;
            gravityDeltaTimeMagnification = 1.0f;
            gravity = 10.0f;
        }

        playerAnimator.SetBool("FlightState", flightState);

        controller.Move(velocity * Time.deltaTime);

        if (stop == true) return;

        /*****************************************************************/

        //ロックオン開始終了　処理

        /*****************************************************************/
        foreach (var e in lockEnemyList)
        {
            if (e == null)
            {
                lockEnemyList.Remove(e);
                return;
            }

            if (e.gameObject.tag == "Enemy")
            {
                if (e.gameObject.GetComponent<EnemyRoutine>().life <= 0)
                {
                    lockEnemyList.Remove(e);
                    return;
                }
            }
            //else if (e.gameObject.tag == "Boss")
            //{
            //    if (e.gameObject.GetComponent<BossRoutine>().life <= 0)
            //    {
            //        lockEnemyList.Remove(e);
            //        return;
            //    }
            //}
        }

        lockEnemyList.Sort(LengthSort); //lockEnemyListをプレイヤーからの距離が短い順にソート

        if ((Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.LeftShift)) && lockEnemyList.Count > 0)//ロックオンする、しない
        {
            if (lockOn == false)
            {
                lockOn = true;
                lockOnBoss = false;
                lockEnemy = lockEnemyList[0];
                transform.LookAt(lockEnemy.transform.position); //ロックした敵の方を向く
                cameraController.GetComponent<CameraTest>().CameraInitialize(); //プレイヤーの後ろに回る
                print("ロックオン開始");
            }
            else
            {
                lockOn = false;
                lockOnBoss = false;
                print("ロックオン終了");
            }
        }

        //ボスをLockEnemyListに追加する、消す
        if (bossEnemy != null)
        {
            if (Vector3.Distance(transform.position, bossEnemy.transform.position) <= lockDistanceY)
            {
                int index = lockEnemyList.IndexOf(bossEnemy);
                if (index == -1)
                    lockEnemyList.Add(bossEnemy);
            }
            else
            {
                int index = lockEnemyList.IndexOf(bossEnemy);
                if (index != -1)
                    lockEnemyList.Remove(bossEnemy);
            }
        }

        //プレイヤーとロックしている敵とY軸が離れすぎたらロックを終了させる
        if (lockEnemy != null && lockEnemy.tag == "Boss" && lockOnBoss == false)
        {
            if (Mathf.Abs(transform.position.y - lockEnemy.transform.position.y) > lockDistanceY)
            {
                lockOnBoss = true;
                lockOn = false;
                print("上下の距離が離れすぎたロック終了");
            }
        }

        if (lockOnBoss == true)
        {
            //if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.LeftShift))//ロックオンする、しない
            //{
            //    lockOnBoss = false;
            //    lockOn = false;
            //}

            if (Mathf.Abs(transform.position.y - lockEnemy.transform.position.y) <= lockDistanceY)
            {
                lockOn = true;
                lockEnemy = bossEnemy;
            }
        }



        //地面との判定　ジャンプ処理
        groundDistance = CheckGroundDistance();
        previosGroundHit = currentGroundHit; //ひとつ前の状態
        currentGroundHit = CheckGrounded(); //現在の状態
        if (previosGroundHit == false && currentGroundHit == true)
        {
            velocityY = 0;
            print("地面に接触");
        }
        //if (controller.isGrounded && torndoHit == false)
        //    velocityY = 0;

        if (currentGroundHit)
            jampState = false;
        else
            jampState = true;

        if (knockBackState == false && jampState == false && Input.GetKeyDown(KeyCode.Space))
        {
            velocityY = jampPower;
            jampState = true;
            playerAnimator.SetTrigger("Jump");
            audioSource.PlayOneShot(jump);
        }

        /*****************************************************************/

        //アニメーション

        /*****************************************************************/
        playerAnimator.SetFloat("VelocityY", velocity.y);
        Vector3 velocityYzero = velocity;
        velocityYzero.y = 0;
        playerAnimator.SetFloat("VelocityMagnitude", velocityYzero.magnitude);
        playerAnimator.SetBool("Lockon", lockOn);
        playerAnimator.SetFloat("LockonAxis", Input.GetAxis("Horizontal"));
        playerAnimator.SetBool("KnockBackState", knockBackState);
        playerAnimator.SetBool("WindMove", windMove);
        playerAnimator.SetBool("GroundHit", currentGroundHit);

        AnimatorStateInfo aniStateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
        if (aniStateInfo.nameHash == Animator.StringToHash("Base Layer.Wait"))
        {
            WaitTime += Time.deltaTime;
            if (WaitTime >= WaitMotinChangeTime)
            {
                playerAnimator.SetTrigger("WaitMotion");
                WaitTime = 0.0f;
            }
        }
        else WaitTime = 0.0f;


        //if (Input.GetKeyDown(KeyCode.B))
        //    SetBlowPower(20.0f);
    }



    public void OnTriggerEnter(Collider other) //ロックオン範囲に入った敵をListに追加
    {
        if (other.gameObject.tag == "Enemy")
            lockEnemyList.Add(other.gameObject);
    }

    public void OnTriggerExit(Collider other) //ロックオン範囲から出た敵をListから削除
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (lockEnemy == other.gameObject) //範囲外出た敵がロックしている敵だったら　ロックを解除
            {
                lockOn = false;
            }
            lockEnemyList.Remove(other.gameObject);
            print("敵が範囲外に出た");
        }
    }

    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //if (windMove == false) return;

        if (hit.gameObject.tag == "Field")
        {
            windPower = 0;
            blowPower = 0;
        }
    }

    public int LengthSort(GameObject a, GameObject b) //Listを敵との距離が近い順にソート
    {
        //if (a != null || b != null)
        //{
        Vector3 VecA = transform.position - a.transform.position;
        Vector3 VecB = transform.position - b.transform.position;

        if (VecA.magnitude > VecB.magnitude) return 1;
        else if (VecA.magnitude < VecB.magnitude) return -1;
        else return 0;
        //}
        //else
        //{
        //    foreach (GameObject n in lockEnemyList)
        //    {
        //        lockEnemyList.Remove(n);
        //        return 0;
        //    }
        //}
        //return 0;
    }

    public bool CheckGrounded() //地面に接地しているかどうかを調べる
    {
        ////controller.isGroundedがtrueならRaycastを使わずに判定終了
        ////if (controller.isGrounded) return true;
        ////放つ光線の初期位置と姿勢
        ////若干体にめり込ませた位置から発射しないと正しく判定できないときがある
        //Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        ////探索距離
        //float tolerance = 1.3f;
        //Debug.DrawRay(ray.origin, ray.direction * tolerance);
        ////Raycastがhitするかどうかで判定
        ////RaycastHit hit;
        ////地面にのみ衝突するようにレイヤを指定する
        //return Physics.Raycast(ray, tolerance, 1 << 8);
        ////return Physics.BoxCast(ray.origin, new Vector3(0.1f, 0.05f, 0.1f), ray.direction, transform.rotation, tolerance, 1 << 8);

        if (groundDistance < 1.3f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public float CheckGroundDistance()
    {
        //若干体にめり込ませた位置から発射しないと正しく判定できないときがある
        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        //探索距離
        float tolerance = Mathf.Infinity;
        Debug.DrawRay(ray.origin, ray.direction * tolerance);
        //Raycastがhitするかどうかで判定
        RaycastHit hit;
        //地面にのみ衝突するようにレイヤを指定する
        Physics.Raycast(ray, out hit, tolerance, 1 << 8);

        return (transform.position - hit.point).magnitude;
    }

    public bool GetLockOnInfo()
    {
        return lockOn;
    }

    public bool GetJampState()
    {
        return jampState;
    }

    public bool GetWindMove()
    {
        return windMove;
    }

    public void SetWindPower(float power, Vector3 direction)
    {
        windPower = power;
        windDirection = direction;
    }

    public void SetVelocityY(int velocity)
    {
        if (knockBackState == true) return;

        velocityY = velocity;
    }

    public void SetBlowPower(float power)
    {
        blowPower = power;
        blowStartTime = Time.timeSinceLevelLoad;
    }

    public void LookForward()
    {
        Vector3 lookPos = transform.position + transform.forward;
        lookPos.y = transform.position.y;

        transform.LookAt(lookPos);
    }

    /*****************************************************************/

    //プレイヤーの移動状態関係

    /*****************************************************************/
    public void Default() //通常移動
    {
        ////カメラの正面向きのベクトルを取得
        //Vector3 cameraForward = Camera.main.transform.forward;
        ////y成分を無視する
        //cameraForward.y = 0;
        ////正規化（長さを1にする）
        //cameraForward.Normalize();
        //velocity =
        //    cameraForward * Input.GetAxis("Vertical") * walkSpeed
        //    + Camera.main.transform.right * Input.GetAxis("Horizontal") * walkSpeed;

        ////キャラクターの向きを変える
        //velocity.y = 0;
        //if (velocity.magnitude > 0)
        //{
        //    transform.LookAt(transform.position + velocity);
        //}


        if (inAvoidance == false)
        {
            Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
            Vector3 right = Camera.main.transform.TransformDirection(Vector3.right);

            velocity = Input.GetAxis("Horizontal") * right + Input.GetAxis("Vertical") * forward;
            velocity *= walkSpeed;

            Vector3 velocityYzero = velocity;
            velocityYzero.y = 0;

            //ベクトルの２乗の長さを返しそれが0.001以上なら方向を変える（０に近い数字なら方向を変えない） 
            if (velocityYzero.magnitude > 0)
            {

                //２点の角度をなだらかに繋げながら回転していく処理（stepがその変化するスピード） 
                float step = 5.0f * Time.deltaTime;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, velocityYzero, step, 0f);

                transform.rotation = Quaternion.LookRotation(newDir);
            }
        }
        else
        {
            inAvoidanceTime += Time.deltaTime;
            if (inAvoidanceTime >= 1.0f)
            {
                inAvoidance = false;
            }
        }

        //回避
        if ((Input.mousePosition.y <= 0 || Input.mousePosition.y >= Screen.height) && avoidanceDecision == false && inAvoidance == false)
        {
            if (Input.GetMouseButton(1) || Input.GetMouseButton(2)) return;

            avoidanceDecision = true;
            avoidanceTime = 0.0f;
            if (Input.mousePosition.y <= 10) avoidanceMousePositionY = 0.0f;
            else avoidanceMousePositionY = Screen.height;
        }

        if (avoidanceDecision == true)
        {
            if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
            {
                avoidanceDecision = false;
                return;
            }

            avoidanceTime += Time.deltaTime;
            if (avoidanceTime >= 1.0f && inAvoidance == false)
            {
                avoidanceDecision = false;
                avoidanceTime = 0.0f;
                return;
            }

            if (avoidanceMousePositionY == 0)
            {
                if (Input.mousePosition.y >= Screen.height)
                {
                    //回避の処理
                    inAvoidance = true;
                    velocity = transform.forward * 10.0f;

                    avoidanceDecision = false;
                    avoidanceTime = 0.0f;
                    inAvoidanceTime = 0.0f;
                }
            }
            else
            {
                if (Input.mousePosition.y <= 0.0f)
                {
                    //回避の処理
                    inAvoidance = true;
                    velocity = transform.forward * 10.0f;

                    avoidanceDecision = false;
                    avoidanceTime = 0.0f;
                    inAvoidanceTime = 0.0f;
                }
            }
        }

        if (lockOn == true)
        {
            cameraController.GetComponent<CameraTest>().CameraInitialize();
            stateProcessor.State = stateLockOn;
        }
        if (windPower >= 1)
        {
            playerAnimator.SetTrigger("WindMoveOn");
            cameraController.GetComponent<CameraTest>().CameraInitialize();
            audioSource.PlayOneShot(glide);
            stateProcessor.State = stateWind;
        }
        if (blowPower >= 1)
        {
            cameraController.GetComponent<CameraTest>().CameraInitialize();
            stateProcessor.State = stateBlow;
        }
    }

    public void LockOn() //ロックオン時移動
    {
        if (lockOn == false)
        {
            LookForward();
            cameraController.GetComponent<CameraTest>().CameraInitialize();
            lockPosition.transform.position = transform.position;
            stateProcessor.State = stateDefault;
            return;
        }
        if (windPower >= 1)
        {
            cameraController.GetComponent<CameraTest>().CameraInitialize();
            lockOn = false;
            lockPosition.transform.position = transform.position;
            playerAnimator.SetTrigger("WindMoveOn");
            audioSource.PlayOneShot(glide);
            stateProcessor.State = stateWind;
            return;
        }
        if (blowPower >= 1)
        {
            cameraController.GetComponent<CameraTest>().CameraInitialize();
            lockOn = false;
            lockPosition.transform.position = transform.position;
            stateProcessor.State = stateBlow;
            return;
        }


        if (lockEnemyList.Count == 0)
        {
            lockOn = false;
            print("範囲内に敵なしロックオン終了");
            return;
        }

        //ロックを1つ近い敵に変更
        if (Input.GetKeyDown(KeyCode.E))
        {
            int index = lockEnemyList.IndexOf(lockEnemy);
            if (index == 0)
                lockEnemy = lockEnemyList[lockEnemyList.Count - 1];
            else
                lockEnemy = lockEnemyList[index - 1];

            transform.LookAt(lockEnemy.transform.position); //ロックした敵の方を向く
            cameraController.GetComponent<CameraTest>().CameraInitialize(); //プレイヤーの後ろに回る
        }
        //ロックを1つ遠い敵に変更
        if (Input.GetKeyDown(KeyCode.R))
        {
            int index = lockEnemyList.IndexOf(lockEnemy);
            if (index == lockEnemyList.Count - 1)
                lockEnemy = lockEnemyList[0];
            else
                lockEnemy = lockEnemyList[index + 1];

            transform.LookAt(lockEnemy.transform.position); //ロックした敵の方を向く
            cameraController.GetComponent<CameraTest>().CameraInitialize(); //プレイヤーの後ろに回る
        }

        if (lockEnemyList != null && lockEnemy == null)
        {
            lockEnemy = lockEnemyList[0];
            cameraController.GetComponent<CameraTest>().CameraInitialize(); //プレイヤーの後ろに回る
        }

        if (lockEnemy == null)
        {
            LookForward();
            cameraController.GetComponent<CameraTest>().CameraInitialize();
            lockOn = false;
            lockPosition.transform.position = transform.position;
            stateProcessor.State = stateDefault;
            return;
        }

        //if (Input.GetAxis("Horizontal") != 0)
        //{
        //    if (Vector3.Distance(transform.position, lockPosition.transform.position) <= 1)
        //    {
        //        //lockRotatePosition.transform.RotateAround(
        //        //     lockEnemy.transform.position,
        //        //     transform.up,
        //        //     walkSpeed * Time.deltaTime * -Input.GetAxis("Horizontal"));
        //        //lockRotatePosition.transform.position = transform.position;
        //        lockPosition.transform.RotateAround(
        //             lockEnemy.transform.position,
        //             transform.up,
        //             1.0f);
        //    }
        //    else
        //    {
        //        lockPosition.transform.RotateAround(
        //            lockEnemy.transform.position,
        //            transform.up,
        //            -1.0f);
        //    }
        //}

        lockPosition.transform.position = transform.position;
        lockPosition.transform.RotateAround(lockEnemy.transform.position, transform.up, 0.01f);

        velocity =
            (lockEnemy.transform.position - transform.position).normalized * Input.GetAxis("Vertical") * walkSpeed
            + (transform.position - lockPosition.transform.position).normalized * Input.GetAxis("Horizontal") * walkSpeed;

        //キャラクターの向きを変える
        transform.LookAt(lockEnemy.transform.position);
    }

    public void Wind() //気流に乗った時の移動
    {
        windMove = true;
        Vector3 right = Camera.main.transform.TransformDirection(Vector3.right);

        velocity =
            (windDirection + right * Input.GetAxis("Horizontal")) + transform.forward
            * windPower;

        windPower -= Time.deltaTime;
        if (Input.GetAxis("Vertical") < 0)
            windPower -= Time.deltaTime * 20;

        transform.LookAt(transform.position + velocity);
        transform.Rotate(new Vector3(0, 0, 1), -15 * Input.GetAxis("Horizontal"));
        //transform.rotation = transform.rotation * Quaternion.AngleAxis(Input.GetAxis("Horizontal") * 10.0f, transform.forward);

        if (windPower <= 0.5 || currentGroundHit)
        {
            //transform.rotation = Quaternion.AngleAxis(-transform.eulerAngles.z, transform.forward);
            windPower = 0;
            windMove = false;
            playerAnimator.SetTrigger("WindMoveOff");
            LookForward();
            cameraController.GetComponent<CameraTest>().CameraInitialize();
            stateProcessor.State = stateDefault;
        }
        //if (windPower <= 0.5 && lockOn == true) stateProcessor.State = stateLockOn;
    }

    public void KnockBackSmall() //ノックバック小が起きた時の移動
    {
        knockBackState = true;

        velocity = Vector3.zero;
    }

    public void KnockBackLarge() //ノックバック大が起きた時の移動
    {
        knockBackState = true;

        velocity =
            transform.forward * -1.0f * knockBackPower;

        if (knockBackPower > 0) knockBackPower -= 0.5f;

        if (lockOn == true)
            transform.LookAt(lockEnemy.transform.position);
    }

    public void Stop()
    {
        stop = true;
        knockBackState = true;
        velocity = Vector3.zero;
        if (currentGroundHit == false && flightState == false) velocityY -= gravity * Time.deltaTime;
        velocity.y = velocityY;
    }

    public void Blow()
    {
        knockBackState = true;
        velocity = -transform.forward * blowPower;
        //blowPower -= Time.deltaTime;
        //Mathf.Lerp(blowPower, 0.0f, blowCurve);
        var diff = Time.timeSinceLevelLoad - blowStartTime;
        var rate = diff / 2.0f;
        var curvePos = blowCurve.Evaluate(rate);
        blowPower = Mathf.Lerp(blowPower, 0.0f, curvePos);
        print(blowPower);

        if (blowPower <= 0.5f)
        {
            knockBackState = false;
            stateProcessor.State = stateDefault;
        }
    }

    public void ChangeKnockBackSmall()
    {
        stateProcessor.State = stateKnockBackSmall;
        Invoke("DefaultOrLockOnChange", 0.5f);
    }

    public void ChangeKnockBackLarge(float power)
    {
        knockBackPower = power;
        stateProcessor.State = stateKnockBackLarge;
        Invoke("DefaultOrLockOnChange", 2.0f);
    }

    public void DefaultOrLockOnChange()
    {
        if (lockOn == false) stateProcessor.State = stateDefault;
        if (lockOn == true) stateProcessor.State = stateLockOn;
        knockBackState = false;
    }

    public void ChangeStop()
    {
        stateProcessor.State = stateStop;
    }

    //public void OnDrawGizmos()
    //{
    //    Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);

    //    RaycastHit hitInfo;
    //    bool hit = Physics.BoxCast(ray.origin, new Vector3(0.25f, 0.05f, 0.25f), ray.direction, out hitInfo, transform.rotation, 100.0f, 1 << 8);
    //    if (hit)
    //    {
    //        Gizmos.DrawRay(transform.position + Vector3.up * 0.1f, ray.direction);
    //        Gizmos.DrawWireCube(transform.position + -transform.up * hitInfo.distance, new Vector3(0.25f, 0.05f, 0.25f) * 2);
    //    }
    //}
}
