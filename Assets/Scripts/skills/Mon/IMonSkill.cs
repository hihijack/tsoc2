using UnityEngine;
using System.Collections;
/// <summary>
/// 怪物技能
/// </summary>
public class IMonSkill : MonoBehaviour
{

    public int level;
    public MonSkillBD skillBD;

    Enermy eCur;

    public Enermy _ECur
    {
        get 
        {
            if (eCur == null)
            {
                eCur = GetComponent<Enermy>();
            }
            return eCur; 
        }
    }

    public Enermy GetCurEnermy()
    {
        return _ECur;
    }

    public virtual void Init(int level) { }

    /// <summary>
    /// 当进入战斗
    /// </summary>
    public virtual void OnEnterBattle() { }

    /// <summary>
    /// 当攻击时（攻击起手）
    /// </summary>
    public virtual void OnAttack(IActor target) { }

    /// <summary>
    /// 当攻击命中时
    /// </summary>
    public virtual void OnAttackHit(IActor target, int attack) { }

    /// <summary>
    /// 当受到攻击命中
    /// </summary>
    public virtual void OnAttackedHit(IActor atker, int attack) { }

    /// <summary>
    /// 当死亡时
    /// </summary>
    public virtual void OnDead() { }

    /// <summary>
    /// 当友军死亡
    /// </summary>
    public virtual void OnAllyDead(Enermy eDead) { }

    /// <summary>
    /// 当伤害目标
    /// </summary>
    public virtual void OnDamageTarget(IActor target, int damage, bool isDS) { }

    /// <summary>
    /// 当受到伤害
    /// </summary>
    /// <param name="damager"></param>
    /// <param name="damage"></param>
    /// <param name="isDS"></param>
    public virtual void OnHurt(IActor damager, int damage, EDamageType damageType, bool isDS) { }

    /// <summary>
    /// 当攻击被闪避
    /// </summary>
    /// <param name="target"></param>
    public virtual void OnAtkLost(IActor target) { }

    /// <summary>
    /// 当闪避一次攻击
    /// </summary>
    /// <param name="atker"></param>
    public virtual void OnAtkedLost(IActor atker) { }

    public virtual void OnHPChange(int hpBefore, int hpCur) { }

    /// <summary>
    /// 主动技能生效
    /// </summary>
    /// <param name="target"></param>
    public virtual void StartEff(IActor target)
    {

    }
}
