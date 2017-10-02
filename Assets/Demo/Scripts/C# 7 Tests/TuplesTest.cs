using UnityEngine;

public class TuplesTest : MonoBehaviour
{
    (int doubled, int squared, string text) GetTuple(int x) => (x * 2, x * x, x.ToString() + "!");

    void Start()
    {
        Debug.Log("<color=yellow>Tuples and Deconstruction:</color>");

        var i = 10;
        var tuple = GetTuple(i);

        Debug.Log($"{i} -> (doubled: {tuple.doubled}, squared: {tuple.squared}, text: \"{tuple.text}\")");

        (int x, int y) t1 = (x: 10, y: 20);
        Debug.Log($"(int x, int y) t1 = (x: 10, y: 20) -> ({t1.x}, {t1.y})");

#pragma warning disable CS8123 // The tuple element name is ignored because a different name is specified by the assignment target.
        (int x, int y) t2 = (y: 10, x: 20);
        Debug.Log($"(int x, int y) t2 = (y: 10, x: 20) -> ({t2.x}, {t2.y})");

        (int x, int y) t3 = (u: 10, v: 20);
        Debug.Log($"(int x, int y) t3 = (u: 10, v: 20) -> ({t3.x}, {t3.y})");
#pragma warning restore CS8123 // The tuple element name is ignored because a different name is specified by the assignment target.

        Debug.Log("");

        // Deconstruction
        // #1 - into new variables
        (int first, int second, string third) = tuple;
        Debug.Log($"first = {first}, second = {second}, third = {third}");

        // #2
        var (primero, segundo, tercero) = tuple;
        Debug.Log($"primero = {primero}, segundo = {segundo}, tercero = {tercero}");

        // #3 - into existing variables
        int a, b = 42;
        string text = "some text";
        (a, b, text) = tuple;
        Debug.Log($"a = {a}, b = {b}, text = {text}");

        // #4 - custom deconstructor 
        var vector = new Vector3(1, 2.5f, 3);
        var (x, _, y) = vector;
        Debug.Log($"3D: {vector} => 2D: x = {x}, y = {y}");

        Debug.Log("");
    }
}

static class Vector3Extensions
{
    public static void Deconstruct(this Vector3 vector, out float x, out float y, out float z)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }
}