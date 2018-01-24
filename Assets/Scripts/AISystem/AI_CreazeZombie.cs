using System;
using UnityEngine;

public class AI_CreazeZombie : IAI
{
    public RandomVal rdmIdleTime = new RandomVal();
    public float atkCreazeOdds;//3连击概率
    AIStateIdle idle;
    AIStateAtk atk1;
    int mAtkCount;

    public override void Init(Enermy npc)
    {
        base.Init(npc);
        idle = new AIStateIdle(this);
        atk1 = new AIStateAtk(27, this);
    }

    public override void DoStart()
    {
        atk1.target = npc.curBattleTarget;
        mAtkCount = 0;
        ToAIState(idle);
    }

    public override void DoUpdate()
    {
        Update_Idle();
        DoUpdate_Atk();
    }

    private void Update_Idle()
    {
        if (curState == idle)
        {
            curState.dur += Time.deltaTime;
            if (curState.dur >= rdmIdleTime.RanVal())
            {
                if (Tools.IsHitOdds(atkCreazeOdds))
                {
                    //3连击
                    mAtkCount = 3;
                }
                else
                {
                    mAtkCount = 1;
                }
                ToAIState(atk1);
            }
        }
    }

    private void DoUpdate_Atk()
    {
        if (curState == atk1)
        {
            //轻击接重击
            if (IsInUnCtl())
            {
                ToAIState(idle);
            }
            else if (IsAtkSuccess())
            {
                mAtkCount--;
                if (mAtkCount > 0)
                {
                    //连击
                    ToAIState(atk1);
                }
                else
                {
                    ToAIState(idle);
                }
            }
        }
    }
}