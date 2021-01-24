using UnityEngine;

namespace Library.src.animation
{
    public class AnimationController
    {
        readonly Animator anim;

        bool reversing = false;
        
        public AnimationController(Animator anim)
        {
            this.anim = anim;
        }

        public void Idle()
        {
            
        }

        public void SetMoving(bool moving)
        {
            
        }

        public void SetRotation(float rotation)
        {
            
        }

        public void Fighting()
        {
            
        }

        public void SetReverse(bool reversing)
        {
            this.reversing = reversing;
        }
        
        
    }
}
