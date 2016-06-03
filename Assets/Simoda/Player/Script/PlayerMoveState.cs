using UnityEngine;
using System.Collections;

namespace PlayerMoveState
{
    //ステートの実行を管理するクラス
    public class StateProcessor
    {
        //ステート本体
        private PlayerMoveState _State;
        public PlayerMoveState State
        {
            set { _State = value; }
            get { return _State; }
        }

        //実行ブリッジ
        public void Execute()
        {
            State.Execute();
        }
    }

    //ステートクラス
    public abstract class PlayerMoveState
    {
        //デリゲート
        public delegate void executeState();
        public executeState exeDelegate;

        //実行処理
        public virtual void Execute()
        {
            if (exeDelegate != null)
            {
                exeDelegate();
            }
        }

        //ステート名を取得するメソッド
        public abstract string getStateName();
    }



    public class PlayerMoveStateDefault : PlayerMoveState
    {
        public override string getStateName()
        {
            return "State:Default";
        }
    }

    public class PlayerMoveStateLockOn : PlayerMoveState
    {
        public override string getStateName()
        {
            return "State:LockOn";
        }
    }

    public class PlayerMoveStateWind : PlayerMoveState
    {
        public override string getStateName()
        {
            return "State:Wind";
        }
    }

    public class PlayerMoveStateKnockBackSmall: PlayerMoveState
    {
        public override string getStateName()
        {
            return "State:KnockBackSmall";
        }
    }

    public class PlayerMoveStateKnockBackLarge : PlayerMoveState
    {
        public override string getStateName()
        {
            return "State:KnockBackLarge";
        }
    }
}
