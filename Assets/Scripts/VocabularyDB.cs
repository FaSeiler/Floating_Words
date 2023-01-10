using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class VocabularyDB : MonoBehaviour
{
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

    private SetGetWordDetails setGetWordDetails;

    void Start()
    {
        setGetWordDetails = FindObjectOfType<SetGetWordDetails>();
        // Load all words into vocabulary

        /// Fabi: EXAMPLE of how a word is created. Note that all translations should be already initialized
        /// when the word is created.
        /// TODO: Delete the following lines later
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
        idx = -1;
    }

    private int idx = 0;
    public void DebugAddWord()
    {
        if (idx == -1)
        {
            return;
        }

        if (idx==0)
        {
            Word newWord = new Word("Example", "Beispiel", "_", "_", "_", "_", null, true);
            vocabulary.Add(newWord.english, newWord);
            idx++;
            PrintVocabulary();
            return;
        }
        if (idx==1)
        {
            Word newWord1 = new Word("Loudspeaker", "Lautsprecher", "_", "_", "_", "_", null, true);
            vocabulary.Add(newWord1.english, newWord1);
            idx++;
            PrintVocabulary();
            return;
        }
        if (idx == 2)
        {
            Word newWord2 = new Word("Fridge", "Kuehlschrank", "_", "_", "_", "_", null, true);
            vocabulary.Add(newWord2.english, newWord2);
            idx++;
            PrintVocabulary();
            return;
        }
        if (idx == 3)
        {
            Word newWord3 = new Word("Dormitory", "Wohnheim", "_", "_", "_", "_", null, true);
            vocabulary.Add(newWord3.english, newWord3);
            idx++;
            PrintVocabulary();
            return;
        }
        if (idx == 4)
        {
            Word newWord4 = new Word("Wallet", "Geldbeutel", "_", "_", "_", "_", null, true);
            vocabulary.Add(newWord4.english, newWord4);
            idx++;
            PrintVocabulary();
            return;
        }
    }

    public void PrintVocabulary()
    {
        string output = "=======================================\n";

        foreach (var item in vocabulary)
        {
            output += item.Value.english + ", ";
        }

        Debug.Log(output);
    }
}
