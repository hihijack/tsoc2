class PropertyNPC : PropertyBase
{
    Enermy enermy;
    public PropertyNPC(Enermy enermy)
    {
        this.enermy = enermy;
    }

    public override int GetAtk(EquipItem ei)
    {
        return UnityEngine.Mathf.CeilToInt(enermy._MonsterBD.atkMin * AtkParmaC + AtkParmaD);
    }

    public override void CalIAS()
    {
        base.CalIAS();
        IAS = (BaseWeaponIAS + IasParmaA) * IasParmaB + IasParmaC;
    }

    public override float GetAtkTimeAfter()
    {
        return base.GetAtkTimeAfter();
    }

    public override float GetAtkTimeBefore()
    {
        return base.GetAtkTimeBefore();
    }

    public override float ParryDamPercent(EDamageType type)
    {
       return 0.9f;
    }
}