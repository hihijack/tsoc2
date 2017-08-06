public class BattleStateDodgeNPC : IBattleState
{
    internal float dur;
    private ManagerBattleStateNPC manager;

    public BattleStateDodgeNPC(ManagerBattleStateNPC managerBattleStateNPC)
    {
        this.manager = managerBattleStateNPC;
        _stateType = EBattleState.Dodge;
    }

    public override IBattleState ActionDodgeEnd(float unControlTime)
    {
        manager.bsUnControl.dur += unControlTime;
        return manager.bsUnControl;
    }
}