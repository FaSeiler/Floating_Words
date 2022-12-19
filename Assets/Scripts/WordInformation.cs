using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System.Net;
using System.IO;

/// <summary>
/// Fetches additional information to a given english word from the internet
/// and returns it in a WordInfo struct
/// </summary>
public class WordInformation
{
    // URL of the dictionary API
    private const string API_URL = "https://api.dictionaryapi.dev/api/v2/entries/en/";

    public struct WordInfo
    {
        public string word_en; // the word
        public string audioURL; // link to a audio recording of the word
        public string partOfSpeech; // "noun", "verb", "adjective", ...
        public string definition; // definition of the word
        public string example; // an example sentence using the word
    }

    public static WordInfo GetWordInfo(string word)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_URL + word);
        request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        using (Stream stream = response.GetResponseStream())
        using (StreamReader reader = new StreamReader(stream))
        {
            string responseString = reader.ReadToEnd();
            //Get the info from the response
            JSONNode parsedText = JSONNode.Parse(responseString);

            WordInfo wordInfo = new WordInfo();
            wordInfo.word_en = parsedText[0]["word"];
            wordInfo.audioURL = parsedText[0]["phonetics"][0]["audio"];
            wordInfo.partOfSpeech = parsedText[0]["meanings"][0]["partOfSpeech"];
            wordInfo.definition = parsedText[0]["meanings"][0]["definitions"][0]["definition"];
            wordInfo.example = parsedText[0]["meanings"][0]["definitions"][0]["example"];

            return wordInfo;
        }
    }

    //public void SetWordInfo(Word word)
    //{
    //    StartCoroutine(GetWordInfo(word.english, (info) =>
    //    {
    //        word.wordInfo = info;
    //    }));
    //}

    /* Uses UnityWebRequest and thus requires MonoBehavior
    //Function to fetch the definition of a word
    public static IEnumerator GetWordInfo(string word, System.Action<WordInfo> callback)
    {
        // Send request to the API
        using (UnityWebRequest webRequest = UnityWebRequest.Get(API_URL + word))
        {
            yield return webRequest.SendWebRequest();

            // Check if the request was successful
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                // Return an error message if the request was not successful
                Debug.LogError(webRequest.error);
            }
            else
            {
                // Get the info from the response
                JSONNode parsedText = JSONNode.Parse(webRequest.downloadHandler.text);

                WordInfo wordInfo = new WordInfo();
                wordInfo.word_en = parsedText[0]["word"];
                wordInfo.audioURL = parsedText[0]["phonetics"][0]["audio"];
                wordInfo.partOfSpeech = parsedText[0]["meaning"][0]["partOfSpeech"];
                wordInfo.definition = parsedText[0]["meaning"][0]["definitions"][0]["definition"];
                wordInfo.definition = parsedText[0]["meaning"][0]["definitions"][0]["example"];

                callback.Invoke(wordInfo);
            }
        }
    }*/
}
