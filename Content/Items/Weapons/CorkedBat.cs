using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria;

namespace Baseball.Content.Items.Weapons{
    public class CorkedBat : Bat
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            //TODO: balance defaults
            sweetSpot = (0.8, 1);
            wobble = 0.25;
        }

        public override void SweetSpot(EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, double hitPower)
        {
            hitPower *= 2;
            // powerful, inaccurate shot.
            //TODO: strong tests, make sure it really works
            Vector2 velocityWithPower = new((float)(velocity.X * hitPower * globalVelocityModifier), (float)(velocity.Y * hitPower * globalVelocityModifier));
            Projectile.NewProjectile(source, source.Player.Center, velocityWithPower.RotatedByRandom(wobble * 4 * hitPower), type, (int)(damage * hitPower), knockback, source.Player.whoAmI); // factor in wobble!
        }
    }
}