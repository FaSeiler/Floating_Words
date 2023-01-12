using UnityEngine;

public class ScreenShot : MonoBehaviour
{
    public static ScreenShot instance;

    private void Start()
    {
        instance = this;
    }

    /// <summary>
    /// Captures screenshot and saves it in the "Resources" folder under the name of the input string.
    /// </summary>
    /// <param name="word"></param>
    /// <returns>The screenshot as a sprite.</returns>
    public Sprite Capture(string word)
    {
        ScreenCapture.CaptureScreenshot(Application.dataPath + "/Resources/" + word + ".png");
        Debug.Log("Captured");
        return LoadScreenshot(word);
    }

    /// <summary>
    /// Captures screenshot and saves it in the "Resources" folder unter the name of the input word.
    /// </summary>
    /// <param name="word"></param>
    /// <returns></returns>
    public Sprite Capture(Word word)
    {
        ScreenCapture.CaptureScreenshot(Application.dataPath + "/Resources/" + word.english + ".png");
        Debug.Log("Captured");
        return LoadScreenshot(word);
    }

    /// <summary>
    /// Loads screenshot from "Resources" folder with the given string as key.
    /// </summary>
    /// <param name="word"></param>
    /// <returns>The loaded screenshot as a sprite.</returns>
    public Sprite LoadScreenshot(string word)
    {
        return Resources.Load<Sprite>(word);
    }

    /// <summary>
    /// Loads screenshot from "Resources" folder with the given string as key.
    /// </summary>
    /// <param name="word"></param>
    /// <returns>The loaded screenshot as a sprite.</returns>
    public Sprite LoadScreenshot(Word word)
    {
        return Resources.Load<Sprite>(word.english);
    }

    /// <summary>
    /// Returns the path of the saved screenshot.
    /// </summary>
    /// <param name="word"></param>
    /// <returns>The screenshot path.</returns>
    public string PathOfScreenshot(string word)
    {
        return Application.dataPath + "/Resources/" + word + ".png";
    }
}
