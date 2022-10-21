using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AnimationParticles {
	[Tooltip("This value defines if we should make the particles we want to spawn a child of our parent, or spawn at the location of the parent in world space.")]
	public bool spawnInWorldSpace;
	[Tooltip("These are the particles that'll be played.")]
	public List<ParticleSystem> particlesToPlay;
	[Tooltip("This is the transform that is used when spawning these particles. See Spawn In World Space for more information.")]
	public Transform particlesParent;
	[Tooltip("This position offset is applied to the spawned particle system.")]
	public Vector3 spawnPositionOffset;
	[Tooltip("This rotation offset is applied to the spawned particle system.")]
	public Vector3 spawnRotationOffset;
	[HideInInspector]
	public List<ParticleSystem> particlesPool;
}

[DisallowMultipleComponent]
[RequireComponent(typeof(Animator))]
public class RRCharacterAnimator : MonoBehaviour {
	[Header("Animation Data")]
	[SerializeField]
	private RRCharacterAnimationData animationData = null;

	[Header("Jump Particles")]
	[Tooltip("This value is used to play particles at the start of the jump animation.")]
	[SerializeField]
	private AnimationParticles jumpAnimation = new AnimationParticles();
	[Tooltip("This value is used to play particles at the end of the jump animation.")]
	[SerializeField]
	private AnimationParticles landAnimation = new AnimationParticles();

	[Header("Movement Particles")]
	[Tooltip("This value is used to play particles whenever we start moving.")]
	[SerializeField]
	private AnimationParticles startMovementAnimation = new AnimationParticles();
	[Tooltip("This value is used to play particles whenever we stop moving.")]
	[SerializeField]
	private AnimationParticles stopMovementAnimation = new AnimationParticles();

	[Header("Walk Particles")]
	[Tooltip("This value defines the minimum amount of movementspeed required before we start playing the left/right particles.")]
	[SerializeField]
	private float minimumWalkingSpeed = 0.1f;
	[Tooltip("This value is used to play particles whenever the left foot touches the ground.")]
	[SerializeField]
	private AnimationParticles leftFootWalkAnimation = new AnimationParticles();
	[Tooltip("This value is used to play particles whenever the right foot touches the ground.")]
	[SerializeField]
	private AnimationParticles rightFootWalkAnimation = new AnimationParticles();

	[Header("Run Particles")]
	[SerializeField]
	[Tooltip("This value defines the minimum amount of movementspeed required before we start playing the left/right particles.")]
	private float minimumRunningSpeed = 0.5f;
	[Tooltip("This value is used to play particles whenever the left foot touches the ground.")]
	[SerializeField]
	private AnimationParticles leftFootRunAnimation = new AnimationParticles();
	[Tooltip("This value is used to play particles whenever the right foot touches the ground.")]
	[SerializeField]
	private AnimationParticles rightFootRunAnimation = new AnimationParticles();

	private int isJumpingHash;
	private int isFallingHash;
	private int movementSpeedHash;
	private int jumpTriggerHash;
	private int hurtTriggerHash;
	private int dieTriggerHash;
	private int rollTriggerHash;
	private int skidTriggerHash;
	private int lookAroundTriggerHash;
	private int resetTriggerHash;

	public Animator AnimatorController { get; private set; }
	public bool IsJumping { get { return AnimatorController != null && GetBool(isJumpingHash); } set { if (AnimatorController != null) CustomBool(isJumpingHash, value); } }
	public bool IsFalling { get { return AnimatorController != null && GetBool(isFallingHash); } set { if (AnimatorController != null) CustomBool(isFallingHash, value); } }
	public float CurrentAnimationMovementSpeed { get { return AnimatorController == null ? 0 : AnimatorController.GetFloat(movementSpeedHash); } }
	
	private Coroutine currentIdleCycle;

	private void Awake() {
		Animator animator = GetComponent<Animator>();

		if (animator == null) {
			Debug.LogError("This object is expecting an Animator component!", gameObject);
			enabled = false;
			return;
		}

		AnimatorController = animator;

		if (AnimatorController.runtimeAnimatorController == null) {
			Debug.LogError("Please make sure to assign the animator controller of the Animator component!", gameObject);
			enabled = false;
		}

		if (animationData == null) {
			Debug.LogError("This object is expecting a valid Animation Data object before it can function!", gameObject);
			enabled = false;
			return;
		}

		InitializeAnimatorHases();
	}

