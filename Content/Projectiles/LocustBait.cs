using System;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Baseball.Content.Projectiles
{
    public class LocustBait : ModProjectile
    {
        private readonly float bounceRestitution = 0.75f;
        private readonly int framesToLocusts = 10; // assume 60fps

        public override void SetDefaults()
        {
            Projectile.arrow = true;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // here we actually want to bounce forever, since the locust timer determines when the projectile dies
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

			return false;
        }

        public override void AI()
        {
            //FIXME: something very bad here.... makes the ui disappear and game stop taking input
            /*Projectile.ai[0] += 1f;
            if(Projectile.ai[0] >= framesToLocusts)
            {
                //TODO: LOCUSTS!!!!
                if(Projectile.owner == Main.myPlayer)
                {
                    // only spawn locusts if we are the main player
                    for(int i = 0; i < 100; i++)
                    {
                        //Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ModContent.ProjectileType<Locust>(), 20, 0f, Projectile.owner, 0f, 0f);
                    }
                }
                Projectile.Kill();
            }*/

            // gravity
            Projectile.velocity.Y = Projectile.velocity.Y + 0.2f; // tweak this value - 0.1 is arrows, 0.4 is knives
            if(Projectile.velocity.Y > 16f) Projectile.velocity.Y = 16f; // terminal velocity - cannot be greater than 16 or we travel thru blocks
        }
    }
}