using System;
using Misc;
using UnityEngine;

namespace Player
{
	[RequireComponent (typeof (Controller2D))]
	public class PlayerController : MonoBehaviour
	{
		[Header("Walking")]
		[SerializeField] private float _moveSpeed;
		
		[Header("Jumping")]
		[SerializeField] private float _maxJumpHeight = 4;
		[SerializeField] private float _minJumpHeight = 1;
		[SerializeField] private float _timeToJumpApex = .4f;
		private float _accelerationTimeAirborne = .2f;
		private float _accelerationTimeGrounded = .1f;
		private float _gravity;
		private float _normalGravity;
		private float _maxJumpVelocity;
		private float _minJumpVelocity;
		
		[Header("Wall Jumping")]
		[SerializeField] private Vector2 _wallJumpClimb;
		[SerializeField] private Vector2 _wallJumpOff;
		[SerializeField] private Vector2 _wallLeap;
		
		[Header("Wall Sliding")]
		[SerializeField] private float _wallSlideSpeedMax = 3;
		[SerializeField] private float _wallStickTime = .25f;
		private float _timeToWallUnstick;
		private bool _wallSliding;
		private int _wallDirX;
		
		[Header("Ascending")]
		[SerializeField] private float _ascendGravity;
		private bool _isAscending;
		
		[Header("Descending")]
		[SerializeField] private float _descendGravity;
		private bool _isDescending;
		
		[Header("Rendering")]
		[SerializeField] private Animator _animator;
		[SerializeField] private SpriteRenderer _spriteRenderer;
		
		private Vector3 _velocity;
		private float _velocityXSmoothing;

		private Controller2D _controller;

		private Vector2 _directionalInput;
		
		public static Action OnPlayerJump;
		public static Action OnPlayerFootStep;

		private void OnEnable()
		{
			AscendZone.OnPlayerEntered += OnPlayerEnteredAscend;
			AscendZone.OnPlayerExited += OnPlayerExitedAscend;
		}

		private void OnDisable()
		{
			AscendZone.OnPlayerEntered -= OnPlayerEnteredAscend;
			AscendZone.OnPlayerExited -= OnPlayerExitedAscend;
		}

		private void Start() {
			_controller = GetComponent<Controller2D> ();

			_normalGravity = -(2 * _maxJumpHeight) / Mathf.Pow (_timeToJumpApex, 2);
			_gravity = _normalGravity;
			_maxJumpVelocity = Mathf.Abs(_gravity) * _timeToJumpApex;
			_minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (_gravity) * _minJumpHeight);
		}

		private void Update() {
			CalculateVelocity ();
			HandleWallSliding ();

			_controller.Move (_velocity * Time.deltaTime, _directionalInput);

			if (_controller.collisions.above || _controller.collisions.below) {
				if (_controller.collisions.slidingDownMaxSlope) {
					_velocity.y += _controller.collisions.slopeNormal.y * -_gravity * Time.deltaTime;
				} else {
					_velocity.y = 0;
				}
			}

			HandleRendering();
		}

		private void HandleRendering()
		{
			if (_directionalInput.x != 0)
			{
				_spriteRenderer.flipX = _directionalInput.x < 0;
			}
			if (_controller.collisions.below)
			{
				_animator.SetBool("isJumping", false);
				_animator.SetBool("isFalling", false);
				_animator.SetFloat("Speed", Mathf.Abs(_velocity.x));
			}
			else
			{
				_animator.SetBool("isJumping", _velocity.y > 0);
				_animator.SetBool("isFalling", _velocity.y < 0);
			}
		}

		public void SetDirectionalInput (Vector2 input) {
			_directionalInput = input;
		}

		public void OnJumpInputDown() 
		{
			if (_wallSliding) {
				OnPlayerJump?.Invoke();

				if (_wallDirX == _directionalInput.x) {
					_velocity.x = -_wallDirX * _wallJumpClimb.x;
					_velocity.y = _wallJumpClimb.y;
				}
				else if (_directionalInput.x == 0) {
					_velocity.x = -_wallDirX * _wallJumpOff.x;
					_velocity.y = _wallJumpOff.y;
				}
				else {
					_velocity.x = -_wallDirX * _wallLeap.x;
					_velocity.y = _wallLeap.y;
				}
			}
			if (_controller.collisions.below) {
				OnPlayerJump?.Invoke();

				if (_controller.collisions.slidingDownMaxSlope) {
					if (_directionalInput.x != -Mathf.Sign (_controller.collisions.slopeNormal.x)) { // not jumping against max slope
						_velocity.y = _maxJumpVelocity * _controller.collisions.slopeNormal.y;
						_velocity.x = _maxJumpVelocity * _controller.collisions.slopeNormal.x;
					}
				} else {
					_velocity.y = _maxJumpVelocity;
				}
			}
		}

		public void OnJumpInputUp() {
			if (_velocity.y > _minJumpVelocity) {
				_velocity.y = _minJumpVelocity;
			}
		}

		public void OnDescendInputDown()
		{
			if (!_controller.collisions.below && _velocity.y < 0 && !_isAscending)
			{
				_isDescending = true;
				_gravity = _descendGravity;
			}
		}
		
		public void OnDescendInputUp()
		{
			if (_isDescending && !_isAscending)
			{
				_isDescending = false;
				_gravity = _normalGravity;
			}
		}
		
		private void HandleWallSliding() {
			_wallDirX = (_controller.collisions.left) ? -1 : 1;
			_wallSliding = false;
			if ((_controller.collisions.left || _controller.collisions.right) && !_controller.collisions.below && _velocity.y < 0) {
				_wallSliding = true;

				if (_velocity.y < -_wallSlideSpeedMax) {
					_velocity.y = -_wallSlideSpeedMax;
				}

				if (_timeToWallUnstick > 0) {
					_velocityXSmoothing = 0;
					_velocity.x = 0;

					if (_directionalInput.x != _wallDirX && _directionalInput.x != 0) {
						_timeToWallUnstick -= Time.deltaTime;
					}
					else {
						_timeToWallUnstick = _wallStickTime;
					}
				}
				else {
					_timeToWallUnstick = _wallStickTime;
				}
			}
		}

		private void CalculateVelocity() {
			float targetVelocityX = _directionalInput.x * _moveSpeed;
			_velocity.x = Mathf.SmoothDamp (_velocity.x, targetVelocityX, ref _velocityXSmoothing, 
				(_controller.collisions.below)?_accelerationTimeGrounded:_accelerationTimeAirborne);
			_velocity.y += _gravity * Time.deltaTime;
		}

		private void OnPlayerExitedAscend()
		{
			_isAscending = false;
			_gravity = _normalGravity;
		}

		private void OnPlayerEnteredAscend()
		{
			_isAscending = true;
			_gravity = _ascendGravity;
		}
		
		public void OnFootStep() => OnPlayerFootStep?.Invoke();
	}
}
