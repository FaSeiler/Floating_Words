using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Draws bounding boxes.
/// </summary>
[Obsolete("This class is deprecated.")]
public class DrawingBoundingBox : MonoBehaviour
{
    public Material lineMaterial;
    private List <GoogleCloudVisionJsonParser.DetectedObjBoundingBox> ObjList;
    private float planex;
    private float planey;
    public GameObject plane;
    public TranslationAPI SI;

    void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }
    void Start()
    {
        //planex = plane.GetComponent<Renderer>().bounds.size.x;
        //planey = plane.GetComponent<Renderer>().bounds.size.y;
        //Debug.Log(plane.GetComponent<Renderer>().bounds.size.ToString());
        ObjList=new List<GoogleCloudVisionJsonParser.DetectedObjBoundingBox>();
    }
    public void setobj(List <GoogleCloudVisionJsonParser.DetectedObjBoundingBox> newobjList)
    {
        if (ObjList == null)
        {
            return;
        }
        
        ObjList.Clear();
        
        foreach (GoogleCloudVisionJsonParser.DetectedObjBoundingBox obj in newobjList.ToArray())
        {
            GoogleCloudVisionJsonParser.DetectedObjBoundingBox newObj = obj;

            newObj.bottom_right = new Vector2(((newObj.bottom_right.x - 0.5f) * planex), ((-newObj.bottom_right.y + 0.5f) * planey));
            newObj.bottom_left = new Vector2(((newObj.bottom_left.x - 0.5f) * planex), ((-newObj.bottom_left.y + 0.5f) * planey));
            newObj.upper_right = new Vector2(((newObj.upper_right.x - 0.5f) * planex), ((-newObj.upper_right.y + 0.5f) * planey));
            newObj.upper_left = new Vector2(((newObj.upper_left.x - 0.5f) * planex), ((-newObj.upper_left.y + 0.5f) * planey));
            ObjList.Add(newObj);
            Vector2 center = (newObj.upper_left + newObj.upper_right + newObj.bottom_left + newObj.bottom_right) / 4;
            SI.ShowLabel(newObj.label, center);
        }
    }
    //Will be called after all regular rendering is done
    //public void OnRenderObject()
    //{
    //    CreateLineMaterial();

    //    GL.PushMatrix();
    //    // Apply the line material
    //    lineMaterial.SetPass(0);
    //    // Set transformation matrix for drawing to
    //    // match our transform
    //    //GL.LoadOrtho();
    //    //GL.MultMatrix(transform.localToWorldMatrix);
    //    //Debug.Log(transform.localToWorldMatrix.ToString());
      
    //    GL.Begin(GL.LINES);  // Draw lines

    //    GL.Color(new Color(1.0f, 0.0f, 0.0f, 1.0f)); // Vertex colors change from red to green



    //    // One vertex at transform position

    //    foreach (JsonParser.DetectedObj obj in ObjList.ToArray())
    //    {
    //        GL.Vertex(obj.BL);
    //        GL.Vertex(obj.BR);

    //        GL.Vertex(obj.BR);
    //        GL.Vertex(obj.UR);

    //        GL.Vertex(obj.UR);
    //        GL.Vertex(obj.UL);

    //        GL.Vertex(obj.UL);
    //        GL.Vertex(obj.BL);
    //    }

    //    GL.End();
    //    GL.PopMatrix();
    //}

   
   // private void OnPostRender()
   // {
        // Set your materials
      //  GL.PushMatrix();
        // yourMaterial.SetPass( );
        // Draw your stuff
    //    GL.PopMatrix();
  //  }
}
