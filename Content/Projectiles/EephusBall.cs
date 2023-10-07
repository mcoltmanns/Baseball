using System;
using Baseball.Common.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Baseball.Content.Projectiles
{
	public class EephusBall : ModProjectile
	{
		
		public override void SetDefaults()
		{
            Projectile.width = 10; // The width of projectile hitbox
			Projectile.height = 10; // The height of projectile hitbox

			Projectile.aiStyle = 0; // The ai style of the projectile (0 means custom AI). For more please reference the source code of Terraria
			Projectile.DamageType = DamageClass.Magic; // What type of damage does this projectile affect?
			Projectile.friendly = true; // Can the projectile deal damage to enemies?
			Projectile.hostile = false; // Can the projectile deal damage to the player?
			Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water? PERHAPS CHANGE
			Projectile.tileCollide = true; // Can the projectile collide with tiles?
			Projectile.extraUpdates = 1;
		}

		public override void AI() {
			Projectile.velocity.Y = Projectile.velocity.Y + 0.2f; // 0.1f for arrow gravity, 0.4f for knife gravity
            if (Projectile.velocity.Y > 16f) // This check implements "terminal velocity". We don't want the projectile to keep getting faster and faster. Past 16f this projectile will travel through blocks, so this check is useful.
            {
	            Projectile.velocity.Y = 16f;
            }
            if (Projectile.velocity.Y > -0.1 && Projectile.velocity.Y < 0.1) {
				float change = Math.Min(2, Projectile.velocity.X * 0.2f);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X + change, Projectile.velocity.Y, ModContent.ProjectileType<EephusBall>(), 10, 10, Projectile.owner);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X - change, Projectile.velocity.Y, ModContent.ProjectileType<EephusBall>(), 10, 10, Projectile.owner);
            }
		}
	}
}