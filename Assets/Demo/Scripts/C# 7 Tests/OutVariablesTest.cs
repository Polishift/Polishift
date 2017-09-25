using UnityEngine;

public class OutVariablesTest : MonoBehaviour
{
	void Start()
	{
		Debug.Log("<color=yellow>Out variables:</color>");

		var s = "100";
		var t = "200";
		if (int.TryParse(s, out int i))
		{
			int.TryParse(t, out int j);
			Debug.Log($"\"{s}\" => {i}, \"{t}\" => {j}");
		}

		var foo = "foo";
		if (int.TryParse(foo, out _))
		{
			Debug.Log($"\"{foo}\" is a number");
		}
		else
		{
			Debug.Log($"\"{foo}\" is not a number");
		}

		Debug.Log("");
	}
}