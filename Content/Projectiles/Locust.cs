using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Baseball.Content.Projectiles
{
    public class Locust : ModProjectile
    {
        public float seekRange = 400;
        public float seekSpeed = 10;

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
            Projectile.aiStyle = 0;
            AIType = 0;
        }

        //TODO: implement locust AI (swarming - home towards closest targe)
        public override void AI()
        {
            NPC target = FindClosestNPCInLineOfSight(seekRange);
            
            if(target != null) // if we find a target
            {
                // home
                Vector2 targetPos = target.position;
                Vector2 dir = Vector2.Normalize(Vector2.Subtract(targetPos, Projectile.position));
                Projectile.rotation = (float)Math.Atan2(dir.Y, dir.X);
                Projectile.position += dir * seekSpeed;
            }
            else // apply gravity TODO: replace this with a fancy idle behavior
            {
                Projectile.velocity.Y += 0.05f;
                if(Projectile.velocity.Y > 16) Projectile.velocity.Y = 16;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            Projectile.Kill();
        }

        public NPC FindClosestNPCInLineOfSight(float range)
        {
            NPC closest = null;

            float sqrRange = range * range;

            for(int k = 0; k < Main.maxNPCs; k++)
            {
                NPC target = Main.npc[k];
                // targetable checks: active, chaseable, max life > 5 (not critter), can take damage, hostile, not immortal
                if(target.CanBeChasedBy())
                {
                    float sqrDist = Vector2.DistanceSquared(target.Center, Projectile.Center);

                    if(sqrDist < sqrRange)
                    {
                        sqrRange = sqrDist;
                        closest = target;
                    }
                }
            }

            return closest;
        }
    }
}