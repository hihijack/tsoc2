using UnityEngine;
using System.Collections;
/// <summary>
/// 领主buff
/// </summary>
public class Buff_LingZhu : IBaseBuff {

    int arm;

    public void Init(IActor target, int arm) 
    {
        this.target = target;
        this.arm = arm;
        baseData = GameDatas.GetBuff(2);
    }

    public override void StartEffect()
    {
        OnAdd();
    }

    public override void OnAdd()
    {
        UIManager.Inst.AddASmallTip(target.actorName + "获得BUFF:" + baseData.name);
        target.Prop.arm += arm;
        UIManager.Inst.uiMain.AddABuffToTarget(target, this);
    }
}
