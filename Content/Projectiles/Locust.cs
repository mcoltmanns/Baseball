using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Baseball.Content.Projectiles
{
    public class Locust : ModProjectile
    {
        private readonly float seekRange = 500;
        private readonly float seekSpeed = 10;
        private readonly float separationDist = 10;
        private readonly float boidsRange = 400;
        private readonly float maxSpeed = 16f; // should not be greater than 16!
        private readonly int lifetime = 15 * 60; // lifetime in frames
        private readonly int spawnParticle = Terraria.ID.DustID.Sluggy; //TODO: finalize
        private readonly int deathParticle = Terraria.ID.DustID.Sluggy;
        private readonly Terraria.Audio.SoundStyle spawnSound = Terraria.ID.SoundID.Item1; //TODO: finalize
        private readonly Terraria.Audio.SoundStyle deathSound = Terraria.ID.SoundID.Item1;

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

        // ai[0] is general purpose frame timer
        //TODO: locust lifetimes
        public override void AI()
        {
            NPC target = FindClosestNPCInLineOfSight(seekRange);
            
            if(target != null) // if we find a target
            {
                // home
                Vector2 targetPos = target.position;
                Vector2 dir = Vector2.Normalize(Vector2.Subtract(targetPos, Projectile.position));
                Projectile.velocity = dir * seekSpeed;
            }
            else 
            {
                // boids with other locusts
                Vector2 swarmCenter = Vector2.Zero;
                Vector2 sep = Vector2.Zero;
                Vector2 avgRelativeVelocity = Vector2.Zero;
                float boidsCount = 0;
                for(int k = 0; k < Main.maxProjectiles; k++)
                {
                    Projectile other = Main.projectile[k]; //TODO: doing it this way is probably slow
                    float distanceSquare = Vector2.DistanceSquared(Projectile.Center, other.Center);
                    if(other.type == Projectile.type && distanceSquare < boidsRange * boidsRange && other.identity != Projectile.identity) // if other is a boid and within distance range and not this boid
                    {
                        boidsCount ++;  
                        // do boid stuff
                        // distance check
                        // if going to collide, enforce separation
                        if(Vector2.DistanceSquared(Projectile.Center + Projectile.velocity, other.Center + other.velocity) < separationDist * separationDist) // boid separation
                        {
                            sep -= other.Center - Projectile.Center;
                        }
                        // move with flock
                        // calculate center of mass
                        swarmCenter += other.Center;
                        // velocity match
                        avgRelativeVelocity += other.velocity;
                    }
                }
                // Make all adjustments with velocity, and clamp velocity to 16! this prevents phasing through blocks
                swarmCenter = swarmCenter / boidsCount - Projectile.Center;
                avgRelativeVelocity = avgRelativeVelocity / boidsCount - Projectile.velocity;
                Projectile.velocity += swarmCenter / 100f + sep / 1f + avgRelativeVelocity / 25f;

                if(Projectile.velocity.LengthSquared() > maxSpeed * maxSpeed) Projectile.velocity /= maxSpeed;//new Vector2(Projectile.velocity.X / maxSpeed, Projectile.velocity.Y / maxSpeed);

                // handle tile collisions
                Projectile.velocity = Collision.TileCollision(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            }

            Projectile.rotation = (float)Math.Atan(Projectile.velocity.Y / Projectile.velocity.X);

            if(Projectile.ai[0] == lifetime) Projectile.Kill();
            Projectile.ai[0]++;
        }

        public override void OnKill(int timeLeft)
        {
            for(int i = 0; i < 15; i++) Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, deathParticle);
            SoundEngine.PlaySound(deathSound);
        }

        public override void OnSpawn(IEntitySource source)
        {
            for(int i = 0; i < 15; i++) Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, spawnParticle);
            SoundEngine.PlaySound(spawnSound);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            Projectile.Kill();
        }

        private NPC FindClosestNPCInLineOfSight(float range)
        {
            NPC closest = null;
            float sqrRange = range * range;
            for(int k = 0; k < Main.maxNPCs; k++)
            {
                NPC target = Main.npc[k];
                // targetable checks: active, chaseable, max life > 5 (not critter), can take damage, hostile, not immortal
                if(target.CanBeChasedBy() && Collision.CanHit(Projectile, target))
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