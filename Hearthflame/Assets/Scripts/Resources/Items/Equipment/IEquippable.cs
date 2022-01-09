using GramophoneUtils.Stats;

namespace GramophoneUtils.Items
{
	public interface IEquippable
	{
		public void Equip(Character statSystem);
		public void Unequip(Character statSystem);
	}
}