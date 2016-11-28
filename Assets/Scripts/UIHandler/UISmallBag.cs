using UnityEngine;
using System.Collections;
using System;

public class UISmallBag : UIHeroBag
{
    public new void Init(GameView gv)
    {
        this.gameView = gv;

        CreateBagGrid();

        // 背包里的物品
        for (int i = 0; i < gameView._MHero.itemsInBag.Count; i++)
        {
            EquipItem eiInBag = gameView._MHero.itemsInBag[i];
            AddAEquipItemToAGrid(eiInBag);
        }

        RefreshGold();
    }
}
