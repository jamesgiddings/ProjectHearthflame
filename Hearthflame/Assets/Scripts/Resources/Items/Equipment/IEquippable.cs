using GramophoneUtils.Stats;

namespace GramophoneUtils.Items
{
	public interface IEquippable
	{
		public void Equip(GramophoneUtils.Characters.Character statSystem);
		public void Unequip(GramophoneUtils.Characters.Character statSystem);
	}
}