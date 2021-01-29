using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class AbilityButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    [SerializeField] private Image image;
    [SerializeField] private float hoverScaleStrength = 0.1f;
    [SerializeField] private float hoverScaleDuration = 0.2f;

    private Button button;

    public void Initialize(Sprite icon, Action onClick = null) {
        image.sprite = icon;
        button = GetComponent<Button>();
        button.onClick.AddListener(() => onClick());
    }

    public void Deinitialize() {
        button.onClick.RemoveAllListeners();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        image.transform.DOKill(true);
        image.transform.DOPunchScale(Vector3.one * hoverScaleStrength, hoverScaleDuration);
    }

    public void OnPointerExit(PointerEventData eventData) {
    }
}
