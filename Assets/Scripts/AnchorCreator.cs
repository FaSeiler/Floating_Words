using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
namespace UnityEngine.XR.ARFoundation.Samples
{
    /// <summary>
    /// Manages creating and removing anchors.
    /// </summary>
    [RequireComponent(typeof(ARAnchorManager))]
    [RequireComponent(typeof(ARRaycastManager))]
    public class AnchorCreator : MonoBehaviour
    {
        [SerializeField]
        GameObject m_anchorPrefab;

        public GameObject prefab
        {
            get => m_anchorPrefab;
            set => m_anchorPrefab = value;
        }

        public static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
        public bool rotateAnchorPrefabOnHit = false;
        public List<ARAnchor> m_Anchors = new List<ARAnchor>();

        List<string> detectedLabels = new List<string>();
        ARRaycastManager m_RaycastManager;
        ARAnchorManager m_AnchorManager;
        AROcclusionManager m_OcclusionManager;

        void Awake()
        {
            m_RaycastManager = GetComponent<ARRaycastManager>();
            m_AnchorManager = GetComponent<ARAnchorManager>();
            m_OcclusionManager = GetComponent<AROcclusionManager>();
        }

        public void RemoveAllAnchors()
        {
            Debug.Log($"Removing all anchors ({m_Anchors.Count})");
            Logger.Log($"Removing all anchors ({m_Anchors.Count})");
            try
            {
                foreach (var anchor in m_Anchors)
                {
                    Destroy(anchor.gameObject);
                }
                m_Anchors.Clear();
                detectedLabels = new List<string>();
            }
            catch (NullReferenceException)
            {
                throw new ArgumentNullException("Anchors have already been destroyed.");
            }
        }

        void SetAnchorText(ARAnchor anchor, string text)
        {
            var canvasTextManager = anchor.GetComponent<CanvasTextManager>();
            if (canvasTextManager)
            {
                canvasTextManager.text = text;
            }
        }
        public void removefromlist(GameObject obj)
        {
            ARAnchor anchor=obj.GetComponent<ARAnchor>();
            if(anchor!=null)
                Debug.Log("Remove");
            m_Anchors.Remove(anchor);
        }
        ARAnchor CreateAnchor(in ARRaycastHit hit)
        {
            ARAnchor anchor = null;

            // If we hit a plane, try to "attach" the anchor to the plane
            if (hit.trackable is ARPlane plane)
            {
                var planeManager = GetComponent<ARPlaneManager>();
                if (planeManager)
                {
                    //Logger.Log("Creating anchor attachment.");
                    var oldPrefab = m_AnchorManager.anchorPrefab;
                    m_AnchorManager.anchorPrefab = prefab;
                    anchor = m_AnchorManager.AttachAnchor(plane, hit.pose);
                    m_AnchorManager.anchorPrefab = oldPrefab;
                    SetAnchorText(anchor, $"Attached to plane {plane.trackableId}");
                    return anchor;
                }
            }

            // Otherwise, just create a regular anchor at the hit pose
            //Logger.Log("Creating regular anchor.");

            GameObject gameObject = null;

            if (rotateAnchorPrefabOnHit)
            {
                // Note: the anchor can be anywhere in the scene hierarchy
                gameObject = Instantiate(prefab, hit.pose.position, hit.pose.rotation);
            }
            else
            {
                // Don't rotate the anchor prefab
                gameObject = Instantiate(prefab, hit.pose.position, Quaternion.identity);
            }

            // Make sure the new GameObject has an ARAnchor component
            anchor = gameObject.GetComponent<ARAnchor>();
            if (anchor == null)
            {
                anchor = gameObject.AddComponent<ARAnchor>();
            }

            SetAnchorText(anchor, $"Anchor (from {hit.hitType})");

            return anchor;
        }

        public void CreateAnchorWithDepth(Vector2 center, string lable)
        {
            ARAnchor anchor = null;
            
            if (m_RaycastManager.Raycast(center, s_Hits, TrackableType.AllTypes))
            {
                Debug.Log(center.ToString() + "  " + lable);
                var hit = s_Hits[0];

                if (VocabularyDB.instance.vocabulary.ContainsKey(lable))
                {
                   foreach(ARAnchor anchorWithSameLable in m_Anchors)
                    {
                        var canvasTextManager = anchorWithSameLable.GetComponent<CanvasTextManager>();
                        if (canvasTextManager && canvasTextManager.text== lable)
                        {
                            Debug.Log("Anchor with Same label 1. : " + lable + " at " + hit.pose.position);
                            Debug.Log("Anchor with Same label 2. : " + lable + " at " + anchorWithSameLable.transform.position);
                            float dis = Vector3.Distance(anchorWithSameLable.transform.position, hit.pose.position);
                            Debug.Log("Distance :" + dis);
                            if (dis < 0.15)
                            {
                                //Update the new anchor
                                m_Anchors.Remove(anchorWithSameLable);
                                Destroy(anchorWithSameLable.gameObject);
                                break;
                            }
                        }
                    }
                }


                GameObject gameObject = Instantiate(prefab, hit.pose.position, hit.pose.rotation);
                anchor = gameObject.GetComponent<ARAnchor>();
                if (anchor == null)
                {
                    anchor = gameObject.AddComponent<ARAnchor>();
                }
                m_Anchors.Add(anchor);
                SetAnchorText(anchor, lable);

                if (!VocabularyDB.instance.vocabulary.ContainsKey(lable))
                {
                    VocabularyDB.instance.AddNewWordToVocabularyDB(lable);
                }
                return;
            }
        }
        
