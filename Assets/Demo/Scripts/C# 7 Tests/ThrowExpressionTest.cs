using System;
using UnityEngine;

public class ThrowExpressionTest : MonoBehaviour
{
	private void Start()
	{
		Debug.Log("<color=yellow>Throw Expression:</color>");

		Debug.Log($"8 / 2 = {Divide(8, 2)}");

		Debug.Log("");
	}

	private int Divide(int a, int b) => (b != 0) ? a / b : throw new ArgumentNullException("Division by zero");
}