using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class provides a dictionary to add and retrieve vocabularies of type "Word".
/// </summary>
public class VocabularyDB : MonoBehaviour
{
    public static VocabularyDB instance;

    public enum LanguageMode // maybe add more
    {
        german,
        chinese,
        japanese,
        spanish,
        french
    }

    public static LanguageMode activeLanguageMode = LanguageMode.german;
    public Dictionary<string, Word> vocabulary = new Dictionary<string, Word>();

    void Start()
    {
        // TODO: Load all words from last session into vocabulary

        instance = this;

        // TODO: Delete the following lines later (example words)
        Word newWord = new Word("Example", "Beispiel", "_", "_", "_", "_", null, true);
        Word newWord1 = new Word("Loudspeaker", "Lautsprecher", "_", "_", "_", "_", null, true);
        Word newWord2 = new Word("Fridge", "Kuehlschrank", "_", "_", "_", "_", null, true);
        Word newWord3 = new Word("Dormitory", "Wohnheim", "_", "_", "_", "_", null, true);
        Word newWord4 = new Word("Wallet", "Geldbeutel", "_", "_", "_", "_", null, true);
        vocabulary.Add(newWord.english, newWord);
        vocabulary.Add(newWord1.english, newWord1);
        vocabulary.Add(newWord2.english, newWord2);
        vocabulary.Add(newWord3.english, newWord3);
        vocabulary.Add(newWord4.english, newWord4);

        Invoke("LoadStoredWords", 2.0f);
    }

    void LoadStoredWords()
    {
        for (int i = 0; i < SetGetWordDetails.instance.allSavedWords.Length; i++)
        {
            Word tempWord = SetGetWordDetails.instance.ReturnWordDetails(SetGetWordDetails.instance.allSavedWords[i]);
            vocabulary.Add(tempWord.english, tempWord);
        }
    }

    public void AddNewWordToVocabularyDB(string newWord) 
    {
        Word word = CreateNewWordWithTranslations(newWord); // Create the word with translations
        //word.screenshot = ScreenShot.instance.Capture(newWord); // Save screenshot for word
        StartCoroutine(ScreenShot.instance.CaptureAndAssign(word));
        SetGetWordDetails.instance.SaveWordDetails(newWord, word.german, word.chinese, word.japanese, word.spanish, word.french, false); // Save word details
        vocabulary.Add(word.english, word); // Add new word to DB dictionary
    }

    private Word CreateNewWordWithTranslations(string label)
    {
        Word newWord = new Word(label, "_", "_", "_", "_", "_", null, true);

        TranslationAPI.instance.TranslateText("en", "de", label, (success, translatedText) =>
        {
            if (success)
            {
                newWord.german = translatedText;
            }
        });
        TranslationAPI.instance.TranslateText("en", "zh-CN", label, (success, translatedText) =>
        {
            if (success)
            {
                newWord.chinese = translatedText;
            }
        });
        TranslationAPI.instance.TranslateText("en", "ja", label, (success, translatedText) =>
        {
            if (success)
            {
                newWord.japanese = translatedText;
            }
        });
        TranslationAPI.instance.TranslateText("en", "es", label, (success, translatedText) =>
        {
            if (success)
            {
                newWord.spanish = translatedText;
            }
        });
        TranslationAPI.instance.TranslateText("en", "fr", label, (success, translatedText) =>
        {
            if (success)
            {
                newWord.french = translatedText;
            }
        });

        newWord.learned = false;
        newWord.screenshot = null;

        return newWord;
    }

    public void PrintVocabulary()
    {
        string output = "";

        foreach (var item in vocabulary)
        {
            output += item.Value.english + ", ";
        }

        Debug.Log(output);
    }
}
