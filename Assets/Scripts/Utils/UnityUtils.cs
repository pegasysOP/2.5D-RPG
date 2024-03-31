using System.Collections.Generic;
using UnityEngine;

public static class UnityUtils
{
    public static void ClearMonoBehaviourList<T>(List<T> list) where T : MonoBehaviour
    {
        foreach (T item in list)
        {
            if (item != null && item.gameObject != null)
                Object.Destroy(item.gameObject);
        }

        list.Clear();
    }

    public static void DestroyGameObjectList(List<GameObject> list)
    {
        if (list == null)
            return;

        foreach (GameObject gameObject in list)
        {
            if (gameObject != null)
            {
                Object.Destroy(gameObject);
            }
        }

        list.Clear();
    }
}
