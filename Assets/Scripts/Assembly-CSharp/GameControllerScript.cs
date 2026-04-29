using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{
	public CursorControllerScript cursorController;

	public BossController bossController;

	public float gameTime;

	public PlayerScript player;

	public Transform playerTransform;

	public Transform cameraTransform;

	public new Camera camera;

	private int cullingMask;

	public EntranceScript[] entrances;

	public GameObject baldiTutor;

	public GameObject baldi;

	public BaldiScript baldiScrpt;

	public AudioClip aud_Prize;

	public AudioClip aud_PrizeMobile;

	public AudioClip aud_AllNotebooks;

	public AudioClip aud_AllNotebooksNull;

	public AudioClip aud_NULLhaha;

	public GameObject principal;

	public GameObject crafters;

	public GameObject playtime;

	public PlaytimeScript playtimeScript;

	public GameObject gottaSweep;

	public GameObject bully;

	public GameObject firstPrize;

	public GameObject TestEnemy;

	public FirstPrizeScript firstPrizeScript;

	public GameObject quarter;

	public AudioSource tutorBaldi;

	public RectTransform boots;

	public string style;

	public string mode;

	public int notebooks;

	public GameObject[] notebookPickups;

	public int failedNotebooks;

	public bool spoopMode;

	public bool finaleMode;

	public bool debugMode;

	public bool mouseLocked;

	public int exitsReached;

	public int lastestExit;

	public int itemSelected;

	public int[] item = new int[3];

	public RawImage[] itemSlot = new RawImage[3];

	private string[] itemNames = new string[13]
	{
		"boş", "Kurabiye", "kapı kilidi", "Müdüsün anahtarı", "Roketatar", "50 kurus", "İrem Hoca Anti kasedi", "Saat", "Camsil gıcırtı engelleyicisi", "Orta Parmak",
		"45.5 Ayaklar", "Müdüsün Düdügü", "Sis bombası"
	};

	public TMP_Text itemText;

	public Object[] items = new Object[10];

	public Texture[] itemTextures = new Texture[10];

	public GameObject chalkErasers;

	public GameObject bsodaSpray;

	public GameObject alarmClock;

	public BoxCollider ChalkCloud;

	public CharacterController playerCharacter;

	public TMP_Text notebookCount;

	public GameObject pauseMenu;

	public GameObject highScoreText;

	public GameObject warning;

	public GameObject reticle;

	public RectTransform itemSelect;

	private int[] itemSelectOffset;

	private bool gamePaused;

	private bool learningActive;

	private float gameOverDelay;

	public AudioSource audioDevice;

	public AudioClip aud_Soda;

	public AudioClip aud_Spray;

	public AudioClip aud_Coming;

	public AudioClip aud_Whistle;

	public AudioClip[] aud_buzz;

	public AudioClip aud_spoop;

	public AudioClip aud_Hang;

	public AudioClip ChaosEarlyLoopStart;

	public AudioClip ChaosEarlyLoop;

	public AudioClip ChaosBuildUp;

	public AudioClip ChaosFinalLoop;

	public AudioClip[] escapeVariants;

	public AudioClip aud_Switch;

	public AudioSource schoolMusic;

	public AudioSource learnMusic;

	public AudioSource escapeMusic;

	public string sceneName;

	public GameControllerScript()
	{
		itemSelectOffset = new int[3] { -80, -40, 0 };
	}

	private void Start()
	{
		sceneName = SceneManager.GetActiveScene().name;
		cullingMask = camera.cullingMask;
		audioDevice = GetComponent<AudioSource>();
		mode = PlayerPrefs.GetString("CurrentMode");
		style = PlayerPrefs.GetString("CurrentStyle");
		schoolMusic.Play();
		if ((style == "glitch" || PlayerPrefs.GetInt("FreeRun") >= 1) && sceneName != "Secret")
		{
			schoolMusic.Stop();
			baldiTutor.SetActive(value: false);
			NotebookScript[] array = Resources.FindObjectsOfTypeAll<NotebookScript>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].noMath = true;
			}
		}
		if (style == "glitch" && sceneName != "Secret")
		{
			mode = "story";
			PlayerPrefs.SetInt("FreeRun", 0);
			chalkErasers.SetActive(value: true);
			baldiTutor.SetActive(value: false);
			baldi.SetActive(value: true);
			int layer = LayerMask.NameToLayer("Ignore Raycast");
			GameObject[] array2 = GameObject.FindGameObjectsWithTag("Window");
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].gameObject.layer = layer;
			}
			array2 = GameObject.FindGameObjectsWithTag("Item");
			foreach (GameObject gameObject in array2)
			{
				if (gameObject.name == "Pickup_SafetyScissors" || gameObject.name == "Pickup_BigBoots")
				{
					Object.Destroy(gameObject, 0f);
				}
			}
		}
		if (mode == "endless")
		{
			baldiScrpt.endless = true;
		}
		LockMouse();
		UpdateNotebookCount();
		itemSelected = 0;
		if (sceneName != "Secret")
		{
			if (style != "glitch")
			{
				gameOverDelay = 0.5f;
			}
			else
			{
				gameOverDelay = aud_spoop.length - 5f;
			}
		}
	}

	private void Update()
	{
		if (!learningActive & !player.gameOver)
		{
			gameTime += Time.deltaTime;
			if (Input.GetButtonDown("Pause"))
			{
				if (!gamePaused)
				{
					PauseGame();
				}
				else
				{
					UnpauseGame();
				}
			}
			if (Input.GetKeyDown(KeyCode.Y) & gamePaused)
			{
				ExitGame();
			}
			else if (Input.GetKeyDown(KeyCode.N) & gamePaused)
			{
				UnpauseGame();
			}
			if (!gamePaused & (Time.timeScale != 1f))
			{
				Time.timeScale = 1f;
			}
			if (Input.GetMouseButtonDown(1) && Time.timeScale != 0f)
			{
				UseItem();
			}
			if (Input.GetAxis("Mouse ScrollWheel") > 0f && Time.timeScale != 0f)
			{
				DecreaseItemSelection();
			}
			else if (Input.GetAxis("Mouse ScrollWheel") < 0f && Time.timeScale != 0f)
			{
				IncreaseItemSelection();
			}
			if (Time.timeScale != 0f)
			{
				if (Input.GetKeyDown(KeyCode.Alpha1))
				{
					itemSelected = 0;
					UpdateItemSelection();
				}
				else if (Input.GetKeyDown(KeyCode.Alpha2))
				{
					itemSelected = 1;
					UpdateItemSelection();
				}
				else if (Input.GetKeyDown(KeyCode.Alpha3))
				{
					itemSelected = 2;
					UpdateItemSelection();
				}
			}
		}
		else if (Time.timeScale != 0f)
		{
			Time.timeScale = 0f;
		}
		if ((player.stamina < 0f) & !warning.activeSelf)
		{
			warning.SetActive(value: true);
		}
		else if ((player.stamina > 0f) & warning.activeSelf)
		{
			warning.SetActive(value: false);
		}
		if (player.gameOver)
		{
			if (mode == "endless" && notebooks > PlayerPrefs.GetInt("HighBooks") && !highScoreText.activeSelf)
			{
				highScoreText.SetActive(value: true);
			}
			Time.timeScale = 0f;
			gameOverDelay -= Time.unscaledDeltaTime * 0.5f;
			camera.farClipPlane = gameOverDelay * 400f;
			if (style == "glitch")
			{
				if (!Application.isEditor)
				{
					GameObject[] array = Object.FindObjectsOfType<GameObject>();
					foreach (GameObject gameObject in array)
					{
						if (gameObject.name.StartsWith("Wall") || gameObject.name.StartsWith("Floor") || gameObject.name.StartsWith("Window") || gameObject.name.StartsWith("Ceiling"))
						{
							float num = 0.3f;
							float num2 = Random.Range(0f - num, num);
							gameObject.transform.position = new Vector3(gameObject.transform.position.x + num2, gameObject.transform.position.y + num2, gameObject.transform.position.z + num2);
						}
					}
				}
				camera.fieldOfView += 0.09f;
			}
			if (!audioDevice.isPlaying)
			{
				if (style != "glitch")
				{
					int num3 = Random.Range(0, aud_buzz.Length);
					audioDevice.PlayOneShot(aud_buzz[num3]);
				}
				else
				{
					audioDevice.PlayOneShot(aud_spoop);
				}
			}
			if (gameOverDelay <= 0f)
			{
				if (mode == "endless")
				{
					if (notebooks > PlayerPrefs.GetInt("HighBooks"))
					{
						PlayerPrefs.SetInt("HighBooks", notebooks);
					}
					PlayerPrefs.SetInt("CurrentBooks", notebooks);
				}
				Time.timeScale = 1f;
				if (style != "glitch")
				{
					SceneManager.LoadScene("GameOver");
				}
				else
				{
					SceneManager.LoadScene("MainMenu");
				}
			}
		}
		if (style != "glitch" && finaleMode && !audioDevice.isPlaying && exitsReached == 2)
		{
			audioDevice.clip = ChaosEarlyLoop;
			audioDevice.loop = true;
			audioDevice.Play();
		}
		if (style != "glitch" && finaleMode && !audioDevice.isPlaying && exitsReached == 3)
		{
			audioDevice.clip = ChaosFinalLoop;
			audioDevice.loop = true;
			audioDevice.Play();
		}
	}

	private void UpdateNotebookCount()
	{
		if (mode == "story")
		{
			notebookCount.text = notebooks + "/7 Defterler";
		}
		else
		{
			notebookCount.text = notebooks + " Defterler";
		}
		if ((notebooks == 7) & (mode == "story"))
		{
			ActivateFinaleMode();
		}
	}

	public void CollectNotebook()
	{
		notebooks++;
		UpdateNotebookCount();
	}

	public void LockMouse()
	{
		if (!learningActive)
		{
			cursorController.LockCursor();
			mouseLocked = true;
			reticle.SetActive(value: true);
		}
	}

	public void UnlockMouse()
	{
		cursorController.UnlockCursor();
		mouseLocked = false;
		reticle.SetActive(value: false);
	}

	public void PauseGame()
	{
		if (learningActive)
		{
			return;
		}
		UnlockMouse();
		AudioSource[] array = Object.FindObjectsOfType<AudioSource>();
		Time.timeScale = 0f;
		gamePaused = true;
		pauseMenu.SetActive(value: true);
		if (sceneName != "Secret")
		{
			AudioSource[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].Pause();
			}
		}
	}

	public void ExitGame()
	{
		if ((style == "glitch") & (sceneName != "Secret"))
		{
			UnpauseGame();
			baldi.SetActive(value: true);
			baldiScrpt.agent.Warp(playerTransform.position);
		}
		else
		{
			SceneManager.LoadScene("MainMenu");
		}
	}

	public void UnpauseGame()
	{
		AudioSource[] array = Object.FindObjectsOfType<AudioSource>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].UnPause();
		}
		Time.timeScale = 1f;
		gamePaused = false;
		pauseMenu.SetActive(value: false);
		LockMouse();
	}

	public void ActivateSpoopMode()
	{
		spoopMode = true;
		if (PlayerPrefs.GetInt("FreeRun") == 0)
		{
			EntranceScript[] array = entrances;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Lower();
			}
		}
		if (style != "glitch")
		{
			baldiTutor.SetActive(value: false);
			if (PlayerPrefs.GetInt("FreeRun") == 0)
			{
				baldi.SetActive(value: true);
				audioDevice.PlayOneShot(aud_Hang);
			}
			principal.SetActive(value: true);
			crafters.SetActive(value: true);
			playtime.SetActive(value: true);
			gottaSweep.SetActive(value: true);
			bully.SetActive(value: true);
			firstPrize.SetActive(value: true);
		}
		learnMusic.Stop();
		schoolMusic.Stop();
	}

	private void ActivateFinaleMode()
	{
		finaleMode = true;
		if (PlayerPrefs.GetInt("FreeRun") == 0)
		{
			EntranceScript[] array = entrances;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Raise();
			}
		}
	}

	public void GetAngry(float value)
	{
		if (!spoopMode)
		{
			ActivateSpoopMode();
		}
		baldiScrpt.GetAngry(value);
	}

	public void ActivateLearningGame()
	{
		learningActive = true;
		UnlockMouse();
		tutorBaldi.Stop();
		if (!spoopMode & (style != "glitch"))
		{
			schoolMusic.Stop();
			learnMusic.Play();
		}
	}

	public void DeactivateLearningGame(GameObject subject)
	{
		camera.cullingMask = cullingMask;
		learningActive = false;
		Object.Destroy(subject);
		LockMouse();
		if (player.stamina < 100f)
		{
			player.stamina = 100f;
		}
		if (PlayerPrefs.GetInt("FreeRun") == 0)
		{
			if (!spoopMode & (style != "glitch"))
			{
				schoolMusic.Play();
				learnMusic.Stop();
			}
			if ((notebooks == 1) & !spoopMode & (style != "glitch"))
			{
				quarter.SetActive(value: true);
				tutorBaldi.PlayOneShot(aud_Prize);
			}
			else if ((notebooks == 7) & (mode == "story") & (style != "glitch"))
			{
				escapeMusic.clip = escapeVariants[0];
				escapeMusic.loop = true;
				escapeMusic.Play();
				if (PlayerPrefs.GetInt("NullDefeated") >= 1)
				{
					audioDevice.PlayOneShot(aud_AllNotebooks, 0.8f);
				}
				else
				{
					audioDevice.PlayOneShot(aud_AllNotebooksNull, 0.8f);
				}
			}
		}
		else if ((notebooks == 7) & (mode == "story") & (style != "glitch"))
		{
			escapeMusic.clip = escapeVariants[1];
			escapeMusic.loop = true;
			escapeMusic.Play();
		}
	}

	private void IncreaseItemSelection()
	{
		itemSelected++;
		if (itemSelected > 2)
		{
			itemSelected = 0;
		}
		itemSelect.anchoredPosition = new Vector3(itemSelectOffset[itemSelected], 0f, 0f);
		UpdateItemName();
	}

	private void DecreaseItemSelection()
	{
		itemSelected--;
		if (itemSelected < 0)
		{
			itemSelected = 2;
		}
		itemSelect.anchoredPosition = new Vector3(itemSelectOffset[itemSelected], 0f, 0f);
		UpdateItemName();
	}

	private void UpdateItemSelection()
	{
		itemSelect.anchoredPosition = new Vector3(itemSelectOffset[itemSelected], 0f, 0f);
		UpdateItemName();
	}

	public void CollectItem(int item_ID)
	{
		if (item[0] == 0)
		{
			item[0] = item_ID;
			itemSlot[0].texture = itemTextures[item_ID];
		}
		else if (item[1] == 0)
		{
			item[1] = item_ID;
			itemSlot[1].texture = itemTextures[item_ID];
		}
		else if (item[2] == 0)
		{
			item[2] = item_ID;
			itemSlot[2].texture = itemTextures[item_ID];
		}
		else
		{
			item[itemSelected] = item_ID;
			itemSlot[itemSelected].texture = itemTextures[item_ID];
		}
		UpdateItemName();
	}

	private void UseItem()
	{
		if (item[itemSelected] == 0)
		{
			return;
		}
		if (item[itemSelected] == 1)
		{
			player.stamina = player.maxStamina * 2f;
			ResetItem();
		}
		else if (item[itemSelected] == 2)
		{
			if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f)), out var hitInfo) && ((hitInfo.collider.tag == "SwingingDoor") & (Vector3.Distance(playerTransform.position, hitInfo.transform.position) <= 10f)))
			{
				hitInfo.collider.gameObject.GetComponent<SwingingDoorScript>().LockDoor(15f);
				ResetItem();
			}
		}
		else if (item[itemSelected] == 3)
		{
			if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f)), out var hitInfo2) && ((hitInfo2.collider.tag == "Door") & (Vector3.Distance(playerTransform.position, hitInfo2.transform.position) <= 10f)))
			{
				DoorScript component = hitInfo2.collider.gameObject.GetComponent<DoorScript>();
				if (component.DoorLocked)
				{
					component.UnlockDoor();
					component.OpenDoor();
					ResetItem();
				}
			}
		}
		else if (item[itemSelected] == 4)
		{
			Object.Instantiate(bsodaSpray, playerTransform.position, cameraTransform.rotation);
			ResetItem();
			player.ResetGuilt("drink", 1f);
			audioDevice.PlayOneShot(aud_Soda);
		}
		else if (item[itemSelected] == 5)
		{
			if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f)), out var hitInfo3))
			{
				if ((hitInfo3.collider.name == "BSODAMachine") & (Vector3.Distance(playerTransform.position, hitInfo3.transform.position) <= 10f))
				{
					ResetItem();
					CollectItem(4);
				}
				else if ((hitInfo3.collider.name == "ZestyMachine") & (Vector3.Distance(playerTransform.position, hitInfo3.transform.position) <= 10f))
				{
					ResetItem();
					CollectItem(1);
				}
				else if ((hitInfo3.collider.name == "PayPhone") & (Vector3.Distance(playerTransform.position, hitInfo3.transform.position) <= 10f))
				{
					hitInfo3.collider.gameObject.GetComponent<TapePlayerScript>().Play();
					ResetItem();
				}
			}
		}
		else if (item[itemSelected] == 6)
		{
			if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f)), out var hitInfo4) && ((hitInfo4.collider.name == "TapePlayer") & (Vector3.Distance(playerTransform.position, hitInfo4.transform.position) <= 10f)))
			{
				hitInfo4.collider.gameObject.GetComponent<TapePlayerScript>().Play();
				ResetItem();
			}
		}
		else if (item[itemSelected] == 7)
		{
			Object.Instantiate(alarmClock, playerTransform.position, cameraTransform.rotation).GetComponent<AlarmClockScript>().baldi = baldiScrpt;
			ResetItem();
		}
		else if (item[itemSelected] == 8)
		{
			if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f)), out var hitInfo5) && ((hitInfo5.collider.tag == "Door") & (Vector3.Distance(playerTransform.position, hitInfo5.transform.position) <= 10f)))
			{
				hitInfo5.collider.gameObject.GetComponent<DoorScript>().SilenceDoor();
				ResetItem();
				audioDevice.PlayOneShot(aud_Spray);
			}
		}
		else if (item[itemSelected] == 9)
		{
			Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
			RaycastHit hitInfo6;
			if (player.jumpRope)
			{
				player.DeactivateJumpRope();
				playtimeScript.Disappoint();
				ResetItem();
			}
			else if (Physics.Raycast(ray, out hitInfo6) && hitInfo6.collider.name == "1st Prize")
			{
				firstPrizeScript.GoCrazy();
				ResetItem();
			}
		}
		else if (item[itemSelected] == 10)
		{
			player.ActivateBoots();
			StartCoroutine(BootAnimation());
			ResetItem();
		}
		else if (item[itemSelected] == 11)
		{
			audioDevice.PlayOneShot(aud_Whistle);
			if (style != "glitch")
			{
				if (principal.activeSelf)
				{
					principal.GetComponent<PrincipalScript>().audioQueue.QueueAudio(aud_Coming);
					principal.GetComponent<PrincipalScript>().Summoned();
				}
			}
			else if (baldi.activeSelf)
			{
				baldiScrpt.baldiWait = 0.005f;
				baldiScrpt.summon = true;
			}
			ResetItem();
		}
		else if (item[itemSelected] == 12)
		{
			BoxCollider boxCollider = Object.Instantiate(ChalkCloud, playerTransform.position, Quaternion.identity);
			Physics.IgnoreCollision(boxCollider, playerCharacter);
			boxCollider.gameObject.transform.position += Vector3.up * 1f;
			ResetItem();
		}
	}

	private IEnumerator BootAnimation()
	{
		float time = 15f;
		float height = 375f;
		boots.gameObject.SetActive(value: true);
		Vector3 localPosition;
		while (height > -375f)
		{
			height -= 375f * Time.deltaTime;
			time -= Time.deltaTime;
			localPosition = boots.localPosition;
			localPosition.y = height;
			boots.localPosition = localPosition;
			yield return null;
		}
		localPosition = boots.localPosition;
		localPosition.y = -375f;
		boots.localPosition = localPosition;
		boots.gameObject.SetActive(value: false);
		while (time > 0f)
		{
			time -= Time.deltaTime;
			yield return null;
		}
		boots.gameObject.SetActive(value: true);
		while (height < 375f)
		{
			height += 375f * Time.deltaTime;
			localPosition = boots.localPosition;
			localPosition.y = height;
			boots.localPosition = localPosition;
			yield return null;
		}
		localPosition = boots.localPosition;
		localPosition.y = 375f;
		boots.localPosition = localPosition;
		boots.gameObject.SetActive(value: false);
	}

	private void ResetItem()
	{
		item[itemSelected] = 0;
		itemSlot[itemSelected].texture = itemTextures[0];
		UpdateItemName();
	}

	public void LoseItem(int id)
	{
		item[id] = 0;
		itemSlot[id].texture = itemTextures[0];
		UpdateItemName();
	}

	private void UpdateItemName()
	{
		itemText.text = itemNames[item[itemSelected]];
	}

	public void ExitReached()
	{
		if (exitsReached < entrances.Length - 1)
		{
			audioDevice.PlayOneShot(aud_Switch, 0.8f);
		}
		escapeMusic.pitch -= 0.12f;
		exitsReached++;
		if ((exitsReached == 1) & (style != "glitch"))
		{
			RenderSettings.ambientLight = Color.red;
		}
		if ((exitsReached == 2) & (style != "glitch"))
		{
			audioDevice.volume = 0.8f;
			audioDevice.clip = ChaosEarlyLoopStart;
			audioDevice.loop = false;
			audioDevice.Play();
		}
		if (exitsReached == entrances.Length - 1)
		{
			if (style != "glitch")
			{
				audioDevice.clip = ChaosBuildUp;
				audioDevice.loop = false;
				audioDevice.Play();
			}
			else
			{
				debugMode = true;
				baldiScrpt.enabled = false;
				bossController.ns.enabled = true;
				bossController.ns.target = principal.transform;
				bossController.ns.agent.speed = 100f;
			}
		}
		if ((exitsReached == entrances.Length) & (style == "glitch"))
		{
			StartCoroutine(bossController.WaitForNULL());
		}
	}

	public void DespawnCrafters()
	{
		crafters.SetActive(value: false);
	}

	public void Fliparoo()
	{
		player.height = 6f;
		player.fliparoo = 180f;
		player.flipaturn = -1f;
		Camera.main.GetComponent<CameraScript>().offset = new Vector3(0f, -1f, 0f);
	}
}
