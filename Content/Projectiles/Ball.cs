using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Baseball.Content.Projectiles
{
    public abstract class Ball : ModProjectile
    {
		/// <summary>
		/// How much energy does this ball lose on each bounce? Affects travel speed and damage.
		/// </summary>
		public float bounceRestitution = 0.75f;

		// implementation should call base.SetDefaults and then change whatever it wants in its setDefaults
		public override void SetDefaults()
        {
			Projectile.arrow = true;
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.aiStyle = ProjAIStyleID.Arrow;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 5;
			Projectile.tileCollide = true;
			AIType = ProjectileID.WoodenArrowFriendly;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.penetrate--;
			if(Projectile.penetrate <= 0) Projectile.Kill();
			else
			{
				Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);

				if(Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
				{
					Projectile.velocity.X = -oldVelocity.X * bounceRestitution;
				}

				if(Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
				{
					Projectile.velocity.Y = -oldVelocity.Y * bounceRestitution;
				}

				Projectile.damage = (int)(Projectile.damage * bounceRestitution);
			}

			return false;
        }
    }
}