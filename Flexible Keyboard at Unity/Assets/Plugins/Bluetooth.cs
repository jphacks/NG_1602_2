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
    static public string GetPairedDevices()
    {
        string str = "";
#if UNITY_ANDROID && !UNITY_EDITOR
        if (m_plugin != null){
            if (m_plugin.Call<bool>("BluetoothEnabled", "")){
                str += m_plugin.Call<string>("BluetoothPairedDevices", "");
            }
            else {
                str += "BluetoothError";
            }
        }
        else {
            str += "BluetoothError";
        }

#else
        str += "Not Andoroid";
#endif
        return str;
    }

    // javaからのstringをname一覧に変換する関数
    static public List<string> Disassembly(string str)
    {
        List<string> names = new List<string>();
        int start = 0;
        bool flag = true;

        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] == '\n')
            {
                if (flag)
                {
                    names.Add(str.Substring(start, i - start));
                    flag = false;
                }
                else
                {
                    start = i + 1;
                    flag = true;
                }
            }
        }

        return names;
    }

    static public string NameToAdress(string name)
    {
        string str = "";
#if UNITY_ANDROID && !UNITY_EDITOR
        if (m_plugin != null){
                str += m_plugin.Call<string>("BluetoothNameTest", name);
        }
#endif
        return str;
    }

    // 接続チャレンジ
    static public bool TryConnect(string address)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (m_plugin != null){
                if (m_plugin.Call<bool>("SelectDevice", address)){
                    if (m_plugin.Call<bool>("GetSocket")){
                        if (m_plugin.Call<bool>("TryConnect")){
                            return true;
                        }
                    }
                }
        }
        return false;
#endif
        return false;
    }

    // 送るチャレンジ
    static public bool TrySend(string str)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (m_plugin != null){
            Debug.Log("ぷらぐいん");
                if (m_plugin.Call<bool>("GetOutputStream")){
            Debug.Log("あうとぷっと");
                    if (m_plugin.Call<bool>("SendString", str)){
            Debug.Log("せんど");
                            return true;
                    }
                }
        }
        return false;
#endif
        return false;
    }
}
