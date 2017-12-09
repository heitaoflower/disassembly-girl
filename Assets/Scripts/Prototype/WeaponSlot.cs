
namespace Prototype
{
    public enum SlotIndex : int
    {
        None = 0,
        I,
        II,
        III,
        Count
    }
    public class WeaponSlot
    {
        public SlotIndex index = default(SlotIndex);

        public WeaponData weapon = null;

        public WeaponSlot(SlotIndex index, WeaponData weapon)
        {
            this.index = index;
            this.weapon = weapon;
        }

    }
}