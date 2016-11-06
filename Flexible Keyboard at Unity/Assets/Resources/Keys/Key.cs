using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Key : MonoBehaviour {
    private int ONE_GRID = 25;

    public string key = "";
    public Vector2 size;

    private Vector2 startPosi;

    public void Set(string key, Vector2 size, Vector2 potition)
    {
        this.key = key;
        name = key;
        GetComponentInChildren<Text>().text = key;
        this.size = size;
        RectTransform RT = GetComponent<RectTransform>();
        RT.sizeDelta = new Vector2(size.x * ONE_GRID, size.y * ONE_GRID);
        RT.anchoredPosition = potition;
    }

    IEnumerator Move()
    {
        if (transform.parent.GetComponent<Board>().editMode)
        {
            RectTransform RT = GetComponent<RectTransform>();
            startPosi = RT.anchoredPosition;
            while (!Input.GetMouseButtonUp(0))
            {
                float x = (225f / Screen.width) * Input.mousePosition.x - 112.5f;
                float y = (400f / Screen.height) * Input.mousePosition.y - 200f;
                RT.anchoredPosition = new Vector2(x, y);
                yield return null;
            }
            GameObject target = GetGameObject(Input.mousePosition, "Cell");
            if (target)
                RT.anchoredPosition = target.GetComponent<RectTransform>().anchoredPosition;
            else
                RT.anchoredPosition = startPosi;
        }
    }

    // 第一引数の位置にある、第二引数のtagのついたゲームオブジェクトを返す関数。
    public GameObject GetGameObject(Vector2 position, string tag)
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = position;
        List<RaycastResult> result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, result);
        for (int i = 0; i < result.Count; i++)
        {
            GameObject obj = result[i].gameObject;
            if (obj.tag == tag) return obj;
        }
        return null;
    }

    // onClickで発動
    public void send()
    {
        // 編集モードのボード下ではsendしない
        if (transform.parent.GetComponent<Board>().editMode) return;
        // マルチタップはsendしない
        if (!FindObjectOfType<Enter>().single) return;

        if (key != "")
        {
            switch (key)
            {
                case "ue":
                    key = ((char)18).ToString();
                    break;
                case "shita":
                    key = ((char)20).ToString();
                    break;
                case "migi":
                    key = ((char)19).ToString();
                    break;
                case "hidari":
                    key = ((char)17).ToString();
                    break;
                case "syuryo":
                    key = ((char)21).ToString();
                    break;
            }
            if (!Bluetooth.TrySend(key))
                Error.error("sendできんかったで。(keyのやつ)");
        }
    }
}
