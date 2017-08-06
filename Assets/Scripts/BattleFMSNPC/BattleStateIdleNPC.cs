public class BattleStateIdleNPC : IBattleState
{
    private ManagerBattleStateNPC manager;

    public BattleStateIdleNPC(ManagerBattleStateNPC managerBattleStateNPC)
    {
        this.manager = managerBattleStateNPC;
        _stateType = EBattleState.Normal;
    }

    public override void Start()
    {
        base.Start();
        manager.npc.OnBSStartIdle();
    }

    public override IBattleState ActionAtk(int skillId, IActor target)
    {
        manager.bsAtkBefore.skillId = skillId;
        manager.bsAtkBefore.target = target;
        return manager.bsAtkBefore;
    }

    public override IBattleState ActionDef()
    {
        return manager.bsDefing;
    }

    public override IBattleState ActionDodge(float dur)
    {
        manager.bsDodge.dur = dur;
        return manager.bsDodge;
    }

    public override IBattleState ActionUnControl(float dur)
    {
        manager.bsUnControl.dur += dur;
        return manager.bsUnControl;
    }
}