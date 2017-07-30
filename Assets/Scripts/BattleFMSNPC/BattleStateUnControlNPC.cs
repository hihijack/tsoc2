public class BattleStateUnControlNPC : IBattleState
{
    internal float dur;
    private ManagerBattleStateNPC manager;

    public BattleStateUnControlNPC(ManagerBattleStateNPC managerBattleStateNPC)
    {
        this.manager = managerBattleStateNPC;
        stateType = EBattleState.Uncontrol;
    }

    public override IBattleState ActionUnControlEnd()
    {
        return manager.bsIdle;
    }
}