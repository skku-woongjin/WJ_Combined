using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ReadheadRobot/Character Controller Data")]
public class RRCharacterControllerData : ScriptableObject {
	[Header("Movement")]
	[Tooltip("The maximum movementspeed of this character. If the component RobotBuddyAnimator is found, we multiply the movement speed using the current movement speed value of the animator.")]
	[SerializeField]
	[ConditionalHide("HasThirdPersonCamera", false)]
	private float maximumMovementSpeed = 8;
	public float MaximumMovementSpeed { get { return maximumMovementSpeed; } }
	[Tooltip("The characters movement speed.")]
	[SerializeField]
	[ConditionalHide("HasThirdPersonCamera", true)]
	private float characterMovementSpeed = 8;
	public float CharacterMovementSpeed { get { return characterMovementSpeed; } }
	[Tooltip("The characters movement speed while falling.")]
	[SerializeField]
	[ConditionalHide("HasThirdPersonCamera", true)]
	private float characterFallMovementSpeed = 3;
	public float CharacterFallMovementSpeed { get { return characterFallMovementSpeed; } }
	[Tooltip("This value defines the gravity that is applied each update cycle to the character.")]
	[SerializeField]
	[ConditionalHide("HasThirdPersonCamera", true)]
	private float gravity = -9.81f;
	public float Gravity { get { return gravity; } }

	[Header("Jumping")]
	[Tooltip("This value defines how quickly the character jumps and how long the character can jump for.")]
	[SerializeField]
	private AnimationCurve jumpCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
	public AnimationCurve JumpCurve { get { return jumpCurve; } }
	[Tooltip("This value is used to define when the animator should transition into the fall state.")]
	[SerializeField]
	[ConditionalHide("HasAnimator", true)]
	private float fallThreshold = 0.1f;
	public float FallThreshold { get { return fallThreshold; } }
	[Tooltip("This value defines the dropoff percentage rate of how much the starting jump velocity decreases each update cycle.")]
	[SerializeField]
	[Range(0, 0.1f)]
	private float jumpSpeedDecreasePercentage = 0.01f;
	public float JumpSpeedDecreasePercentage { get { return jumpSpeedDecreasePercentage; } }
	[Tooltip("This value defines if you can move while jumping.")]
	[SerializeField]
	private bool moveWhileJumping = false;
	public bool MoveWhileJumping { get { return moveWhileJumping; } }
	[Tooltip("This value defines if you can rotate while jumping.")]
	[SerializeField]
	private bool rotateWhileJumping = false;
	public bool RotateWhileJumping { get { return rotateWhileJumping; } }
	[Tooltip("This value defines if you wish to stop a jump when the character hits their head against something.")]
	[SerializeField]
	private bool detectHeadJumpCollision = true;
	public bool DetectHeadJumpCollision { get { return detectHeadJumpCollision; } }
	[Tooltip("This value defines if you wish to hold the jump button, or jump on button down.")]
	[SerializeField]
	private bool holdToJump = true;
	public bool HoldToJump { get { return holdToJump; } }

	[Header("Rotation")]
	[Tooltip("This value defines how quickly this character will rotate.")]
	[SerializeField]
	[ConditionalHide("HasThirdPersonCamera", false)]
	private float withoutCameraRotationSpeed = 5f;
	public float WithoutCameraRotationSpeed { get { return withoutCameraRotationSpeed; } }
	[Tooltip("This value defines how quickly this character will rotate.")]
	[SerializeField]
	[ConditionalHide("HasThirdPersonCamera", true)]
	private float rotationSpeed = 30f;
	public float RotationSpeed { get { return rotationSpeed; } }
	[Tooltip("This value defines how quickly this character will rotate while falling.")]
	[SerializeField]
	[ConditionalHide("HasThirdPersonCamera", true)]
	private float rotationFallSpeed = 10f;
	public float RotationFallSpeed { get { return rotationFallSpeed; } }
	[Tooltip("The amount of degrees the character can rotate around the camera per second")]
	[SerializeField]
	[ConditionalHide("HasThirdPersonCamera", true)]
	private float rotationDegreePerSecond = 120;
	public float RotationDegreePerSecond { get { return rotationDegreePerSecond; } }

	[Header("Camera")]
	[Tooltip("If you wish to use the ThirdPersonFollowCamera in your math, be sure to assign this value.")]
	[SerializeField]
	private RRThirdPersonFollowCamera thirdPersonFollowCamera = null;
	public RRThirdPersonFollowCamera ThirdPersonFollowCamera { get { return thirdPersonFollowCamera; } }

	[Header("Input")]
	[Tooltip("This value defines the input axis that's to detect the vertical movement input value.")]
	[SerializeField]
	[InputAxes]
	private string verticalAxis = "Vertical";
	public string VerticalAxis { get { return verticalAxis; } }
	[Tooltip("This value defines the input axis that's to detect the horizontal movement input value.")]
	[SerializeField]
	[InputAxes]
	private string horizontalAxis = "Horizontal";
	public string HorizontalAxis { get { return horizontalAxis; } }
	[Tooltip("This value defines the input axis that's to detect a jump input.")]
	[SerializeField]
	[InputAxes]
	private string jumpAxis = "Jump";
	public string JumpAxis { get { return jumpAxis; } }

	[HideInInspector]
	[SerializeField]
	private bool HasThirdPersonCamera { get { return thirdPersonFollowCamera != null; } }
}
