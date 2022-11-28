using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingBoundingBox : MonoBehaviour
{
    public Material lineMaterial;
    private List <JsonParser.DetectedObj> ObjList;
    private float planex;
    private float planey;
    public GameObject plane;
    public ShowInfo SI;
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
        planex = plane.GetComponent<Renderer>().bounds.size.x;
        planey = plane.GetComponent<Renderer>().bounds.size.y;
        Debug.Log(plane.GetComponent<Renderer>().bounds.size.ToString());
        ObjList=new List<JsonParser.DetectedObj>();
        SI = (ShowInfo)this.gameObject.GetComponent(typeof(ShowInfo));
    }
    public void setobj(List <JsonParser.DetectedObj> newobjList)
    {
        ObjList.Clear();
        foreach (JsonParser.DetectedObj obj in newobjList.ToArray())
        {
            JsonParser.DetectedObj newObj = obj;

            newObj.BR = new Vector2(((newObj.BR.x - 0.5f) * planex), ((-newObj.BR.y + 0.5f) * planey));
            newObj.BL = new Vector2(((newObj.BL.x - 0.5f) * planex), ((-newObj.BL.y + 0.5f) * planey));
            newObj.UR = new Vector2(((newObj.UR.x - 0.5f) * planex), ((-newObj.UR.y + 0.5f) * planey));
            newObj.UL = new Vector2(((newObj.UL.x - 0.5f) * planex), ((-newObj.UL.y + 0.5f) * planey));
            ObjList.Add(newObj);
            Vector2 center = (newObj.UL + newObj.UR + newObj.BL + newObj.BR) / 4;
            SI.ShowLabel(newObj.Label, center);
        }
    }
    //Will be called after all regular rendering is done
    public void OnRenderObject()
    {
        CreateLineMaterial();

        GL.PushMatrix();
        // Apply the line material
        lineMaterial.SetPass(0);
        // Set transformation matrix for drawing to
        // match our transform
        //GL.LoadOrtho();
        //GL.MultMatrix(transform.localToWorldMatrix);
        //Debug.Log(transform.localToWorldMatrix.ToString());
      
        GL.Begin(GL.LINES);  // Draw lines

        GL.Color(new Color(1.0f, 0.0f, 0.0f, 1.0f)); // Vertex colors change from red to green



        // One vertex at transform position

        foreach (JsonParser.DetectedObj obj in ObjList.ToArray())
        {
            GL.Vertex(obj.BL);
            GL.Vertex(obj.BR);

            GL.Vertex(obj.BR);
            GL.Vertex(obj.UR);

            GL.Vertex(obj.UR);
            GL.Vertex(obj.UL);

            GL.Vertex(obj.UL);
            GL.Vertex(obj.BL);
        }

        GL.End();
        GL.PopMatrix();
    }

   
   // private void OnPostRender()
   // {
        // Set your materials
      //  GL.PushMatrix();
        // yourMaterial.SetPass( );
        // Draw your stuff
    //    GL.PopMatrix();
  //  }
}
