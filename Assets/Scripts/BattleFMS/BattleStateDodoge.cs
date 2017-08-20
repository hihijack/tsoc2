using UnityEngine;

public class BattleStateDodoge : IBattleState
{
    public float dur;
    float curTime;
    private ManagerBattleState manager;

    public BattleStateDodoge(ManagerBattleState manager)
    {
        this.manager = manager;
        _stateType = EBattleState.Dodge;
    }

    public override void Start()
    {
        base.Start();
        manager.hero.OnBSStartDodge(dur);
        curTime = 0f;
    }

    public override void End()
    {
        base.End();
        manager.hero.OnBSEndDodge();
    }

    public override void Update()
    {
        base.Update();
        curTime += Time.deltaTime;
        if (curTime > dur)
        {
            manager.ActionDodgeEnd(0.5f);
        }
    }

    public override IBattleState ActionDodgeEnd(float unControlTime)
    {
        return manager.bsNormal;
    }

    public override IBattleState ActionAtk()
    {
        return manager.bsAtkBefore;
    }

    public override IBattleState ActionPowerStart()
    {
        return manager.bsPowering;
    }

    public override IBattleState ActionDef()
    {
        return manager.bsDefing;
    }

    public override IBattleState ActionSKill(ISkill skill)
    {
        skill.SetCaster(manager.hero);
        if (skill.GetBaseData().targetType != ESkillTargetType.None)
        {
            skill.SetTarget(manager.hero.curTarget);
        }
        if (skill.CheckCast())
        {
            manager.bsSkilling.skill = skill;
            return manager.bsSkilling;
        }
        else
        {
            return null;
        }
    }
}