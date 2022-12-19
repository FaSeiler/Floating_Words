using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// This UI component displays detailed information about a vocab.
/// This inspector is opened by clicking on a vocab entry in the dictionary UI
/// </summary>
public class VocabInspectorUI : MonoBehaviour
{
    public GameObject childGO;

    [Space(10)]
    public Word activeWord;
    public TextMeshProUGUI word_en_text;
    public TextMeshProUGUI word_translation_text;
    public TextMeshProUGUI word_definition_text;
    public Image word_screenshot;


    public void OpenVocabInspector(Word word)
    {
        childGO.SetActive(true);
        activeWord = word;
        UpdateVocabInspectorUI(activeWord);
    }

    public void CloseVocabInspector()
    {
        childGO.SetActive(false);
    }

    void UpdateVocabInspectorUI(Word newWord)
    {
        word_en_text.text = newWord.english;

        switch (VocabularyDB.activeLanguageMode)
        {
            case VocabularyDB.LanguageMode.english:
                word_translation_text.text = newWord.english;
                break;
            case VocabularyDB.LanguageMode.german:
                word_translation_text.text = newWord.german;
                break;
            case VocabularyDB.LanguageMode.chinese:
                word_translation_text.text = newWord.chinese;
                break;
            case VocabularyDB.LanguageMode.japanese:
                word_translation_text.text = newWord.japanese;
                break;
            case VocabularyDB.LanguageMode.spanish:
                word_translation_text.text = newWord.spanish;
                break;
            case VocabularyDB.LanguageMode.french:
                word_translation_text.text = newWord.french;
                break;
            default:
                break;
        }

        word_definition_text.text = newWord.wordInfo.definition;
        word_screenshot.sprite = newWord.screenshot;
    }

}
