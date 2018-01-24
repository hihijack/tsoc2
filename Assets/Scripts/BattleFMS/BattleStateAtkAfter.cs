public class BattleStateAtkAfter : IBattleState
{
    private ManagerBattleState managerBattleState;
    float durTime;
    public BattleStateAtkAfter(ManagerBattleState managerBattleState)
    {
        this.managerBattleState = managerBattleState;
        _stateType = EBattleState.AtkAfter;
    }

    public override void Start()
    {
        base.Start();
        managerBattleState.hero.OnBSStartAtkAfter();
        durTime = 0f;
    }

    public override void Update()
    {
        base.Update();
        durTime += UnityEngine.Time.deltaTime;
        if (durTime >= managerBattleState.hero.Prop.GetAtkTimeAfter())
        {
            managerBattleState.ActionAtkAfterTimeEnd();
        }
    }

    public override IBattleState ActionAtkAfterTimeEnd()
    {
        return managerBattleState.bsNormal;
    }

    public override IBattleState ActionSKill(ISkill skill)
    {
        skill.SetCaster(managerBattleState.hero);
        if (skill.GetBaseData().targetType != ESkillTargetType.None)
        {
            skill.SetTarget(managerBattleState.hero.curTarget);
        }
        if (skill.CheckCast())
        {
            managerBattleState.bsSkilling.skill = skill;
            return managerBattleState.bsSkilling;
        }
        else
        {
            return null;
        }

    }
}
