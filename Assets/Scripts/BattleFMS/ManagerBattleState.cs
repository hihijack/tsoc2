using System;

public class ManagerBattleState
{
    public Hero hero;
    IBattleState curState;
    #region BattleStaets
    public BattleStateNormal bsNormal;
    public BattleStateAtkBefore bsAtkBefore;
    public BattleStateAtkAfter bsAtkAfter;
    public BattleStateDefing bsDefing;
    public BattleStatePowering bsPowering;
    public BattleStateSkilling bsSkilling;
    public BattleStateUnControl bsUnControl;
    public BattleStateHit bsHit;

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
    #endregion


    public ManagerBattleState(Hero hero)
    {
        this.hero = hero;
        bsNormal = new BattleStateNormal(this);
        bsAtkBefore = new BattleStateAtkBefore(this);
        bsAtkAfter = new BattleStateAtkAfter(this);
        bsDefing = new BattleStateDefing(this);
        bsPowering = new BattleStatePowering(this);
        bsSkilling = new BattleStateSkilling(this);
        bsUnControl = new BattleStateUnControl(this);
        bsHit = new BattleStateHit(this);
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
        CurState = bsNormal;
        hero._BattleState = EBattleState.Normal;
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
            CurState = next;
            CurState.Start();
            hero._BattleState = CurState.StateType;
        }
    }

    #region Actions
    internal void ActionAtkBeforeTimeEnd()
    {
        if (CurState != null)
        {
            IBattleState next = CurState.ActionAtkBeforeTimeEnd();
            ChangeState(next);
        }
    }

    internal void ActionHitEnd()
    {
        if (CurState != null)
        {
            IBattleState next = CurState.ActionHitEnd();
            ChangeState(next);
        }
    }

    internal void ActionAtk()
    {
        if (CurState != null)
        {
            IBattleState next = CurState.ActionAtk();
            ChangeState(next);
        }
    }

    public void ActionAtkAfterTimeEnd()
    {
        if (CurState != null)
        {
            IBattleState next = CurState.ActionAtkAfterTimeEnd();
            ChangeState(next);
        }
    }

    public void ActionStop()
    {
        if (CurState != null)
        {
            IBattleState next = CurState.ActionStop();
            ChangeState(next);
        }
    }

    internal void ActionStopDef()
    {
        if (CurState != null)
        {
            IBattleState next = CurState.ActionStopDef();
            ChangeState(next);
        }
    }

    internal void ActionDef()
    {
        if (CurState != null)
        {
            IBattleState next = CurState.ActionDef();
            ChangeState(next);
        }
    }

    internal void ActionPoweringStart()
    {
        if (CurState != null)
        {
            IBattleState next = CurState.ActionPowerStart();
            ChangeState(next);
        }
    }

    internal void ActionPoweringOver()
    {
        if (CurState != null)
        {
            IBattleState next = CurState.ActionPowerOver();
            ChangeState(next);
        }
    }

    internal void ActionUnControlEnd()
    {
        if (CurState != null)
        {
            IBattleState next = CurState.ActionUnControlEnd();
            ChangeState(next);
        }
    }

    internal void ActionUnContol(float dur)
    {
        if (CurState != null)
        {
            IBattleState next = CurState.ActionUnControl(dur);
            ChangeState(next);
        }
    }

    internal void ActionSkill(ISkill skill)
    {
        if (CurState != null)
        {
            IBattleState next = CurState.ActionSKill(skill);
            ChangeState(next);
        }
    }

    internal void ActionSkillEnd(ISkill skill)
    {
        if (CurState != null)
        {
            IBattleState next = CurState.ActionSKillEnd(skill);
            ChangeState(next);
        }
    }
    #endregion
}
