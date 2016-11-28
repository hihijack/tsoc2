using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class Tools{
	public static GameObject LoadResourcesGameObject(string path){
		//Debug.Log("LoadResourceGameObject:"+path);
        GameObject gobj = null;
		UnityEngine.Object obj = Resources.Load(path);
        if (obj == null)
        {
            Debug.LogError("LoadResourcesGameObject Error;Path = " + path);
        }
        else
        {
            gobj = UnityEngine.Object.Instantiate(obj) as GameObject;
        }
        return gobj;
	}

	public static GameObject LoadResourcePrefab(string path){
		return Resources.Load(path) as GameObject;
	}
	
	public static GameObject LoadResourcesGameObject(string path, GameObject gobjParent, float x, float y, float z){
		GameObject gobj = null;
		gobj = LoadResourcesGameObject(path);
		if (gobj != null) {
			gobj.transform.parent = gobjParent.transform;
			gobj.transform.localPosition = new Vector3(x, y, z);
		}
		return gobj;
	}
	
	public static GameObject LoadResourcesGameObject(string path, GameObject gobjParent){
		GameObject gobj = null;
		gobj = LoadResourcesGameObject(path);
		if (gobj != null) {
			gobj.transform.parent = gobjParent.transform;
		}
		return gobj;
	}
	
//	public static string Md5Sum(string strToEncrypt)
//	{
//		System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
//		byte[] bytes = ue.GetBytes(strToEncrypt);
//	 
//		// encrypt bytes
//		System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
//		byte[] hashBytes = md5.ComputeHash(bytes);
//	 
//		// Convert the encrypted bytes back to a string (base 16)
//		string hashString = "";
//	 
//		for (int i = 0; i < hashBytes.Length; i++)
//		{
//			hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
//		}
//	 
//		return hashString.PadLeft(32, '0');
//	}
	
	public static string GetGUID(){
       System.Guid myGUID = System.Guid.NewGuid();
	   //Debug.Log("myGUID:" + myGUID.ToString("N"));

       return myGUID.ToString("N");
    }
	
	public static string[] StringSplit(string txt,string character){
		return Regex.Split(txt,character,RegexOptions.IgnoreCase);
	}
	
	public static string FormatTime(int seconds){
		string strTime;
		if(seconds < 0){
			strTime = "00:00:00";
		}else{
			int intH = seconds / 3600;
			string strH = intH < 10 ? "0" + intH.ToString() : intH.ToString();
			int intM = (seconds % 3600) / 60;
			string strM = intM < 10 ? "0" + intM.ToString() : intM.ToString();
			int intS = seconds % 3600 % 60;
			string strS = intS < 10 ? "0" + intS.ToString() : intS.ToString();
			strTime = strH + ":" + strM + ":" + strS;
		}
		return strTime;
	}

	
	public static int MathfRound(float num){
		return (int)Mathf.Floor(num + 0.5f);
	}
	
	public static int CashToDiamon(int cash){
		int rate = 10;
		return (int)Mathf.Ceil(cash * 1.0f/ rate);
	}
	
	public static int GetMax(int[] nums){
		int r = 0;
		if (nums == null || nums.Length == 0){
			return r;
		}
		r = nums[0];
		foreach (int item in nums) {
			if (item > r) {
				r = item;
			}
		}
		return r;
	}
	
	public static T GetComponentInChildByPath<T> (GameObject gobjParent, string path) where T : Component{
		Component r = null;
		if (gobjParent == null){
			return null;
		}
//		Transform tf = GetTransformInChildByPath(gobjParent, path);
		Transform tf = GetTransformInChildByPathSimple(gobjParent, path);
		if (tf != null){
			r = tf.gameObject.GetComponent<T>();
		}
		return r as T;
	}
	
	// path , split by "/"
	public static Transform GetTransformInChildByPath(GameObject gobjParent, string path){
		Transform r = null;
		if (gobjParent == null){
			return r;
		}
		string[] strArr = path.Split('/');
		r = gobjParent.transform;
		foreach (string strPathItem in strArr) {
			if (r == null) {
				continue;
			}
			r = r.FindChild(strPathItem);
		}
		if (r == null) {
			Debug.Log("!!!Can't get transform by path : " + path +". in Tools.GetTransformInChildByPath");
		}
		return r;
	}
	
	public static Transform GetTransformInChildByPathSimple(GameObject gobjParent, string path){
		Transform r = null;
		if(gobjParent != null){
			r = gobjParent.transform.FindChild(path);
		}
		return r;
	}
	
	public static GameObject GetGameObjectInChildByPathSimple(GameObject gobjParent, string path){
		GameObject gobj = null;
		Transform tf = GetTransformInChildByPathSimple(gobjParent, path);
		if(tf != null){
			gobj = tf.gameObject;
		}
		return gobj;
	}
	
	public static int GetIntLength(int i){
		int length = 0;
		length = i.ToString().Length;
		return length;
	}
	
	public static void ChangeLayersRecursively(Transform trans, string name)
	{
		try {
			trans.gameObject.layer = LayerMask.NameToLayer(name);
			foreach (Transform child in trans)
		    {
		        child.gameObject.layer = LayerMask.NameToLayer(name);
		        ChangeLayersRecursively(child, name);
		    }
		} catch (Exception ex) {
			Debug.Log(ex.ToString());	
		}
	}
	
	public static void ChangeLayersRecursively(GameObject gobj, string name){
		try {
			
			gobj.layer = LayerMask.NameToLayer(name);
			foreach (Transform child in gobj.transform)
		    {
		        child.gameObject.layer = LayerMask.NameToLayer(name);
		        ChangeLayersRecursively(child, name);
		    }
		} catch (Exception ex) {
			Debug.Log(ex.ToString());	
		}
	}
	
	public static int FindIntValInArr(int[] arr, int ivalue){
		int r = -1;
		for (int i = 0; i < arr.Length; i++) {
			if (arr[i] == ivalue) {
				r = i;
				break;
			}
		}
		return r;
	}
	
	public static string FloatToPercent(float fVal){
		string per = "";
		per = Mathf.Round(fVal * 100).ToString() + "%";
		return per;
	}
	
	public static Hashtable Hash(params object[] args){
		Hashtable hashTable = new Hashtable(args.Length/2);
		if (args.Length %2 != 0) {
			return null;
		}else{
			int i = 0;
			while(i < args.Length - 1) {
				hashTable.Add(args[i], args[i+1]);
				i += 2;
			}
			return hashTable;
		}
	}
	
	public static void SetGameObjMaterial(GameObject gobj, string materialName){
		Material changMaterial = UnityEngine.Object.Instantiate(Resources.Load("Materials/"+ materialName)) as Material;
		if(changMaterial != null){
			gobj.GetComponent<Renderer>().material = changMaterial;
		}else{
			Debug.LogError("can't find material:" + materialName);
		}
	}

	/// <summary>
	/// Instantiate an object by path and add it to the specified parent.
	/// </summary>
	public static GameObject AddNGUIChild(GameObject gobjParent, string path){
		GameObject r = null;
		GameObject gobjPrefab = LoadResourcePrefab(path);
		if(gobjParent != null && gobjPrefab != null){
			 r = NGUITools.AddChild(gobjParent, gobjPrefab);
		}else{
			Debug.LogError("Error In AddNGUIChild");
		}
		return r;
	}
	
	public static string Num2RomanTxt(int n){
	  int[] arabic = new int[13];
	  string[] roman = new string[13];
	  int i = 0;
	  string o = "";
	
	  arabic[0] = 1000;
	  arabic[1] = 900;
	  arabic[2] = 500;
	  arabic[3] = 400;
	  arabic[4] = 100;
	  arabic[5] = 90;
	  arabic[6] = 50;
	  arabic[7] = 40;
	  arabic[8] = 10;
	  arabic[9] = 9;
	  arabic[10] = 5;
	  arabic[11] = 4;
	  arabic[12] = 1;
	
	  roman[0] = "M";
	  roman[1] = "CM";
	  roman[2] = "D";
	  roman[3] = "CD";
	  roman[4] = "C";
	  roman[5] = "XC";
	  roman[6] = "L";
	  roman[7] = "XL";
	  roman[8] = "X";
	  roman[9] = "IX";
	  roman[10] = "V";
	  roman[11] = "IV";
	  roman[12] = "I";
	
	  while (n > 0)
	  {
		  while (n >= arabic[i])
		  {
		  n = n - arabic[i];
		  o = o + roman[i];
		  }
		  i++;
	  }
	  return o;
	}
 public static bool IsTouchLayer(Camera cameraSeeTheLayer, string layer)
    {
        bool r = false;
        string strLayer = "";
        Vector3 posMouse = Input.mousePosition;
        posMouse.z = 10;

        Ray ray = cameraSeeTheLayer.ScreenPointToRay(posMouse);

        RaycastHit[] rhs;
        rhs = Physics.RaycastAll(ray);
        if (rhs != null)
        {
            foreach (RaycastHit rh in rhs)
            {
                GameObject gobjHit = rh.collider.gameObject;
                if (gobjHit != null)
                {
                    strLayer += LayerMask.LayerToName(gobjHit.layer);
                }
            }
        }

        if (!string.IsNullOrEmpty(strLayer))
        {
            if (strLayer.Contains(layer))
            {
                r = true;
            }
        }
        return r;
    }
public static Collider GetColliderInDirection(GameObject ori, Vector3 direction, float distance, string layermaskname)
    {
        Collider c = null;
        Debug.DrawRay(ori.transform.position, direction);
        RaycastHit[] rhs = Physics.RaycastAll(ori.transform.position, direction, distance, 1 << LayerMask.NameToLayer(layermaskname));
        float minDistance = 1000;
        foreach (RaycastHit rh in rhs)
        {
            if(rh.distance < minDistance){
                minDistance = rh.distance;
                c = rh.collider;
            }
        }
        return c;
    }
	 /// <summary>
    /// ��ȡ�������߽�
    /// </summary>
    /// <param name="gobjs"></param>
    /// <returns></returns>
    public static Bounds GetBoundsOfMultiGobj(GameObject[] gobjs)
    {
        GameObject gobjOri = gobjs[0];
        Bounds boundOri = gobjOri.GetComponent<Renderer>().bounds;
        foreach (GameObject gobjTemp in gobjs)
        {
            boundOri.Encapsulate(gobjTemp.GetComponent<Renderer>().bounds);
        }
        return boundOri;
    }

    public static void SetUIPosBy3DGameObj(GameObject gobj2d, GameObject gobj3d, Camera camer3d, Camera camera2d, Vector3 offset)
    {
        Vector3 v1 = camer3d.WorldToViewportPoint(gobj3d.transform.position + offset);
        Vector3 v2 = camera2d.ViewportToWorldPoint(v1);
        v2.z = 0;
        gobj2d.transform.position = v2;
    }

    public static void SetUIPosBy3DGameObj(GameObject gobj2d, Vector3 pos3d, Camera camer3d, Camera camera2d)
    {
        Vector3 v1 = camer3d.WorldToViewportPoint(pos3d);
        Vector3 v2 = camera2d.ViewportToWorldPoint(v1);
        v2.z = 0;
        gobj2d.transform.position = v2;
    }

	/// <summary>
	/// �Ƿ����м���
	/// </summary>
	/// <returns>
	/// <c>true</c> if this instance is hit odds the specified odds; otherwise, <c>false</c>.
	/// </returns>
	/// <param name='odds'>
	/// If set to <c>true</c> odds.
	/// </param>
	public static bool IsHitOdds(int odds){
		bool r = false;
		if(odds >= 0 && odds <= 100){
			System.Random ran = new System.Random();
			int ranVal = ran.Next(1, 101);
			r = (odds >= ranVal);
		}else{
			Debug.LogError("error");
		}
		return r;
	}

    public static bool IsHitOdds(float odds)
    {
        bool r = false;
        if (odds >= 0 && odds <= 1)
        {
            float ranVal = UnityEngine.Random.Range(0.0f, 1.0f);
            r = (odds >= ranVal);
        }
        else
        {
            Debug.LogError("error");
        }
        return r;
    }

    public static void ActiveCollider(GameObject gobj, bool isActive) 
    {
        if (gobj.GetComponent<Collider>() != null)
        {
            gobj.GetComponent<Collider>().enabled = isActive;
        }
        Collider[] colliders = gobj.GetComponentsInChildren<BoxCollider>();
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = isActive;
        }
    }

    /// <summary>
    /// html颜色转Unity颜色
    /// </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    public static Color hexToColor(string hex)
    {
        hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
        hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
        byte a = 255;//assume fully visible unless specified in hex
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        //Only use alpha if the string has enough characters
        if (hex.Length == 8)
        {
            a = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        }
        return new Color32(r, g, b, a);
    }

		public static T AddCommentToGobj<T>(GameObject gobjTarget)where T : Component
		{
				if (gobjTarget != null) 
				{
					return gobjTarget.AddComponent<T> ();
				} 
				return null;
		}

		public static void RemoveComment(MonoBehaviour obj)
		{
			UnityEngine.GameObject.DestroyObject (obj);
		}
}