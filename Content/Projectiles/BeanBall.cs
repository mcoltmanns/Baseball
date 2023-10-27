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
			if (Projectile.ai[0] < 60f)
			{
				Projectile.ai[2] = 0;
				Projectile.ai[0] = 60f;
				Projectile.velocity.Y = Projectile.velocity.Y + 0.1f;
			}
			else {
				if (Projectile.ai[2] == 0) {
					for(int i = 0; i < 200; i++) {
						NPC target = Main.npc[i];
						if(!target.friendly && target.active) {
							float shootToX = target.position.X + (float)target.width * 0.5f - Projectile.Center.X;
           					float shootToY = target.position.Y - Projectile.Center.Y;
           					float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
							if (distance < 480f) {
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
					distance = 3f / distance;
   
               		//Multiply the distance by a multiplier if you wish the projectile to have go faster
               		shootToX *= distance * 5;
               		shootToY *= distance * 5;

               		//Set the velocities to the shoot values
               		Projectile.velocity.X = shootToX;
               		Projectile.velocity.Y = shootToY;
				}
			}
		}
	}
}