	private void InitializeAnimatorHases() {
		isJumpingHash = Animator.StringToHash(animationData.IsJumping);
		isFallingHash = Animator.StringToHash(animationData.IsFalling);
		movementSpeedHash = Animator.StringToHash(animationData.MovementSpeed);
		jumpTriggerHash = Animator.StringToHash(animationData.JumpTrigger);
		hurtTriggerHash = Animator.StringToHash(animationData.HurtTrigger);
		dieTriggerHash = Animator.StringToHash(animationData.DieTrigger);
		rollTriggerHash = Animator.StringToHash(animationData.RollTrigger);
		skidTriggerHash = Animator.StringToHash(animationData.Skidrigger);
		lookAroundTriggerHash = Animator.StringToHash(animationData.LookAroundTrigger);
		resetTriggerHash = Animator.StringToHash(animationData.ResetTrigger);
	}

	public void Jump(bool ignoreIsJumpingCheck = false) {
		if (ignoreIsJumpingCheck == false && IsJumping) return;

		CustomTrigger(jumpTriggerHash);
	}

	public void Hurt() {
		CustomTrigger(hurtTriggerHash);
	}

	public void Die() {
		CustomTrigger(dieTriggerHash);
	}

	public void Roll() {
		CustomTrigger(rollTriggerHash);
	}

	public void Skid() {
		CustomTrigger(skidTriggerHash);
	}

	public void LookAround() {
		if (IsJumping || IsFalling) return;

		CustomTrigger(lookAroundTriggerHash);
	}

	public void Reset() {
		CustomTrigger(resetTriggerHash);
	}

	public void MovementSpeed(float currentValue, bool ignoreDamping = false) {
		if (enabled == false) return;

		if (currentValue < 0) {
			currentValue = -currentValue;
		}

		// No need to update this again
		if (CurrentAnimationMovementSpeed == 0 && currentValue == 0) {
			return;
		}

		if (CurrentAnimationMovementSpeed < 0.15f && currentValue > 0.85f && HasParticleSystemPlayingInPool(stopMovementAnimation.particlesPool) == false) {
			PlayParticles(startMovementAnimation);
		}
		else if (CurrentAnimationMovementSpeed > 0.85f && currentValue < 0.15f && HasParticleSystemPlayingInPool(startMovementAnimation.particlesPool) == false) {
			PlayParticles(stopMovementAnimation);
		}

		if (ignoreDamping)
			CustomFloat(movementSpeedHash, currentValue);
		else
			CustomFloat(movementSpeedHash, currentValue, animationData.MovementDamping);
	}

	public void ResetCustomTrigger(string triggerToReset) {
		if (AnimatorController == null) return;
		if (enabled == false) return;

		AnimatorController.ResetTrigger(triggerToReset);
	}

	public void ResetCustomTrigger(int triggerHash) {
		if (AnimatorController == null) return;
		if (enabled == false) return;

		AnimatorController.SetTrigger(triggerHash);
	}

	public void CustomTrigger(string triggerToUse) {
		if (AnimatorController == null) return;
		if (enabled == false) return;
		
		AnimatorController.SetTrigger(triggerToUse);
	}

	public void CustomTrigger(int triggerHash) {
		if (AnimatorController == null) return;
		if (enabled == false) return;

		AnimatorController.SetTrigger(triggerHash);
	}

	public void CustomFloat(string floatName, float floatValue) {
		if (AnimatorController == null) return;
		if (enabled == false) return;

		AnimatorController.SetFloat(floatName, floatValue);
	}

	public void CustomFloat(int floatHash, float floatValue) {
		if (AnimatorController == null)
			return;
		if (enabled == false)
			return;

		AnimatorController.SetFloat(floatHash, floatValue);
	}

	public void CustomFloat(string floatName, float floatValue, float dampingValue) {
		if (AnimatorController == null) return;
		if (enabled == false) return;

		AnimatorController.SetFloat(floatName, floatValue, dampingValue, Time.deltaTime);
	}

	public void CustomFloat(int floatHash, float floatValue, float dampingValue) {
		if (AnimatorController == null)
			return;
		if (enabled == false)
			return;

		AnimatorController.SetFloat(floatHash, floatValue, dampingValue, Time.deltaTime);
	}

	public void CustomBool(string boolName, bool boolValue) {
		if (AnimatorController == null) return;
		if (enabled == false) return;

		AnimatorController.SetBool(boolName, boolValue);
	}

	public void CustomBool(int boolHash, bool boolValue) {
		if (AnimatorController == null) return;
		if (enabled == false) return;

		AnimatorController.SetBool(boolHash, boolValue);
	}

	public bool GetBool(string boolName) {
		if (AnimatorController == null) return false;

		return AnimatorController.GetBool(boolName);
	}

	public bool GetBool(int boolHash) {
		if (AnimatorController == null) return false;

		return AnimatorController.GetBool(boolHash);
	}

