using UnityEngine;

public class LocalFunctionsTest : MonoBehaviour
{
    void Start()
    {
        Debug.Log("<color=yellow>Local Functions:</color>");

        var suffix = "!";
        Print(10);
        Debug.Log("");

        void Print(int i) => Debug.Log("Printed from a local function: " + i + suffix);
    }
}