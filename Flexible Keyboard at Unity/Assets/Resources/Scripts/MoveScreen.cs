using UnityEngine;
using System.Collections;

public class MoveScreen : MonoBehaviour {
    RectTransform[] rectTransforms;
    public string currentScreen = "Bottom"; // Bottom, Center, Head, Right, Left

    void Start () {
        Init();
#if UNITY_EDITOR
        Move("Up");
#endif
    }

    void Init()
    {
        rectTransforms = new RectTransform[5];
        for(int i = 0; i < 5; i++)
            rectTransforms[i] = transform.GetChild(i).GetComponent<RectTransform>();
    }

    // x移動
    void MoveX(int x)
    {
        for (int i = 0; i < 5; i++)
        {
            RectTransform A = rectTransforms[i];
            A.anchoredPosition = new Vector2(A.anchoredPosition.x + x, A.anchoredPosition.y);
        }
    }
    // y移動
    void MoveY(int y)
    {
        for (int i = 0; i < 5; i++)
        {
            RectTransform A = rectTransforms[i];
            A.anchoredPosition = new Vector2(A.anchoredPosition.x, A.anchoredPosition.y + y);
        }
    }

    // 指定方向へ画面遷移
    void Move(string direction)
    {
        switch (currentScreen)
        {
            case "Bottom":
                if (direction == "Up")
                {
                    currentScreen = "Center";
                    MovingY(-1);
                }
                break;
            case "Head":
                if (direction == "Down")
                {
                    currentScreen = "Center";
                    MovingY(1);
                }
                break;
            case "Right":
                if (direction == "Left")
                {
                    currentScreen = "Center";
                    MovingX(1);
                }
                break;
            case "Left":
                if (direction == "Right")
                {
                    currentScreen = "Center";
                    MovingX(-1);
                }
                break;
            case "Center":
                switch (direction)
                {
                    case "Up":
                        currentScreen = "Head";
                        MovingY(-1);
                        break;
                    case "Right":
                        currentScreen = "Right";
                        MovingX(-1);
                        break;
                    case "Left":
                        currentScreen = "Left";
                        MovingX(1);
                        break;
                }
                break;
            default:
                Error.error("現在の画面がおかしいんだよ");
                break;
        }
    }

    void MovingX(int sign)
    {
        StartCoroutine(MovingX_Animation(sign));
    }

    IEnumerator MovingX_Animation(int sign)
    {
        for (int i = 0; i < 9; i++)
        {
            MoveX(25 * sign);
            yield return null;
        }
    }

    void MovingY(int sign)
    {
        StartCoroutine(MovingY_Animation(sign));
    }

    IEnumerator MovingY_Animation(int sign)
    {
        for (int i = 0; i < 16; i++)
        {
            MoveY(25 * sign);
            yield return null;
        }
    }
}
