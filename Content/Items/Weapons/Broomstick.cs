using System;
using Baseball.Content.Items.Ammo;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Baseball.Content.Items.Weapons
{
    public class Broomstick : Bat
    {
        public override double PowerMeterRate => 1;

        public override int AmmoId => ModContent.ItemType<BasicBallAmmo>();

        public override int UseTime => 20; //TODO: balance me!

        public override int Damage => 50; //TODO: balance me!

        public override int Knockback => 6; //TODO: balance me!

        public override int CritChance => 6; //TODO: balance me!

        public override int Price => 10000; //TODO: balance me!

        public override int Rarity => ItemRarityID.Green; //TODO: balance me!

        public override SoundStyle UseSoundId => SoundID.Item1; //TODO: placeholder

        public override (double, double) SweetSpotRange => (0.8, 1); //TODO: balance me!

        public override double Wobble => 0.1; //TODO: balance me!

        public double breakageChance = 0.25; // what are the chances of breaking your broomstick when you hit the sweet spot?

        public override void SweetSpot(EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, double hitPower)
        {
            Random rand = new();
            if(rand.NextDouble() <= breakageChance)
            {
                Item.consumable = true; // make the item consumable
                //TODO: play special sound on break
                for(int i = 0; i < 10; i++)
                {
                    Dust.NewDust(position, source.Player.width, source.Player.height, DustID.WoodFurniture); // spawn some dust particles
                }
            }
            base.SweetSpot(source, position, velocity.RotatedByRandom(Wobble), type, damage, knockback, hitPower);
        }
    }
}