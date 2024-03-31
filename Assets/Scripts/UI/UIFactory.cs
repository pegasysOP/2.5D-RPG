using System;
using UnityEngine;

public class UIFactory : MonoBehaviour
{
    #region Prefabs
    [Header("Prefabs")]
    [SerializeField] private ExtendedSelectionBox extendedSelectionBoxPrefab;
    #endregion

    private static UIFactory _instance;
    private static UIFactory Instance
    {
        get
        {
            if (_instance != null)
                return _instance;
            else
                throw new UIFactoryInstanceNotFoundException();
        }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    public static ExtendedSelectionBox SpawnExtendedSelectionBox()
    {
        return Instantiate(Instance.extendedSelectionBoxPrefab);
    }
}

public class UIFactoryInstanceNotFoundException : Exception
{
    public UIFactoryInstanceNotFoundException() : base("UI Factory Singleton Instance Not Found")
    {
    }
}
