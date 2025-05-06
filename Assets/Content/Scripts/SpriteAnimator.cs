// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// SpriteAnimator
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
	public SpriteRenderer spriteRenderer;

	public SpriteAnimation[] animations;

	[HideInInspector] public string currentFrameTag = "null";
	
	private SpriteAnimation currentAnimation;

	private int currentAnimID;

	private float frameTimer;

	private int currentFrame;

	private void Start()
	{
		currentAnimation = animations[0];
		currentAnimID = 0;
		if (spriteRenderer == null)
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
		}
		if (spriteRenderer == null)
		{
			Debug.LogError("No SpriteRenderer found on this GameObject.");
		}
		else
		{
			PlayAnimation(currentAnimation.animationName);
		}
	}

	private void Update()
	{
		if (animations.Length != 0)
		{
			Animate();
		}
	}

	private void Animate()
	{
		SpriteAnimation spriteAnimation = animations[currentAnimID];
		frameTimer += Time.deltaTime;
		if (frameTimer >= 1f / spriteAnimation.frameRate)
		{
			frameTimer = 0f;
			currentFrame = (currentFrame + 1) % spriteAnimation.frames.Length;
			spriteRenderer.sprite = spriteAnimation.frames[currentFrame];
			
			if (currentFrame == spriteAnimation.frames.Length - 1 && !spriteAnimation.loop)
			{
				PlayAnimation(0, true);
			}
		}
	}

	public void PlayAnimation(int animationID, bool forcePlay = false)
	{
		if(currentAnimation.unstoppable && !forcePlay) return;
		
		//Debug.Log("Playing animation: " + animationID);
		if (currentAnimID != animationID)
		{
			if (animationID < 0 || animationID >= animations.Length)
			{
				Debug.LogError("Invalid animation ID.");
				return;
			}
			currentAnimID = animationID;
			currentAnimation = animations[animationID];
			currentFrame = 0;
			frameTimer = 0f;
			spriteRenderer.sprite = animations[currentAnimID].frames[currentFrame];
		}
	}

	public void PlayAnimation(string animationName)
	{
		if(currentAnimation.unstoppable) return;

		if (currentAnimation.animationName == animationName)
		{
			return;
		}
		int num = -1;
		for (int i = 0; i < animations.Length; i++)
		{
			if (animations[i].animationName == animationName)
			{
				num = i;
				break;
			}
		}
		if (num == -1)
		{
			Debug.LogError("Animation with name '" + animationName + "' not found.");
		}
		else
		{
			PlayAnimation(num);
		}
	}

	public void StopAnimation()
	{
		currentAnimID = 0;
		currentAnimation = animations[0];
		spriteRenderer.sprite = null;
	}

	public int GetCurrentAnimIndex()
	{
		return currentAnimID;
	}

	public string GetCurrentAnimName()
	{
		return currentAnimation.animationName;
	}
}