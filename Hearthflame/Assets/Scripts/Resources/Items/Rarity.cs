using UnityEngine;

namespace GramophoneUtils.Items
{
    [CreateAssetMenu(fileName = "New Rarity", menuName = "Items/Rarity")]
    public class Rarity : ScriptableObject
    {
        [SerializeField] private new string name = "New Rarity Name";
        [SerializeField] private Color colour = new Color(255, 255, 255, 1);

        public string Name => name;
        public UnityEngine.Color Colour => colour;
    }
}
