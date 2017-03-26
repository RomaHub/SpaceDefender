using UnityEngine;
using UnityEngine.Networking;

public class PlayerMove : NetworkBehaviour
{

	[SerializeField]
	private float _movementSpeed;

	private float _spaceWidth;

	private Rigidbody2D _rigidBody2D;

    private void Start()
	{

		_spaceWidth = Camera.main.orthographicSize;

		if(!isLocalPlayer)
		{
			Destroy(this);
			return;
		}

		_rigidBody2D = GetComponent<Rigidbody2D>();

	}

    private void FixedUpdate()
	{
		
		float movementInputValue = Input.GetAxis("Horizontal");

		Vector3 deltaTranslation = transform.position + transform.right * movementInputValue * _movementSpeed * Time.deltaTime;
		if(Mathf.Abs(deltaTranslation.x) <= Mathf.Abs(_spaceWidth))
			_rigidBody2D.MovePosition(deltaTranslation);

	}

}
