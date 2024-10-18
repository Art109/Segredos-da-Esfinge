using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Data", menuName ="SpriteStorage")]
public class DataStorage : ScriptableObject
{
  [SerializeField]Sprite[] sprites;

  public Sprite[] Sprites{get{return sprites;}}
}
