using System;
using Baseball.Common.Players;
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
		public abstract float BounceRestitution { get; }
		/// <summary>
		/// Which vanilla AI program should this ball use? If your ball implements custom AI via override AI(), this field does not matter and should be set to 0. If your ball is implementing custom AI, make sure override PreAI() returns true!
		/// </summary>
		public abstract int AIStyle { get; }
		/// <summary>
		/// Can this ball hurt enemies?
		/// </summary>
		public abstract bool Friendly { get; }
		/// <summary>
		/// Can this ball hurt players and friendly NPCs?
		/// </summary>
		public abstract bool Hostile { get; }
		/// <summary>
		/// How many tiles or NPCs can this ball hit before dying? -1 penetrates endlessly.
		/// </summary>
		public abstract int Penetrate { get; }
		/// <summary>
		/// Can this ball collide with tiles?
		/// </summary>
		public abstract bool TileCollide { get; }
		/// <summary>
		/// Which vanilla projectile's behavior will this ball copy? Set to 0 for no copy.
		/// </summary>
		public abstract int AItype { get; }

		// Override me!
		public override void SetDefaults()
        {
			Projectile.arrow = true;
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.aiStyle = AIStyle;
			Projectile.friendly = Friendly;
			Projectile.hostile = Hostile;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = Penetrate;
			Projectile.tileCollide = TileCollide;
			AIType = AItype;
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
					Projectile.velocity.X = -oldVelocity.X * BounceRestitution;
				}

				if(Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
				{
					Projectile.velocity.Y = -oldVelocity.Y * BounceRestitution;
				}

				Projectile.damage = (int)(Projectile.damage * BounceRestitution);
			}

			return false;
        }
    }
}