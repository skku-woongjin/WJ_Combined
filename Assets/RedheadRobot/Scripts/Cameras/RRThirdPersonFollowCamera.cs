using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Camera))]
public class RRThirdPersonFollowCamera : MonoBehaviour {
	[Header("Camera Settings")]
	[Tooltip("This value defines how far away the camera will stay from the target.")]
	[SerializeField]
	private float followDistance = 5;
	[Tooltip("This value defiens how high the camera is above the target.")]
	[SerializeField]
	private float followHeight = 3;
	[Tooltip("This value defines how quickly the camera goes from its current position to its desired position.")]
	[SerializeField]
	private float cameraSmoothDampTime = 0.1f;
	[Tooltip("This value defines the amount of seconds the camera will wait before rotating to the back of the target.")]
	[SerializeField]
	private float resetAfterSeconds = 1.5f;
	[Tooltip("This value defines how quickly the camera goes from its current position to its desired position whenever it is rotation to the back of the target.")]
	[SerializeField]
	private float cameraResetSmoothDampTime = 5f;
	[Tooltip("This value defines the offset to apply based on the Follow Target.")]
	[SerializeField]
	private Vector3 positionOffset = new Vector3(0, 1.5f, 0);
	[Tooltip("This value defines the target this camera will look at.")]
	[SerializeField]
	private Transform followTarget;
	public Transform Followtarget { get { return followTarget; } set { followTarget = value; } }

	[Header("Debug")]
	[Tooltip("This value allows you to see some debugging information in your Scene view.")]
	[SerializeField]
	private bool drawDebugLines = false;
	[Tooltip("This value allows you to see some debugging information in your Scene view whenever you have this object selected.")]
	[SerializeField]
	private bool drawDebugLinesOnSelection = false;
	
	private float currentResetTimer;
	private float DesiredDamping { get { return currentResetTimer <= 0 ? cameraResetSmoothDampTime : cameraSmoothDampTime; } }
	private Vector3 DesiredRightDirection { get { return currentResetTimer <= 0 ? followTarget.forward : lookDirection.normalized; } }
	private Vector3 lookDirection;
	private Vector3 targetPosition;
	private Vector3 cameraSmoothVelocity = Vector3.zero;
	private Vector3 lastFollowTargetPosition = Vector3.zero;

	private void Start() {
		// In case the end user forgot to assign the follow target, we try to use the object with the Player tag instead.
		if (followTarget == null) {
			followTarget = GameObject.FindGameObjectWithTag("Player").transform;
		}

		currentResetTimer = resetAfterSeconds;
	}

	// We are updating the base position automatically whenever we are changing some values in edit mode, so we need to make the transform component read only.
	private void OnValidate() {
		if (followTarget == null || Application.isPlaying) return;
		transform.hideFlags = HideFlags.NotEditable;

		Vector3 characterOffset;
		transform.position = CalculateTargetPosition(out characterOffset);
		transform.LookAt(characterOffset);
	}

	private void LateUpdate() {
		if (followTarget == null) {
			Debug.LogWarning("No follow target found, disabling this component. You'll have to manually enable this component again after assinging a Follow Target.");
			enabled = false;
			return;
		}

		// Reset the reset timer in case we moved
		currentResetTimer = (lastFollowTargetPosition - followTarget.position).sqrMagnitude != 0
			? resetAfterSeconds
			: currentResetTimer - Time.deltaTime;

		Vector3 characterOffset;
		// If the player hasn't moved for resetAfterSeconds time then we move to the back of the follow target otherwise we use the look direction
		targetPosition = CalculateTargetPosition(out characterOffset);
		WallCompensation(characterOffset, ref targetPosition);

		if (currentResetTimer > 0) {
			transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref cameraSmoothVelocity, DesiredDamping);
			transform.LookAt(characterOffset);
		}
		else {
			float angleDot = Vector3.Dot(transform.forward, -followTarget.right);
			transform.RotateAround(characterOffset, followTarget.up, Vector3.Angle(transform.position, targetPosition) * Time.deltaTime * cameraResetSmoothDampTime * angleDot);
			transform.LookAt(characterOffset);
		}

		lastFollowTargetPosition = followTarget.position;
	}

	private void WallCompensation(Vector3 startPosition, ref Vector3 newTargetPosition) {
		RaycastHit hit;
		if (Physics.Linecast(startPosition, newTargetPosition, out hit)) {
			if (hit.transform == followTarget) return;
			
			newTargetPosition = new Vector3(hit.point.x, newTargetPosition.y, hit.point.z);
		}
	}

	private Vector3 CalculateTargetPosition(out Vector3 characterOffset) {
		if (followTarget == null) return characterOffset = Vector3.zero;

		characterOffset = followTarget.position + positionOffset;
		lookDirection = characterOffset - transform.position;
		lookDirection.y = 0;

		return followTarget.position + followTarget.up * followHeight - DesiredRightDirection * followDistance;
	}

	private void OnDrawGizmos() {
		if (drawDebugLines == false) return;

		DrawDebugLines();
	}

	private void OnDrawGizmosSelected() {
		if (drawDebugLinesOnSelection == false) return;

		DrawDebugLines();
	}

	private void DrawDebugLines() {
		if (followTarget == null) return;

		Vector3 characterOffset;
		Debug.DrawLine(followTarget.position, CalculateTargetPosition(out characterOffset), Color.magenta);
		Debug.DrawRay(transform.position, lookDirection, Color.green);
	}
}