	public float GetFloat(string floatName) {
		if (AnimatorController == null) return 0;

		return AnimatorController.GetFloat(floatName);
	}

	public float GetFloat(int floatHash) {
		if (AnimatorController == null) return 0;

		return AnimatorController.GetFloat(floatHash);
	}

	private void Update() {
		if ((CurrentAnimationMovementSpeed != 0 || IsJumping) && currentIdleCycle != null) {
			StopCoroutine(currentIdleCycle);
			currentIdleCycle = null;
			Reset();
		}
		if (CurrentAnimationMovementSpeed == 0 && IsJumping == false && currentIdleCycle == null) {
			currentIdleCycle = StartCoroutine(IdleCycle());
		}
	}

	private IEnumerator IdleCycle() {
		float timeToWait = Random.Range(animationData.IdleRange.x, animationData.IdleRange.y);

		while (timeToWait > 0) {
			timeToWait -= Time.deltaTime;
			yield return null;
		}

		LookAround();
		currentIdleCycle = null;
	}

	// Triggered by AnimationEvent
	private void RROnJumped() {
		IsJumping = true;

		if (IsJumping && enabled)
			PlayParticles(jumpAnimation);
	}

	// Triggered by AnimationEvent
	public void RROnJumpLand() {
		IsJumping = false;

		if (enabled)
			PlayParticles(landAnimation);
	}

	// Triggered by AnimationEvent
	private void RROnLeftFootWalkDown() {
		if (CurrentAnimationMovementSpeed >= minimumWalkingSpeed && enabled && IsJumping == false)
			PlayParticles(leftFootWalkAnimation);
	}

	// Triggered by AnimationEvent
	private void RROnRightFootWalkDown() {
		if (CurrentAnimationMovementSpeed >= minimumWalkingSpeed && enabled && IsJumping == false)
			PlayParticles(rightFootWalkAnimation);
	}

	// Triggered by AnimationEvent
	private void RROnLeftFootRunDown() {
		if (CurrentAnimationMovementSpeed >= minimumRunningSpeed && enabled && IsJumping == false)
			PlayParticles(leftFootRunAnimation);
	}

	// Triggered by AnimationEvent
	private void RROnRightFootRunDown() {
		if (CurrentAnimationMovementSpeed >= minimumRunningSpeed && enabled && IsJumping == false)
			PlayParticles(rightFootRunAnimation);
	}

	private void PlayParticles(AnimationParticles animationParticles) {
		for (int i = 0; i < animationParticles.particlesToPlay.Count; i++) {
			if (animationParticles.particlesToPlay[i] == null) continue;

			ParticleSystem particlesToPlay = GetParticleSystemFromPool(ref animationParticles.particlesPool, animationParticles.particlesToPlay[i], animationParticles.particlesParent);

			if (particlesToPlay == null) continue;

			// Reset the current position and rotation back to that of the prefab
			particlesToPlay.transform.localPosition = animationParticles.particlesToPlay[i].transform.localPosition;
			particlesToPlay.transform.localRotation = animationParticles.particlesToPlay[i].transform.localRotation;

			particlesToPlay.transform.position = animationParticles.particlesParent != null
				? animationParticles.particlesParent.transform.position
				: transform.position;

			if (animationParticles.spawnInWorldSpace)
				particlesToPlay.transform.SetParent(null, true);

			// Apply the position and rotation offset
			particlesToPlay.transform.position += animationParticles.spawnPositionOffset;
			particlesToPlay.transform.rotation *= Quaternion.Euler(animationParticles.spawnRotationOffset);

			particlesToPlay.Play();
		}
	}

	private ParticleSystem GetParticleSystemFromPool(ref List<ParticleSystem> poolToUse, ParticleSystem systemToPlay, Transform parentObject = null) {
		if (systemToPlay == null) return null;
		if (poolToUse == null) {
			poolToUse = new List<ParticleSystem>();
		}

		for (int i = 0; i < poolToUse.Count; i++) {
			if (poolToUse[i] == null) continue;
			if (poolToUse[i].isPlaying) continue;

			poolToUse[i].transform.SetParent(parentObject);
			return poolToUse[i];
		}

		// No system found in the related pool, creating a new system and adding it to the pool, then returning it
		poolToUse.Add(Instantiate(systemToPlay, parentObject));
		return poolToUse[poolToUse.Count - 1];
	}

	private bool HasParticleSystemPlayingInPool(List<ParticleSystem> poolToCheck) {
		if (poolToCheck == null) return false;

		for (int i = 0; i < poolToCheck.Count; i++) {
			if (poolToCheck[i] == null) continue;
			if (poolToCheck[i].isPlaying) return true;
		}

		return false;
	}
}