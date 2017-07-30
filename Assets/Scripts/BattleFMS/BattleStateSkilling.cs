public class BattleStateSkilling : IBattleState
{
    internal ISkill skill;
    private ManagerBattleState managerBattleState;

    public BattleStateSkilling(ManagerBattleState managerBattleState)
    {
        this.managerBattleState = managerBattleState;
    }

    public override void Start()
    {
        base.Start();
        managerBattleState.hero.OnBSStartSkill(skill);
    }

    public override void Update()
    {
        base.Update();
    }

    public override IBattleState ActionSKillEnd(ISkill skill)
    {
        return managerBattleState.bsNormal;
    }
}