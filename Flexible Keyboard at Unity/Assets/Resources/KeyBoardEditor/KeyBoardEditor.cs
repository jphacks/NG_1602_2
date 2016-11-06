using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class KeyBoardEditor : MonoBehaviour {
    public GameObject key;
    public GameObject board;
    string position;
    bool direction;

    // position : Left,Center,Right
    // direction : 縦かどうか(縦はture)
    public void Set(string position, bool direction)
    {
        this.position = position;
        this.direction = direction;
        transform.GetChild(0).SendMessage("MakeGrid");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject target = GetGameObject(Input.mousePosition, "Key");
            if (target)
                target.SendMessage("Move");
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

    // keyの生成
    void CreateKey()
    {
        // トグルの値とる
        IEnumerable<Toggle> toggles = transform.GetChild(2).GetComponentInChildren<ToggleGroup>().ActiveToggles();
        Vector2 size = new Vector2();
        foreach (Toggle toggle in toggles)
        {
            if (toggle.isOn)
                size = ToVector2FromString(toggle.gameObject.name);
        }
        string value = transform.GetComponentInChildren<Dropdown>().captionText.text;
        // keyの生成
        GameObject newKey = Instantiate(key);
        newKey.transform.SetParent(transform.GetChild(0) , false);
        newKey.GetComponent<Key>().Set(value, size, new Vector2());
    }

    // sizeのトグルの名前からvec2を作る関数。
    Vector2 ToVector2FromString(string str)
    {
        int x = 0;
        int y = 0;

        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] == ' ')
            {
                x = int.Parse(str.Substring(0, i));
                y = int.Parse(str.Substring(i + 1, str.Length - (i + 1)));
                return new Vector2(x, y);
            }
        }

        Error.error("この文字列スペースないから分解できないんですけど。Vector2にするの無理やわ～");
        return new Vector2();
    }

    // 編集終了関数
    void FinishEdit()
    {
        Transform target = null;
        switch (position)
        {
            case "Left":
                target = transform.parent.GetChild(1);
                break;
            case "Center":
                target = transform.parent.GetChild(2);
                break;
            case "Right":
                target = transform.parent.GetChild(3);
                break;
            default:
                Error.error("editor set miss position not left or center or right\n※ただし、英文的な正しさについては考えないものとする。");
                break;
        }
        if (target)
        {
            Destroy(target.GetChild(0).gameObject);
            GameObject newBaord = Instantiate(board);
            newBaord.transform.SetParent(target, false);

            Transform editorBoard = transform.GetChild(0);
            int num = editorBoard.childCount;
            List<Transform> keys = new List<Transform>();
            for (int i = 0; i < num; i++)
            {
                GameObject child = editorBoard.GetChild(i).gameObject;
                if (child.tag == "Key")
                    keys.Add(child.transform);
            }
            foreach(Transform a in keys)
                a.SetParent(newBaord.transform, false);
        }
        Destroy(gameObject);
    }

    void ButtonDown(string str)
    {
        switch (str)
        {
            case "noEdit":
                Destroy(gameObject);
                break;
            case "add":
                transform.GetChild(2).gameObject.SetActive(true);
                break;
            case "finish":
                FinishEdit();
                break;
            case "create key":
                CreateKey();
                transform.GetChild(2).gameObject.SetActive(false);
                break;
            case "return":
                transform.GetChild(2).gameObject.SetActive(false);
                break;
            default:
                Error.error("editerのボタンのcase文にない文字受け取ったよ。editorがね。");
                break;
        }
    }
}
