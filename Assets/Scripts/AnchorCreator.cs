using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
namespace UnityEngine.XR.ARFoundation.Samples
{
    [RequireComponent(typeof(ARAnchorManager))]
    [RequireComponent(typeof(ARRaycastManager))]
    public class AnchorCreator : MonoBehaviour
    {
        [SerializeField]
        GameObject m_Prefab;
        public static Word word;
        public TranslationAPI showInfo;

        public GameObject prefab
        {
            get => m_Prefab;
            set => m_Prefab = value;
        }

        public bool rotateAnchorPrefabOnHit = false;

        public void RemoveAllAnchors()
        {
            Logger.Log($"Removing all anchors ({m_Anchors.Count})");
            foreach (var anchor in m_Anchors)
            {
                Destroy(anchor.gameObject);
            }
            m_Anchors.Clear();
        }

        void Awake()
        {
            
            m_RaycastManager = GetComponent<ARRaycastManager>();
            m_AnchorManager = GetComponent<ARAnchorManager>();
            
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
                        if (anchorWithSameLable.GetComponent<CanvasTextManager>().text == lable)
                        {
                            Debug.Log("Anchor with Same label 1. : " + lable + " at " + hit.pose.position);
                            Debug.Log("Anchor with Same label 2. : " + lable + " at " + anchorWithSameLable.transform.position);
                            float dis = Vector3.Distance(anchorWithSameLable.transform.position, hit.pose.position);
                            Debug.Log("Distance :" + dis);
                            if (dis < 0.2)
                            {
                                return;
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

        public static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

        public List<ARAnchor> m_Anchors = new List<ARAnchor>();

        ARRaycastManager m_RaycastManager;

        ARAnchorManager m_AnchorManager;
    }
}
