class PropertyNPC : PropertyBase
{
    Enermy enermy;
    public PropertyNPC(Enermy enermy)
    {
        this.enermy = enermy;
    }
    public override void CalAtk()
    {
        Atk = UnityEngine.Mathf.CeilToInt(AtkBaseA * AtkParmaC + AtkParmaD);
    }

    public override void CalIAS()
    {
        base.CalIAS();
        IAS = (BaseWeaponIAS + IasParmaA) * IasParmaB + IasParmaC;
        enermy.SetAnimRateByIAS();
    }
}