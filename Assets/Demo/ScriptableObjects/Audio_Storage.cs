using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "AudioStorage")]
public class Audio_Storage : ScriptableObject
{
    [SerializeField]
    private List<AudioClip> audioClips;

    private Dictionary<string, AudioClip> audioClipDictionary;

    private void OnEnable()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        audioClipDictionary = new Dictionary<string, AudioClip>();

        foreach (AudioClip clip in audioClips)
        {
            if (!audioClipDictionary.ContainsKey(clip.name))
            {
                audioClipDictionary.Add(clip.name, clip);
            }
        }
    }

    public AudioClip GetAudioClipByName(string clipName)
    {
        if (audioClipDictionary.TryGetValue(clipName, out AudioClip clip))
        {
            return clip;
        }
        else
        {
            Debug.LogWarning($"AudioClip '{clipName}' not found in Audio Storage.");
            return null;
        }
    }
}
