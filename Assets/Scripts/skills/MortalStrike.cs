using UnityEngine;
using System.Collections;

/// <summary>
/// 立即造成150%的伤害
/// </summary>
public class MortalStrike : ISkill{

    public float damageRate;

	private IActor target;

	public override void Init()
    {
        SetBaseData(GameDatas.GetSkillBD(2));
        SetDamageRateByLevel();
    }

    void SetDamageRateByLevel()
    {
        damageRate = GetBaseData().jdData[_Level-1]["val"].AsFloat;
    }

	public override void SetCaster(IActor _caster){
		caster = _caster;
	}
	
	public override void SetTarget(IActor _target){
		target = _target;
	}
	
	public override IEnumerator Act ()
	{
		if(!InCD && target != null && CheckCost()){
			StartCD();
			caster.IsSkilling = true;
			
			// 施法前摇
            yield return new WaitForSeconds(GetBaseData().casttime);

            // 特效
            GameManager.commonCPU.CreateEffect("eff_hand_two_2", target.transform.position, new Color(150f/255, 246f/255, 1f), -1f);

            int damage = (int)(caster.GetAtk() * damageRate);
			caster.DamageTarget(damage, target);
			
			// 施法后摇
			yield return new WaitForSeconds(0.5f);
            
			caster.IsSkilling = false;
		}
	}

    public override void OnLevelChange()
    {
        SetDamageRateByLevel();
    }
}
