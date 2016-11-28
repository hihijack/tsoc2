using UnityEngine;
using System.Collections;

public class UIEquipItemInfo : MonoBehaviour {

    public UILabel txt;

    EquipItem ei;
    GameView gameView;



    public void Init(GameView gameView, EquipItem equipitem) 
    {
        this.ei = equipitem;
        txt.text = gameView.GetEquipItemDesc(ei);
    }
}
