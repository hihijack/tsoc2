using UnityEngine;
using System.Collections;
using System.IO;

public static class GameManager {

    public static GameView gameView;
    public static DbAccess dba;

    public static void ConTODB()
    {
        if (dba != null)
        {
            CloseConToDB();
        }
        else
        {

            //如果运行在编辑器中
#if UNITY_EDITOR || UNITY_WEBGL || UNITY_STANDALONE_WIN
            //通过路径找到第三方数据库
            string appDBPath = Application.streamingAssetsPath + "/tsoc.db";
            dba = new DbAccess("URI=file:" + appDBPath);
            //如果运行在Android设备中
#elif UNITY_ANDROID
 
		//将第三方数据库拷贝至Android可找到的地方
		string appDBPath = Application.persistentDataPath + "/tsoc.db";
 
		//如果已知路径没有地方放数据库，那么我们从Unity中拷贝
		if(!File.Exists(appDBPath))
 
 		{
			//用www先从Unity中下载到数据库
		    WWW loadDB = new WWW(Application.streamingAssetsPath + "/tsoc.db"); 
            while(!loadDB.isDone) {}
			//拷贝至规定的地方
		    File.WriteAllBytes(appDBPath, loadDB.bytes);
		}
 
		//在这里重新得到db对象。
		dba = new DbAccess("URI=file:" + appDBPath);
 
#endif
            Debug.Log("连接数据库完成");
        }
        
    }

    public static void CloseConToDB()
    {
        if (dba != null)
        {
            dba.CloseSqlConnection();
            dba = null;
        }
    }

    public static Hero hero;

    public static CommonCPU commonCPU;
}
