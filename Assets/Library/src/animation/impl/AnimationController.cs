using Library.src.util;
using UnityEngine;

namespace Library.src.animation.impl
{
    public class RatAnimationController : IAnimationController
    {
        readonly Animator anim;

        //cached animator values
        readonly int brawl = Animator.StringToHash(EnvironmentUtil.BRAWL);
        readonly int move = Animator.StringToHash(EnvironmentUtil.MOVEMENT);
        readonly int slash = Animator.StringToHash(EnvironmentUtil.SLASH);
        readonly int turning = Animator.StringToHash(EnvironmentUtil.TURNING);
        //for rewinding
        readonly int reverse = Animator.StringToHash(EnvironmentUtil.REWIND);

        public RatAnimationController(Animator anim)
        {
            this.anim = anim;
        }

        public void Idle()
        {
        }

        public void Rewind(bool rewinding)
        {
            anim.SetBool(reverse, rewinding);
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
    }
}
