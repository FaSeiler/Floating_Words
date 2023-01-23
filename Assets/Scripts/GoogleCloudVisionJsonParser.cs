using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.ARFoundation.Samples;

/// <summary>
/// Extracts the label and bounding box of a gcloud vision REST response.
/// </summary>
public class GoogleCloudVisionJsonParser : MonoBehaviour
{
    public AnchorCreator anchorCreater;
    public TextMeshProUGUI debugText;

    public struct DetectedObjBoundingBox
    {
        public DetectedObjBoundingBox(string label, List<Vector2> coords)
        {
            this.label = label;
            bottom_left = coords[0];
            bottom_right = coords[1];
            upper_right = coords[2];
            upper_left = coords[3];

        }

        public string label { get; set; }
        public Vector2 bottom_left { get; set; }
        public Vector2 bottom_right { get; set; }
        public Vector2 upper_right { get; set; }
        public Vector2 upper_left { get; set; }

    }

    public void ExtractInfo(string text)
    {
        debugText.text = text;
        int startIndex = 0;
        List <DetectedObjBoundingBox> newDectedObjList = new List <DetectedObjBoundingBox>();

        while (text.IndexOf("name", startIndex) != -1)
        {
            // Extract label
            startIndex = text.IndexOf("name", startIndex) + 7; //"name"="....:
            string label = text.Substring(startIndex, text.IndexOf("\"", startIndex) - startIndex);
            List<Vector2> coords = new List<Vector2>();
            // Extract Bounding Box
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
                    startIndex = text.IndexOf('y', startIndex) + 3;
                    float y = (float)Convert.ToDouble(text.Substring(startIndex, text.IndexOf('}', startIndex) - startIndex));
                    Vector2 coord = new Vector2(x, 1-y);
                    coords.Add(coord);
                }
                catch (FormatException)
                {
                    Debug.Log("Incorrect format of input, getting new data next iteration");
                }
            }
            if(coords.Count == 4)
            {
                DetectedObjBoundingBox newDectedObjBoundingBox = new DetectedObjBoundingBox(label, coords);
                newDectedObjList.Add(newDectedObjBoundingBox);

                Vector2 center =   (newDectedObjBoundingBox.upper_left + 
                                    newDectedObjBoundingBox.bottom_right + 
                                    newDectedObjBoundingBox.upper_right + 
                                    newDectedObjBoundingBox.bottom_left) / 4;
                
                center.x *= Screen.width;
                center.y *= Screen.height;
                //we ignore the packaged goods
                if (label == "Packagedgoods")
                {
                    Debug.Log("No package good for you");
                }
                else
                {
                    anchorCreater.CreateAnchorWithDepth(center, newDectedObjBoundingBox.label);
                }
                //anchorCreater.CreateAnchorWithDepthMap(center, Screen.width, Screen.height, newDectedObj.Label);
                
                PrintBoundingBox(newDectedObjBoundingBox);
            }        
        }
    }

    public void PrintBoundingBox(DetectedObjBoundingBox objBoundingBox)
    {
        string boundingBoxString = "New Detected Object : " + objBoundingBox.label + "\n"
            + "BottomLeft Coordinates : x= " + objBoundingBox.bottom_left.x + "  y= " + objBoundingBox.bottom_left.y + "\n"
            + "BottomRight Coordinates : x= " + objBoundingBox.bottom_right.x + "  y= " + objBoundingBox.bottom_right.y + "\n"
            + "UpRight Coordinates : x= " + objBoundingBox.upper_right.x + "  y= " + objBoundingBox.upper_right.y + "\n"
            + "UpLeft Coordinates : x= " + objBoundingBox.upper_left.x + "  y= " + objBoundingBox.upper_left.y + "\n";
        Debug.Log(boundingBoxString);
    }
}





