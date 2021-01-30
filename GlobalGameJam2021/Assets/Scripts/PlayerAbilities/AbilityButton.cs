using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using System;
using TMPro;

public class AbilityButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;
    [Space]
    [SerializeField] private Sprite moveSprite;
    [SerializeField] private Sprite attackSprite;
    [SerializeField] private Sprite flareSprite;
    [Space]
    [SerializeField] private float hoverScaleIn = 1.2f;
    [SerializeField] private float hoverScaleInDuration = 0.2f;
    [SerializeField] private float hoverScaleOutDuration = 0.2f;
    [Space]
    [SerializeField] private float clickScaleStrength = -0.1f;
    [SerializeField] private float clickScaleDuration = 0.2f;

    private Button button;

    public void Initialize(PlayerAbilityManager.AbilityType type, Action onClick = null) {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => onClick());
        button.onClick.AddListener(OnButtonClicked);

        switch (type) {
            case PlayerAbilityManager.AbilityType.Move: image.sprite = moveSprite; text.text = "Move"; break;
            case PlayerAbilityManager.AbilityType.Attack: image.sprite = attackSprite; text.text = "Attack"; break;
            case PlayerAbilityManager.AbilityType.Flare: image.sprite = flareSprite; text.text = "Flare"; break;
        }
    }

    public void Deinitialize() {
        button.onClick.RemoveAllListeners();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        image.transform.DOKill(true);
        image.transform.DOScale(hoverScaleIn, hoverScaleInDuration).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData) {
        image.transform.DOKill(true);
        image.transform.DOScale(1f, hoverScaleOutDuration);
    }

    private void OnButtonClicked() {
        image.transform.DOKill(true);
        image.transform.DOPunchScale(Vector3.one * clickScaleStrength, clickScaleDuration);
    }
}
