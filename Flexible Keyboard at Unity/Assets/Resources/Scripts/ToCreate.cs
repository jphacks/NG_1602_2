using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ToCreate : MonoBehaviour {
    public GameObject editor;

    void ButtonDown(string str)
    {
        if (str == "create")
        {
            GameObject newEditor = Instantiate(editor);
            newEditor.transform.SetParent(transform.parent, false);
            // トグルの値とる
            IEnumerable<Toggle> toggles = transform.GetChild(1).GetComponent<ToggleGroup>().ActiveToggles();
            string position = "";
            foreach(Toggle toggle in toggles)
            {
                if (toggle.isOn) position = toggle.gameObject.name;
            }
            toggles = transform.GetChild(2).GetComponent<ToggleGroup>().ActiveToggles();
            bool direction = true;
            foreach (Toggle toggle in toggles)
            {
                if (toggle.isOn)
                {
                    if (toggle.gameObject.name == "true")
                        direction = true;
                    else
                        direction = false;
                }
            }
            newEditor.GetComponent<KeyBoardEditor>().Set(position, direction);
        }
    }
}
