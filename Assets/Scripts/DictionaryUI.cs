using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictionaryUI : MonoBehaviour
{
    public GameObject uiGO;
    public GameObject vocabEntryParent;
    public GameObject vocabEntryPrefab;

    public void OpenDictionaryUI()
    {
        uiGO.SetActive(true);
        BuildDictionaryUI();
    }

    public void CloseDictionaryUI()
    {
        uiGO.SetActive(false);
    }

    // This adds/instantiates all the vocabulary buttons in the dictionary UI
    private void BuildDictionaryUI()
    {
        if (VocabularyDB.instance == null)
        {
            return;
        }

        DestroyAllChildrenGO(vocabEntryParent);
        Dictionary<string, Word> vocabulary = VocabularyDB.instance.vocabulary;
        
        foreach (KeyValuePair<string, Word> vocab in vocabulary)
        {
            AddUIVocabEntry(vocab.Value);
        }
    }

    // Adds one UI entry for a vocab and sets its properties
    private void AddUIVocabEntry(Word word)
    {
        GameObject vocabEntryGO = Instantiate(vocabEntryPrefab, vocabEntryParent.transform);
        vocabEntryGO.name = word.english;
        VocabEntryUI vocabEntryUI = vocabEntryGO.GetComponent<VocabEntryUI>();
        vocabEntryUI.InitVocab(word);
    }

    private void DestroyAllChildrenGO(GameObject parent)
    {
        foreach (Transform child in parent.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
