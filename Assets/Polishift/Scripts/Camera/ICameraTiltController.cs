using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Camera
{
    public interface ICameraTiltController 
    {
        bool canTiltFurtherBackward();
        bool canTiltFurtherForward();

        void tiltBackward();
        void tiltForward();
    }
}
