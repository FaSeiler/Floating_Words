using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SimpleJSON;
using UnityEngine.Networking;

public class WebCamTextureToCloudVision : MonoBehaviour
{

	public string url = "https://vision.googleapis.com/v1/images:annotate?key=";
	public string apiKey = ""; //Put your google cloud vision api key here
	public float captureIntervalSeconds = 5.0f;
	public int requestedWidth = 640;
	public int requestedHeight = 480;
	public FeatureType featureType = FeatureType.OBJECT_LOCALIZATION;
	public int maxResults = 10;
	public Quaternion baseRotation; //x=90, y=180, z=0

	public JsonParser jasonParser;

	WebCamTexture webcamTexture;
	Texture2D texture2D;

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
		//TYPE_UNSPECIFIED,
		//FACE_DETECTION,
		//LANDMARK_DETECTION,
		//LOGO_DETECTION,
		//LABEL_DETECTION,
		//TEXT_DETECTION,
		//SAFE_SEARCH_DETECTION,
		//IMAGE_PROPERTIES,
		OBJECT_LOCALIZATION
	}

	// Use this for initialization
	void Start()
	{
		Screen.sleepTimeout = SleepTimeout.NeverSleep; ; // Stop turning off mobile screen

		GameObject es = GameObject.Find("EventSystem");
		jasonParser = (JsonParser)es.gameObject.GetComponent(typeof(JsonParser));

		Application.targetFrameRate = 30;

		if (apiKey == null || apiKey == "")
			Debug.LogError("No API key. Please set your API key into the \"Web Cam Texture To Cloud Vision(Script)\" component.");

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
		if (devices.Length > 0)
		{
			webcamTexture = new WebCamTexture(devices[0].name, requestedWidth, requestedHeight);
			Renderer r = GetComponent<Renderer>();
			if (r != null)
			{
				Material m = r.material;
				if (m != null)
				{
					m.mainTexture = webcamTexture;
				}
			}
			webcamTexture.Play();
			StartCoroutine("Capture");
		}
	}

	void Update()
	{
		transform.rotation = baseRotation * Quaternion.AngleAxis(webcamTexture.videoRotationAngle, Vector3.up);
	}

	private IEnumerator Capture()
	{
		while (true)
		{
			if (this.apiKey == null)
				yield return null;

			yield return new WaitForSeconds(captureIntervalSeconds);

			Color[] pixels = webcamTexture.GetPixels();

			if (pixels.Length == 0)
				yield return null;

			if (texture2D == null || webcamTexture.width != texture2D.width || webcamTexture.height != texture2D.height)
			{
				texture2D = new Texture2D(webcamTexture.width, webcamTexture.height, TextureFormat.RGBA32, false);
			}

			texture2D.SetPixels(pixels);
			byte[] jpg = texture2D.EncodeToJPG();
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

				StartCoroutine(SendGCloudRequest(url, postData));
			}
		}
	}

	IEnumerator SendGCloudRequest(string url, byte[] byteData)
	{
		WWWForm form = new WWWForm();
		form.AddBinaryData("image", byteData, "imagedata.raw");
		
		using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
			www.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");
			www.uploadHandler = new UploadHandlerRaw(byteData);
			www.uploadHandler.contentType = "application/json; charset=UTF-8";
			www.downloadHandler = new DownloadHandlerBuffer();

			yield return www.SendWebRequest();
			www.uploadHandler.Dispose();

			if (www.result == UnityWebRequest.Result.ConnectionError)
			{
				Debug.Log(www.error);
			}
			else
			{
				string responses = www.downloadHandler.text.Replace("\n", "").Replace(" ", "");
				www.downloadHandler.Dispose();
				www.Dispose();

				JSONNode res = JSON.Parse(responses);
				string fullText = res["responses"][0]["textAnnotations"][0]["description"].ToString().Trim('"');
				jasonParser.ExtractInfo(responses);
			}

			www.uploadHandler.Dispose();
			www.downloadHandler.Dispose();
			www.Dispose();
		}
		
	}

#if UNITY_WEBGL
	void OnSuccessFromBrowser(string jsonString) {
		Debug.Log(jsonString);	
	}

	void OnErrorFromBrowser(string jsonString) {
		Debug.Log(jsonString);	
	}
#endif

}
