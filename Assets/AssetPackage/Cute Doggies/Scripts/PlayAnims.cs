using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnims : MonoBehaviour
{
	public List<string> mAnimNames = new List<string>();
	public Animator mAnimator;

	void Start()
	{
		if (mAnimator == null) mAnimator = gameObject.GetComponent<Animator>();
        mAnimator.Play("SlowWalk");
	}
	public void startwalk(){
        if (mAnimator == null) mAnimator = gameObject.GetComponent<Animator>();
        mAnimator.Play("SlowWalk");
	}
	public void walkStop(){
        if (mAnimator == null) mAnimator = gameObject.GetComponent<Animator>();
        mAnimator.Play("StIdle");
	}

	public void OnButtonClick( int aIndex )
	{
		if (mAnimator == null) mAnimator = gameObject.GetComponent<Animator>();
		mAnimator.Play( mAnimNames[ aIndex ] );
	}

	public void OnButtonClickWithAnimName( string aAnimStateName )
	{
		if (mAnimator == null) mAnimator = gameObject.GetComponent<Animator>();
		mAnimator.Play( aAnimStateName );
	}

}
