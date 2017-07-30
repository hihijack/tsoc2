using UnityEngine;
using System.Collections;

/// <summary>
/// 绝地狂怒：生命值小于30%时，攻击力提升
/// </summary>
public class MonJueDiKuangNu : IMonSkill {

    float percent;
    public override void Init(int level)
    {
        base.Init(level);
        this.level = level;
        this.skillBD = GameDatas.GetMonSkillBD(15);
        this.percent = skillBD.GetFloatVal(level, "val");
    }

    public override void OnHPChange(int hpBefore, int hpCur)
    {
        base.OnHPChange(hpBefore, hpCur);
        int hpTri = Mathf.FloorToInt(_ECur._Prop.HpMax * 0.3f); 
        if (hpBefore >= hpTri && hpCur < hpTri)
        {
            _ECur._Prop.AtkParmaC *= (1 + percent);
            GameManager.commonCPU.CreateEffect("eff_juedikuangnu", _ECur.GetPos(), Color.white, -1f);
        }
    }
}
