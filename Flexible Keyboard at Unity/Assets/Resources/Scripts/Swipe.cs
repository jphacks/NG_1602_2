using UnityEngine;
using System.Collections;

public class Swipe : MonoBehaviour {
    public Vector2 startPos;
    public Vector2 direction;
    public bool directionChosen;
    void Update()
    {
        // Track a single touch as a direction control.
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Handle finger movements based on touch phase.
            switch (touch.phase)
            {
                // Record initial touch position.
                case TouchPhase.Began:
                    startPos = touch.position;
                    directionChosen = false;
                    break;

                // Determine direction by comparing the current touch position with the initial one.
                case TouchPhase.Moved:
                    direction = touch.position - startPos;
                    break;

                // Report that a direction has been chosen when the finger is lifted.
                case TouchPhase.Ended:
                    directionChosen = true;
                    break;
            }
        }
        if (directionChosen)
        {
            directionChosen = false;
            string result = GetDirection(direction);
            if (result != null) SendMessage("Move", result);
        }
    }

    private string GetDirection(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) <= 150 && Mathf.Abs(dir.y) <= 150) return null;

        if (Mathf.Abs(dir.x) >= Mathf.Abs(dir.y))
        {
            if (dir.x < 0)
                return "Right";
            else
                return "Left";
        }
        else
        {
            if (dir.y < 0)
                return "Up";
            else
                return "Down";
        }
    }
}
