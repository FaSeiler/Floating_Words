using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShot : MonoBehaviour
{
    public static ScreenShot instance;
    public Image dictionaryImage;

    private void Start()
    {
        instance = this;
    }

    public void Capture(string word)
    {
        string tempName = word;
        ScreenCapture.CaptureScreenshot(Application.dataPath + "/Resources/" + tempName + ".png");
        Debug.Log("Captured");
    }

    public string PathOfImage(string word)
    {
        return Application.dataPath+"/Resources/"+word+".png";   
    }

    public void AssignToDictionary(string word)
    {
        dictionaryImage.sprite = Resources.Load<Sprite>(word);
    }
}
