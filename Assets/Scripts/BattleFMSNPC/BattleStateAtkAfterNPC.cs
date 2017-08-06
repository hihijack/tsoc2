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
        return manager.bsIdle;
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
}