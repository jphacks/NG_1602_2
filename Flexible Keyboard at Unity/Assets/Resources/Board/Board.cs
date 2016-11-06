using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour {
    public GameObject cell;
    public bool editMode = false;

    void MakeGrid()
    {
        GameObject parent = new GameObject();
        parent.name = "Grid";
        parent.transform.SetParent(transform, false);
        RectTransform pRect = parent.AddComponent<RectTransform>();
        pRect.anchorMax = new Vector2(1, 1);
        pRect.anchorMin = new Vector2(0, 0);

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                MakeCell(parent.transform, new Vector2(i * 25 - 100, 175 - j * 25));
            }
        }
    }

    void MakeCell(Transform parent, Vector2 position)
    {
        GameObject newCell = Instantiate(cell, position, Quaternion.identity) as GameObject;
        newCell.transform.SetParent(parent, false);
    }
}
