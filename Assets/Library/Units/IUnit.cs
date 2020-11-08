using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnit
{
    void Attack(GameObject target);
    void Flag(bool flag);
    void Move(Vector3 target);
    void TakeDamage(float damage);
    void Heal(float heal);

    bool InCombat();
}