        public void CreateAnchorWithDepthMap(Vector2 screenPos, int screenWidth, int screenHeight, string lable)
        {
            ARAnchor anchor = null;
            Vector2 center = new Vector2(screenPos.x * screenWidth, screenPos.y * screenHeight);
            
            detectedLabels.Add(lable);
            
            if (m_OcclusionManager.TryAcquireEnvironmentDepthCpuImage(out var cpuImage) && cpuImage.valid)
            {
                using(cpuImage)
                {
                    //Assert.IsTrue(cpuImage.planeCount == 1);
                    var plane = cpuImage.GetPlane(0);
                    var dataLength = plane.data.Length;
                    var pixelStride = plane.pixelStride;
                    var rowStride = plane.rowStride;
                    //Assert.AreEqual(0, dataLength % rowStride, "dataLength should be divisible by rowStride without a remainder");
                    //Assert.AreEqual(0,  rowStride% pixelStride, "rowStride should be divisible by pixelStride without a remainder");
                    
                    var depthTextureX = (int) (cpuImage.width * screenPos.x);
                    var depthTextureY = (int) (cpuImage.height * (screenPos.y));
                    var pixelData = plane.data.GetSubArray(depthTextureY * rowStride + depthTextureX * pixelStride, pixelStride);

                    float depthInMeters = ConvertPixelDataToDistanceInMeters(pixelData.ToArray(), cpuImage.format);
                    
                    if (m_RaycastManager.Raycast(center, s_Hits, TrackableType.AllTypes))
                    {
                        Debug.Log(center.ToString() + "  " + lable);
                        var hit = s_Hits[0];
                        Debug.Log("value from raycasting" + hit.pose.position.z);

                        //if (VocabularyDB.instance.vocabulary.ContainsKey(lable))
                        //{
                        //foreach(ARAnchor anchorWithSameLable in m_Anchors)
                            //{
                                //if (anchorWithSameLable.GetComponent<CanvasTextManager>().text == lable)
                                //{
                                    //Debug.Log("Anchor with Same label 1. : " + lable + " at " + hit.pose.position);
                                    //Debug.Log("Anchor with Same label 2. : " + lable + " at " + anchorWithSameLable.transform.position);
                                    //float dis = Vector3.Distance(anchorWithSameLable.transform.position, hit.pose.position);
                                    //Debug.Log("Distance :" + dis);
                                    //if (dis < 0.2)
                                    //{
                                        //return;
                                    //}
                                //}
                            //}
                        //}

                        // Here we just simply replace the depth value obtained from depth map.
                        Vector3 hitPos = new Vector3(hit.pose.position.x, hit.pose.position.y, depthInMeters);

                        Debug.Log("value from depthmap" + depthInMeters);

                        GameObject gameObject = Instantiate(prefab, hit.pose.position, hit.pose.rotation);
                        anchor = gameObject.GetComponent<ARAnchor>();
                        if (anchor == null)
                        {
                            anchor = gameObject.AddComponent<ARAnchor>();
                        }
                        m_Anchors.Add(anchor);
                        SetAnchorText(anchor, lable);

                        if (!VocabularyDB.instance.vocabulary.ContainsKey(lable))
                        {
                            VocabularyDB.instance.AddNewWordToVocabularyDB(lable);
                        }
                        return;
                    }
                }
            }
        }

        // NOTE: can be further improved, check depthlab's implementation.
        float ConvertPixelDataToDistanceInMeters(byte[] data, XRCpuImage.Format format) 
        {
            switch (format) 
            {
                case XRCpuImage.Format.DepthUint16:
                    return BitConverter.ToUInt16(data, 0) / 1000f;
                case XRCpuImage.Format.DepthFloat32:
                    return BitConverter.ToSingle(data, 0);
                default:
                    throw new Exception($"Format not supported: {format}");
            }
        }
    }
}
