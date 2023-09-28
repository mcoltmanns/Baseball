using System;
using Baseball.Common.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Baseball.Content.Projectiles
{
    public class Ball : ModProjectile
    {
		public float bounceRestitution;
		public float reflectRestitution;

		public override void SetDefaults()
        {
			Projectile.arrow = true;
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.aiStyle = ProjAIStyleID.Arrow; // or 1
			Projectile.friendly = true;
			Projectile.hostile = false; // having the projectile be hostile but not hurt players is the easiest way to get it to reflect
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 5;
			Projectile.tileCollide = true;
			bounceRestitution = 0.75f; // how much energy (speed AND damage) is lost on bounce?
			reflectRestitution = 1f; // how much energy (speed AND damage) is lost on reflection?
			AIType = ProjectileID.WoodenArrowFriendly;
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers) // trying to mitigate player damage/knockback. not ideal!
        {
            modifiers.FinalDamage *= 0;
			modifiers.FinalDamage -= float.PositiveInfinity;
			modifiers.Knockback *= 0;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info) // this works, only if the projectile is hostile. also has the issue that it damages the player.
        {
            if(target.GetModPlayer<BatPlayer>().isReflecting)
			{
				Projectile.penetrate--;
				if(Projectile.penetrate <= 0) Projectile.Kill();
				else
				{
					if(Math.Abs(Projectile.velocity.X - Projectile.oldVelocity.X) > float.Epsilon)
					{
						Projectile.velocity.X = -Projectile.oldVelocity.X * bounceRestitution;
					}

					if(Math.Abs(Projectile.velocity.Y - Projectile.oldVelocity.Y) > float.Epsilon)
					{
						Projectile.velocity.Y = -Projectile.oldVelocity.Y * bounceRestitution;
					}

					Projectile.damage = (int)(Projectile.damage * bounceRestitution);
				}
			}
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