public class BattleStateAtkAfterNPC : IBattleState
{
    public int skillId;
    public IActor target;
    private ManagerBattleStateNPC manager;

    public BattleStateAtkAfterNPC(ManagerBattleStateNPC managerBattleStateNPC)
    {
        this.manager = managerBattleStateNPC;
        _stateType = EBattleState.AtkAfter;
    }

    public override IBattleState ActionAtkAfterTimeEnd()
    {
        if (skillId == 32)
        {
            //愤怒攻击，特殊处理
            MonSkillBD skill = GameDatas.GetMonSkillBD(skillId);
            float stiff = skill.GetFloatVal(1, "stiff");
            manager.bsUnControl.dur = stiff;
            return manager.bsUnControl;
        }
        else
        {
            return manager.bsIdle;
        }
    }

    public override IBattleState ActionUnControl(float dur)
    {
        manager.bsUnControl.dur += dur;
        return manager.bsUnControl;
    }

    public override void Start()
    {
        base.Start();
        manager.npc.OnBSStartAtkAfter(skillId, target);
    }

    public override void End()
    {
        base.End();
        manager.npc.OnBSEndAtkAfter(skillId, target);
    }
}