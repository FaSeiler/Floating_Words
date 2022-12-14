using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class VocabularyDB : MonoBehaviour
{
    public enum LanguageMode // maybe add more
    {
        english,
        german,
        chinese,
        japanese,
        spanish,
        french
    }

    public static LanguageMode activeLanguageMode = LanguageMode.german;
    string path;
    string Doc;

    Dictionary<string, Info> Vocabulary = new Dictionary<string, Info>();

    // Fabi to Wang: use following Datastructure for the Dictionary (key:string = english word):
    //Dictionary<string, Word> Vocabulary = new Dictionary<string, Word>();

    public struct Info
    {
        public string word;
        public bool Learnt;
        public string pic;//base 64

        public override string ToString()
        {
            if (pic == null)
                return word + "_" + Learnt;
            else
                return word + "_" + Learnt + "_" + pic;
        }
    }
    void Start()
    {
        path = Application.persistentDataPath;
        UpdateDoc();
        //if (!File.Exists(path + "\\Eng.txt")) {
        //    File.Create(path + "\\Eng.txt");
        //}
        if (!File.Exists(path + "\\Deu.txt"))
        {
            File.Create(path + "\\Deu.txt");
        }
        if (!File.Exists(path + "\\Cn.txt"))
        {
            File.Create(path + "\\Cn.txt");
        }

        StreamReader sr = new StreamReader(path + "\\" + activeLanguageMode.ToString() + ".txt");
        string line;
        while ((line = sr.ReadLine()) != null)
        {
            string[] split = line.Split(';');

            Info info = new Info();
            string[] infosplit = line.Split('_');
            info.word = split[0];
            info.Learnt = bool.Parse(split[1]);
            if (split.Length > 2)
                info.pic = split[2];
            Vocabulary.Add(split[0], info);
        }
    }
    public void StoreNewWord(string Engword,string Translation)
    {
        Info info= new Info();
        info.word = Translation;
        info.Learnt = false;
        Vocabulary.Add(Engword, info);
    }
    public void LearnTheWord(string Engword)
    {
        Info newInfo = Vocabulary[Engword];
        newInfo.Learnt = true;
        Vocabulary[Engword] = newInfo;
    }

    public void SaveCapture(string word)
    {
        string picName = activeLanguageMode.ToString() + "_" + word;
        File.WriteAllBytes(path+ "/" + picName, WebCamTextureToCloudVision.jpg);
    }
    
    public void LoadCapture(string word)
    {
        //pop up a window
    }

    public void UpdateDoc()
    {
        Doc = path + "\\" + activeLanguageMode.ToString() + ".txt";
    }

    public void OnApplicationQuit()
    {
        StreamWriter sw = new StreamWriter(Doc);
        for (int i= 0; i < Vocabulary.Count; i++)
            sw.WriteLine(Vocabulary.ElementAt(i).Key + ";" + Vocabulary.ElementAt(i).Value.ToString());
    }
}
