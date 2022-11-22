using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JsonParser : MonoBehaviour
{
    public static JsonParser instance;
    public GameObject glPrefab;

    public Vector3 e, f, g, h;
    
    public TextMeshProUGUI debug_Text;


    private void Start()
    {
        instance = this;
    }
    public struct DetectedObj
    {
        public DetectedObj(string label, List<Vector2> coords)
        {
            Label = label;
            BL = coords[0];
            BR = coords[1];
            UR = coords[2];
            UL = coords[3];

        }
        public string Label { get; }
        public Vector2 BL { get; }
        public Vector2 BR { get; }
        public Vector2 UR { get; }
        public Vector2 UL { get; }

    }
    public void ExtractInfo(string text)
    {
        Debug.Log(text);
        int startIndex = 0;
        while (text.IndexOf("name", startIndex) != -1)
        {
            //extract label
            startIndex = text.IndexOf("name", startIndex) + 7; //"name"="....:
            string label = text.Substring(startIndex, text.IndexOf("\"", startIndex) - startIndex);
            List<Vector2> coords = new List<Vector2>();
            //extract Bb
            startIndex = text.IndexOf("normalizedVertices", startIndex);
            for (int i = 0; i < 4; i++)
            {
                startIndex = text.IndexOf("x", startIndex) + 3;
                float x = (float)Convert.ToDouble(text.Substring(startIndex, text.IndexOf(',', startIndex) - startIndex));
                startIndex = text.IndexOf('y', startIndex) + 3;
                float y = (float)Convert.ToDouble(text.Substring(startIndex, text.IndexOf('}', startIndex) - startIndex));
                //Debug.Log(x);
                //Debug.Log(y);
                Vector2 coord = new Vector2(x, y);
                coords.Add(coord);
            }
            DetectedObj newDectedObj = new DetectedObj(label, coords);
            printInfo(newDectedObj);
        }
    }

    public void printInfo(DetectedObj obj)
    {
        Debug.Log(obj.BR);
        string info = "New Detected Object : " + obj.Label + "\n"
            + "BottomLeft Coordinates : x= " + obj.BL.x + "  y= " + obj.BL.y + "\n"
            + "BottomRight Coordinates : x= " + obj.BR.x + "  y= " + obj.BR.y + "\n"
            + "UpRight Coordinates : x= " + obj.UR.x + "  y= " + obj.UR.y + "\n"
            + "UpLeft Coordinates : x= " + obj.UL.x + "  y= " + obj.UL.y + "\n";
        Debug.Log(info);
        debug_Text.text = "New Detected Object : " + obj.Label;
        e = Camera.main.ViewportToWorldPoint(obj.BL);
        f = Camera.main.ViewportToWorldPoint(obj.UL);
        g = Camera.main.ViewportToWorldPoint(obj.UR);
        h = Camera.main.ViewportToWorldPoint(obj.BR);
        GameObject tempPrefab= Instantiate(glPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        Destroy(tempPrefab, 1.5f);

    }

}
