using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bluetooth : MonoBehaviour {
    static AndroidJavaObject bluetoothPlugin = null;

    public void Awake()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        // プラグイン名をパッケージ名+クラス名で指定する。
        bluetoothPlugin = new AndroidJavaObject( "tmy.jack.mybluetoothplugin.BluetoothManage" );
#endif
    }

    // 名前とアドレスの一覧を返す
    static public List<string[]> GetPairedDevices()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (bluetoothPlugin != null){
            if (bluetoothPlugin.Call<bool>("Enabled")){
                string str = bluetoothPlugin.Call<string>("GetPairedDevices");
                return ChangeForList(str);
            }
            else {
                Error.error("Bluetoothが使えない状態になってるよ。");
            }
        }
        else {
            Error.error("プラグインのセット(Awakeの処理)ができていません。");
        }
#endif
        return new List<string[]>();
    }

    // string型のデバイス一覧をlist型に変換する関数
    static private List<string[]> ChangeForList(string str)
    {
        List<string[]> deviceList = new List<string[]>();
        int nameStart = 0;
        int addressStart = 0;
        int count = 0;

        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] == '\n')
            {
                count++;
                if (count == 1)
                {
                    addressStart = i + 1;
                }
                else if (count == 2)
                {
                    string name = str.Substring(nameStart, i - nameStart);
                    string address = str.Substring(addressStart, i - addressStart);
                    deviceList.Add(new string[] { name, address });
                    nameStart = i + 1;
                    count = 0;
                }
                else
                    Error.error("ChangeForList (string -> list　の処理) のcountがおかしいですよ。");
            }
        }

        return deviceList;
    }

    // 選択したデバイスと接続試みる関数。
    static public bool TryConnect(string address)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (bluetoothPlugin != null){
                if (bluetoothPlugin.Call<bool>("SelectDevice", address)){
                    if (bluetoothPlugin.Call<bool>("GetSocket")){
                        if (bluetoothPlugin.Call<bool>("TryConnect")){
                            return true;
                        }
                        else {
                            Error.error("bluetooth接続に失敗しました。");
                        }
                    }
                    else {
                        Error.error("socketの取得に失敗しました。");
                    }
                }
                else {
                    Error.error("デバイスの選択に失敗しました。");
                }
        }
        else {
            Error.error("プラグインのセット(Awakeの処理)ができていません。");
        }
#endif
        return false;
    }

    // 文字列を送る関数。
    static public bool TrySend(string str)
    {
        // androidなら頑張って送る
#if UNITY_ANDROID && !UNITY_EDITOR
        if (bluetoothPlugin != null){
                if (bluetoothPlugin.Call<bool>("GetOutputStream")){
                    if (bluetoothPlugin.Call<bool>("SendString", str)){
                            return true;
                    }
                    else {
                        Error.error("アプトプットストリームは取れましたが、文字列の送信に失敗しました。");
                    }
                }
                else {
                    Error.error("アウトプットストリームが取得できません。");
                }
        }
        else {
            Error.error("プラグインのセット(Awakeの処理)ができていません。");
        }
        return false;
#endif
        // unityならデバッグログ
#if UNITY_EDITOR
        Debug.Log(str);
        return true;
#endif
    }
}
