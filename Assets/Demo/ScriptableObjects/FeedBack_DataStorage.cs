using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "FeedBackStorage")]
public class FeedBack_DataStorage : ScriptableObject
{
    [SerializeField]
    private List<Sprite> sprites;

    private Dictionary<string, Sprite> spriteDictionary;

    private void OnEnable()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        spriteDictionary = new Dictionary<string, Sprite>();

        foreach (Sprite sprite in sprites)
        {
            if (!spriteDictionary.ContainsKey(sprite.name))
            {
                spriteDictionary.Add(sprite.name, sprite);
            }
        }
    }

    public Sprite GetFeedBackByName(string spriteName)
    {
        if (spriteDictionary.TryGetValue(spriteName, out Sprite sprite))
        {
            return sprite;
        }
        else
        {
            Debug.LogWarning($"Sprite '{spriteName}' not found in Feedback Storage.");
            return null;
        }
    }
}
