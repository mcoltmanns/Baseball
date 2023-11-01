using System;
using Baseball.Common.Players;
using Microsoft.Xna.Framework;
using Steamworks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Baseball.Content.Projectiles
{
	public class CurveBall : ModProjectile
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
			Projectile.ai[0] += 1f; // Use a timer to wait 15 ticks before applying gravity.
			Projectile.velocity.Y = Projectile.velocity.Y + 0.07f;
			if (Projectile.velocity.Y > 16f)
			{
				Projectile.velocity.Y = 16f;
			}
		}
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
			Random random = new Random();
            if (random.Next(3) == 0) {
				target.AddBuff(BuffID.Confused, 60 * 4);
			}
		}
    }
}