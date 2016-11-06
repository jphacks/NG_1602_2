using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Error : MonoBehaviour {
    static public void error(string str)
    {
        GameObject.FindGameObjectWithTag("AlertCanvas").transform.GetChild(0).GetComponent<Text>().text = str;
        SceneManager.LoadScene("_Error");
    }
}
