using UnityEngine;
using DG.Tweening;

public class Tile : MonoBehaviour {

    [SerializeField] private Transform visual;
    public Entity myEntity;
    [SerializeField] private Transform randomVisualHolder;

    [Header("Flip")]
    [SerializeField] private float flipDuration = 0.3f;

    public bool isFlipped = false;

    private void Awake() {
        foreach (Transform t in randomVisualHolder) {
            t.gameObject.SetActive(false);
        }
        randomVisualHolder.GetChild(Random.Range(0, randomVisualHolder.childCount)).gameObject.SetActive(true);
    }

    public void FlipTile() {
        isFlipped = !isFlipped;

        Vector3 rotation = new Vector3(0, 0, gameObject.transform.localEulerAngles.x - 180);
        visual.DOKill(true);
        visual.DOLocalRotate(rotation, flipDuration, RotateMode.LocalAxisAdd).SetEase(Ease.OutBack);
    }

    public void Hover() {
        transform.position += new Vector3(0, 0.1f, 0);
    }

    public void UnHover() {
        transform.position += new Vector3(0, -0.1f, 0);
    }
}
