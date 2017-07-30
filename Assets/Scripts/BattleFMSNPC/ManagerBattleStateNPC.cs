using System;

public class ManagerBattleStateNPC
{
    public Enermy npc;
    IBattleState curState;
    IBattleState lastState;

    #region BattleStaets
    public BattleStateIdleNPC bsIdle;
    public BattleStateAtkBeforeNPC bsAtkBefore;
    public BattleStateAtkAfterNPC bsAtkAfter;
    public BattleStateUnControlNPC bsUnControl;
    public BattleStateDodgeNPC bsDodge;
    public BattleStateDefingNPC bsDefing;

    public IBattleState CurState
    {
        get
        {
            return curState;
        }

        set
        {
            curState = value;
        }
    }

    public IBattleState LastState
    {
        get
        {
            return lastState;
        }

        set
        {
            lastState = value;
        }
    }
    #endregion

    public ManagerBattleStateNPC(Enermy npc)
    {
        this.npc = npc;
        bsIdle = new BattleStateIdleNPC(this);
        bsAtkBefore = new BattleStateAtkBeforeNPC(this);
        bsAtkAfter = new BattleStateAtkAfterNPC(this);
        bsDodge = new BattleStateDodgeNPC(this);
        bsDefing = new BattleStateDefingNPC(this);
        bsUnControl = new BattleStateUnControlNPC(this);
    }

    /// <summary>
    /// 回到静止状态的上一个状态。非静止状态返回Idle
    /// </summary>
    /// <returns></returns>
    internal EBattleState LastStateToIdle()
    {
        EBattleState lastStateType = EBattleState.Normal;
        if (CurState == bsIdle && LastState != null)
        {
            lastStateType = LastState.StateType;
        }
        return lastStateType;
    }

    /// <summary>
    /// 硬直中
    /// </summary>
    /// <returns></returns>
    public bool InUnControl()
    {
        return CurState == bsUnControl;
    }

    //仅战斗中执行
    internal void Update()
    {
        if (CurState != null)
        {
            CurState.Update();
        }
    }

    internal void Clear()
    {
        CurState = null;
    }

    internal void Start()
    {
        CurState = bsIdle;
        CurState.Start();
    }

    void ChangeState(IBattleState next)
    {
        if (next != null)
        {
            if (CurState != null)
            {
                CurState.End();
            }
            LastState = CurState;
            CurState = next;
            CurState.Start();
        }
    }

    #region Actions
    public void ActionAtk(int skillId, IActor target)
    {
        if (CurState != null)
        {
            IBattleState next = CurState.ActionAtk(skillId, target);
            ChangeState(next);
        }
    }

    public void ActionAtkBeforeEnd(int skillId, IActor target)
    {
        if (CurState != null)
        {
            IBattleState next = CurState.ActionAtkBeforeTimeEnd(skillId, target);
            ChangeState(next);
        }
    }

    public void ActionAtkAfterEnd()
    {
        if (CurState != null)
        {
            IBattleState next = CurState.ActionAtkAfterTimeEnd();
            ChangeState(next);
        }
    }

    public void ActionDef()
    {
        if (CurState != null)
        {
            IBattleState next = CurState.ActionDef();
            ChangeState(next);
        }
    }

    public void ActionStopDef()
    {
        if (CurState != null)
        {
            IBattleState next = CurState.ActionStopDef();
            ChangeState(next);
        }
    }

    public void ActionDodge(float dur)
    {
        if (CurState != null)
        {
            IBattleState next = CurState.ActionDodge(dur);
            ChangeState(next);
        }
    }

    public void ActionDogleTimeEnd(float unCtlTime)
    {
        if (CurState != null)
        {
            IBattleState next = CurState.ActionDodgeEnd(unCtlTime);
            ChangeState(next);
        }
    }

    public void ActionUnControl(float dur)
    {
        if (CurState != null)
        {
            IBattleState next = CurState.ActionUnControl(dur);
            ChangeState(next);
        }
    }

    public void ActionUnControlEnd()
    {
        if (CurState != null)
        {
            IBattleState next = CurState.ActionUnControlEnd();
            ChangeState(next);
        }
    }


    #endregion
}