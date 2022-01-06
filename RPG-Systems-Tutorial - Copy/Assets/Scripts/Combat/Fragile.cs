using UnityEngine;

namespace GramophoneUtils.Combat
{
    public class Fragile : MonoBehaviour, IDamageable
    {
        public void DealDamage(int amount) => Destroy(gameObject);
    }
}
