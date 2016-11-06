using UnityEngine;
using System.Collections;

public class Enter : MonoBehaviour {

    public bool single = true;

    // タッチ
    Touch touch;
    int count = 0;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            this.touch = Input.touches[0];

            // tapしてはじめてから4フレームいない
            count++;
            if (this.touch.phase == TouchPhase.Began)
                count = 0;

            if (count < 4)
            {
                Debug.Log("Input.touchCount == " + Input.touchCount);
                switch (Input.touchCount)
                {
                    case 1: // 1本指でタッチ
                        single = true;
                        break;
                    default: // 2本指以上
                        single = false;
                        count = 10;
                        if (GetComponent<MoveScreen>().currentScreen == "Bottom")
                            return;
                        string key = ((char)10).ToString();
                        if (!Bluetooth.TrySend(key))
                            Error.error("sendできんかったで。(keyのやつ)");
                        else Debug.Log("Enter");
                        break;
                }
            }
        }
    }
}
