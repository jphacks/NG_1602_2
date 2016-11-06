using UnityEngine;
using System.Collections;

public class Swipe : MonoBehaviour {
    private Vector2 start;
    private Vector2 finish;

	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            start = Input.mousePosition;
            StartCoroutine(SwipeCoroutine());
        }
	}

    IEnumerator SwipeCoroutine()
    {
        yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
        finish = Input.mousePosition;
        string result = GetDirection(start, finish);
        if(result != null) SendMessage("Move", result);
    }

    private string GetDirection(Vector2 s, Vector2 f)
    {
        float var_x = s.x - f.x;
        float var_y = s.y - f.y;

        if (Mathf.Abs(var_x) <= 150 && Mathf.Abs(var_y) <= 150) return null;

        if (Mathf.Abs(var_x) >= Mathf.Abs(var_y))
        {
            if (var_x > 0)
                return "Right";
            else
                return "Left";
        }
        else
        {
            if (var_y > 0)
                return "Up";
            else
                return "Down";
        }
    }
}
