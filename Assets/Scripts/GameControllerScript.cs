using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameControllerScript : MonoBehaviour
{
    private int PlayerInput;
    private float timeToMakeMove = 3;
    private float timeBetweenChallenges = 1;

    private float fastestTime = 1.45f;
    private float timeDecreaseInt = .3f;

    private int ChallengesComplete = 0;
    private float TimeIncreaseInterval = 3;

    private int Challenge;
    public GameObject[] ChallengeCanvases = new GameObject[5];
    public GameObject PassCanvas;
    public GameObject FailCanvas;

    public Slider TimerSlider;
    public GameObject TimerPanel;

    public GameObject StartTimerObject;
    public Text StartTimer;

    private bool MakeMove = false;
    private bool Pass = false;

    private bool Failed = false;

    private GameObject LastPanelObject;

    private bool PlayVocals;
    private bool ToxicVocals;

    private bool NotMoving = false;

    private void Awake()
    {
        Application.targetFrameRate = 30;
    }
    void Start()
    {
        print("TimesPlayed: " + MenuSceneScript.TimesPlayed);
        PlayVocals = MenuSceneScript.VocalSounds;
        ToxicVocals = MenuSceneScript.ToxicSounds;

        StartTimerObject.SetActive(true);
        PassCanvas.SetActive(false);
        FailCanvas.SetActive(false);

        for (int i = 0; i < ChallengeCanvases.Length; i++)
        {
            ChallengeCanvases[i].SetActive(false);
        }

        StartCoroutine(RunActions(0));
    }

    void Update()
    {
        if (MakeMove)                        //While directions are up, this will run
        {
            PlayerInput = InputManager.PlayerInput;

            if (Challenge != 0 && Challenge != 5)    //If the challenge is not to stay still or to shake
            {
                if (PlayerInput == Challenge)                       //Then Check if Player made the right move
                {
                    Pass = true;
                    MakeMove = false;

                    StartCoroutine(ChangePanel(LastPanelObject, PassCanvas));          //If they complete task before time is up, run pass canvas now. We dont do this with hold still because they need to be still until the time runs out
                    PlayerInput = 6;                                           //Reset Player Input
                }
                else if (PlayerInput != Challenge)       //If they did not make the right move
                {
                    if (PlayerInput != 0 && Challenge != 5)       //And the move they made was not just staying still then they fail. This makes it so they can stay still for a lil then make the move. They also dont fail if they shook the phone while turning
                    {
                        MakeMove = false;
                        OnFail();
                    }
                }
            }
            else if (Challenge == 0)                            //If the challenge is to stay still
            {
                if (PlayerInput != 0)               //If they do anything else but stay still
                {
                    MakeMove = false;
                    OnFail();
                }
                else
                {
                    NotMoving = true;          //If they dont move this will be = to true, if it is still true by the time runs out they pass
                }
            }
            else if(Challenge == 5)                                 //if the challenge is to shake
            {
                if (PlayerInput == Challenge)                       //Then Check if Player made the right move
                {
                    Pass = true;
                    MakeMove = false;

                    StartCoroutine(ChangePanel(LastPanelObject, PassCanvas));          //If they complete task before time is up, run pass canvas now. We dont do this with hold still because they need to be still until the time runs out
                    PlayerInput = 6;                                           //Reset Player Input
                }
                else if (PlayerInput != Challenge)       //If they did not make the right move
                {
                    if (PlayerInput != 0 && PlayerInput != 1 && PlayerInput != 2 && PlayerInput != 3)       //And the move they made was not just staying still then they fail. This makes it so they can stay still for a lil then make the move. It also doesnt fail them if the rotate the phone left, right or forward
                    {
                        MakeMove = false;
                        OnFail();
                    }
                }
            }
        }
    }

    private IEnumerator RunActions(float waitTime)
    {
        StartTimer.text = "3";
        if (PlayVocals)
            AudioManager.instance.Play("3");
        yield return new WaitForSeconds(1);
        if (PlayVocals)
            AudioManager.instance.Play("2");
        StartTimer.text = "2";
        yield return new WaitForSeconds(1);
        if (PlayVocals)
            AudioManager.instance.Play("1");
        StartTimer.text = "1";
        yield return new WaitForSeconds(1);
        if (PlayVocals)
            AudioManager.instance.Play("Go");

        LastPanelObject = StartTimerObject;
        while (!Failed)
        {
            Challenge = Random.Range(0, 6);
            StartCoroutine(ChangePanel(LastPanelObject, ChallengeCanvases[Challenge]));
            ChallengeCanvases[Challenge].transform.GetChild(0).GetComponent<Animator>().ResetTrigger("ImageComeIn");
            ChallengeCanvases[Challenge].transform.GetChild(0).GetComponent<Animator>().SetTrigger("ImageComeIn");

            StartCoroutine(Timer());
            StartCoroutine(PlaySound(Challenge));
            MakeMove = true;                                           //Allows if statement in update run to check if right task was complete in the time
            yield return new WaitForSeconds(timeToMakeMove);
            MakeMove = false;

            if (Challenge == 0 && NotMoving)                    //of the challenge is not to move and not moving is true by the time is up, then they pass and reset NotMoving
            {
                Pass = true;
                NotMoving = false;
            }

            if (Pass && !Failed)
            {
                if (Challenge == 0)           //If challenge is hold still, update function wont activate passcanvas, so we do it here.
                {
                    StartCoroutine(ChangePanel(LastPanelObject, PassCanvas));
                }
                ChallengesComplete += 1;

                if (ChallengesComplete == TimeIncreaseInterval)   //Every three moves player gets less time to make move, and less time inbetween moves.
                {
                    if (timeToMakeMove >= fastestTime)
                    {
                        timeToMakeMove -= timeDecreaseInt;
                    }
                    else
                    {
                        timeToMakeMove = fastestTime;
                    }

                    TimeIncreaseInterval += 3;
                }
            }
            else if (!Failed)
            {
                OnFail();
                break;
            }
            else if (Failed)
            {
                break;
            }

            Pass = false;                                      //Reset Pass bool

            yield return new WaitForSeconds(timeBetweenChallenges);
        }
    }

    private IEnumerator Timer()
    {
        float SliderValue = 1;
        float startTime = Time.time;
        while (SliderValue > 0)
        {
            float t = (Time.time - startTime) / timeToMakeMove;
            SliderValue = Mathf.SmoothStep(1, 0, t);
            TimerSlider.value = SliderValue;
            yield return new WaitForSeconds(0);
        }
    }

    private IEnumerator PlaySound(int Challenge)
    {
        yield return new WaitForSeconds(0.25f);
        if (ToxicVocals && PlayVocals)
        {
            if (Challenge == 0)
            {
                int R = Random.Range(0, 2);
                if (R == 0)
                {
                    AudioManager.instance.Play("HoldStillT");
                }
                else
                {
                    AudioManager.instance.Play("HoldStillT2");
                }
            }
            else if (Challenge == 1)
            {
                AudioManager.instance.Play("TurnLeftT");
            }
            else if (Challenge == 2)
            {
                AudioManager.instance.Play("TurnRightT");
            }
            else if (Challenge == 3)
            {
                AudioManager.instance.Play("TurnForwardT");
            }
            else if (Challenge == 4)
            {
                int R = Random.Range(0, 2);
                if (R == 0)
                {
                    AudioManager.instance.Play("TapT");
                }
                else
                {
                    AudioManager.instance.Play("TapT2");
                }
            }
            else if (Challenge == 5)
            {
                AudioManager.instance.Play("ShakeT");
            }
        }
        else if(PlayVocals)
        {
            if (Challenge == 0)
            {
                AudioManager.instance.Play("HoldStill");
            }
            else if (Challenge == 1)
            {
                AudioManager.instance.Play("TurnLeft");
            }
            else if (Challenge == 2)
            {
                AudioManager.instance.Play("TurnRight");
            }
            else if (Challenge == 3)
            {
                AudioManager.instance.Play("TurnForward");
            }
            else if (Challenge == 4)
            {
                AudioManager.instance.Play("Tap");
            }
            else if (Challenge == 5)
            {
                AudioManager.instance.Play("Shake");
            }
        }
    }
    private void OnFail()
    {
        MenuSceneScript.LastScore = ChallengesComplete;

        if (ChallengesComplete > MenuSceneScript.HighestScore)
        {
            if (ToxicVocals && PlayVocals)
            {
                AudioManager.instance.Play("HighScoreT");
            }
            else if(PlayVocals)
            {
                AudioManager.instance.Play("HighScore");
            }
            MenuSceneScript.HighestScore = ChallengesComplete;
        }

        StartCoroutine(ChangePanel(LastPanelObject, FailCanvas));
        Failed = true;
    }

    private IEnumerator ChangePanel(GameObject LastPanel, GameObject NewPanel)
    {
        if (NewPanel.tag == "ChallengeCanvas" && PlayVocals)
        {
            AudioManager.instance.Play("NextSlide");
        }
        else if (NewPanel.tag == "FailCanvas")
        {
            if (ToxicVocals && PlayVocals)
            {
                int R = Random.Range(0, 13);

                if (R == 0)
                {
                    AudioManager.instance.Play("FailedT");
                }
                else if (R == 1)
                {
                    AudioManager.instance.Play("FailedT1");
                }
                else if (R == 2)
                {
                    AudioManager.instance.Play("FailedT2");
                }
                else if (R == 3)
                {
                    AudioManager.instance.Play("FailedT3");
                }
                else if (R == 4)
                {
                    AudioManager.instance.Play("FailedT4");
                }
                else if (R == 5)
                {
                    AudioManager.instance.Play("FailedT5");
                }
                else if (R == 6)
                {
                    AudioManager.instance.Play("FailedT6");
                }
                else if (R == 7)
                {
                    AudioManager.instance.Play("FailedT7");
                }
                else if (R == 8)
                {
                    AudioManager.instance.Play("FailedT8");
                }
                else if (R == 9)
                {
                    AudioManager.instance.Play("FailedT9");
                }
                else if (R == 10)
                {
                    AudioManager.instance.Play("FailedT10");
                }
                else if (R == 11)
                {
                    AudioManager.instance.Play("FailedT11");
                }
                else
                {
                    AudioManager.instance.Play("FailedT12");
                }
            }
            else if (PlayVocals)
            {
                int S = Random.Range(0, 3);
                if (S == 0)
                {
                    AudioManager.instance.Play("Failed");
                }
                else if (S == 1)
                {
                    AudioManager.instance.Play("Failed2");
                }
                else
                {
                    AudioManager.instance.Play("Failed3");
                }
            }
            else
            {
                AudioManager.instance.Play("WrongSound");
            }
        }
        else if (NewPanel.tag == "PassCanvas")
        {
            if (ToxicVocals && PlayVocals)
            {
                int R = Random.Range(0, 7);

                if (R == 0)
                {
                    AudioManager.instance.Play("PassT1");
                }
                else if (R == 1)
                {
                    AudioManager.instance.Play("PassT2");
                }
                else if (R == 2)
                {
                    AudioManager.instance.Play("PassT3");
                }
                else if (R == 3)
                {
                    AudioManager.instance.Play("PassT4");
                }
                else if (R == 4)
                {
                    AudioManager.instance.Play("PassT5");
                }
                else if (R == 5)
                {
                    AudioManager.instance.Play("PassT6");
                }
                else
                {
                    AudioManager.instance.Play("PassT7");
                }
            }
            else if(PlayVocals)
            {
                int S = Random.Range(0, 3);
                if (S == 0)
                {
                    AudioManager.instance.Play("Pass");
                }
                else if (S == 1)
                {
                    AudioManager.instance.Play("Pass1");
                }
                else
                {
                    AudioManager.instance.Play("Pass2");
                }
            }
        }

        NewPanel.SetActive(false);

        RectTransform LastPanelRect = LastPanel.transform.GetComponent<RectTransform>();
        RectTransform NewPanelRect = NewPanel.transform.GetComponent<RectTransform>();

        NewPanelRect.DOAnchorPos(new Vector2(1000, 0), 0);
        if (NewPanel.tag != "ChallengeCanvas")
        {
            NewPanel.transform.SetSiblingIndex(10);
        }
        else
        {
            NewPanel.transform.SetSiblingIndex(10);
            TimerPanel.transform.SetSiblingIndex(11);
        }
        NewPanel.SetActive(true);
        NewPanelRect.DOAnchorPos(Vector2.zero, .25f);

        float Avalue = 0;
        float startTime = Time.time;
        Image CurPanelImage = NewPanel.GetComponent<Image>();

        while (Avalue <= 1)
        {
            float t = (Time.time - startTime) / .25f;
            Avalue = Mathf.SmoothStep(0, 1, t);
            CurPanelImage.color = new Color(CurPanelImage.color.r, CurPanelImage.color.g, CurPanelImage.color.b, Avalue);
            yield return null;
        }

        yield return new WaitForSeconds(.25f);
        print("TurnToFalse");
        LastPanel.SetActive(false);
        LastPanelObject = NewPanel;
    }

    public void FailedCanvasNextButton()
    {
        AudioManager.instance.Play("Click");
        SceneManager.LoadScene("BetweenGameScene");
    }
}
