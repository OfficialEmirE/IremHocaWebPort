using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
	public CharacterController cc;

	public float walkSpeed;

	public float runSpeed;

	public float stamina;

	public float staminaDrop;

	public float staminaRise;

	public float staminaMax;

	private float sensitivity;

	private float mouseSensitivity;

	private bool running;

	private void Awake()
	{
		mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity");
	}

	private void Start()
	{
		stamina = staminaMax;
		Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }

	private void Update()
	{
        // Esc tuşuna basınca sahneyi değiştir
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Buraya gitmek istediğin sahnenin adını yaz (Örn: "Menu" veya "MainScene")
            SceneManager.LoadSceneAsync("MainMenu");
        }

        running = Input.GetButton("Run");
		MouseMove();
		PlayerMove();
		StaminaUpdate();
	}

    private void MouseMove()
    {
        // Hassasiyet 0 ise (veya atanmamışsa) 1.0f yap ki çarpım bozulmasın
        if (mouseSensitivity <= 0) mouseSensitivity = 1.0f;

        // Döndürme miktarını hesapla
        float rotationAmount = Input.GetAxis("Mouse X") * 100f * mouseSensitivity * Time.deltaTime;

        // Direkt transform üzerine ekleyerek döndür (en güvenli yol)
        base.transform.Rotate(0f, rotationAmount, 0f);
    }

    private void PlayerMove()
	{
		float num = walkSpeed;
		if ((stamina > 0f) & running)
		{
			num = runSpeed;
		}
		Vector3 vector = base.transform.right * Input.GetAxis("Strafe");
		Vector3 vector2 = base.transform.forward * Input.GetAxis("Forward");
		sensitivity = Mathf.Clamp((vector + vector2).magnitude, 0f, 1f);
		cc.Move((vector + vector2).normalized * num * sensitivity * Time.deltaTime);
	}

	public void StaminaUpdate()
	{
		if (cc.velocity.magnitude > cc.minMoveDistance)
		{
			if (running)
			{
				stamina = Mathf.Max(stamina - staminaDrop * Time.deltaTime, 0f);
			}
		}
		else if (stamina < staminaMax)
		{
			stamina += staminaRise * Time.deltaTime;
		}
	}
}
