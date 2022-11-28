using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JsonParser : MonoBehaviour
{
    private DrawingBoundingBox DBB;
    private ShowInfo SI;
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
        public string Label { get; set; }
        public Vector2 BL { get; set; }
        public Vector2 BR { get; set; }
        public Vector2 UR { get; set; }
        public Vector2 UL { get; set; }

    }
    void Start()
    {
        DBB = (DrawingBoundingBox)this.gameObject.GetComponent(typeof(DrawingBoundingBox));
        SI = (ShowInfo)this.gameObject.GetComponent(typeof(ShowInfo));
    }
    public void ExtractInfo(string text)
    {
        SI.CleanAll();
        Debug.Log(text);
        int startIndex = 0;
        List <DetectedObj> newDectedObjList = new List <DetectedObj>();
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
                //Debug.Log(text.Substring(startIndex, text.IndexOf(',', startIndex) - startIndex));
                float x = (float)Convert.ToDouble(text.Substring(startIndex, text.IndexOf(',', startIndex) - startIndex));
                startIndex = text.IndexOf('y', startIndex) + 3;
                float y = (float)Convert.ToDouble(text.Substring(startIndex, text.IndexOf('}', startIndex) - startIndex));
                //Debug.Log(x);
                //Debug.Log(y);
                Vector2 coord = new Vector2(x, y);
                coords.Add(coord);
            }
            DetectedObj newDectedObj = new DetectedObj(label, coords);
            newDectedObjList.Add(newDectedObj);
            //SI.ShowLabel(label, coords);
            //printInfo(newDectedObj);
        }
        DBB.setobj(newDectedObjList);
        
        
    }

    public void printInfo(DetectedObj obj)
    {
        string info = "New Detected Object : " + obj.Label + "\n"
            + "BottomLeft Coordinates : x= " + obj.BL.x + "  y= " + obj.BL.y + "\n"
            + "BottomRight Coordinates : x= " + obj.BR.x + "  y= " + obj.BR.y + "\n"
            + "UpRight Coordinates : x= " + obj.UR.x + "  y= " + obj.UR.y + "\n"
            + "UpLeft Coordinates : x= " + obj.UL.x + "  y= " + obj.UL.y + "\n";
        Debug.Log(info);
    }

}
