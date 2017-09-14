using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Camera
{
    public interface ICameraTiltController //Interface so we can mock these methods and provide differing implementations
    {
        bool canTiltFurtherBackward();
        bool canTiltFurtherForward();

        void tiltBackward();
        void tiltForward();
    }
}
