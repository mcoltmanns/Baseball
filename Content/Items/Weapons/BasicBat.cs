using Baseball.Content.Items.Ammo;
using Terraria.ID;
using Terraria.ModLoader;

namespace Baseball.Content.Items.Weapons
{
    public class BasicBat : Bat
    {
        public override double PowerMeterRate => 1;
        public override int AmmoId => ModContent.ItemType<BasicBallAmmo>();
        public override int UseTime => 20;
        public override int Damage => 50;
        public override int Knockback => 6;
        public override int CritChance => 0;
        public override int Price => 10000;
        public override int Rarity => ItemRarityID.Green;
        public override Terraria.Audio.SoundStyle UseSoundId => SoundID.Item1;
    }
}