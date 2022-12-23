using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShot : MonoBehaviour
{
    public Hashtable pictures = new Hashtable();
    public Image dictionaryImage;

    public void Capture(string word)
    {
        string tempName = word;
        ScreenCapture.CaptureScreenshot(Application.dataPath + "/Resources/" + tempName + ".png");
        Debug.Log("Captured");
        pictures.Add(word, tempName + ".png");
    }

    public void RemoveImage(string word)
    {
        if (pictures.ContainsKey(word))
        {
            pictures.Remove(word);
        }
    }

    public string PathOfImage(string word)
    {
        return Application.dataPath+"/Resources/"+word+".png";   
    }

    public void AssignToDictionary(string word)
    {
        dictionaryImage.sprite = Resources.Load<Sprite>("name1");
    }
}
