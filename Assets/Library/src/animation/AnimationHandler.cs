﻿using Library.src.units;
using Library.src.util;
using UnityEngine;

namespace Library.src.animation
{
    public class AnimationHandler
    {
        Animator animator;
        UnitController controller;
        Unit unit;
        GameObject gameObject;
        
        //serializing animations
        readonly int brawl;
        readonly int move;
        readonly int slash;
        readonly int turn;
        

        public AnimationHandler(Unit unit)
        {
            this.unit = unit;
            controller = unit.controller;
            gameObject = controller.gameObject;

            if (!gameObject.TryGetComponent(out animator))
            {
                Debug.LogError("Cannot load animator attached to : " + gameObject.name);
            }
            
            animator.SetBool(EnvironmentUtil.ANIM_SLASH, true);

            brawl = Animator.StringToHash(EnvironmentUtil.ANIM_BRAWL);
            move = Animator.StringToHash(EnvironmentUtil.ANIM_SLASH);
            slash = Animator.StringToHash(EnvironmentUtil.ANIM_MOVE);
            turn = Animator.StringToHash(EnvironmentUtil.ANIM_TURNING);
        }


        public void Brawl(bool isBrawling)
        {
            animator.SetBool(brawl, isBrawling);
        }
        
        public void Move(bool isMoving) 
        {
            animator.SetBool(move, isMoving);
        }
        
        public void Slash()
        {
            animator.SetTrigger(slash);
        }
        
        public void Turn(float rotationDelta)
        {
            animator.SetFloat(turn, rotationDelta);
        }
    }
}