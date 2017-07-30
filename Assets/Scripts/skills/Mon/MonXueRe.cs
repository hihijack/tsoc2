using UnityEngine;
using System.Collections;

/// <summary>
/// 血热：生命值小于30%时，攻速提升
/// </summary>

public class MonXueRe : IMonSkill {

    float val;

    public override void Init(int level)
    {
        base.Init(level);
        this.level = level;
        this.skillBD = GameDatas.GetMonSkillBD(17);
        this.val = skillBD.GetFloatVal(level, "val");
    }

    public override void OnHPChange(int hpBefore, int hpCur)
    {
        base.OnHPChange(hpBefore, hpCur);
        int hpTri = Mathf.FloorToInt(_ECur._Prop.HpMax * 0.3f);
        if (hpBefore >= hpTri && hpCur < hpTri)
        {
            _ECur._Prop.IasParmaB *= (1 + val);
            GameManager.commonCPU.CreateEffect("eff_xuere", _ECur.GetPos(), Color.white, -1f);
        }
    }
}
