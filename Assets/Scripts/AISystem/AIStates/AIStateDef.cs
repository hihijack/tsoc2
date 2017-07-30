﻿public class AIStateDef : IAIState
{
    public AIStateDef(IAI AI) : base(AI) { }
    public override void OnInto()
    {
        base.OnInto();
        AI.npc.gFSMManager.ActionDef();
    }
}