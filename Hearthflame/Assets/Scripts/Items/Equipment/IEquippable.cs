using GramophoneUtils.Stats;

namespace GramophoneUtils.Items
{
	public interface IEquippable
	{
		public void Equip(StatSystem statSystem);
		public void Unequip(StatSystem statSystem);
	}
}