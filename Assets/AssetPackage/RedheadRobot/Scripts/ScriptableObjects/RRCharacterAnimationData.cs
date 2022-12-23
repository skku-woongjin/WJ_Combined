using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ReadheadRobot/Character Animation Data")]
public class RRCharacterAnimationData : ScriptableObject {
	[Header("Idling")]
	[Tooltip("This value defines the minimum and maximum amount of time we need to have no movement before we do our idling animations.")]
	[SerializeField]
	[MinMaxSlider(0, 100)]
	private Vector2 idleRange = new Vector2Int(1, 5);
	public Vector2 IdleRange { get { return idleRange; } }

	[Header("Movement")]
	[Tooltip("This value defines the damping of the movement blending")]
	[SerializeField]
	private float movementDamping = 0.1f;
	public float MovementDamping { get { return movementDamping; } }

	[Header("AnimationController Parameters")]
	[SerializeField]
	private string isJumping = "IsJumping";
	public string IsJumping { get { return isJumping; } }
	[SerializeField]
	private string isFalling = "IsFalling";
	public string IsFalling { get { return isFalling; } }
	[SerializeField]
	private string movementSpeed = "MovementSpeed";
	public string MovementSpeed { get { return movementSpeed; } }
	[SerializeField]
	private string jumpTrigger = "Jump";
	public string JumpTrigger { get { return jumpTrigger; } }
	[SerializeField]
	private string hurtrigger = "Hurt";
	public string HurtTrigger { get { return hurtrigger; } }
	[SerializeField]
	private string dieTrigger = "Die";
	public string DieTrigger { get { return dieTrigger; } }
	[SerializeField]
	private string rollTrigger = "Roll";
	public string RollTrigger { get { return rollTrigger; } }
	[SerializeField]
	private string skidTrigger = "Skid";
	public string Skidrigger { get { return skidTrigger; } }
	[SerializeField]
	private string lookAroundTrigger = "LookAround";
	public string LookAroundTrigger { get { return lookAroundTrigger; } }
	[SerializeField]
	private string resetTrigger = "Reset";
	public string ResetTrigger { get { return resetTrigger; } }
}
