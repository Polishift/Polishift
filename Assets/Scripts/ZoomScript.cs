using UnityEngine;

public class ZoomScript : MonoBehaviour
{
    public const int MINIMUM_HEIGHT = 20; //let this be determined by terrain
    public const int MAXIMUM_HEIGHT = 50;

    private const float STEEPEST_ANGLE = 50f;
    private const float FLATTEST_ANGLE = 36f;
    private const float HORIZONTAL_ACCELARATION = 1.25f;

    private float currentHeight;

    void Start()
    {
    }

    void Update()
    {
        updateForwardTilt();
        updateCameraPosition();
    }


    private void updateForwardTilt()
    {
        var currentForwardTilt = this.transform.eulerAngles.x;
        if (currentHeight < 30 && currentForwardTilt >= FLATTEST_ANGLE)
        {
            this.transform.eulerAngles = this.transform.eulerAngles + Vector3.left; //flatter
        }
        else if (currentHeight >= 30 && currentForwardTilt < STEEPEST_ANGLE)
        {
            this.transform.eulerAngles = this.transform.eulerAngles + Vector3.right; //steeper
        }
    }

    private void updateCameraPosition()
    {
        currentHeight = this.transform.position.y;

        if (canScrollFurtherForward())
        {
            this.transform.position += Vector3.down;
        }
        else if (canScrollFurtherBackward())
        {
            this.transform.position += Vector3.up;
        }
    }

    private bool canScrollFurtherForward()
    {
        return Input.GetAxis("ScrollWheel") > 0 && currentHeight > MINIMUM_HEIGHT;
    }

    private bool canScrollFurtherBackward()
    {
        return Input.GetAxis("ScrollWheel") < 0 && currentHeight < MAXIMUM_HEIGHT;
    }
}
