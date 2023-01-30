using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class ObjectImages : MonoBehaviour
{
    public static ObjectImages instance;
    public List<Sprite> objectSprites = new List<Sprite>();
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //0- packagedgoods
    //1- Mobilephone
    //2- Pen
    //3- Groomingtrimmer
    //4- Person
    //5- Hat
    //6- Lighting
    //7- Tableware
    //8- Shoe
    //9- ComputerKeyboard
    //10- Mouse
    //11- Laptop
    //12- Sunglasses
    //13- Bottle
    //14- Luggage
    //15- Coffee
    //16- Drink
    //17- Wallet
    //18- <DEFAULT- ALL OBJECTS NOT MENTIONED>  
    public void AssignImage(Word word)
    {
        int m = 0;
        if (word.english.Contains("packagedgoods"))
        {
            m = 0;
        }
        switch (word.english)
        {
            case "Mobilephone":
                m = 1;
                break;
            case "Pen":
                m = 2;
                break;
            case "Groomingtrimmer":
                m = 3;
                break;
            case "Person":
                m = 4;
                break;
            case "Hat":
                m = 5;
                break;
            case "Lighting":
                m = 6;
                break;
            case "Tableware":
                m = 7;
                break;
            case "Shoe":
                m = 8;
                break;
            case "Computerkeyboard":
                m = 9;
                break;
            case "Mouse":
                m = 10;
                break;
            case "Laptop":
                m = 11;
                break;
            case "Sunglasses":
                m = 12;
                break;
            case "Bottle":
                m = 13;
                break;
            case "Luggage&bags":
                m = 14;
                break;
            case "Coffeecup":
                m = 15;
                break;
            case "Drink":
                m = 16;
                break;
            case "Wallet":
                m = 17;
                break;
            default:
                m = 18;
                break;
        }
        word.screenshot = objectSprites[m];
    }
}
