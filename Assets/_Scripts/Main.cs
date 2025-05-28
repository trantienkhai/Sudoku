using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public Button easyButton;
    public Button middleButton;
    public Button hardButton;

    public void OnClickEasy()
    {
        SceneManager.LoadScene("GamePlay");
        GameSetting.EasyMiddleHardManager = 1;
    }

    public void OnClickMiddle()
    {
        SceneManager.LoadScene("GamePlay");
        GameSetting.EasyMiddleHardManager = 2;
    }

    public void OnClickHard()
    {
        SceneManager.LoadScene("GamePlay");
        GameSetting.EasyMiddleHardManager = 3;
    }
}
