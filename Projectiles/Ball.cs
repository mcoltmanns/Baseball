using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Baseball.Projectiles
{
    public class Ball : ModProjectile
    { public override void SetDefaults()
        {
			Projectile.arrow = true;
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.aiStyle = ProjAIStyleID.Arrow; // or 1
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			AIType = ProjectileID.WoodenArrowFriendly;
        }
    }
}