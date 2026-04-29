using TMPro;
using UnityEngine;

public class StyleChalkText : MonoBehaviour
{
	[SerializeField]
	private TMP_Text StyleText;

	private void Awake()
	{
		Check();
	}

	private void Update()
	{
		Check();
	}

	private void Check()
	{
		if (PlayerPrefs.GetString("CurrentStyle") == "classic")
		{
			StyleText.text = "Hikaye Mod";
		}
		else if (PlayerPrefs.GetString("CurrentStyle") == "glitch")
		{
			if (PlayerPrefs.GetInt("NullDefeated") >= 1)
			{
				StyleText.text = "Glitch Style";
			}
			else
			{
				StyleText.text = "Pembe Mod";
			}
		}
	}
}
