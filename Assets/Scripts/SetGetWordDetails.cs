using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGetWordDetails : MonoBehaviour
{
    public static SetGetWordDetails instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        
        //SaveWordDetails("aaaab", "bru", "caa", "d", "e", "f", false);
        //ReturnWordDetails("aaaab");
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
            return newWord;
        }

        else
        {
            return null;
        }
    }
}
