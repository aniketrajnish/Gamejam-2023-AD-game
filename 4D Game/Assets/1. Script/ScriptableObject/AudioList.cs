using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Audio/AudioList", order = 2)]
public class AudioDatabase : ScriptableObject
{
    public Music[] MusicList;
    public Sound[] SoundList;
}


