using UnityEngine;

public class StyleButton : MonoBehaviour
{
	public enum Style
	{
		Classic = 0,
		Glitch = 1
	}

	public Style currentStyle;

	public void SetStyle()
	{
		PlayerPrefs.SetString("CurrentStyle", currentStyle.ToString().ToLower());
	}
}
