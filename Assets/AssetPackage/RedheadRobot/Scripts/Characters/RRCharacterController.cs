using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(CharacterController))]
public class RRCharacterController : MonoBehaviour {
	[Header("Character Data")]
	[SerializeField]
	private RRCharacterControllerData characterData = null;

	[Header("Debug")]
	[SerializeField]
	private bool enableDebugInformation = false;

	private RRThirdPersonFollowCamera thirdPersonFollowCamera = null;
	private CharacterController characterController = null;
	private RRCharacterAnimator animator = null;
	private float currentMovementSpeed;
	private float currentDirection;
	private float currentJumpTimer;
	private float currentGravity;
	private bool isGrounded = false;
	private bool isJumping = false;
	private bool isFalling = false;
	private bool HasAnimator { get { return animator != null && animator.enabled; } }
	private Vector3 movementDirection;
	private Vector3 JumpStartVelocity;

	// Basic initialization
	private void Awake() {
		characterController = GetComponent<CharacterController>();

		if (characterController == null) {
			Debug.LogError("This object requires a Character Controller component to function!", gameObject);
			enabled = false;
			return;
		}

		characterController.enableOverlapRecovery = true;

		if (characterData == null) {
			Debug.LogError("This object requires character data to be assigned before it can function!", gameObject);
			enabled = false;
			return;
		}

		// In case someone removes the animator component and puts it in one of it's children
		animator = GetComponent<RRCharacterAnimator>() ?? GetComponentInChildren<RRCharacterAnimator>();

		if (animator == null) {
			Debug.LogWarning("No RobotBuddyAnimator found, animations will not be applied!", gameObject);
		}
		else if (animator.enabled == false) {
			Debug.LogWarning("The RobotBuddyAnimator is disabled, animations will not be applied!", gameObject);
		}

		FindAndAssignThirdPersonCamera();
		
		if (gameObject.activeInHierarchy && enabled && characterData.ThirdPersonFollowCamera != null && thirdPersonFollowCamera == null) {
			thirdPersonFollowCamera = Instantiate(characterData.ThirdPersonFollowCamera);
			thirdPersonFollowCamera.Followtarget = transform;
		}
	}

	private void FindAndAssignThirdPersonCamera() {
		// Try and see if there is already a third person follow camera in the scene that we probablty spawned in using the OnValidate function
		RRThirdPersonFollowCamera[] thirdPersonFollowCameras = FindObjectsOfType<RRThirdPersonFollowCamera>();
		for (int i = 0; i < thirdPersonFollowCameras.Length; i++) {
			if (thirdPersonFollowCameras[i] == null)
				continue;
			if (thirdPersonFollowCameras[i].Followtarget == null)
				continue;
			if (thirdPersonFollowCameras[i].Followtarget != transform)
				continue;

			thirdPersonFollowCamera = thirdPersonFollowCameras[i];
			break;
		}
	}

	// Ensure the center point of the CharacterController component is always 1.1f
	private void OnValidate() {
		FindAndAssignThirdPersonCamera();

		if (Application.isPlaying == false && gameObject.activeInHierarchy && enabled && characterData != null && characterData.ThirdPersonFollowCamera != null && thirdPersonFollowCamera == null) {
			thirdPersonFollowCamera = Instantiate(characterData.ThirdPersonFollowCamera);
			thirdPersonFollowCamera.Followtarget = transform;
		}
	}

	// Check which kind of input logic we'll use
	private void Update() {
		JumpLogic();

		if (thirdPersonFollowCamera == null) {
			NoCameraMovement();
		}
		else {
			ThirdPersonCameraMovement();
		}
	}

	private void FixedUpdate() {
		if (thirdPersonFollowCamera)
			CameraLocomotion();
	}

	#region Jumping

	private bool CanJumpToLocation(Vector3 currentPosition, Vector3 characterHeight, Vector3 jumpSpeed) {
		return CheckForCollision(currentPosition + characterHeight + jumpSpeed, characterController.radius) == false;
	}

	private void StartFalling() {
		if (HasAnimator) {
			animator.IsFalling = true;
		}

		isFalling = true;
	}

	private void StartJumping() {
		if (HasAnimator) {
			animator.Jump(true);
			animator.IsJumping = true;
			animator.IsFalling = false;
			animator.MovementSpeed(0, true);
		}

		isJumping = true;
		isFalling = false;
		JumpStartVelocity = characterData.MoveWhileJumping
			? transform.forward * Mathf.Clamp(currentMovementSpeed, -1, 1) * characterData.CharacterMovementSpeed
			: Vector3.zero;
	}

	private void ResetJumpSettings() {
		if (HasAnimator) {
			animator.IsJumping = false;
			animator.IsFalling = false;
		}

		isJumping = false;
		isFalling = false;
		currentJumpTimer = 0;
	}

