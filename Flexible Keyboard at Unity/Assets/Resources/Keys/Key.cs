using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour {
    private int ONE_GRID = 25;

    public string key = "";
    public Vector2 size; 

    public void Set(string key, Vector2 size, Vector2 potition)
    {
        this.key = key;
        name = key;
        this.size = size;
        RectTransform RT = GetComponent<RectTransform>();
        RT.sizeDelta = new Vector2(size.x * ONE_GRID, size.y * ONE_GRID);
        RT.anchoredPosition = potition;
    }

    public void send()
    {
        if (key != "")
        {
            if (!Bluetooth.TrySend(key))
                Error.error("sendできんかったで。(keyのやつ)");
        }
    }
}
