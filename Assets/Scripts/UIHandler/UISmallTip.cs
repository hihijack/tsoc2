using UnityEngine;
using System.Collections;

public class UISmallTip : MonoBehaviour {

    public UIGrid grid;
    public GameObject gobjPrebItem;

    public void AddATip(string content) 
    {
        GameObject gobjTipItem = NGUITools.AddChild(grid.gameObject, gobjPrebItem);
        // 设置文本
        UILabel uiText = gobjTipItem.GetComponent<UILabel>();
        uiText.text = content;
        GObjLife gl = gobjTipItem.GetComponent<GObjLife>();
        gl.OnDie = OnItemTipDie;
        grid.Reposition();
    }

    void OnItemTipDie(GameObject gobjItemTip)
    {
        grid.Reposition();
    }
}
