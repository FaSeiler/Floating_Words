using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Controlls the Flashcard/Quiz window and provides
/// functionalities for creating question and answer sets
/// from the vocabs in the provideddictionary.
/// </summary>
public class Flashcards : MonoBehaviour
{
    public GameObject flashCardsUIGO;

    public Button option0_btn;
    public Button option1_btn;
    public Button option2_btn;
    public Button option3_btn;

    public TextMeshProUGUI option1_text;
    public TextMeshProUGUI option2_text;
    public TextMeshProUGUI option3_text;
    public TextMeshProUGUI option4_text;

    public Button continue_btn;

    public TextMeshProUGUI activeQuizzedWord_text;

    private List<Word> vocabulary;

    private Word activeQuizzedWord = null;
    private int activeQuizzedWordIdx = 0;

    public int correctOption;

    public Color color_green;
    public Color color_red;
    public Color color_default;


    // REQUIRES: at least 4 words in vocabulary
    private void Start()
    {
        continue_btn.onClick.AddListener(Continue);
        option0_btn.onClick.AddListener(delegate { OnClickButton(option0_btn, 0); });
        option1_btn.onClick.AddListener(delegate { OnClickButton(option1_btn, 1); });
        option2_btn.onClick.AddListener(delegate { OnClickButton(option2_btn, 2); });
        option3_btn.onClick.AddListener(delegate { OnClickButton(option3_btn, 3); });
    }

    private void OnClickButton(Button clickedBtn, int btnId)
    {
        if (btnId == correctOption)
        {
            // Correct option selected
            clickedBtn.gameObject.GetComponent<Image>().color = color_green;
        }
        else
        {
            // Incorrect option selected
            clickedBtn.gameObject.GetComponent<Image>().color = color_red;
        }
    }

    public void OpenFlashCardsUI()
    {
        vocabulary = VocabDictionaryToList(VocabularyDB.instance.vocabulary);

        if (vocabulary.Count < 4)
        {
            return;
        }

        flashCardsUIGO.SetActive(true);

        CreateShuffledVocabList();
        activeQuizzedWordIdx = -1;
        Continue();
    }

    public void CloseFlashCardsUI()
    {
        flashCardsUIGO.SetActive(false);
    }

    private List<Word> VocabDictionaryToList(Dictionary<string, Word> vocabDict)
    {
        List<Word> newList = new List<Word>();

        foreach (var item in vocabDict)
        {
            newList.Add(item.Value);
        }

        return newList;
    }

    private void CreateShuffledVocabList()
    {
        for (int i = 0; i < vocabulary.Count; i++)
        {
            Word temp = vocabulary[i];
            int randInt = Random.Range(i, vocabulary.Count);
            vocabulary[i] = vocabulary[randInt];
            vocabulary[randInt] = temp;
        }
    }

    /// <summary>
    /// Choose new word for the quiz
    /// </summary>
    private void Continue()
    {
        ChooseNextWord();
        SetOptionButtons();
        ResetAllButtonColors();
    }

    private void ResetAllButtonColors()
    {
        option0_btn.gameObject.GetComponent<Image>().color = color_default;
        option1_btn.gameObject.GetComponent<Image>().color = color_default;
        option2_btn.gameObject.GetComponent<Image>().color = color_default;
        option3_btn.gameObject.GetComponent<Image>().color = color_default;
    }

    private void ChooseNextWord()
    {
        // Last word reached
        if (activeQuizzedWordIdx == vocabulary.Count - 1)
        {
            //CreateShuffledVocabList();
            activeQuizzedWordIdx = -1;
        }

        activeQuizzedWordIdx++;
        activeQuizzedWord = vocabulary[activeQuizzedWordIdx];

        activeQuizzedWord_text.text = activeQuizzedWord.english;
    }

    private void SetOptionButtons()
    {
        List<int> answerIdxsUsed = new List<int>();
        answerIdxsUsed.Add(activeQuizzedWordIdx);

        int idx;

        idx = GetUnusedAnswerIdx(answerIdxsUsed);
        option1_text.text = vocabulary[idx].GetWordForLanguage(VocabularyDB.activeLanguageMode);
        answerIdxsUsed.Add(idx);

        idx = GetUnusedAnswerIdx(answerIdxsUsed);
        option2_text.text = vocabulary[idx].GetWordForLanguage(VocabularyDB.activeLanguageMode);
        answerIdxsUsed.Add(idx);

        idx = GetUnusedAnswerIdx(answerIdxsUsed);
        option3_text.text = vocabulary[idx].GetWordForLanguage(VocabularyDB.activeLanguageMode);
        answerIdxsUsed.Add(idx);

        idx = GetUnusedAnswerIdx(answerIdxsUsed);
        option4_text.text = vocabulary[idx].GetWordForLanguage(VocabularyDB.activeLanguageMode);
        answerIdxsUsed.Add(idx);


        correctOption = Random.Range(0, 4);
        if (correctOption == 0)
        {
            option1_text.text = activeQuizzedWord.GetWordForLanguage(VocabularyDB.activeLanguageMode);
        }
        else if (correctOption == 1)
        {
            option2_text.text = activeQuizzedWord.GetWordForLanguage(VocabularyDB.activeLanguageMode);
        }
        else if (correctOption == 2)
        {
            option3_text.text = activeQuizzedWord.GetWordForLanguage(VocabularyDB.activeLanguageMode);
        }
        else if (correctOption == 3)
        {
            option4_text.text = activeQuizzedWord.GetWordForLanguage(VocabularyDB.activeLanguageMode);
        }
    }

    private int GetUnusedAnswerIdx(List<int> answersUsed)
    {
        int rand = Random.Range(0, vocabulary.Count);
        while (answersUsed.Contains(rand))
        {
            rand++;

            if (rand == vocabulary.Count)
            {
                rand = 0;
            }
        }

        return rand;
    }
}
