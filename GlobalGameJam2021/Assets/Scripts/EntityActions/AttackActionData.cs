﻿using UnityEngine;

[CreateAssetMenu(menuName = "Data/ActionData/Attack")]
public class AttackActionData : EntityActionData {

    [Header("Straight")]
    [Min(0)] public int North;
    [Min(0)] public int East;
    [Min(0)] public int South;
    [Min(0)] public int West;

    [Header("Sideways")]
    [Min(0)] public int NorthEast;
    [Min(0)] public int SouthEast;
    [Min(0)] public int SouthWest;
    [Min(0)] public int NorthWest;

}
