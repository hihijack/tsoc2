using System;
using UnityEngine;
/// <summary>
/// 蝙蝠AI：闪避接二连击
/// </summary>
public class AI_Bat : IAI
{
    AIStateIdle sIdle;
    AIStateDodge sDodge;
    AIStateAtk sAtkL;
    AIStateAtk sAtkL2;

    public override void Init(Enermy npc)
    {
        base.Init(npc);
        sIdle = new AIStateIdle(this);
        sDodge = new AIStateDodge(this);
        sAtkL = new AIStateAtk(27, this);
        sAtkL2 = new AIStateAtk(27, this);
    }

    public override void DoStart()
    {
        base.DoStart();
        sAtkL.target = npc.curBattleTarget;
        sAtkL2.target = npc.curBattleTarget;
        ToAIState(sIdle);
    }

    public override void DoUpdate()
    {
        Update_Idle();
        Update_Dodge();
        Update_AtkL();
        Update_AtkL2();
    }

    private void Update_AtkL2()
    {
        if (curState == sAtkL2)
        {
            if (IsInUnCtl() || IsAtkSuccess())
            {
                ToAIState(sIdle);
            }
        }
    }

    private void Update_AtkL()
    {
        if (curState == sAtkL)
        {
            if (IsInUnCtl())
            {
                ToAIState(sIdle);
            }
            else if (IsAtkSuccess())
            {
                ToAIState(sAtkL2);
            }
        }
    }

    private void Update_Dodge()
    {
        if (curState == sDodge)
        {
            if (IsDodgeSuccess())
            {
                ToAIState(sAtkL);
            }
        }
    }

    private void Update_Idle()
    {
    }

    public override void OnAtked(IActor atker)
    {
        base.OnAtked(atker);
        if (curState == sIdle)
        {
            ToAIState(sDodge);
        }
    }
}