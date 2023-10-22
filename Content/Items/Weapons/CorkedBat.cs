using Baseball.Content.Items.Ammo;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace Baseball.Content.Items.Weapons{
    public class CorkedBat : Bat
    {
        public override double PowerMeterRate => 1;

        public override int AmmoId => ModContent.ItemType<BasicBallAmmo>();

        public override int UseTime => 20; // TODO balance me!

        public override int Damage => 50; // TODO balance me!

        public override int Knockback => 6; // TODO balance me!

        public override int CritChance => 6; // TODO balance me!

        public override int Price => 10000; // TODO balance me!

        public override int Rarity => ItemRarityID.Green; // TODO balance me!

        public override SoundStyle UseSoundId => SoundID.Item1; // TODO placeholder

        public override (double, double) SweetSpotRange => (0.8, 1); // TODO balance me!

        public override double Wobble => 0.25; // TODO balance me!

        public override void SweetSpot(EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, double hitPower)
        {
            hitPower *= 2;
            // powerful, inaccurate shot.
            Vector2 velocityWithPower = new((float)(velocity.X * hitPower * globalVelocityModifier), (float)(velocity.Y * hitPower * globalVelocityModifier));
            Projectile.NewProjectile(source, source.Player.Center, ApplyWobble(velocityWithPower, Wobble * 1000), type, (int)(damage * hitPower), knockback, source.Player.whoAmI); // factor in wobble!
        }
    }
}