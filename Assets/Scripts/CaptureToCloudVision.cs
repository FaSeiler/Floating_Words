using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class CaptureToCloudVision : MonoBehaviour
{

    public string url = "https://vision.googleapis.com/v1/images:annotate?key=";
    public string apiKey = ""; //Put your google cloud vision api key here
    public float captureIntervalSeconds = 5.0f;
    public int requestedWidth = 640;
    public int requestedHeight = 480;
    public FeatureType featureType = FeatureType.OBJECT_LOCALIZATION;
    public int maxResults = 10;
    public Quaternion baseRotation; //x=90, y=180, z=0
    public JsonParser jp;
    public Canvas canvas;
    public RawImage image;
    bool takePic=false;

    Dictionary<string, string> headers;
    Texture2D m_LastCameraTexture;

    [System.Serializable]
    public class AnnotateImageRequests
    {
        public List<AnnotateImageRequest> requests;
    }

    [System.Serializable]
    public class AnnotateImageRequest
    {
        public Image image;
        public List<Feature> features;
    }


    [System.Serializable]
    public class Image
    {
        public string content;
    }
    [System.Serializable]
    public class Feature
    {
        public string type;
        public int maxResults;
    }


    public enum FeatureType
    {
        OBJECT_LOCALIZATION
    }

    void Start()
    {
        Application.targetFrameRate = 30;

        WebCamDevice[] devices = WebCamTexture.devices;
        for (var i = 0; i < devices.Length; i++)
        {
            Resolution[] resolutionsAvailable = devices[i].availableResolutions;

            if (resolutionsAvailable != null)
            {
                for (int j = 0; j < resolutionsAvailable.Length; j++)
                {
                    Debug.Log("Res [" + j + "]: " + resolutionsAvailable[j].ToString()); // This will only be logged on IPhone or Android devices
                }

                // For OnePlus 8 the first entry is the highest resolution
                requestedWidth = resolutionsAvailable[0].width;
                requestedHeight = resolutionsAvailable[0].height;
            }

            Debug.Log(devices[i].name);
        }

        Screen.sleepTimeout = SleepTimeout.NeverSleep; ; // Stop turning off mobile screen


        headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json; charset=UTF-8");

        if (apiKey == null || apiKey == "")
            Debug.LogError("No API key. Please set your API key into the \"Web Cam Texture To Cloud Vision(Script)\" component.");
        
        StartCoroutine("Capture");

    }

    void Update()
    {
        //if (Input.touchCount == 0)
        //    return;

        //var touch = Input.GetTouch(0);
        //Debug.Log(touch.position.ToString());
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {

        if (takePic)
        {
            //canvas.enabled = false;
            takePic = false;
            var tempRT = RenderTexture.GetTemporary(source.width, source.height);
            Graphics.Blit(source, tempRT);
            m_LastCameraTexture = new Texture2D(source.width, source.height, TextureFormat.RGBA32, false);
            m_LastCameraTexture.ReadPixels(new Rect(0, 0, source.width, source.height), 0, 0, false);
            m_LastCameraTexture.Apply();
            //canvas.enabled = true;
            //byte[] png=m_LastCameraTexture.EncodeToPNG();

            //System.IO.File.WriteAllBytes(Application.persistentDataPath + "//saved.png", png);
            //if(File.Exists(Application.persistentDataPath + "//saved.png"))
            //{
            //    Debug.Log("exist");
            //}

            //Debug.Log(Application.persistentDataPath);

            RenderTexture.ReleaseTemporary(tempRT);
        }
        Graphics.Blit(source, destination);
    }
    private IEnumerator Capture()
    {
        while (true)
        {
            if (this.apiKey == null)
                yield return null;

            yield return new WaitForSeconds(captureIntervalSeconds);

            takePic = true;
            yield return new WaitForSeconds(0.1f);
            //canvas.enabled = false;
            //yield return new WaitForEndOfFrame();
            //m_LastCameraTexture = ScreenCapture.CaptureScreenshotAsTexture();
            //canvas.enabled = true;


            var jpg = m_LastCameraTexture.EncodeToJPG();
            string base64 = System.Convert.ToBase64String(jpg);

            // #if UNITY_WEBGL	
            // 			Application.ExternalCall("post", this.gameObject.name, "OnSuccessFromBrowser", "OnErrorFromBrowser", this.url + this.apiKey, base64, this.featureType.ToString(), this.maxResults);
            // #else

            AnnotateImageRequests requests = new AnnotateImageRequests();
            requests.requests = new List<AnnotateImageRequest>();

            AnnotateImageRequest request = new AnnotateImageRequest();
            request.image = new Image();
            request.image.content = base64;
            request.features = new List<Feature>();
            Feature feature = new Feature();
            feature.type = this.featureType.ToString();
            feature.maxResults = this.maxResults;
            request.features.Add(feature);
            requests.requests.Add(request);

            string jsonData = JsonUtility.ToJson(requests, false);//INPUT
            if (jsonData != string.Empty)
            {
                string url = this.url + this.apiKey;
                byte[] postData = System.Text.Encoding.Default.GetBytes(jsonData);
                using (WWW www = new WWW(url, postData, headers))
                {
                    yield return www;
                    if (string.IsNullOrEmpty(www.error))
                    {
                        string responses = www.text.Replace("\n", "").Replace(" ", "");
                        JSONNode res = JSON.Parse(responses);
                        string fullText = res["responses"][0]["textAnnotations"][0]["description"].ToString().Trim('"');
                        jp.ExtractInfo(responses);
                    }
                    else
                    {
                        Debug.Log("Error: " + www.error);
                    }
                }
            }
        }

    }
}


// Update is called once per frame
