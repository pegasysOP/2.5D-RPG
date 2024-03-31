using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ExtendedSelectionBoxOption : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI optionText;
    [SerializeField] private GameObject selectionArrow;

    [HideInInspector] public UnityEvent<int> OnClick;
    [HideInInspector]public UnityEvent<int> OnHover;

    private int index = -1;
    public int Index {  get { return index; } }

    public void Init(int index, string message, bool selected)
    {
        this.index = index;
        optionText.text = message;
        selectionArrow.SetActive(selected);
    }

    public void SetSelected(bool selected)
    {
        selectionArrow.SetActive(selected);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick.Invoke(index);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetSelected(true);
        OnHover.Invoke(index);
    }
}
