using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.Networking;
using SimpleJSON;

public class TranslationAPI : MonoBehaviour
{
    public static TranslationAPI instance;
    public Canvas canv;
    public GameObject textPrefab;

    void Start()
    {
        instance = this;
    }

    public void TranslateText(string sourceLanguage, string targetLanguage, string sourceText, Action<bool, string> callback)
    {
        StartCoroutine(TranslateTextRoutine(sourceLanguage, targetLanguage, sourceText, callback));
    }
    
    private static IEnumerator TranslateTextRoutine(string sourceLanguage, string targetLanguage, string sourceText, Action<bool, string> callback)
    {
        var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={sourceLanguage}&tl={targetLanguage}&dt=t&q={UnityWebRequest.EscapeURL(sourceText)}";

        var webRequest = UnityWebRequest.Get(url);

        yield return webRequest.SendWebRequest();

        if (webRequest.isHttpError || webRequest.isNetworkError)
        {
            Debug.LogError(webRequest.error);
            callback.Invoke(false, string.Empty);

            yield break;
        }

        var parsedTexts = JSONNode.Parse(webRequest.downloadHandler.text);
        var translatedText = parsedTexts[0][0][0];

        callback.Invoke(true, translatedText);
    }

    [Obsolete("ShowLabel is deprecated, as it relies on 2D labels instead of 3D labels.")]
    public void ShowLabel(string label, Vector2 center)
    {
        TranslateText("en", "de", label, (success, translatedText) =>
        {
            if (success)
            {
                Debug.Log(translatedText);

                GameObject newText = Instantiate(textPrefab, new Vector2(0, 0), Quaternion.identity);

                //TODO : optimize the code
                newText.transform.SetParent(canv.gameObject.transform);
                newText.transform.localScale = new Vector3(
                newText.transform.localScale.x * canv.gameObject.transform.localScale.x,
                newText.transform.localScale.y * canv.gameObject.transform.localScale.y,
                newText.transform.localScale.z * canv.gameObject.transform.localScale.z);

                newText.transform.position = center;
                newText.GetComponent<TMP_Text>().text = translatedText;
            }
        });
    }
}