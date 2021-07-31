using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BetweenGamesScript : MonoBehaviour
{
    public GameObject MainPanel;
    public Sprite[] Backings = new Sprite[6];

    public Text HighestScoreText;
    public Text LastScoreText;

    public SaveDataScript SaveData;

    private Color32 PlayButtonGreen = new Color32(190, 255, 143, 255);
    private Color32 MenuButtonGreen = new Color32(143,255,190,255);

    private Color32 PlayButtonRed = new Color32(255,165,143,255);
    private Color32 MenuButtonRed = new Color32(255,218,143,255);

    public GameObject PlayAgainB;
    public GameObject MainMenuB;

    private void Awake()
    {
        Save();

        HighestScoreText.text = MenuSceneScript.HighestScore.ToString();
        LastScoreText.text = MenuSceneScript.LastScore.ToString();
    }
    void Start()
    {
        int RandomBacking = Random.Range(0, Backings.Length - 1);
        MainPanel.GetComponent<Image>().sprite = Backings[RandomBacking];

        if (RandomBacking < 3)
        {
            PlayAgainB.GetComponent<Image>().color = PlayButtonGreen;
            MainMenuB.GetComponent<Image>().color = MenuButtonGreen;
        }
        else
        {
            PlayAgainB.GetComponent<Image>().color = PlayButtonRed;
            MainMenuB.GetComponent<Image>().color = MenuButtonRed;
        }

        MenuSceneScript.TimesPlayed += 1;
        if (MenuSceneScript.TimesPlayed >= 5)
        {
            AdController.AdInstance.ShowAd("video");
            MenuSceneScript.TimesPlayed = 0;
        }
    }

    public void PlayAgain()
    {
        AudioManager.instance.Play("Click");
        SceneManager.LoadScene("GameScene");
    }

    public void MenuScene()
    {
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
