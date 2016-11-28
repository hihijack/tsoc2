using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

public class OutLog : MonoBehaviour
{
    static List<string> mLines = new List<string>();
    static List<string> mWriteTxt = new List<string>();
    private static string outpath = Application.persistentDataPath + "/outLog.txt";
    public static bool isLog = false;
    public static bool enableLog = false;
    public static void ToggleLog()
    {
        if (!enableLog)
        {
            return;
        }

        GameObject gobjLog = GameObject.FindGameObjectWithTag("Log");
        if (gobjLog == null)
        {
            gobjLog = new GameObject();
            gobjLog.name = "log";
            gobjLog.tag = "Log";
            gobjLog.AddComponent<OutLog>();
            DontDestroyOnLoad(gobjLog);
            OutLog.StartLog();
        }
        if (isLog)
        {
            isLog = false;
        }
        else
        {
            isLog = true;
        }
    }

   public static void StartLog()
   {
        //Application.persistentDataPath Unity中只有这个路径是既可以读也可以写的。
        //每次启动客户端删除之前保存的Log
        if (System.IO.File.Exists(outpath))
        {
            File.Delete(outpath);
        }
        //在这里做一个Log的监听
        Application.RegisterLogCallback(HandleLog);
        Debug.Log("开启LOG监听");//#######
      
   }

    void Update()
    {
        if (isLog)
        {
            //因为写入文件的操作必须在主线程中完成，所以在Update中写入文件。
            if (mWriteTxt.Count > 0)
            {
                string[] temp = mWriteTxt.ToArray();
                foreach (string t in temp)
                {
                    using (StreamWriter writer = new StreamWriter(outpath, true, Encoding.UTF8))
                    {
                        writer.WriteLine(t + "----------" + DateTime.Now.ToString() + "/" + Time.frameCount);
                    }
                    mWriteTxt.Remove(t);
                }
            }
        }
    }

    public static void HandleLog(string logString, string stackTrace, LogType type)
    {
        mWriteTxt.Add(logString);
        Log(logString);
        //Log(stackTrace);
    }

    //这里我把错误的信息保存起来，用来输出在手机屏幕上
    static private void Log(params object[] objs)
    {
        if (isLog)
        {
            string text = "";
            for (int i = 0; i < objs.Length; ++i)
            {
                if (i == 0)
                {
                    text += objs[i].ToString();
                }
                else
                {
                    text += ", " + objs[i].ToString();
                }
            }
            if (true)
            {
                if (mLines.Count > 20)
                {
                    mLines.RemoveAt(0);
                }
                mLines.Add(text);

            }  
        }
    }

    void OnGUI()
    {
        if (isLog)
        {
            GUI.color = Color.red;
            for (int i = 0, imax = mLines.Count; i < imax; ++i)
            {
                GUILayout.Label(mLines[i]);
            }
        }
    }
}