using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ExtendedFlycam : MonoBehaviour
{
 
	/*
	EXTENDED FLYCAM
		Desi Quintans (CowfaceGames.com), 17 August 2012.
		Based on FlyThrough.js by Slin (http://wiki.unity3d.com/index.php/FlyThrough), 17 May 2011.
 
	LICENSE
		Free as in speech, and free as in beer.
 
	FEATURES
		WASD/Arrows:    Movement
		          Q:    Climb
		          E:    Drop
                      Shift:    Move faster
                    Control:    Move slower
                        End:    Toggle cursor locking to screen (you can also press Ctrl+P to toggle play mode on and off).
	*/
 
	[Header("Movement properties: ")]
	[SerializeField] private float cameraSensitivity = 90;
	[SerializeField] private float climbSpeed = 4;
	[SerializeField] private float normalMoveSpeed = 10;
	[SerializeField] private float slowMoveFactor = 0.25f;
	[SerializeField] private float fastMoveFactor = 3;

	[Header("Camera")] 
	[SerializeField] private Camera myCamera;

	private float rotationX;
	private float rotationY;

	private bool menuIsActive;

	private void Start () {
		Cursor.visible = true;
	}
 
	private void Update () {
		if(!menuIsActive) {
			if (Input.GetMouseButton(1)) {
				rotationX += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
				rotationY += Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
				rotationY = Mathf.Clamp(rotationY, -90, 90);

				Quaternion localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
				localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);
				transform.localRotation = localRotation;
			}

			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
				Transform cameraTransform = transform;
				Vector3 position = cameraTransform.position;
				position += cameraTransform.forward *
				            (normalMoveSpeed * fastMoveFactor * Input.GetAxis("Vertical") * Time.deltaTime);
				position += transform.right *
				            (normalMoveSpeed * fastMoveFactor * Input.GetAxis("Horizontal") * Time.deltaTime);
				transform.position = position;
			}
			else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) {
				Transform cameraTransform = transform;
				Vector3 position = cameraTransform.position;
				position += cameraTransform.forward *
				            (normalMoveSpeed * slowMoveFactor * Input.GetAxis("Vertical") * Time.deltaTime);
				position += transform.right *
				            (normalMoveSpeed * slowMoveFactor * Input.GetAxis("Horizontal") * Time.deltaTime);
				transform.position = position;
			}
			else {
				Transform cameraTransform = transform;
				Vector3 position = cameraTransform.position;
				position += cameraTransform.forward * (normalMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime);
				position += transform.right * (normalMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime);
				transform.position = position;
			}


			if (Input.GetKey(KeyCode.Q)) {
				Transform cameraTransform = transform;
				cameraTransform.position += cameraTransform.up * (climbSpeed * Time.deltaTime);
			}

			if (Input.GetKey(KeyCode.E)) {
				Transform cameraTransform = transform;
				cameraTransform.position -= cameraTransform.up * (climbSpeed * Time.deltaTime);
			}
		}
	}

	public bool MenuIsActive {
		get => menuIsActive;
		set => menuIsActive = value;
	}
}