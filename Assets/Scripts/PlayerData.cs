using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int HighestSaveScore;
    public int LastSaveScore;

    public int TimesPlayed2;

    public bool VocalSound;
    public bool MusicSound;
    public bool ToxicSound;

    public bool FirstTime;

    public PlayerData(SaveDataScript player)
    {
        HighestSaveScore = player.HighestScore1;
        LastSaveScore = player.LastScore1;

        VocalSound = player.VocalSound1;
        MusicSound = player.MusicSound1;
        ToxicSound = player.ToxicSound1;
    }
}