	private void JumpLogic() {
		// Reset jump logic in case we are on the ground
		if (isGrounded) {
			ResetJumpSettings();

			// We fell on the ground while jumping, but we still want to keep jumping
			if (Input.GetButton(characterData.JumpAxis) && currentJumpTimer == 0 &&
			    Input.GetButtonDown(characterData.JumpAxis) == false) {
				// Because we skip the jump land animation, we gotta call the particles ourselves
				animator.RROnJumpLand();

				StartJumping();
				currentGravity = 0;
			}

			// Initiate start jump
			if ((Input.GetButton(characterData.JumpAxis) && isJumping == false && characterData.HoldToJump) ||
				(Input.GetButtonDown(characterData.JumpAxis) && isJumping == false && characterData.HoldToJump == false)) {
				StartJumping();
				currentGravity = 0;
			}
		}

		float jumpCurveEndTime = characterData.JumpCurve[characterData.JumpCurve.length - 1].time;

		// While we are pressing the jump button and we are still allowed to jump, continue our jump logic
		if (((Input.GetButton(characterData.JumpAxis) && characterData.HoldToJump) || 
			(isJumping && characterData.HoldToJump == false)) && 
			currentJumpTimer < jumpCurveEndTime) {
			float curvePercentage = currentJumpTimer / jumpCurveEndTime;
			float curveEndTime = characterData.JumpCurve[characterData.JumpCurve.length - 1].time;
			Vector3 desiredJumpAddition = new Vector3(0, characterData.JumpCurve.Evaluate(curveEndTime * curvePercentage), 0) * Time.deltaTime;

			// In case there is a collider above us, so we gotta stop jumping
			if (CanJumpToLocation(transform.position, new Vector3(0, characterController.height, 0), desiredJumpAddition) == false && characterData.DetectHeadJumpCollision) {
				StartFalling();
			}

			if (isFalling == false) {
				transform.position += desiredJumpAddition;
				currentJumpTimer += Time.deltaTime;
			}
		}

		// We stopped pressing the jump button OR we reached the maximum amount of jump time we are allowed to jump
		if ((Input.GetButtonUp(characterData.JumpAxis) && characterData.HoldToJump) || currentJumpTimer > jumpCurveEndTime) {
			StartFalling();
		}

		// We just fell off an edge without jumping, so active falling logic
		if (characterController.velocity.y > 0 && characterController.velocity.y * Time.deltaTime <= -characterData.FallThreshold && isGrounded == false) {
			StartFalling();
		}
	}

	#endregion

	private bool CheckForCollision(Vector3 startPos, float radius) {
		Collider[] overlapColliders = Physics.OverlapSphere(startPos, radius);

		for (int i = 0; i < overlapColliders.Length; i++) {
			if (overlapColliders[i].transform == transform)
				continue;

			if (enableDebugInformation) {
				Debug.Log("collided with: " + overlapColliders[i].transform.name, gameObject);
			}

			return true;
		}

		return false;
	}

	#region No Third Person Camera

	// In case someone sets the Third Person Follow Camera field to None, we still need to be able to do our movement (however, this input is then based on the forward of the character instead of the camera).
	private void NoCameraMovement() {
		float lastHorizontalMovement = Input.GetAxis(characterData.HorizontalAxis);

		if (isJumping && characterData.RotateWhileJumping == false) {
			lastHorizontalMovement = 0;
		}

		// Rotate around y - axis
		transform.Rotate(0, lastHorizontalMovement * characterData.WithoutCameraRotationSpeed, 0);

		movementDirection = transform.forward;

		// We need to invert our math to make sure we can also support backwards movement properly
		float forwardAxisValue = Input.GetAxisRaw(characterData.VerticalAxis);
		if (forwardAxisValue < 0) {
			forwardAxisValue = -forwardAxisValue;
			movementDirection = -movementDirection;
		}

		float lastForwardMovementSpeed = currentMovementSpeed;
		float verticalInput = forwardAxisValue;

		currentMovementSpeed = characterData.MaximumMovementSpeed * verticalInput;

		verticalInput = isJumping && characterData.MoveWhileJumping == false ? lastForwardMovementSpeed : currentMovementSpeed;

		characterController.SimpleMove(movementDirection * verticalInput);
	}
	#endregion

	#region Third Person Camera Movement

