using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour {

    public string key = "";

    public void send()
    {
        if (key != "")
        {
            Debug.Log("sendしたがるよ");
            if (!Bluetooth.TrySend(key))
                Error.error("sendできんかったで。(keyのやつ)");
        }
    }
}
