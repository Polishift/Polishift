using UnityEngine;
using UnityEngine.Assertions.Comparers;

namespace Map_Displaying.Country_Meshes_Generation.Meshing
{
    public class BorderOutlineCreator : MonoBehaviour
    {
        //Used in scaling the country vs. its border outline
        private readonly int XSIZE_DIVIDER = 100;
        private readonly float TINY_RELATIVE_XSIZE = 0.001f;
        private readonly float SMALL_RELATIVE_XSIZE = 0.6f;        
        private readonly float LARGE_RELATIVE_XSIZE = 2f;
        private readonly float HUGE_RELATIVE_XSIZE =  60f;
        
        private Mesh _currentMesh;
        private GameObject _outlineObject;

        private void Start()
        {
            _currentMesh = gameObject.GetComponent<MeshFilter>().mesh;

            //Adding child mesh. We do this first because the border outline needs to be bigger than the scaled-down original
            CreateOutlineMesh();

            /* Temporarily making the outline the parent so we can scale down 
            the country mesh to be smaller whilst not scaling down the outline */
            MakeOutlineParent();

            //Scaling original down so the borders appear around it
            ScaleDownCountryMesh();

            MakeOutlineChildAgain();
        }

        private void CreateOutlineMesh()
        {
            var outlineObjectName = gameObject.name + " outline";

            GameObject borderOutlineObject = new GameObject() {name = outlineObjectName};
            this._outlineObject = borderOutlineObject;
            borderOutlineObject.transform.position = gameObject.transform.position;

            borderOutlineObject.AddComponent<MeshFilter>();
            borderOutlineObject.AddComponent<MeshRenderer>();

            borderOutlineObject.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Diffuse"));
            borderOutlineObject.GetComponent<MeshRenderer>().material.color = Color.black;
            borderOutlineObject.GetComponent<MeshFilter>().mesh = _currentMesh;
        }

        private void MakeOutlineParent()
        {
            this.gameObject.transform.parent = _outlineObject.gameObject.transform;
        }

        private void ScaleDownCountryMesh()
        {
            var scaleDown = DetermineScaleDownFactor();
            
            gameObject.transform.localScale = new Vector3(scaleDown, scaleDown, 0);

            //Setting Z to be just a bit in front of the border mesh so there's no blurring of colors
            gameObject.transform.localPosition = new Vector3(0, 0, -0.01f);
        }

        private float DetermineScaleDownFactor()
        {
            var scalingFactorsPerCountrySize = new float [4] { 0.94f,  0.985f, 0.99f, 0.9995f };
            
            var xySizeOfCountryMesh = GetXYSizeOfMesh();
            var xSizeFactor = xySizeOfCountryMesh.x / XSIZE_DIVIDER;
            
            //tiny country
            if (xSizeFactor > TINY_RELATIVE_XSIZE && xSizeFactor < SMALL_RELATIVE_XSIZE)
            {
                return scalingFactorsPerCountrySize[0];
            }
            //small country
            else if (xSizeFactor > SMALL_RELATIVE_XSIZE && xSizeFactor < LARGE_RELATIVE_XSIZE )
            {
                return scalingFactorsPerCountrySize[1];
            }
            //large country
            else if (xSizeFactor > LARGE_RELATIVE_XSIZE && xSizeFactor < HUGE_RELATIVE_XSIZE )
            {
                return scalingFactorsPerCountrySize[2];
            }
            //huge country
            else
            {
                return scalingFactorsPerCountrySize[3];
            }
        }

        private Vector2 GetXYSizeOfMesh()
        {
            var meshBounds = gameObject.GetComponent<MeshFilter>().mesh.bounds;
            float xSize = meshBounds.max.x - meshBounds.min.x;
            float ySize = meshBounds.max.y - meshBounds.min.y;
            
            return new Vector2(xSize, ySize);
        }
        
        //todo implement this
        private void MakeOutlineChildAgain()
        {
            this.gameObject.transform.parent = null;
            GameObject.Find(_outlineObject.name).transform.parent = gameObject.transform;
        }
    }
}