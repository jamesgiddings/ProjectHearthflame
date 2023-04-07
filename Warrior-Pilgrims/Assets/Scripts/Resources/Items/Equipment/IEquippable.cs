namespace GramophoneUtils.Items
{
	public interface IEquippable
	{
		public void Equip(ICharacter statSystem);
		public void Unequip(ICharacter statSystem);
	}
}