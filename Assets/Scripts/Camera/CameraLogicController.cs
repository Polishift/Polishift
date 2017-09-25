using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Camera
{
    [Serializable]
    public class CameraLogicController
    {
        private ICameraMovementController cameraMovementController;
        private ICameraTiltController cameraTiltController;


        public void UpdateForwardTilt()
        {
            if (cameraTiltController.canTiltFurtherBackward())
            {
                cameraTiltController.tiltBackward();
            }
            else if (cameraTiltController.canTiltFurtherForward())
            {
                cameraTiltController.tiltForward();
            }
        }

        public void UpdateCameraPosition()
        {
            if (cameraMovementController.canScrollFurtherForward())
            {
                cameraMovementController.moveCameraDownwards();
            }
            else if (cameraMovementController.canScrollFurtherBackward())
            {
                cameraMovementController.moveCameraUpwards();
            }
        }


        /*
            Setters
        */  

        public void SetCameraMovementController(ICameraMovementController cameraMovementController)
        {
            this.cameraMovementController = cameraMovementController;
        }

        public void SetCameraTiltController(ICameraTiltController cameraTiltController)
        {
            this.cameraTiltController = cameraTiltController;
        }
    }

}
