class PropertyHero : PropertyBase
{
    Hero hero;
    public PropertyHero(Hero hero)
    {
        this.hero = hero;
    }

    public override void CalAtk()
    {
        int baseAtk = 0;
        if (AtkBaseA > 0 && AtkBaseB > 0)
        {
            //双持
            baseAtk = UnityEngine.Mathf.CeilToInt((AtkBaseA + AtkBaseB));
        }
        else if (AtkBaseA > 0)
        {
            baseAtk = AtkBaseA;
        }
        else
        {
            baseAtk = IConst.ATK_EMPTY;
        }



        Atk = UnityEngine.Mathf.CeilToInt(
            (
                (baseAtk * AtkParmaA + AtkParmaB) * (1 + Strength * IConst.ATK_PHY_PER_STR))
            );
        Atk = UnityEngine.Mathf.CeilToInt(Atk * AtkParmaC + AtkParmaD);
    }

    public override void CalIAS()
    {
        base.CalIAS();
        //负重惩罚
        float loadOff = 1f;
        if (Load >= 26)
        {
            //攻速降低10%
            loadOff = 0.9f;
        }
        IAS = (BaseWeaponIAS * loadOff * (1 + Agility * IConst.IAS_PERCENT_PER_AGI * 0.01f) + IasParmaA) * IasParmaB + IasParmaC;
        hero.SetAnimRateByIAS();
    }
}