using UnityEngine;

namespace Interfaces
{
    public interface IDamageable
    {
        public void TakeDamage(float damage);   //this function should remove "damage" from a health value
    }
}
