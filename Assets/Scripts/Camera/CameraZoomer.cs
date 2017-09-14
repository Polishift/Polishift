using Assets.Scripts.Camera;
using System;
using UnityEngine;

namespace Assets.Scripts.Camera
{
    public class CameraZoomer : MonoBehaviour, ICameraMovementController, ICameraTiltController
    {
        private CameraLogicController cameraLogicController;

        private float minimumHeight; //this will be determined by the proximity to terrain
        private const float MINIMUM_HEIGHT_BUFFER = 10f; //The distance from any terrain that is always kept
        private const float MAXIMUM_HEIGHT = 50f;

        private const float startingAngle = 89f; //Watch out for rounding
        private const float FLATTEST_ANGLE = 50f;
        private const float HORIZONTAL_ACCELARATION = 1.25f;


        void Start()
        {
            this.cameraLogicController = new CameraLogicController();

            //we implemented all three interfaces here, so this is legal
            cameraLogicController.SetCameraMovementController(this);
            cameraLogicController.SetCameraTiltController(this);
        }

        void Update()
        {
            this.minimumHeight = MAXIMUM_HEIGHT - (this.GetDistanceToMainTerrain() + MINIMUM_HEIGHT_BUFFER);
            Debug.Log("this.minimumHeight = " + this.minimumHeight);

            cameraLogicController.UpdateForwardTilt();
            cameraLogicController.UpdateCameraPosition();
        }


        /*
           Interface implementations which are used by the cameraLogicController
        */

        #region ICameraMovementController implementation
        public bool canScrollFurtherForward()
        {
            return Input.GetAxis("ScrollWheel") > 0 && transform.position.y > minimumHeight;
        }

        public bool canScrollFurtherBackward()
        {
            return Input.GetAxis("ScrollWheel") < 0 && transform.position.y < MAXIMUM_HEIGHT;
        }

        public void moveCameraDownwards() { this.transform.position += Vector3.down; }

        public void moveCameraUpwards() { this.transform.position += Vector3.up; }
        #endregion


        #region ICameraTiltController implementation
        public bool canTiltFurtherBackward()
        {
            return transform.position.y <= minimumHeight && this.transform.eulerAngles.x >= FLATTEST_ANGLE;
        }

        public bool canTiltFurtherForward()
        {
            return transform.position.y > minimumHeight && this.transform.eulerAngles.x < startingAngle;
        }

        public void tiltBackward()
        {
            this.transform.eulerAngles = this.transform.eulerAngles + (Vector3.left * 2);
        }

        public void tiltForward()
        {
            this.transform.eulerAngles = this.transform.eulerAngles + (Vector3.right * 2);
        }
        #endregion

        
        private float GetDistanceToMainTerrain()
        {
            //TODO: Define the tag somewhere else so it's synced.
            var mainTerrainCollider = GameObject.FindGameObjectWithTag("MainTerrain").GetComponent<TerrainCollider>();
            var closestPoint = mainTerrainCollider.ClosestPointOnBounds(this.transform.position);

            return Vector3.Distance(this.transform.position, closestPoint);
        }
    }
}