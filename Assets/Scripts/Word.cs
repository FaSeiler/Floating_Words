using UnityEngine;

public class Word
{
    // Only use this for creating and intializing a new Word!
    public Word(string english, string german, string chinese, string japanese, string spanish, string french,
                Sprite screenshot, bool learned)
    {
        this.english = english;
        this.german = german;
        this.chinese = chinese;
        this.japanese = japanese;
        this.spanish = spanish;
        this.french = french;

        this.screenshot = screenshot;
        this.learned = learned;
        this.wordInfo = FreeDictionaryAPI.GetWordInfo(english);

        wordInitialized = true;
    }

    public bool wordInitialized = false;

    // Supported languages
    public string english;
    public string german;
    public string chinese;
    public string japanese;
    public string spanish;
    public string french;

    public FreeDictionaryAPI.WordInfo wordInfo;
    public Sprite screenshot;
    public bool learned; // Unused variable

    /// <summary>
    /// Returns the words translation for a given language
    /// </summary>
    /// <param name="languageMode">The language to get the translation from.</param>
    /// <returns>The translated word string</returns>
    public string GetWordForLanguage(VocabularyDB.LanguageMode languageMode)
    {
        switch (languageMode)
        {
            case VocabularyDB.LanguageMode.german:
                return german;
            case VocabularyDB.LanguageMode.chinese:
                return chinese;
            case VocabularyDB.LanguageMode.japanese:
                return japanese;
            case VocabularyDB.LanguageMode.spanish:
                return spanish;
            case VocabularyDB.LanguageMode.french:
                return french;
            default:
                return english;
        }
    }
}
