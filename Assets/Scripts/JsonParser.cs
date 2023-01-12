using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.ARFoundation.Samples;

public class JsonParser : MonoBehaviour
{
    public DrawingBoundingBox DBB;
    public ShowInfo SI;
    public AnchorCreator anchorCreater;
    public TextMeshProUGUI debugText;

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

    public void ExtractInfo(string text)
    {
        SI.CleanAll();
        Debug.Log(text);
        debugText.text = text;
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
                try
                { 
                    if(text.IndexOf("x", startIndex)> text.IndexOf("y", startIndex)|| text.IndexOf("x", startIndex) == -1)
                    {
                        Debug.Log("incomplete response");
                        break;
                    }
                    startIndex = text.IndexOf("x", startIndex) + 3;
                    float x = (float)Convert.ToDouble(text.Substring(startIndex, text.IndexOf(',', startIndex) - startIndex));
                    
                    if ((text.IndexOf("y", startIndex) > text.IndexOf("x", startIndex)&& i!=3 )|| text.IndexOf("y", startIndex)==-1)
                    {
                        Debug.Log("incomplete response");
                        break;
                    }
                    //float x = (float)double.Parse(text.Substring(startIndex, text.IndexOf(',', startIndex) - startIndex), System.Globalization.NumberFormatInfo.InvariantInfo);
                    startIndex = text.IndexOf('y', startIndex) + 3;
                    //float y = (float)double.Parse(text.Substring(startIndex, text.IndexOf('}', startIndex) - startIndex),System.Globalization.NumberFormatInfo.InvariantInfo);
                    float y = (float)Convert.ToDouble(text.Substring(startIndex, text.IndexOf('}', startIndex) - startIndex));

                    //Debug.Log(x);
                    //Debug.Log(y);
                    Vector2 coord = new Vector2(x, 1-y);
                    coords.Add(coord);
                }
                catch (FormatException)
                {
                    Debug.Log("Incorrect format of input, getting new data next iteration");

                    //Vector2 coord = new Vector2(0.5f, 0.5f);
                    //coords.Add(coord);
                }
            }
            if(coords.Count == 4)
            {
                DetectedObj newDectedObj = new DetectedObj(label, coords);
                newDectedObjList.Add(newDectedObj);

                Vector2 center = (newDectedObj.UL + newDectedObj.BR + newDectedObj.UR + newDectedObj.BL) / 4;
                center.x *= Screen.width;
                center.y *= Screen.height;
                anchorCreater.CreateAnchorWithDepth(center, newDectedObj.Label);
                //SI.ShowLabel(label, coords);
                printInfo(newDectedObj);
            }        
        }
        //DBB.setobj(newDectedObjList);
        
        
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





