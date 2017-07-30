public class AIStateAtk : IAIState
{
    public int skillId;

    public IActor target;

    public AIStateAtk(int skillId, IAI AI) : base(AI)
    {
        this.skillId = skillId;
    }

    public override void OnInto()
    {
        base.OnInto();
        AI.npc.gFSMManager.ActionAtk(skillId, target);
    }
}
