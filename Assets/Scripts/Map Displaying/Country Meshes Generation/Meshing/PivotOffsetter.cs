using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public class PivotOffsetter : MonoBehaviour
    {
        public void Start()
        {
            //Get the vertices from the gameObject
            Vector3[] objectVerts = gameObject.GetComponent <MeshFilter>().mesh.vertices;
            //Initialize an offset
            Vector3 offset = Vector3.zero;
            //Loop through our vertices and add them to our offset
            for(int i = 0; i < objectVerts.Length; i++)
            {
                offset += objectVerts[i];    
            }
            //Divide our offset by the amount of vertices in the gameObject (Getting the average)
            offset = offset / objectVerts.Length;
            //Loop through our vertices and subtract the offset
            for(int i = 0; i < objectVerts.Length; i++)
            {
                objectVerts[i] -= offset;
            }

            //Assign the modified vertices to our gameObeject
            gameObject.GetComponent<MeshFilter>().mesh.vertices = objectVerts;
            
            gameObject.GetComponent<MeshFilter>().mesh.RecalculateNormals();
            gameObject.GetComponent<MeshFilter>().mesh.RecalculateBounds();

            
            //Adjust the position of our gameObject to offset the vertices being offset. lol
            gameObject.transform.position += offset;
        }
    }
}