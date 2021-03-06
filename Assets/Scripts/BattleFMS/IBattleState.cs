﻿using System;

public class IBattleState
{
    protected EBattleState _stateType;

    public EBattleState StateType
    {
        get
        {
            return _stateType;
        }
    }

    public virtual void Update()
    {
    }

    public virtual IBattleState ActionAtk(int skillId, IActor target)
    {
        return null;
    }

    public virtual IBattleState ActionAtk()
    {
        return null;
    }

    public virtual void Start()
    {
    }

    /// <summary>
    /// 不能再开始/结束时切换状态机
    /// </summary>
    public virtual void End()
    {

    }

    public virtual IBattleState ActionAtkBeforeTimeEnd()
    {
        return null;
    }

    public virtual IBattleState ActionAtkBeforeTimeEnd(int skillID, IActor target)
    {
        return null;
    }

    public virtual IBattleState ActionHitEnd()
    {
        return null;
    }

    public virtual IBattleState ActionAtkAfterTimeEnd()
    {
        return null;
    }

    public virtual IBattleState ActionStop()
    {
        return null;
    }

    public virtual IBattleState ActionStopDef()
    {
        return null;
    }

    public virtual IBattleState ActionDef()
    {
        return null;
    }

    public virtual IBattleState ActionPowerStart()
    {
        return null;
    }

    public virtual IBattleState ActionPowerOver()
    {
        return null;
    }

    public virtual IBattleState ActionUnControlEnd()
    {
        return null;
    }

    public virtual IBattleState ActionUnControl(float dur)
    {
        return null;
    }

    public virtual IBattleState ActionSKill(ISkill skill)
    {
        return null;
    }

    public virtual IBattleState ActionSKillEnd(ISkill skill)
    {
        return null;
    }

    public virtual IBattleState ActionDodge(float dur)
    {
        return null;
    }

    public virtual IBattleState ActionDodgeEnd(float unControlTime)
    {
        return null;
    }

    internal virtual IBattleState ActionHurted(float unCtlDur)
    {
        return null;
    }
}
