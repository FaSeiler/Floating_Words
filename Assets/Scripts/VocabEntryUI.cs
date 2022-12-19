using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// One entry in the dictionary UI for every word in the global vocab dictionary.
/// Entry is a button that opens detailed information of the vocab in an inspector window.
/// </summary>
public class VocabEntryUI : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Word word = null;

    // Set vocab properties of this UI entry
    public void InitVocab(Word word)
    {
        this.word = word;
        text.text = word.english;
    }

    // Opens the inspector for this vocab and gives detailed information about it
    public void OpenVocabInspectorForEntry()
    {
        if (word == null)
        {
            Debug.Log("Error opening inspector. Word is not initialized!");
            return;
        }

        VocabInspectorUI vocabInspectorUI = FindObjectOfType<VocabInspectorUI>();
        vocabInspectorUI.OpenVocabInspector(word);
    }
}
