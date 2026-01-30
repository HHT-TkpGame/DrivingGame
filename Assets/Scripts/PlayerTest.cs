using UnityEngine;

public class PlayerTest : MonoBehaviour
{
	[SerializeField] float moveSpeed = 5f;
	[SerializeField] float rotateSpeed = 120f;

	CharacterController characterController;
	private void Start()
	{
		characterController = GetComponent<CharacterController>();
	}
	void Update()
	{
		Move();
		Rotate();
	}

	void Move()
	{
		float moveX = 0f;
		if (Input.GetKey(KeyCode.W))
		{
			moveX = 1f;
		}
		else if (Input.GetKey(KeyCode.S))
		{
			moveX = -1f;
		}
		Vector3 move = transform.right * moveSpeed * moveX;
		characterController.Move(move*Time.deltaTime);
	}
	void Rotate()
	{
		float rotateY = 0f;

		if (Input.GetKey(KeyCode.D))
			rotateY = 1f;
		else if (Input.GetKey(KeyCode.A))
			rotateY = -1f;

		transform.Rotate(Vector3.up, rotateY * rotateSpeed * Time.deltaTime);
	}
		private void OnTriggerEnter(Collider other)
	{
		Debug.Log("ddwdw"+other.name);
	}
	private void OnTriggerExit(Collider other)
	{
		Debug.Log("íEèo" + other.name);
	}
}
