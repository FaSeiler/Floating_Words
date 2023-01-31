using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores the additional information of a specific word persistently 
/// in the PlayerPrefs.
/// </summary>
public class SetGetWordDetails : MonoBehaviour
{
    public static SetGetWordDetails instance;

    public string[] allSavedWords;

    void Start()
    {
        instance = this;

        //testing
        //SaveWordDetails("aaaabooo", "bru", "caa", "d", "e", "f", false);
        //SaveWordDetails("aaaabo22", "bru", "caa", "d", "e", "f", false);
        //SaveWordDetails("aaaab2", "bru", "caa", "d", "e", "f", false);

        StoreAllWordsInArray();
        //Debug.Log("Count-" + allSavedWords.Length);
        //for (int i = 0; i < allSavedWords.Length; i++)
        //{
        //    Debug.Log(allSavedWords[i]);
        //}
    }

    public void SaveWordDetails(string english, string german, string chinese, string japanese, string spanish, string french, bool learned)
    {
        int temp = 0;
        if (learned)
        {
            Debug.Log("tru world");
            temp = 1;
        }
        if (!PlayerPrefs.HasKey(english))
        {
            PlayerPrefs.SetString(english, english);
            PlayerPrefs.SetString(english + "_german", german);
            PlayerPrefs.SetString(english + "_chinese", chinese);
            PlayerPrefs.SetString(english + "_spanish", spanish);
            PlayerPrefs.SetString(english + "japanese", japanese);
            PlayerPrefs.SetString(english + "_french", french);
            PlayerPrefs.SetInt(english + "_learned", temp);

            if (!PlayerPrefs.HasKey("allwords"))
            {
                PlayerPrefs.SetString("allwords", english);
            }
            else
            {
                string allwords = PlayerPrefs.GetString("allwords");
                string final_list = allwords + "|" + english;
                Debug.Log("concat-" + final_list);
                PlayerPrefs.SetString("allwords", final_list);
                PlayerPrefs.Save();
            }
        }
    }

    public Word ReturnWordDetails(string word)
    {
        if (PlayerPrefs.HasKey(word))
        {
            string english = PlayerPrefs.GetString(word);
            string german = PlayerPrefs.GetString(word + "_german");
            string chinese = PlayerPrefs.GetString(word + "_chinese");
            string japanese = PlayerPrefs.GetString(word + "_japanese");
            string spanish = PlayerPrefs.GetString(word + "_spanish");
            string french = PlayerPrefs.GetString(word + "_french");
            bool temp = false;
            if (PlayerPrefs.GetInt(word + "_learned") == 1)
            {
                temp = true;
            }
            Debug.Log(english + german + chinese + temp);
            Word newWord = new Word(english, german, chinese, japanese, spanish, french, null, temp);
            ObjectImages.instance.AssignImage(newWord);
            return newWord;
        }

        else
        {
            return null;
        }
    }

    public void StoreAllWordsInArray()
    {
        Debug.Log("pref=" + PlayerPrefs.GetString("allwords"));
        if (PlayerPrefs.GetString("allwords") !="")
        {
            allSavedWords = PlayerPrefs.GetString("allwords").Split('|');
        }
        
    }
}
