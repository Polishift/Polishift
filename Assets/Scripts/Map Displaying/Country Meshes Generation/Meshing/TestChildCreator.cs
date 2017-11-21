using System.Collections;
using System.Collections.Generic;
using MeshesGeneration;
using UnityEngine;

public class TestChildCreator : MonoBehaviour
{
    void Start()
    {
        var thisMesh = gameObject.GetComponent<MeshFilter>().mesh;
        
        //Adding child mesh
        var childsName = gameObject.name + " outline";  
        
        GameObject newChildObject = new GameObject() {name = childsName };
        newChildObject.transform.parent = gameObject.transform;

        newChildObject.AddComponent<MeshFilter>();
        newChildObject.AddComponent<MeshRenderer>();
        newChildObject.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Diffuse"));

        newChildObject.GetComponent<MeshFilter>().mesh = thisMesh;    
        
        
        //Changing color
        transform.Find(childsName).GetComponent<MeshRenderer>().material.color = Color.red;
            
        //Scaling
    }
}