using UnityEngine;
using System.Collections;

public class MoveScreen : MonoBehaviour {
    RectTransform[] rectTransforms;
    RectTransform B;
    RectTransform C;
    RectTransform D;
    RectTransform E;

    void Start () {
        Init();
	}
	
	// Update is called once per frame
	void Update () {
	
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
    void MovingX(int num)
    {
        StartCoroutine(MovingX_Animation(num));
    }

    IEnumerator MovingX_Animation(int num)
    {
        int sign = num / Mathf.Abs(num);
        for (int i = 0; i < 9 * Mathf.Abs(num); i++)
        {
            MoveX(25 * sign);
            yield return null;
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
    void MovingY(int num)
    {
        StartCoroutine(MovingY_Animation(num));
    }

    IEnumerator MovingY_Animation(int num)
    {
        int sign = num / Mathf.Abs(num);
        for (int i = 0; i < 16 * Mathf.Abs(num); i++)
        {
            MoveY(25 * sign);
            yield return null;
        }
    }
}
