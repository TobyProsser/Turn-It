using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsSceneController : MonoBehaviour
{
    private int VocalSound = 1;
    private int MusicSound = 1;
    private int ToxicSound = 0;

    public Sprite OnButton;
    public Sprite OffButton;

    public GameObject VocalB;
    public GameObject MusicB;
    public GameObject ToxicB;

    public SaveDataScript SaveData;

    private void Start()
    {
        if (MenuSceneScript.VocalSounds)
        {
            VocalB.GetComponent<Image>().sprite = OnButton;
            VocalSound = 1;
        }
        else
        {
            VocalB.GetComponent<Image>().sprite = OffButton;
            VocalSound = 0;
        }

        if (MenuSceneScript.MusicSounds)
        {
            MusicB.GetComponent<Image>().sprite = OnButton;
            MusicSound = 1;
        }
        else
        {
            MusicB.GetComponent<Image>().sprite = OffButton;
            MusicSound = 0;
        }

        if (MenuSceneScript.ToxicSounds)
        {
            ToxicB.GetComponent<Image>().sprite = OnButton;
            ToxicSound = 1;
        }
        else
        {
            ToxicB.GetComponent<Image>().sprite = OffButton;
            ToxicSound = 0;
        }
    }
    public void VocalS()
    {
        AudioManager.instance.Play("Click");
        if (VocalSound == 0)
        {
            VocalSound = 1;
            VocalB.GetComponent<Image>().sprite = OnButton;
            MenuSceneScript.VocalSounds = true;
        }
        else
        {
            VocalSound = 0;
            VocalB.GetComponent<Image>().sprite = OffButton;
            MenuSceneScript.VocalSounds = false;
        }
    }

    public void MusicS()
    {
        AudioManager.instance.Play("Click");
        if (MusicSound == 0)
        {
            MusicSound = 1;
            MusicB.GetComponent<Image>().sprite = OnButton;
            MenuSceneScript.MusicSounds = true;
        }
        else
        {
            MusicSound = 0;
            MusicB.GetComponent<Image>().sprite = OffButton;
            MenuSceneScript.MusicSounds = false;
            AudioManager.instance.Stop("Music");
        }
    }

    public void ToxisS()
    {
        AudioManager.instance.Play("Click");
        if (ToxicSound == 0)
        {
            ToxicSound = 1;
            ToxicB.GetComponent<Image>().sprite = OnButton;
            MenuSceneScript.ToxicSounds = true;

        }
        else
        {
            ToxicSound = 0;
            ToxicB.GetComponent<Image>().sprite = OffButton;
            MenuSceneScript.ToxicSounds = false;
        }
    }

    public void MainMenu()
    {
        Save();
        AudioManager.instance.Play("Click");
        SceneManager.LoadScene("MenuScene");
    }

    private void Save()
    {
        SaveData = GameObject.Find("SaveObject").GetComponent<SaveDataScript>();

        SaveData.HighestScore1 = MenuSceneScript.HighestScore;
        SaveData.LastScore1 = MenuSceneScript.LastScore;

        SaveData.TimesPlayed1 = MenuSceneScript.TimesPlayed;

        SaveData.VocalSound1 = MenuSceneScript.VocalSounds;
        SaveData.MusicSound1 = MenuSceneScript.MusicSounds;
        SaveData.ToxicSound1 = MenuSceneScript.ToxicSounds;

        SaveSystem.SavePlayer(SaveData);
    }
}
