using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShot : MonoBehaviour
{
    public Hashtable pictures = new Hashtable();
    // Start is called before the first frame update
    void Start()
    {
        Capture("name1");
        PathOfImage("name1");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Capture(string word)
    {
        string tempName = System.DateTime.Now.ToString().Replace("-", "_").Replace(":", "_");
        ScreenCapture.CaptureScreenshot(Application.dataPath + "/" + tempName + ".png");
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
        return Application.dataPath+"/"+pictures[word].ToString();   
    }

    //way to assign image to Image with url
    //public Image img;

    //// The source image
    //public string url = "kkkkkkkkk";
    //IEnumerator Start()
    //{
    //    WWW www = new WWW(url);
    //    yield return www;
    //    img.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
    //}
}
