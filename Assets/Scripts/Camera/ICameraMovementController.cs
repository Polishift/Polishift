using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Camera
{
    public interface ICameraMovementController //Interface so we can mock these methods and provide differing implementations
    {
        bool canScrollFurtherForward();
        bool canScrollFurtherBackward();

        void moveCameraDownwards();
        void moveCameraUpwards();
    }
}
