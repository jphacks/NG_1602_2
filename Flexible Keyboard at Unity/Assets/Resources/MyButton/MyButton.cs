using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MyButton : MonoBehaviour {

    public GameObject receiver = null;
    public string buttonName = "";

    public void Set(GameObject obj, string name)
    {
        receiver = obj;
        this.buttonName = name;
        GetComponentInChildren<Text>().text = name;
    }
    public void Set(GameObject obj, string name, bool flag)
    {
        receiver = obj;
        this.buttonName = name;
        if (flag) GetComponentInChildren<Text>().text = name;
    }

    public void ButtonDown()
    {
        if (receiver) receiver.SendMessage("ButtonDown", buttonName);
    }
}
