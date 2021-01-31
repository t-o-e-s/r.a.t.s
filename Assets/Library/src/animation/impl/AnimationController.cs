using System;
using System.Collections;
using System.Collections.Generic;
using Library.src.units;
using Library.src.util;
using UnityEngine;

namespace Library.src.animation.impl
{
    public class RatAnimationController : IAnimationController
    {
        readonly Animator anim;
        //prefab with 3D model attached
        readonly GameObject model;
        readonly UnitController mono;

        //cached animator values
        readonly int brawl = Animator.StringToHash(EnvironmentUtil.BRAWL);
        readonly int move = Animator.StringToHash(EnvironmentUtil.MOVEMENT);
        readonly int slash = Animator.StringToHash(EnvironmentUtil.SLASH);
        readonly int turning = Animator.StringToHash(EnvironmentUtil.TURNING);
        //for rewinding
        readonly int reverse = Animator.StringToHash(EnvironmentUtil.REWIND);

        Coroutine rotationRoutine;

        public RatAnimationController(Animator anim, UnitController mono)
        {
            this.anim = anim;
            model = Util.GetFromChildren(anim.gameObject, EnvironmentUtil.MODEL);
            this.mono = mono;
        }

        public void Idle()
        {
            SetMoving(false);
            SetBrawling(false);
            Rewind(false);
        }

        public void Rewind(bool rewinding)
        {
            anim.SetBool(reverse, rewinding);
            if (rotationRoutine != null) mono.StopCoroutine(rotationRoutine);
            rotationRoutine = mono.StartCoroutine(RotateBody(rewinding));
        }

        public void SetMoving(bool moving)
        {
            anim.SetBool(move, moving);
        }
        
        public void SetBrawling(bool brawling)
        {
            anim.SetBool(brawl, brawling);
        }

        public void SetSlash()
        {
            anim.SetTrigger(slash);
        }

        public void SetTurning(float deltaRotation)
        {
            anim.SetFloat(turning, deltaRotation);
        }

        //method is used to rotate the 3d model of the unit 
        IEnumerator RotateBody(bool rewinding)
        {
            var parentRotation = model.transform.parent.rotation;
            var childRotation = model.transform.rotation;
            var lastParentRotation = parentRotation.eulerAngles.y;
            var targetRotation = !rewinding ? 180f : 0f;
            var childYValue = parentRotation.eulerAngles.y;
            
            do
            {
                var rotationDelta = parentRotation.eulerAngles.y - lastParentRotation;
                lastParentRotation = parentRotation.eulerAngles.y;

                childYValue += rewinding ? rotationDelta : -rotationDelta;
                model.transform.rotation = Quaternion.Euler(0f, childYValue, 0f);
                yield return null;
            }
            while (Mathf.Abs(targetRotation - childRotation.y) > 0.1f);
        }
    }
}
