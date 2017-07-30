public class BattleStatePowering : IBattleState
{
    private ManagerBattleState managerBattleState;

    public BattleStatePowering(ManagerBattleState managerBattleState)
    {
        this.managerBattleState = managerBattleState;
        stateType = EBattleState.Powering;
    }

    public override void Start()
    {
        base.Start();
        managerBattleState.hero.OnBSStartPowering();
    }

    public override void Update()
    {
        base.Update();
        managerBattleState.hero.OnBSUpdatePowering();
    }

    public override IBattleState ActionPowerOver()
    {
        return managerBattleState.bsAtkBefore;
    }

    public override IBattleState ActionUnControl(float dur)
    {
        managerBattleState.bsUnControl.dur += dur;
        return managerBattleState.bsUnControl;
    }
}