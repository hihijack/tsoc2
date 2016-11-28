using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIMission : MonoBehaviour {

    public UILabel txtTitle;
    public UILabel txtDesc;
    public UIGrid gridTargets;
    public GameObject prefabItemMissoinTarget;
    
    public void Init()
    {
        MissionBD curMission = GameManager.hero._CurMainMission;
        MissionBD missionParent = GameDatas.GetMissionBD(curMission.parent);
        // title
        txtTitle.text = missionParent.targetDesc;
        // 当前任务描述
        txtDesc.text = curMission.desc;
        // 任务目标
        List<MissionBD> missions = GameDatas.GetChildMissions(missionParent.id);
        //missions.Sort(new ComparMissionByStep());
        for (int i = 0; i < missions.Count; i++)
        {
            MissionBD missionChild = missions[i];
            if (missionChild.step <= curMission.step)
            {
                GameObject gobjChildItem = NGUITools.AddChild(gridTargets.gameObject, prefabItemMissoinTarget);
                UILabel txtChildItem = gobjChildItem.GetComponent<UILabel>();
                txtChildItem.text = "-" + missionChild.targetDesc;
                if (missionChild.step < curMission.step)
                {
                    txtChildItem.color = Color.green;
                }
            }
        }
        gridTargets.Reposition();
    }    
}
