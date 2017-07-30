using UnityEngine;
using System.Collections;

public class UITrial : MonoBehaviour
{
    public  UILabel txtBest;
    public UINumberSelect uiNumerSelect;
    public UIButton btnClose;
    public UIButton btnGoTo;

    public void Init() 
    {
        int bestTrial = GameManager.hero._BestTrial;
        txtBest.text = bestTrial.ToString();
        uiNumerSelect.defaultNum = bestTrial + 1;
        uiNumerSelect.useLowerLimit = true;
        uiNumerSelect.lowerlimitNum = 1;
        uiNumerSelect.useUpperLimit = true;
        uiNumerSelect.upperlimitNum = bestTrial + 1;

        btnClose.onClick.Add(new EventDelegate(BtnClick_OnClose));
        btnGoTo.onClick.Add(new EventDelegate(BtnClick_GoTo));
    }

    void BtnClick_OnClose() 
    {
        UIManager.Inst.CloseUITrial();
    }

    void BtnClick_GoTo() 
    {
        int curCelect = uiNumerSelect.GetCurNumber();
        GameManager.gameView.GoToTrial(curCelect);
        UIManager.Inst.CloseUITrial();
    }
}
