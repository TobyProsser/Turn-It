using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class MenuSceneScript : MonoBehaviour
{
    public GameObject MainPanel;
    public Sprite[] Backings = new Sprite[6];

    public static int HighestScore;
    public static int LastScore;

    public static int TimesPlayed;

    public static bool ToxicSounds = false;
    public static bool VocalSounds = true;
    public static bool MusicSounds = true;

    public GameObject Button1, Button2, Button3, Button4;
    private Color32 GreenColor = new Color32(205, 255, 170, 255);
    private Color32 RedColor = new Color32(255, 211, 143, 255);

    // Start is called before the first frame update
    void Start()
    {
        LoadData();
        print("Toxic: " + ToxicSounds);
        int RandomBacking = Random.Range(0, Backings.Length - 1);
        MainPanel.GetComponent<Image>().sprite = Backings[RandomBacking];
        if (RandomBacking <= 2)
        {
            Button1.GetComponent<Image>().color = GreenColor;
            Button2.GetComponent<Image>().color = GreenColor;
            Button3.GetComponent<Image>().color = GreenColor;
            Button4.GetComponent<Image>().color = GreenColor;
        }
        else
        {
            Button1.GetComponent<Image>().color = RedColor;
            Button2.GetComponent<Image>().color = RedColor;
            Button3.GetComponent<Image>().color = RedColor;
            Button4.GetComponent<Image>().color = RedColor;
        }

        if (MusicSounds)
        {
            AudioManager.instance.Play("Music");
        }
    }

    public void play()
    {
        AudioManager.instance.Play("Click");
        SceneManager.LoadScene("GameScene");
    }

    public void Exit()
    {
        AudioManager.instance.Play("Click");
        Application.Quit();
    }

    public void Options()
    {
        AudioManager.instance.Play("Click");
        SceneManager.LoadScene("OptionsScene");
    }

    private void LoadData()
    {
        string path = Application.persistentDataPath + "/player.scores";
        if (File.Exists(path))
        {
            PlayerData data = SaveSystem.LoadPlayer();

            if (data != null)
            {
                HighestScore = data.HighestSaveScore;
                LastScore = data.LastSaveScore;

                TimesPlayed = data.TimesPlayed2;
                ToxicSounds = data.ToxicSound;
                VocalSounds = data.VocalSound;
                MusicSounds = data.MusicSound;
            }
            else
            {
                Debug.Log("No Saved Data");
            }
        }
    }

    public void TutScene()
    {
        AudioManager.instance.Play("Click");
        SceneManager.LoadScene("TutorialScene");
    }
}
