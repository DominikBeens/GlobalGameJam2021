using UnityEngine;

public class StaticEntity : Entity {

    private void Awake() {
        int rotation = Random.Range(0, 4) * 90;
        visual.transform.localEulerAngles = new Vector3(0, rotation, 0);
    }
}
