using UnityEngine;

public class MoreExpressionBodiedMembersTest : MonoBehaviour
{
	private class Foo
	{
		private int value;

		public int Id
		{
			get => value;
			set => this.value = value;
		}

		public Foo() => value = 42;
		~Foo() => Debug.Log("Good bye!");
	}

	private void Start()
	{
		Debug.Log("<color=yellow>More Expression Bodied Members:</color>");

		var foo = new Foo();
		Debug.Log(foo.Id);

		Debug.Log("");
	}
}