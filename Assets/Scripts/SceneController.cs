using UnityEngine;
using System.Collections;

public class SceneController : MonoBehaviour {


	public void LoadScene(string sceneName)
	{
		if (sceneName == "")
		{
			sceneName = Application.loadedLevelName;
		}
		Application.LoadLevel (sceneName);
	}

}
