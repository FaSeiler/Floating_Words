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
        this.wordInfo = WordInformation.GetWordInfo(english);

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

    public WordInformation.WordInfo wordInfo;
    public Sprite screenshot;
    public bool learned; // Fabi: "Not sure what this is used for?!" -> comes from Wang

    // TODO THIS COME FROM WANG.
    //public override string ToString()
    //{
    //    if (screenshot == null)
    //        return english + "_" + learned;
    //    else
    //        return english + "_" + learned + "_" + pic;
    //}
}
