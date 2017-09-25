using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Camera
{
    public interface ICameraMovementController 
    {
        bool canScrollFurtherForward();
        bool canScrollFurtherBackward();

        void moveCameraDownwards();
        void moveCameraUpwards();
    }
}
