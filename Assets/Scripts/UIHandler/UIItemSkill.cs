using UnityEngine;
using System.Collections;

public class UIItemSkill : MonoBehaviour
{
    public UISprite icon;
    public UILabel txtName;

    public SkillBD data;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Refresh()
    {
        if (data != null)
        {
            icon.alpha = 1f;
            icon.spriteName = data.iconName;
            txtName.text = data.name;
        }
        else
        {
            icon.alpha = 0f;
            txtName.text = "未分配";
        }
    }

    public void RefreshForOther()
    {
        if (data != null)
        {
            icon.alpha = 1f;
            icon.spriteName = data.iconName;
            txtName.text = data.name;
        }
        else
        {
            icon.alpha = 0f;
            txtName.text = "不分配";
        }
    }

    void OnHover(bool isOver)
    {
        if (data == null)
        {
            return;
        }

        if (isOver)
        {
            UIManager.Inst.ShowSkillInfo(data, NGUIToolsEx.GetUIPos(GameView.Inst.cameraUI.transform, transform));
        }
        else
        {
            UIManager.Inst.HideSkillInfo();
        }
    }
}
