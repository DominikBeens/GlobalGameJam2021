using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LevelSelectButton : MonoBehaviour {

    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI levelNameText;

    public void Initialize(Level level, Action onClick) {
        levelNameText.text = level.levelName;
        button.onClick.AddListener(() => onClick?.Invoke());
    }

    private void OnDestroy() {
        button.onClick.RemoveAllListeners();
    }
}