	// This movement is based on the forward of our camera
	private void ThirdPersonCameraMovement() {
		float lastForwardMovementSpeed = currentMovementSpeed;
		float verticalInput = Input.GetAxisRaw(characterData.VerticalAxis);
		float lastRightDirection = currentDirection;
		float horizontalInput = Input.GetAxisRaw(characterData.HorizontalAxis);
		
		if (isGrounded) {
			currentGravity = 0;
		}

		if (isJumping && characterData.MoveWhileJumping == false) {
			verticalInput = 0;
		}

		if (isJumping && characterData.RotateWhileJumping == false) {
			horizontalInput = 0;
		}

		RotationToWorldSpace(transform, thirdPersonFollowCamera.transform, ref currentDirection, ref currentMovementSpeed, horizontalInput, verticalInput, characterData.RotateWhileJumping && isGrounded == false ? characterData.RotationFallSpeed : characterData.RotationSpeed);

		if (isJumping && characterData.MoveWhileJumping == false) {
			currentMovementSpeed = lastForwardMovementSpeed;
		}

		if (isJumping && characterData.RotateWhileJumping == false) {
			currentDirection = lastRightDirection * verticalInput;
		}

		if (HasAnimator && isJumping == false) {
			animator.MovementSpeed(Mathf.Clamp(currentMovementSpeed, -1, 1));
		}

		transform.Rotate(0, currentDirection, 0);

		float desiredMovementSpeedMultiplied = characterData.MoveWhileJumping && isGrounded == false ? characterData.CharacterFallMovementSpeed : characterData.CharacterMovementSpeed;
		Vector3 movementMotion = transform.forward * Mathf.Clamp(currentMovementSpeed, -1, 1) * desiredMovementSpeedMultiplied;

		// when we are jumping and we have MoveWhileJumping support, we take the jump start velocity and add an offset onto it based on the current movement
		// We then override the movementMotion with the adjusted jump start velocity
		if (characterData.MoveWhileJumping && isGrounded == false) {
			JumpStartVelocity += movementMotion * Time.deltaTime;
			JumpStartVelocity *= 1 - characterData.JumpSpeedDecreasePercentage;
			movementMotion = JumpStartVelocity;
		}

		movementMotion.y = currentGravity;
		currentGravity += characterData.Gravity * Time.deltaTime;
		movementMotion *= Time.deltaTime;

		// if 0 gravity is applied, the is grounded check aways fails, since there is no collision
		// To prevent this, we apply a really small fake gravity value
		if (movementMotion.y == 0) {
			movementMotion.y = -0.01f;
		}

		CollisionFlags collFlags = characterController.Move(movementMotion);

		isGrounded = (collFlags & CollisionFlags.Below) != 0 || (collFlags & CollisionFlags.CollidedBelow) != 0;
	}

	// Allows us to rotate around the camera slightly better
	private void CameraLocomotion() {
		float horizontalMovement = Input.GetAxis(characterData.HorizontalAxis);

		if (currentMovementSpeed != 0 && ((currentDirection >= 0 && horizontalMovement >= 0) || (currentDirection < 0 && horizontalMovement < 0))) {
			Vector3 rotationAmount = Vector3.Lerp(Vector3.zero, new Vector3(0f, characterData.RotationDegreePerSecond * horizontalMovement < 0 ? -1 : 1), Mathf.Abs(horizontalMovement));
			Quaternion deltaRotation = Quaternion.Euler(rotationAmount * Time.deltaTime);
			transform.rotation *= deltaRotation;
		}
	}

	// Allows us to convert our local rotation to world space rotation
	private void RotationToWorldSpace(Transform rootTransform, Transform targetCamera, ref float directionOut, ref float speedOut, float horizontalInput, float verticalInput, float desiredRotationSpeed) {
		if (rootTransform == null || targetCamera == null) {
			return;
		}

		Vector3 rootDirection = rootTransform.forward;
		Vector3 rotationDirection = new Vector3(horizontalInput, 0, verticalInput);
		speedOut = rotationDirection.normalized.sqrMagnitude;

		Vector3 cameraDirection = targetCamera.forward;
		cameraDirection.y = 0;
		Quaternion referentialshift = Quaternion.FromToRotation(Vector3.forward, cameraDirection);

		Vector3 moveDirection = referentialshift * rotationDirection;
		Vector3 axisSign = Vector3.Cross(moveDirection, rootDirection);

		if (enableDebugInformation) {
			Debug.DrawRay(rootTransform.position + rootTransform.up * 3, moveDirection, Color.green);
			Debug.DrawRay(rootTransform.position + rootTransform.up * 3, axisSign, Color.red);
			Debug.DrawRay(rootTransform.position + rootTransform.up * 3, rootDirection, Color.magenta);
			Debug.DrawRay(rootTransform.position + rootTransform.up * 3, rotationDirection, Color.blue);
		}

		if (axisSign.y == 0 && moveDirection.z != -1) {
			directionOut = 0;
			return;
		}

		float angleRootToMove = Vector3.Angle(rootDirection, moveDirection) * (axisSign.y >= 0 ? -1 : 1);
		angleRootToMove /= 180;
		directionOut = angleRootToMove * desiredRotationSpeed;
	}
	#endregion
}
