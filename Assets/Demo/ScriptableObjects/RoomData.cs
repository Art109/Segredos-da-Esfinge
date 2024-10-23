using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="RoomData", menuName ="Data")]
public class RoomData : ScriptableObject
{
    [SerializeField] List<GameObject> rooms;

    public List<GameObject> Rooms => rooms;
}
