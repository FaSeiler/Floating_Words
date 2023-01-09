using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;
using SimpleJSON;

public class ShowInfo : MonoBehaviour
{
    public Canvas canv;
    public GameObject Textprefab;
    public VocabularyDB vocabularyDB;

    public void ShowLabel(string label,Vector2 center)
    {
        TranslateText("en","de", label, (success, translatedText) =>
        {
            if (success)
            {
                Debug.Log(translatedText);

                GameObject newText= Instantiate(Textprefab, new Vector2(0,0), Quaternion.identity);

                //TODO : optimize the code
                newText.transform.SetParent(canv.gameObject.transform);
                newText.transform.localScale = new Vector3(
                newText.transform.localScale.x * canv.gameObject.transform.localScale.x,
                newText.transform.localScale.y * canv.gameObject.transform.localScale.y,
                newText.transform.localScale.z * canv.gameObject.transform.localScale.z);

                newText.transform.position = center;
                //newText.GetComponent<TMP_Text>().text=label;
                newText.GetComponent<TMP_Text>().text=translatedText;
            }
        });
    }
    //private void Start()
    //{
    //    SaveTranslationsToWord("ComputerKeyboard");
    //}

    public Word SaveTranslationsToWord(string label)
    {
        Word newWord = new Word(label, "_", "_", "_", "_", "_", null, true);
        TranslateText("en", "de", label, (success, translatedText) =>
        {
            if (success)
            {
                newWord.german = translatedText;
            }
        });
        TranslateText("en", "zh-CN", label, (success, translatedText) =>
        {
            if (success)
            {
                newWord.chinese = translatedText;
            }
        });
        TranslateText("en", "ja", label, (success, translatedText) =>
        {
            if (success)
            {
                newWord.japanese = translatedText;
            }
        });
        TranslateText("en", "es", label, (success, translatedText) =>
        {
            if (success)
            {
                newWord.spanish = translatedText;
            }
        });
        TranslateText("en", "fr", label, (success, translatedText) =>
        {
            if (success)
            {
                newWord.french = translatedText;
            }
        });
        newWord.learned = false;
        newWord.screenshot = null;
        
        vocabularyDB.vocabulary.Add(newWord.english, newWord);
        return newWord;
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


    public void CleanAll()
    {
        // TODO: Rework labels are now not on canvas but attached to 3D anchors!
        //foreach (Transform child in canv.transform)
        //{
        //    GameObject.Destroy(child.gameObject);
        //}
    }
}