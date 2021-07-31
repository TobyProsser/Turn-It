using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialSceneManager : MonoBehaviour
{
    public GameObject Canvas1, Canvas2;
    private int TapCount = 0;

    private void Awake()
    {
        Canvas1.SetActive(true);
        Canvas2.SetActive(false);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (TapCount == 0)
            {
                AudioManager.instance.Play("Click");
                Canvas1.SetActive(false);
                Canvas2.SetActive(true);
                TapCount++;
            }
            else
            {
                AudioManager.instance.Play("Click");
                SceneManager.LoadScene("GameScene");
            }
        }
    }
}
