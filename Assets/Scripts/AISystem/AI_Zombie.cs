using System;
using UnityEngine;

public class AI_Zombie : IAI
{
    public float atkInterMin = 2f;
    public float atkInterMax = 3f;
    public float heavyAtkOdds = 0.2f;

    AIStateIdle stateIdle;
    AIStateAtk stateAtk;
    AIStateAtk stateHeavy;

    public override void Init(Enermy npc)
    {
        base.Init(npc);
        stateIdle = new AIStateIdle(this);
        stateAtk = new AIStateAtk(27, this);
        stateHeavy = new AIStateAtk(28, this);
    }

    public override void DoStart()
    {
        ToAIState(stateIdle);
    }

    public override void DoUpdate()
    {
        Update_Idle();
        Update_Atk();
        Update_AtkHeavy();
    }

    private void Update_AtkHeavy()
    {
        if (curState == stateHeavy)
        {
            if (IsInUnCtl() || LastStateToIdle() == EBattleState.AtkAfter)
            {
                ToAIState(stateIdle);
            }
        }
    }

    private void Update_Atk()
    {
        if (curState == stateAtk)
        {
            EBattleState lastStateToIdle = LastStateToIdle();
            if (IsInUnCtl() || lastStateToIdle == EBattleState.AtkAfter)
            {
                ToAIState(stateIdle);
            }
        }
    }

    private void Update_Idle()
    {
        if (curState == stateIdle)
        {
            curState.dur += Time.deltaTime;

            if (curState.dur >= UnityEngine.Random.Range(atkInterMin,atkInterMax))
            {
                if (Tools.IsHitOdds(heavyAtkOdds))
                {
                    stateHeavy.target = npc.curBattleTarget;
                    ToAIState(stateHeavy);
                }
                else
                {
                    stateAtk.target = npc.curBattleTarget;
                    ToAIState(stateAtk);
                }
            }
        }
    }
}