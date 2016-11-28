using UnityEngine;
using System.Collections;

/// <summary>
/// 恶魔甲：生命值小于30%时，护甲提升
/// </summary>
public class MonEMoJia : IMonSkill {

    float percent;
    public override void Init(int level)
    {
        base.Init(level);
        this.level = level;
        this.skillBD = GameDatas.GetMonSkillBD(16);
        this.percent = this.skillBD.GetFloatVal(level,"val");
    }

    public override void OnHPChange(int hpBefore, int hpCur)
    {
        base.OnHPChange(hpBefore, hpCur);
        int hpTri = Mathf.FloorToInt(_ECur._HpMax * 0.3f);
        if (hpBefore >= hpTri && hpCur < hpTri)
        {
            _ECur.DefIncrease(percent);
            GameManager.commonCPU.CreateEffect("eff_emojia", _ECur.GetPos(), Color.white, -1f);
        }
    }
}
