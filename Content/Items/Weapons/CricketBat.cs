using System;
using System.Diagnostics;
using Baseball.Common.Players;
using Baseball.Content.Items.Ammo;
using log4net.Repository.Hierarchy;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Baseball.Content.Items.Weapons
{
    public class CricketBat : Bat
    {
        private double sweetSpotSize = 0.5;
        private Random rand;

        public override void SetDefaults()
        {
            base.SetDefaults();
            rand = new Random();
            wobble = 0.2;
            double sweetSpotStart = (1 - sweetSpotSize) * rand.NextDouble();
            sweetSpot = (sweetSpotStart, sweetSpotStart + sweetSpotSize);

            ammoID = ModContent.ItemType<LocustBait>();
            Item.useAmmo = ammoID;
        }
        
        public override void SweetSpot(EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, double hitPower)
        {
            Vector2 velocityWithPower = new((float)(velocity.X * hitPower * globalVelocityModifier), (float)(velocity.Y * hitPower * globalVelocityModifier));
            Mod.Logger.Debug("velocityWithPower is " + velocityWithPower.ToString() + " || velocity is " + velocity.ToString() + " || power is " + hitPower.ToString());
            Projectile.NewProjectile(source, source.Player.Center, velocityWithPower, type, (int)(damage * hitPower), knockback, source.Player.whoAmI, ai0:1); // shoot a locustball
            // reposition sweet spot brackets
            double sweetSpotStart = (1 - sweetSpotSize) * rand.NextDouble();
            sweetSpot = (sweetSpotStart, sweetSpotStart + sweetSpotSize);
        }
    }
}