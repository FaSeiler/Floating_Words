﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
namespace UnityEngine.XR.ARFoundation.Samples
{
    [RequireComponent(typeof(ARAnchorManager))]
    [RequireComponent(typeof(ARRaycastManager))]
    [RequireComponent(typeof(AROcclusionManager))]
    public class AnchorCreator : MonoBehaviour
    {
        [SerializeField]
        GameObject m_Prefab;
        public static Word word;
        public ShowInfo showInfo;
        public VocabularyDB vocabularyDB;

        public GameObject prefab
        {
            get => m_Prefab;
            set => m_Prefab = value;
        }

        public bool rotateAnchorPrefabOnHit = false;
        private List<string> detectedLabels = new List<string>();

        public void RemoveAllAnchors()
        {
            Logger.Log($"Removing all anchors ({m_Anchors.Count})");
            foreach (var anchor in m_Anchors)
            {
                Destroy(anchor.gameObject);
            }
            m_Anchors.Clear();
            detectedLabels = new List<string>();
        }

        void Awake()
        {
            
            m_RaycastManager = GetComponent<ARRaycastManager>();
            m_AnchorManager = GetComponent<ARAnchorManager>();
            m_OcclusionManager = GetComponent<AROcclusionManager>();
            
        }

        void SetAnchorText(ARAnchor anchor, string text)
        {
            var canvasTextManager = anchor.GetComponent<CanvasTextManager>();
            if (canvasTextManager)
            {
                canvasTextManager.text = text;
            }
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


        public void CreateAnchorWithDepth(Vector2 screenPos, int screenWidth, int screenHeight, string lable)
        {
            ARAnchor anchor = null;
            Vector2 center = new Vector2(screenPos.x * screenWidth, screenPos.y * screenHeight);
            if (detectedLabels.Contains(lable))
            {
                Debug.Log("label already detected");
                return;
            }
            
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

                    float depthInMeters = convertPixelDataToDistanceInMeters(pixelData.ToArray(), cpuImage.format);
                    
                    if (m_RaycastManager.Raycast(center, s_Hits, TrackableType.AllTypes))
                    {
                        Debug.Log(center.ToString() + "  " + lable);
                        var hit = s_Hits[0];
                        //hit.pose.position.z = depthInMeters;
                        //
                        Vector3 hitPos = new Vector3(hit.pose.position.x, hit.pose.position.y, depthInMeters);
                        GameObject gameObject = Instantiate(prefab, hit.pose.position, hit.pose.rotation);
                        anchor = gameObject.GetComponent<ARAnchor>();
                        if (anchor == null)
                        {
                            anchor = gameObject.AddComponent<ARAnchor>();
                        }
                        m_Anchors.Add(anchor);
                        SetAnchorText(anchor, lable);

                        if (!vocabularyDB.vocabulary.ContainsKey(lable))
                        {
                            Word word = showInfo.SaveTranslationsToWord(lable);
                            SetGetWordDetails.instance.SaveWordDetails(lable, word.german, word.chinese, word.japanese, word.spanish, word.french, false);
                        }
                        return;
                
                    }
                }
            }

            //if (m_RaycastManager.Raycast(center, s_Hits, TrackableType.AllTypes))
            //{
                //Debug.Log(center.ToString() + "  " + lable);
                //var hit = s_Hits[0];
                //hit.pose.position.z = depthInMeters;
                //GameObject gameObject = Instantiate(prefab, hit.pose.position, hit.pose.rotation);
                //anchor = gameObject.GetComponent<ARAnchor>();
                //if (anchor == null)
                //{
                    //anchor = gameObject.AddComponent<ARAnchor>();
                //}
                //m_Anchors.Add(anchor);
                //SetAnchorText(anchor, lable);

                //if (!vocabularyDB.vocabulary.ContainsKey(lable))
                //{
                    //Word word = showInfo.SaveTranslationsToWord(lable);
                    //SetGetWordDetails.instance.SaveWordDetails(lable, word.german, word.chinese, word.japanese, word.spanish, word.french, false);
                //}
                //return;
                
            //}
        }

        float convertPixelDataToDistanceInMeters(byte[] data, XRCpuImage.Format format) 
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
        

        void Update()
        {
            //if (Input.touchCount == 0)
            //    return;

            //var touch = Input.GetTouch(0);
            //Logger.Log(touch.position.ToString());
            //if (touch.phase != TouchPhase.Began)
            //    return;

            //// Raycast against planes and feature points
            //const TrackableType trackableTypes =
            //    TrackableType.PlaneWithinPolygon |
            //    TrackableType.FeaturePoint;

            //// Perform the raycast
            //if (m_RaycastManager.Raycast(touch.position, s_Hits, trackableTypes))
            //{
            //    // Raycast hits are sorted by distance, so the first one will be the closest hit.
            //    var hit = s_Hits[0];

            //    // Create a new anchor
            //    var anchor = CreateAnchor(hit);
            //    if (anchor)
            //    {
            //        // Remember the anchor so we can remove it later.
            //        m_Anchors.Add(anchor);
            //    }
            //    else
            //    {
            //        Logger.Log("Error creating anchor");
            //    }
            //}
        }


        public static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

        public List<ARAnchor> m_Anchors = new List<ARAnchor>();

        ARRaycastManager m_RaycastManager;

        ARAnchorManager m_AnchorManager;
        
        AROcclusionManager m_OcclusionManager;
    }
}
