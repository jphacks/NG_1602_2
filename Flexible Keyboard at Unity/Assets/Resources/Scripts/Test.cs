using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Test : MonoBehaviour {
    private List<string[]> deviceList = null;

    public void test()
    {
        if (deviceList != null) Error.error("このボタンは2回押されるとこまるわ(´・ω・)");

        deviceList = new List<string[]>();
        GameObject myButton = Resources.Load<GameObject>("MyButton/Prefab");

        deviceList = Bluetooth.GetPairedDevices();
        for (int i = 0; i < deviceList.Count; i++)
        {
            GameObject newButton = Instantiate(myButton, new Vector2(0, -(40 + i * 40)), Quaternion.identity) as GameObject;
            newButton.transform.SetParent(transform.parent, false);
            newButton.GetComponent<MyButton>().Set(gameObject, i + ":" + deviceList[i][0]);
        }
    }

    void ButtonDown(string str)
    {
        if (str == "send")
        {
            Debug.Log("sendしたがるよ");
            if (!Bluetooth.TrySend("b"))
                Error.error("sendできんかったで。(Bluetooth.csじゃないほう)");
            return;
        }

        // send以外のとき
        Debug.Log("ボタン押したよ");
        int index = 0;
        try
        {
            index = int.Parse(Cut(str));
        }
        catch (FormatException e)
        {
            Debug.Log(e.Message);
            Error.error("string -> int に失敗したで。");
        }
        string address = deviceList[index][1];
        bool tryresult = Bluetooth.TryConnect(address);
        Debug.Log("try : " + tryresult);
        if (tryresult)
        {
            GameObject myButton = Resources.Load<GameObject>("MyButton/Prefab");
            GameObject newButton = Instantiate(myButton, new Vector2(0, 50), Quaternion.identity) as GameObject;
            newButton.transform.SetParent(transform.parent, false);
            newButton.GetComponent<MyButton>().Set(gameObject, "send");
        }
        else
        {
            Error.error("接続に失敗したで。(Bluetooth.csじゃないほう)");
        }
    }

    // 初めの:までのstringを返す
    private string Cut(string str)
    {
        string result = "";
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] == ':')
            {
                result = str.Substring(0, i);
                if (result == "") Error.error("この文字列先頭が「 : 」になっとるで。");
                return result;
            }
        }
        Error.error("この文字列「 : 」がないで。");
        return result;
    }
}
