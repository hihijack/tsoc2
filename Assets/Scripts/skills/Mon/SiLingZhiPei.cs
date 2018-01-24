using UnityEngine;
using System.Collections;

/// <summary>
/// 死灵支配 发现目标时，警戒中的友军提升攻击力与防御力
/// </summary>
public class SiLingZhiPei : IMonSkill
{
    float pstAtk;
    float pstArm;

    public override void Init(int level)
    {
        base.Init(level);
        this.level = level;
        this.skillBD = GameDatas.GetMonSkillBD(31);
        pstAtk = skillBD.GetFloatVal(level, "atk");
        pstArm = skillBD.GetFloatVal(level, "arm");
    }

    public override void OnFindTarget()
    {
        Debug.LogError("OnFindTarget");//#########
        base.OnFindTarget();
        //警戒中的队友
        for (int i = 0; i < GameView.Inst.mListEnermys.Count; i++)
        {
            Enermy e = GameView.Inst.mListEnermys[i];
            if (e != _ECur && e._State != EActorState.Dead && e._AIState == EAIState.FindTarget)
            {
                //提升攻击力防御力
                Buff_SiLingZhiPei buff = e.gameObject.AddComponent<Buff_SiLingZhiPei>();
                buff.Init(e, pstAtk, pstArm);
                buff.StartEffect();
            }
        }
    }
}
