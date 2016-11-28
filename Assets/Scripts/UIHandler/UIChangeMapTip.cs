using UnityEngine;
using System.Collections;

public class UIChangeMapTip : MonoBehaviour {

    public UIButton btnComfirm;
    public UILabel txtTip;

    GameMapBaseData mapTarget;
    int targetMGId;

    public void Init(GameMapBaseData mapTarget, int targetMGId)
    {
        this.mapTarget = mapTarget;
        this.targetMGId = targetMGId;

        txtTip.text = "前往" + mapTarget.name;
        btnComfirm.onClick.Add(new EventDelegate(OnBtn_ChangeMapComfirm));
    }

    void OnBtn_ChangeMapComfirm()
    {
        GameManager.gameView.ComfirmChangeMap(mapTarget, targetMGId);
    }
}
