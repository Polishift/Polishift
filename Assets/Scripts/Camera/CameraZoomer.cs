using Assets.Scripts.Camera;
using System;
using UnityEngine;

namespace Assets.Scripts.Camera
{
    public class CameraZoomer : MonoBehaviour, ICameraMovementController, ICameraTiltController
    {
        private CameraLogicController cameraLogicController;

        private const int MINIMUM_HEIGHT = 20; //let this be determined by terrain
        private const int MAXIMUM_HEIGHT = 50;

        private const float startingAngle = 89f; //Culprit because of rounding. 
        private const float FLATTEST_ANGLE = 40f;
        private const float HORIZONTAL_ACCELARATION = 1.25f;


        void Start()
        {
            this.cameraLogicController = new CameraLogicController();

            //we implemented both interfaces here, so this is legal
            cameraLogicController.SetCameraMovementController(this);
            cameraLogicController.SetCameraTiltController(this);
        }

        void Update()
        {
            cameraLogicController.UpdateForwardTilt();
            cameraLogicController.UpdateCameraPosition();
        }


        #region ICameraMovementController implementation
        public bool canScrollFurtherForward()
        {
            return Input.GetAxis("ScrollWheel") > 0 && transform.position.y > MINIMUM_HEIGHT;
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
            Debug.Log("canTiltFurtherBackward this.transform.eulerAngles.x = " + this.transform.eulerAngles.x);
            return transform.position.y <= MINIMUM_HEIGHT && this.transform.eulerAngles.x >= FLATTEST_ANGLE;
        }

        public bool canTiltFurtherForward()
        {
            Debug.Log("canTiltFurtherForward this.transform.eulerAngles.x = " + this.transform.eulerAngles.x);
            return transform.position.y > MINIMUM_HEIGHT && this.transform.eulerAngles.x < startingAngle;
        }

        public void tiltBackward()
        {
            this.transform.eulerAngles = this.transform.eulerAngles + Vector3.left;
        }

        public void tiltForward()
        {
            this.transform.eulerAngles = this.transform.eulerAngles + Vector3.right;
        }
        #endregion
    }
}