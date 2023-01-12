using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CaptureToCloudVision : MonoBehaviour
{
    public string url = "https://vision.googleapis.com/v1/images:annotate?key=";
    public string apiKey = ""; // Put your google cloud vision api key here
    public FeatureType featureType = FeatureType.OBJECT_LOCALIZATION;
    public int maxResults = 10;
    public float captureIntervalSeconds = 5.0f;
    public int requestedWidth = 640;
    public int requestedHeight = 480;
    public JsonParser jp;
    public Slider intervalSlider;
    public TextMeshProUGUI intervalText;

    private bool takePic=false;
    private Dictionary<string, string> headers;
    private Texture2D m_LastCameraTexture;


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
        if (apiKey == null || apiKey == "")
        {
            Debug.LogError("No API key. Please set your API key into the \"Web Cam Texture To Cloud Vision(Script)\" component.");
        }

        if (intervalSlider != null)
        {
            intervalSlider.value = captureIntervalSeconds;
            UpdateIntervalText();
            intervalSlider.onValueChanged.AddListener(delegate { IntervalValueChanged(); });
        }

        Application.targetFrameRate = 30;
        Screen.sleepTimeout = SleepTimeout.NeverSleep; ; // Stop turning off mobile screen

        headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json; charset=UTF-8");
        
        StartCoroutine("Capture");
    }

    private void IntervalValueChanged()
    {
        captureIntervalSeconds = intervalSlider.value;
        UpdateIntervalText();
    }

    private void UpdateIntervalText()
    {
        intervalText.text = "Update interval: " + captureIntervalSeconds.ToString("F2") + " seconds";
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (takePic)
        {
            takePic = false;
            var tempRT = RenderTexture.GetTemporary(source.width, source.height);
            Graphics.Blit(source, tempRT);
            m_LastCameraTexture = new Texture2D(source.width, source.height, TextureFormat.RGBA32, false);
            m_LastCameraTexture.ReadPixels(new Rect(0, 0, source.width, source.height), 0, 0, false);
            m_LastCameraTexture.Apply();
            RenderTexture.ReleaseTemporary(tempRT);
        }

        Graphics.Blit(source, destination);
    }
    private IEnumerator Capture()
    {
        while (true)
        {
            if (this.apiKey == null)
            {
                yield return null;
            }

            yield return new WaitForSeconds(captureIntervalSeconds);

            takePic = true;
            yield return new WaitForSeconds(0.1f);

            var jpg = m_LastCameraTexture.EncodeToJPG();
            string base64 = System.Convert.ToBase64String(jpg);

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