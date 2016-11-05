using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Test : MonoBehaviour {
    private List<string[]> deviceList = null;

    RectTransform A;
    RectTransform B;
    RectTransform C;
    RectTransform D;
    RectTransform E;
    void Init()
    {
        Transform Pparent = transform.parent.parent;
        A = Pparent.GetChild(0).GetComponent<RectTransform>();
        B = Pparent.GetChild(1).GetComponent<RectTransform>();
        C = Pparent.GetChild(2).GetComponent<RectTransform>();
        D = Pparent.GetChild(3).GetComponent<RectTransform>();
        E = Pparent.GetChild(4).GetComponent<RectTransform>();
    }
    void addAnPoX(int x)
    {
        A.anchoredPosition = new Vector2(A.anchoredPosition.x + x, A.anchoredPosition.y);
        B.anchoredPosition = new Vector2(B.anchoredPosition.x + x, B.anchoredPosition.y);
        C.anchoredPosition = new Vector2(C.anchoredPosition.x + x, C.anchoredPosition.y);
        D.anchoredPosition = new Vector2(D.anchoredPosition.x + x, D.anchoredPosition.y);
        E.anchoredPosition = new Vector2(E.anchoredPosition.x + x, E.anchoredPosition.y);
    }
    void addAnPoY(int y)
    {
        A.anchoredPosition = new Vector2(A.anchoredPosition.x, A.anchoredPosition.y + y);
        B.anchoredPosition = new Vector2(B.anchoredPosition.x, B.anchoredPosition.y + y);
        C.anchoredPosition = new Vector2(C.anchoredPosition.x, C.anchoredPosition.y + y);
        D.anchoredPosition = new Vector2(D.anchoredPosition.x, D.anchoredPosition.y + y);
        E.anchoredPosition = new Vector2(E.anchoredPosition.x, E.anchoredPosition.y + y);
    }

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
            Init();
            addAnPoY(-400);
//            GameObject myButton = Resources.Load<GameObject>("MyButton/Prefab");
//            GameObject newButton = Instantiate(myButton, new Vector2(0, 50), Quaternion.identity) as GameObject;
//            newButton.transform.SetParent(transform.parent, false);
//            newButton.GetComponent<MyButton>().Set(gameObject, "send");
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
