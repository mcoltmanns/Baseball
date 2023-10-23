using System;
using Baseball.Content.Items.Ammo;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Baseball.Content.Items.Weapons
{
    public class CricketBat : Bat
    {
        private double sweetSpotSize = 0.1;
        private Random rand;

        public override void SetDefaults()
        {
            base.SetDefaults();
            rand = new Random();
            wobble = 0.2;
            double sweetSpotStart = (1 - sweetSpotSize) * rand.NextDouble();
            sweetSpot = (sweetSpotStart, sweetSpotStart + sweetSpotSize);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            double sweetSpotStart = (1 - sweetSpotSize) * rand.NextDouble();
            sweetSpot = (sweetSpotStart, sweetSpotStart + sweetSpotSize);
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override void SweetSpot(EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, double hitPower)
        {
            Item.shoot = ModContent.ItemType<LocustBait>(); // switch to locust ball
            base.SweetSpot(source, position, velocity, type, damage, knockback, hitPower); // fire without wobble factor
            Item.shoot = ammoID; // switch back to default ball
            // reposition sweet spot brackets
            double sweetSpotStart = (1 - sweetSpotSize) * rand.NextDouble();
            sweetSpot = (sweetSpotStart, sweetSpotStart + sweetSpotSize);
        }
    }
}