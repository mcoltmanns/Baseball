using System;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Baseball.Content.Projectiles
{
    public class LocustBait : ModProjectile
    {
        private readonly float bounceRestitution = 0.75f;
        private readonly int framesToLocusts = 1 * 60; // assume 60fps

        public override void SetDefaults()
        {
            //Projectile.arrow = true;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.aiStyle = 0;
            AIType = 0;
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

        // sweet spot flag is ai[0] - when 1, we hit the sweet spot
        // ai[1] is locust timer
        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            Projectile.netUpdate = true;

            Projectile.velocity.Y += 0.2f; // gravity
            if(Projectile.velocity.Y > 16) Projectile.velocity.Y = 16; // terminal velocity

            Projectile.ai[1] += 1;
            if(Projectile.ai[1] >= framesToLocusts)
            {
                if(Projectile.ai[0] == 1)
                {
                    //LOCUSTS!!
                    // locusts should probably only happen if main.myplayer == projectile.owner
                    Mod.Logger.Debug("LOCUSTS!!");
                    Projectile.Kill();
                }
            }
        }
    }
}