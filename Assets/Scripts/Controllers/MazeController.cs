using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using UI;

namespace SpecialControllers
{
    public class MazeController : Singleton<MazeController>, ISpecialWaypoint
    {
        public bool MazeActive { get; set; } = false;

        public bool ActiveScript { get; set; }

        private float CurrentTime = 0;
        [SerializeField] float TimeBeforeGiveup = 120; //time in seconds 

        private bool TimeElapsed = false;

        [SerializeField] private Material MainMaterial;
        [SerializeField] private Material HighContrastMaterial;
        private bool HighContrastOn = false;

        [SerializeField] private ARRaycastManager _raycastmanager;
        [SerializeField] private GameObject _maze;
        [SerializeField] private GameObject _arCamera;
        [SerializeField] private GameObject _arSession;
        [SerializeField] private GameObject _camera;
        [SerializeField] private GameObject _postMazeUI;
        [SerializeField] private GameObject _explanationMark;


        [SerializeField] private GameObject _contrastButton;
        [SerializeField] private InfoCubeManger _infoCubeManger;
        [SerializeField] private GiveupButton _giveupButton;
        [SerializeField] private Button _resetmaze;


        private GameObject _mazeInstance;

        private static List<ARRaycastHit> _hits = new List<ARRaycastHit>();

        public Texture2D tmp;

        public override void Awake()
        {
            base.Awake();

            _infoCubeManger.FinsihedCubes += EndMaze;

            GameObject resetButtonObject = _resetmaze.gameObject;
            resetButtonObject.SetActive(true);

            _resetmaze.onClick.AddListener(ResetMaze);
        }

        public void MaterialSwitch()
        {
            if (_mazeInstance != null)
            {
                GameObject mazeChild = GameObject.FindGameObjectWithTag("Maze");

                MeshRenderer m = mazeChild.GetComponent<MeshRenderer>();

                if (!HighContrastOn)
                {
                    HighContrastOn = true;
                    m.materials[1] = HighContrastMaterial;

                }
                else
                {
                    HighContrastOn = false;
                    m.materials[1] = MainMaterial;
                }
            }
        }

        public void EnableMaze()
        {
            TimeElapsed = false;
            CurrentTime = 0;
            MazeActive = true;

            _arCamera.SetActive(true);
            _arSession.SetActive(true);
            ActiveScript = true;
            _camera.SetActive(false);
            //_contrastButton.SetActive(true);
            GameObject resetButtonObject = _resetmaze.gameObject;
            resetButtonObject.SetActive(true);

            InfoCubeReader.Instance.ActiveScript = true;
        }

        public void DisableMaze()
        {
            _arCamera.SetActive(false);
            _arSession.SetActive(false);
            ActiveScript = false;
            _camera.SetActive(true);
            //_contrastButton.SetActive(false);

            if (_mazeInstance != null)
            {
                Destroy(_mazeInstance);
                _mazeInstance = null;
            }

            GameObject resetButtonObject = _resetmaze.gameObject;
            resetButtonObject.SetActive(false);
            _infoCubeManger.EndViaDebug();

            MazeActive = false;
            InfoCubeReader.Instance.ActiveScript = false;
        }

        private void Update()
        {
            if (ActiveScript == false)
                return;

            if (Input.touchCount > 0)
            {

                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    Ray ray = new Ray(_arCamera.transform.position, Vector3.down);

                    _hits = new List<ARRaycastHit>();

                    Vector2 t = Input.GetTouch(0).position;

                    // if (_raycastmanager.Raycast(ray, _hits, TrackableType.PlaneWithinBounds))
                    if (_raycastmanager.Raycast(t, _hits, TrackableType.PlaneWithinBounds))
                    {

                        var hitPose = _hits[0].pose;

                        if (_mazeInstance != null)
                        {
                            return;
                            /* Destroy(_mazeInstance);
                              _mazeInstance = null;*/
                        }

                        //_text.text = _arCamera.transform.position.x + "-" + _arCamera.transform.position.z +" --- "+ hitPose.position.x + "-" + hitPose.position.z;

                        Vector3 l = new Vector3(hitPose.position.x, hitPose.position.y, hitPose.position.z);

                        _mazeInstance = Instantiate(_maze, l, hitPose.rotation);

                        _infoCubeManger.LoadCubes();
                    }
                }
            }

            if (_mazeInstance != null)
            {
                CurrentTime += Time.deltaTime;

                if (CurrentTime >= TimeBeforeGiveup && TimeElapsed == false)
                {
                    _explanationMark.SetActive(true);
                    TimeElapsed = true;

                    _giveupButton.gameObject.SetActive(true);
                    _giveupButton.EnableGiveupButton(this);
                }

                //deze if statement probeert er voor te zorgen de hoogte verschil verbeterd wordt. Note: nog niet getest kan fout gaan

                /*Ray ray = new Ray(_arCamera.transform.position, Vector3.down);

                if (_raycastmanager.Raycast(ray, _hits, TrackableType.Planes))
                {

                    Pose hitPose = _hits[0].pose;

                    Vector3 Location = _mazeInstance.transform.position;
                    Location.y = hitPose.position.y;

                    _mazeInstance.transform.position = Location;
                }*/
            }
        }

        public void EndMaze()
        {
            _contrastButton.SetActive(false);
            _arCamera.SetActive(false);
            _arSession.SetActive(false);
            ActiveScript = false;
            _camera.SetActive(true);
            if (_mazeInstance != null)
            {
                Destroy(_mazeInstance);
                _mazeInstance = null;
            }
            _postMazeUI.SetActive(true);
            _infoCubeManger.FinsihedCubes -= EndMaze;

        }

        public void ResetMaze()
        {
            if (_mazeInstance != null)
            {
                Destroy(_mazeInstance);
                _mazeInstance = null;
            }
        }
    }
}
