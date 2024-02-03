using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler// required interface when using the OnPointerEnter method.
{

    [SerializeField] private GameObject arrow;

    //Do this when the cursor enters the rect area of this selectable UI object.
    public void OnPointerEnter(PointerEventData eventData)
    {
        arrow.SetActive(true);
        arrow.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 90);
        arrow.GetComponent<RectTransform>().localPosition = GetComponent<RectTransform>().localPosition;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        arrow.SetActive(false);
    }
}