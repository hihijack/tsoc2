using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public enum EAnimState
{
    WalkF,
    WalkB,
    WalkR,
    WalkL
}

public class Avtoar2D : MonoBehaviour {

    Dictionary<EEquipPart, NodeSprite> dicData = new Dictionary<EEquipPart, NodeSprite>();

    public EAnimState animState = EAnimState.WalkF;

    public int frameInterval = 10;

    SpriteRenderer srBase;
    int curAnimIndex = 0;

    static Dictionary<string, Sprite> dicSprites = new Dictionary<string, Sprite>();


    public void SetDicSpriteNode(NodeSprite ns) 
    {
        if (dicData.ContainsKey(ns.part))
        {
            dicData[ns.part] = ns;
        }
        else
        {
            dicData.Add(ns.part, ns);
        }
    }

    public void RemoveSpriteNode(EEquipPart part) 
    {
        if (dicData.ContainsKey(part))
        {
            if (part == EEquipPart.Helm)
            {
                // 脱下头盔，显示默认头发
                SetDicSpriteNode(new NodeSprite(EEquipPart.Helm, "hair_1", Color.red));
            }
            else
            {
                dicData.Remove(part);
                GameObject gobjPart = Tools.GetGameObjectInChildByPathSimple(gameObject, part.ToString());
                DestroyObject(gobjPart);
            }
        }
    }


    // Use this for initialization
	void Start () {
     
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.frameCount % frameInterval == 0)
        {
            string animName = "f";
            //switch (animState)
            //{
            //    case EAnimState.WalkF:
            //        animName = "f";
            //        break;
            //    case EAnimState.WalkB:
            //        animName = "b";
            //        break;
            //    case EAnimState.WalkR:
            //        animName = "r";
            //        break;
            //    case EAnimState.WalkL:
            //        animName = "l";
            //        break;
            //    default:
            //        break;
            //}
            //// 设置裸体Sprite
            //srBase.sprite = GetSprite(baseSpriteName, animName, curAnimIndex);
            //// 设置头发Sprite
            //srHair.sprite = GetSprite(hairSpriteName,animName, curAnimIndex);

            //// 设置耳朵
            //if (animState == EAnimState.WalkL || animState == EAnimState.WalkR)
            //{
            //    srEar.gameObject.SetActive(true);
            //    srEar.sprite = GetSprite(earSpriteName, animName, curAnimIndex);
            //}
            //else
            //{
            //    srEar.gameObject.SetActive(false);
            //}

            //SpriteRenderer srRoot = gGobjRoot.GetComponent<SpriteRenderer>();
            //srRoot.sprite = GetSprite(gGobjRoot.name, animName, curAnimIndex);
            //foreach (Transform tfChild in gGobjRoot.transform)
            //{
            //    SpriteRenderer srChild = tfChild.GetComponent<SpriteRenderer>();
            //    srChild.sprite = GetSprite(tfChild.name, animName, curAnimIndex);
            //}

            foreach (NodeSprite ns in dicData.Values)
            {
                GameObject gobjPart = Tools.GetGameObjectInChildByPathSimple(gameObject, ns.part.ToString());
                if (gobjPart == null)
                {
                    gobjPart = new GameObject();
                    gobjPart.name = ns.part.ToString();
                    gobjPart.AddComponent<SpriteRenderer>();
                    gobjPart.transform.parent = transform;
                    gobjPart.transform.localPosition = Vector3.zero;
                    gobjPart.transform.localScale = Vector3.one;
                }
                SpriteRenderer sr = gobjPart.GetComponent<SpriteRenderer>();
                sr.sprite = GetSprite(ns.spName, animName, curAnimIndex);
                sr.sortingOrder = ns.layer;
                sr.sortingLayerName = "actor";
                sr.color = ns.color;
            }

            //curAnimIndex++;
            //if (curAnimIndex > 3)
            //{
            //    curAnimIndex = 0;
            //}
        }
	}

    //void CreateRoot()
    //{
    //    GameObject gobjRoot = null;
    //    for (int i = 0; i < jdData.Count; i++)
    //    {
    //        JSONNode jdNode = jdData[i];
    //        string spName = jdNode["sp"];
    //        int layer = jdNode["layer"].AsInt;
    //        Color color = new Color(jdNode["cr"].AsInt / 255, jdNode["cg"].AsInt / 255, jdNode["cb"].AsInt / 255);

    //        GameObject gobjTemp = new GameObject();
    //        gobjTemp.name = spName;
    //        SpriteRenderer sr = gobjTemp.AddComponent<SpriteRenderer>();
    //        sr.sortingOrder = layer;
    //        sr.color = color;

    //        if (gobjRoot == null)
    //        {
    //            gobjRoot = gobjTemp;
    //            gobjRoot.transform.localScale = new Vector3(5f, 5f, 1f);
    //        }
    //        else
    //        {
    //            gobjTemp.transform.parent = gobjRoot.transform;
    //            gobjTemp.transform.localPosition = Vector3.zero;
    //            gobjTemp.transform.localScale = Vector3.one;
    //        }
    //    }
    //}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="partname">部位名</param>
    /// <param name="animName">动作名</param>
    /// <param name="aniIndex">动作序列</param>
    /// <returns></returns>
    Sprite GetSprite(string partname, string animName, int aniIndex) 
    {
        Sprite r = null;
        r = GameManager.commonCPU.GetSprite(partname + "_" + animName + "_" + aniIndex);
        return r;
    }

    void SetAnimState(EAnimState state) 
    {
        animState = state;
        curAnimIndex = 0;
    }
}
