using UnityEngine;

public class PatternMatchingTest : MonoBehaviour
{
	void Start()
	{
		Debug.Log("<color=yellow>Pattern Matching:</color>");

		var go = new GameObject("Mario");
		go.hideFlags = HideFlags.HideAndDontSave;

		Match(go);
		Match(1);
		Match(3d);
		Match(8f);
		Match(12f);
		Match("hi");
		Match("hello world");
		Match(null);

		Debug.Log("");
	}

	private void Match(object o)
	{
		// Pattern matching
		if (o is GameObject go)
		{
			Debug.Log($"o is GameObject, name={go.name}");
			return;
		}

		if (o is 8f)
		{
			Debug.Log("o is 8.0f");
			return;
		}

		if (o is string s && s.Length > 5)
		{
			Debug.Log($"o is a long string: \"{s}\"");
			return;
		}

		switch (o)
		{
			case 1: // Constant pattern
				Debug.Log("o is 1");
				break;

			case int d: // Type pattern
				Debug.Log($"o is an int: {d}");
				break;

			case float f when f > 10:
				Debug.Log($"o is a float greater than 10: {f}");
				break;

			case var some when some != null: // Var pattern
				Debug.Log($"o is some object: {some}");
				break;

			default:
				Debug.Log("o is something else");
				break;
		}
	}
}