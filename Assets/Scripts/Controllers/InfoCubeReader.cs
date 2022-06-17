using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

using UnityEngine.UI;

namespace SpecialControllers
{
    public class InfoCubeReader : Singleton<InfoCubeReader>
    {
        public bool ActiveScript { get;  set; } = false;

        [SerializeField] private ARRaycastManager _raycastmanager;
        [SerializeField] private GameObject _arCamera;
        [SerializeField] private GameObject _arSession;
        [SerializeField] private GameObject _camera;

        private GameObject Cube;
        private static List<ARRaycastHit> _hits = new List<ARRaycastHit>();

        //private void HandleWaypointChanged(ApiWaypoint waypoint)
        //{
        //    if (waypoint.Id == 15)
        //    {
        //        /*_arCamera.SetActive(true);
        //        _arSession.SetActive(true);           

        //        _camera.SetActive(false);*/
        //        ActiveScript = true;
        //    }
        //    else
        //    {
        //        /* _arCamera.SetActive(false);
        //         _arSession.SetActive(false);

        //         _camera.SetActive(true);*/
        //        ActiveScript = false;
        //    }
        //}

        private void Update()
        {
            if (ActiveScript == false)
            {
                return;
            }

            RaycastHit hit;

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    if (Physics.Raycast(_arCamera.transform.position, _arCamera.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
                    {
                        Debug.Log("Raycasting Detected: " + hit.collider.tag);
                        if (hit.collider.Equals("InfoCube"))
                        {
                            GameObject Object = hit.collider.gameObject;

                            InfoCubeObject C;

                            if (Object.TryGetComponent(out C))
                            {
                                C.CubeTapped();
                                // _text.text = "help";
                            }
                        }
                    }
                }
            }

        }
    }
}
