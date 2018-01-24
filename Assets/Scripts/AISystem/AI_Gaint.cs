using System;
using UnityEngine;
/// <summary>
/// 会被激怒的巨人攻击
/// </summary>
public class AI_Gaint : IAI
{
    public RandomVal rdmIdleDur = new RandomVal();
    public float oddsAtkHeavy;
    AIStateIdle sIdle;
    AIStateAtk sAtkL;
    AIStateAtk sAtkH;
    AIStateAtk sAtkFury;
    
    public override void Init(Enermy npc)
    {
        base.Init(npc);
        sIdle = new AIStateIdle(this);
        sAtkL = new AIStateAtk(27, this);
        sAtkH = new AIStateAtk(28, this);
        sAtkFury = new AIStateAtk(32, this);
    }

    public override void DoStart()
    {
        base.DoStart();
        ToAIState(sIdle);
    }

    public override void DoUpdate()
    {
        base.DoUpdate();
        Update_Idle();
        Update_AtkL();
        Update_AtkH();
        Update_AtkFury();
    }

    private void Update_AtkFury()
    {
        if (curState == sAtkFury)
        {
            if (IsInUnCtl())
            {
                ToAIState(sIdle);
            }
            else if (IsAtkSuccess())
            {
                ToAIState(sIdle);
            }
        }
    }

    private void Update_AtkH()
    {
        if (curState == sAtkH)
        {
            if (IsInUnCtl())
            {
                ToAIState(sIdle);
            }
            else if (IsAtkSuccess())
            {
                if (atkMiss)
                {
                    ToAIState(sAtkFury);
                }
                else
                {
                    ToAIState(sIdle);
                }
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
                if (atkMiss)
                {
                    ToAIState(sAtkFury);
                }
                else
                {
                    ToAIState(sIdle);
                }
            }
           
        }
    }

    private void Update_Idle()
    {
        if (curState == sIdle)
        {
            curState.dur += Time.deltaTime;
            if (curState.dur >= rdmIdleDur.RanVal())
            {
                if (HitOdds(oddsAtkHeavy))
                {
                    ToAIState(sAtkH);
                }
                else
                {
                    ToAIState(sAtkL);
                }
            }
        }
    }
}
