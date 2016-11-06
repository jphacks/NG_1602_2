using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BluetoothSetting : MonoBehaviour {
    private List<string[]> deviceList = null;

    void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        StartCoroutine(WaitTap());
#endif
    }

    // tapされるまで待つ
    IEnumerator WaitTap()
    {
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        ShowList();
    }

    // ペアリング済みのデバイス一覧を表示する
    public void ShowList()
    {
        GetComponentInChildren<Text>().text = "";

        deviceList = new List<string[]>();
        GameObject myButton = Resources.Load<GameObject>("MyButton/Prefab");

        deviceList = Bluetooth.GetPairedDevices();
        if (deviceList.Count == 0)
            Error.error("ペアリング済みデバイスが存在しません");
        for (int i = 0; i < deviceList.Count; i++)
        {
            GameObject newButton = Instantiate(myButton, new Vector2(0, -(0 + i * 40)), Quaternion.identity) as GameObject;
            newButton.transform.SetParent(transform, false);
            newButton.GetComponent<MyButton>().Set(gameObject, i + ":" + deviceList[i][0]);
        }
    }

    void ButtonDown(string str)
    {
        // strからintを取得
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
        // addressから接続を試みる
        string address = deviceList[index][1];
        bool tryresult = Bluetooth.TryConnect(address);
        // 成功したら画面遷移
        if (tryresult)
            transform.parent.SendMessage("Move", "Up");
        // 失敗はエラー
        else
            Error.error("接続に失敗したで。(Bluetooth.csじゃないほう)");
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
