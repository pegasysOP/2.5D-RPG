using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static TMPro.TMP_Compatibility;

public static class UIUtils
{
    public enum Direction { Up, Down, Left, Right }

    public static ExtendedSelectionBox ShowExtendedSelectionBox(RectTransform parentRect, List<string> options, UnityAction<int> optionSelectedAction, int selectedIndex = 0, Direction spawnDirection = Direction.Right)
    {
        ExtendedSelectionBox extendedSelectionBox = UIFactory.SpawnExtendedSelectionBox();
        SetPosition(extendedSelectionBox.GetComponent<RectTransform>(), parentRect, spawnDirection);

        extendedSelectionBox.Init(options, optionSelectedAction, selectedIndex);

        return extendedSelectionBox;
    }

    private static void SetPosition(RectTransform objectRect, RectTransform parentRect, Direction direction)
    {
        objectRect.SetParent(parentRect);
        objectRect.transform.localScale = Vector3.one;

        switch (direction)
        {
            case Direction.Up:
                objectRect.anchorMin        = new Vector2(0.5f, 0.0f);
                objectRect.anchorMax        = new Vector2(0.5f, 0.0f);
                objectRect.pivot            = new Vector2(0.5f, 0.0f);
                objectRect.anchoredPosition = new Vector2(0.0f, parentRect.rect.height);
                break;
            case Direction.Down:
                objectRect.anchorMin        = new Vector2(0.5f, 1.0f);
                objectRect.anchorMax        = new Vector2(0.5f, 1.0f);
                objectRect.pivot            = new Vector2(0.5f, 1.0f);
                objectRect.anchoredPosition = new Vector2(0.0f, -parentRect.rect.height);
                break;
            case Direction.Left:
                objectRect.anchorMin        = new Vector2(1.0f, 0.5f);
                objectRect.anchorMax        = new Vector2(1.0f, 0.5f);
                objectRect.pivot            = new Vector2(1.0f, 0.5f);
                objectRect.anchoredPosition = new Vector2(-parentRect.rect.width, 0.0f);
                break;
            case Direction.Right:
                objectRect.anchorMin        = new Vector2(0.0f, 0.5f);
                objectRect.anchorMax        = new Vector2(0.0f, 0.5f);
                objectRect.pivot            = new Vector2(0.0f, 0.5f);
                objectRect.anchoredPosition = new Vector2(parentRect.rect.width, 0.0f);
                break;
        }
    }
}
