namespace Library.src.animation
{
    public interface IAnimationController
    {
        //state
        void Idle();
        void Rewind(bool rewinding);
        
        //movement
        void SetMoving(bool moving);
        
        //combat
        void SetBrawling(bool brawling);
        void SetSlash();

        //general
        void SetTurning(float deltaRotation);
    }
}