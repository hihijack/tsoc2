public class BattleStateHit : IBattleState
{
    private ManagerBattleState managerBattleState;

    public BattleStateHit(ManagerBattleState managerBattleState)
    {
        this.managerBattleState = managerBattleState;
    }

    public override void Start()
    {
        base.Start();
        managerBattleState.hero.OnBSStartHit();
        managerBattleState.ActionHitEnd();
    }

    public override void Update()
    {
        base.Update();
    }

    public override IBattleState ActionHitEnd()
    {
        return managerBattleState.bsAtkAfter;
    }
}