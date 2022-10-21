using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RRUIAnimationControl : MonoBehaviour {
	[SerializeField]
	private Slider movementSpeedSlider = null;
	[SerializeField]
	private Text movementSpeedsliderValueText = null;
	[SerializeField]
	private RRCharacterAnimator characterAnimator = null;
	[SerializeField]
	private AnimationCurve jumpCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
	[SerializeField]
	private Transform jumpObstacle = null;

	private Coroutine characterJumpCycle;
	private Vector3 characterStartPosition;

	private void Awake() {
		if (characterAnimator != null)
			characterStartPosition = characterAnimator.transform.position;
	}

	public void UpdateSliderValueText(float currentValue) {
		if (movementSpeedSlider == null) return;
		if (movementSpeedsliderValueText == null) return;

		movementSpeedsliderValueText.text = Mathf.RoundToInt(currentValue / movementSpeedSlider.maxValue * 100).ToString();
	}

	public void ToggleJumpObstacle() {
		if (jumpObstacle == null) {
			Debug.LogError("The jump obstacle has not been assigned!");
			return;
		}

		jumpObstacle.gameObject.SetActive(!jumpObstacle.gameObject.activeInHierarchy);
	}

	public void CharacterJump() {
		if (characterJumpCycle != null || characterAnimator == null) return;

		characterJumpCycle = StartCoroutine(JumpCycle());
	}

	public void CharacterHurt() {
		if (characterAnimator == null) return;

		characterAnimator.Hurt();
	}

	public void CharacterDie() {
		if (characterAnimator == null) return;

		characterAnimator.Die();
	}

	public void CharacterRoll() {
		if (characterAnimator == null) return;

		characterAnimator.Roll();
	}

	public void CharacterSkid() {
		if (characterAnimator == null) return;

		characterAnimator.Skid();
	}

	public void CharacterReset() {
		if (characterAnimator == null) return;

		characterAnimator.Reset();
	}

	private bool CanJumpToLocation(Vector3 currentPosition, Vector3 characterHeight, Vector3 jumpSpeed) {
		Collider[] overlapColliders = Physics.OverlapSphere(currentPosition + characterHeight + jumpSpeed, 0.5f);

		for (int i = 0; i < overlapColliders.Length; i++) {
			if (overlapColliders[i].transform == characterAnimator.transform)
				continue;

			return false;
		}

		return true;
	}

	private IEnumerator JumpCycle() {
		float currentTime = 0;
		float endTime = jumpCurve[jumpCurve.length - 1].time;

		characterAnimator.Jump(true);
		characterAnimator.IsJumping = true;

		while (currentTime < endTime) {
			Vector3 desiredJumpLocation = new Vector3(0, jumpCurve.Evaluate(currentTime / endTime), 0) * Time.deltaTime;

			if (CanJumpToLocation(characterAnimator.transform.position, new Vector3(0, 1.75f, 0), desiredJumpLocation) == false) {
				currentTime = endTime;
				yield return null;
			}

			characterAnimator.transform.position += desiredJumpLocation;
			currentTime += Time.deltaTime;

			yield return null;
		}

		characterAnimator.IsFalling = true;

		while (characterAnimator.transform.position.y > characterStartPosition.y) {
			characterAnimator.transform.position += Physics.gravity * Time.deltaTime;
			yield return null;
		}

		characterAnimator.IsJumping = false;
		characterAnimator.IsFalling = false;

		characterAnimator.transform.position = characterStartPosition;
		characterJumpCycle = null;
	}

	private void Update() {
		if (characterAnimator == null) return;
		if (movementSpeedSlider == null) return;

		characterAnimator.MovementSpeed(movementSpeedSlider.value);
	}
}
