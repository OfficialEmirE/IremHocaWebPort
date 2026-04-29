using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
	public UIController uc;

	public Selectable firstButton;

	public Selectable dummyButtonPC;

	public Selectable dummyButtonElse;

	public GameObject back;

	private void Start()
	{
	}

	public void OnEnable()
	{
		uc.firstButton = firstButton;
		uc.dummyButtonPC = dummyButtonPC;
		uc.dummyButtonElse = dummyButtonElse;
		uc.SwitchMenu();
	}

	private void Update()
	{
		if (Input.GetButtonDown("Cancel") && back != null)
		{
			back.SetActive(value: true);
			base.gameObject.SetActive(value: false);
		}
	}
}
