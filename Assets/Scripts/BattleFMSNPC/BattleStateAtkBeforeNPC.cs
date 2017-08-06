public class BattleStateAtkBeforeNPC : IBattleState
{
    public int skillId;
    public IActor target;
    private ManagerBattleStateNPC manager;

    public BattleStateAtkBeforeNPC(ManagerBattleStateNPC managerBattleStateNPC)
    {
        this.manager = managerBattleStateNPC;
        _stateType = EBattleState.AtkBefore;
    }

    public override void Start()
    {
        base.Start();
        manager.npc.OnBSStartAtk(skillId, target);
    }

    public override IBattleState ActionAtkBeforeTimeEnd(int skillId, IActor target)
    {
        manager.bsAtkAfter.skillId = skillId;
        manager.bsAtkAfter.target = target;
        return manager.bsAtkAfter;
    }

    public override IBattleState ActionUnControl(float dur)
    {
        manager.bsUnControl.dur += dur;
        return manager.bsUnControl;
    }
}