using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExtendedSelectionBox : MonoBehaviour
{
    [SerializeField] private Transform contentTransform;
    [SerializeField] private ExtendedSelectionBoxOption optionComponentPrefab;

    private List<ExtendedSelectionBoxOption> optionComponents = new List<ExtendedSelectionBoxOption>();
    private UnityAction<int> optionSelectedAction;

    public void Init(List<string> options, UnityAction<int> optionSelectedAction, int selectedIndex)
    {
        this.optionSelectedAction = optionSelectedAction;

        ClearOldOptionComponents();
        CreateOptionComponents(options, selectedIndex);
    }

    private void CreateOptionComponents(List<string> options, int selectedIndex)
    {
        for (int i = 0; i < options.Count; i++)
        {
            ExtendedSelectionBoxOption optionComponent = Instantiate(optionComponentPrefab, contentTransform);
            optionComponent.Init(i, options[i], i == selectedIndex);
            optionComponent.OnClick.AddListener(OnOptionSelected);
            optionComponent.OnHover.AddListener(OnOptionHover);

            optionComponents.Add(optionComponent);
        }
    }

    private void ClearOldOptionComponents()
    {
        foreach (ExtendedSelectionBoxOption optionComponent in optionComponents)
        {
            optionComponent.OnClick.RemoveListener(OnOptionSelected);
            optionComponent.OnHover.RemoveListener(OnOptionHover);
        }

        UnityUtils.ClearMonoBehaviourList(optionComponents);
    }

    private void OnOptionSelected(int index)
    {
        if (index > optionComponents.Count)
        {
            Debug.LogError($"ExtendedSelectionBox > Selected index out of range for number of options: {index}/{optionComponents.Count}");
            return;
        }

        optionSelectedAction(index);
        Destroy(gameObject);
    }

    private void OnOptionHover(int index)
    {
        for(int i = 0; i < optionComponents.Count; i++)
        {
            if (optionComponents[i] != null)
            {
                ExtendedSelectionBoxOption optionComponent = optionComponents[i];
                optionComponent.SetSelected(optionComponent.Index == index);
            }
        }
    }
}
