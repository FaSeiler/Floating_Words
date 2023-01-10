using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShot : MonoBehaviour
{
    public static ScreenShot instance;

    private void Start()
    {
        instance = this;
    }

    public Sprite Capture(string word)
    {
        ScreenCapture.CaptureScreenshot(Application.dataPath + "/Resources/" + word + ".png");
        Debug.Log("Captured");
        return LoadScreenshot(word);
    }

    public Sprite Capture(Word word)
    {
        ScreenCapture.CaptureScreenshot(Application.dataPath + "/Resources/" + word.english + ".png");
        Debug.Log("Captured");
        return LoadScreenshot(word);
    }

    public Sprite LoadScreenshot(string word)
    {
        return Resources.Load<Sprite>(word);
    }

    public Sprite LoadScreenshot(Word word)
    {
        return Resources.Load<Sprite>(word.english);
    }

    public string PathOfImage(string word)
    {
        return Application.dataPath + "/Resources/" + word + ".png";
    }
}
