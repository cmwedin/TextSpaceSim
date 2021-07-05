using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerSO", menuName = "TextSpaceSim/Characters/New Player", order = 0)]
public class PlayerSO : CharacterSO {
    private string _savedStoryJson;
    public string SavedStoryJson { get => _savedStoryJson; }

    //! almost certainly needs input validation
    public void SaveStory(string storyJson) {
        _savedStoryJson = storyJson;
    }
}