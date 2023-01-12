using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.Net;
using System.IO;

/// <summary>
/// Fetches additional information to a given english word from the 
/// open source dictionary API "https://dictionaryapi.dev/" and
/// returns it in a WordInfo struct.
/// </summary>
public class FreeDictionaryAPI
{
    // URL of the dictionary API
    private const string API_URL = "https://api.dictionaryapi.dev/api/v2/entries/en/";

    public struct WordInfo
    {
        public string word_en; // The english word
        public string audioURL; // Link to a audio recording of the word
        public string partOfSpeech; // "noun", "verb", "adjective", ...
        public string definition; // Definition of the word
        public string example; // An example sentence using the word
    }

    /// <summary>
    /// Fetches additional information of an english word from the internet.
    /// </summary>
    /// <param name="word"></param>
    /// <returns>WordInfo struct</returns>
    public static WordInfo GetWordInfo(string word)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_URL + word);
        request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        try
        {
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
                wordInfo.example = parsedText[0]["meanings"][0]["definitions"][0]["example"];

                // Since we are only detecting real word objects all vocabs will be nouns
                // therefore discard all definitions that are not "noun"
                for (int i = 0; i < parsedText[0]["meanings"].Count; i++)
                {
                    var tmp = parsedText[0]["meanings"][i]["partOfSpeech"].ToString();
                    if (tmp == "\"noun\"")
                    {
                        wordInfo.partOfSpeech = parsedText[0]["meanings"][i]["partOfSpeech"];
                        wordInfo.definition = parsedText[0]["meanings"][i]["definitions"][0]["definition"];
                        break;
                    }
                }

                return wordInfo;
            }
        }
        catch (WebException)
        {
            Debug.Log("404 Not found");
            WordInfo nullResult = new WordInfo();
            return nullResult;
        }
    }
}
