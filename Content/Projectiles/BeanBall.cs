using System;
using Baseball.Common.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Baseball.Content.Projectiles
{
	public class BeanBall : ModProjectile
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

		public override void AI() 
		{
			Projectile.ai[0] += 1f; // Use a timer to wait 15 ticks before applying gravity.
			if (Projectile.ai[0] < 50f)
			{
				Projectile.ai[2] = 0;
				Projectile.velocity.Y = Projectile.velocity.Y + 0.1f;
			}
			else {
				if (Projectile.ai[2] == 0) {
					float curDist = 480f;
					Projectile.velocity.Y = Projectile.velocity.Y + 0.1f;
					for(int i = 0; i < 200; i++) {
						NPC target = Main.npc[i];
						if(target.CanBeChasedBy()) {
							float shootToX = target.position.X + (float)target.width * 0.5f - Projectile.Center.X;
           					float shootToY = target.position.Y - Projectile.Center.Y;
           					float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
							if (distance < curDist) {
								curDist = distance;
								Projectile.ai[2] = 1;
								Projectile.ai[1] = i;
							}
							

						}
					}
				}
				else {
					NPC target = Main.npc[(int) Projectile.ai[1]];
					float shootToX = target.position.X + (float)target.width * 0.5f - Projectile.Center.X;
           			float shootToY = target.position.Y - Projectile.Center.Y;
           			float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

               		shootToX /= distance;
               		shootToY /= distance;

               		//Set the velocities to the shoot values
               		Projectile.velocity.X += shootToX;
               		Projectile.velocity.Y += shootToY;
					float newSpeed = (float)System.Math.Sqrt((double)(Projectile.velocity.X * Projectile.velocity.X + Projectile.velocity.Y * Projectile.velocity.Y));
					
					float multiplier = 1 - (shootToX * Projectile.velocity.X + shootToY * Projectile.velocity.Y) / newSpeed;
					float strength = 0.3f * (3 - Math.Min(2, distance / 40));
					Projectile.velocity.X += strength * multiplier * (shootToX -  Projectile.velocity.X);
               		Projectile.velocity.Y += strength * multiplier * (shootToY -  Projectile.velocity.Y);
					if (newSpeed > 25f) {
						Projectile.velocity.X *= 25 / newSpeed;
						Projectile.velocity.Y *= 25 / newSpeed;
					}
				}
			}
		}
	}
}