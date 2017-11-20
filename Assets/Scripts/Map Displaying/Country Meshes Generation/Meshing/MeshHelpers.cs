using UnityEngine;

namespace MeshesGeneration
{
    public static class MeshHelpers
    {
        public static Mesh ChangeMeshColor(Color newColor, Mesh mesh)
        {
            Vector3[] vertices = mesh.vertices;

            // create new colors array where the colors will be created.
            Color[] colors = new Color[vertices.Length];

            for (int i = 0; i < vertices.Length; i++)
                colors[i] = Color.Lerp(Color.red, Color.green, vertices[i].y);

            // assign the array of colors to the Mesh.
            mesh.colors = colors;

            return mesh;
        }
    }
}