using Baseball.Common.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Baseball.Content.Items.Weapons
{
    /*
    Abstract class for bat shooting behavior. Handles all the communication with ShotPowerPlayer and ShotPowerMeter.
    Each new bat must implement this class. Defaults, recipes, and other behaviors are implemented by overriding the appropriate ModItem method.
    */
    public abstract class Bat : ModItem{
        public double globalVelocityModifier = 1;

        //BatPlayer batPlayer;
        // don't forget to override!
        private double powerMeterRate = 1; // how fact does this bat's power meter oscillate (units/sec)
        private double reflectionTime = 0.5; // how long is the reflection hitbox active? (units/sec)
        private double reflectionCooldown = 1; // how long is the player forced to wait in between reflection attempts? (units/sec)

        // Override me!
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.holdStyle = ItemHoldStyleID.HoldGuitar; // guitar looks pretty solid
            Item.useTime = 10; // cooldown
            Item.useAnimation = 10; // animation time
            Item.autoReuse = false;

            Item.DamageType = DamageClass.Melee;
            Item.damage = 50;
            Item.knockBack = 6;
            Item.crit = 6; // crit chance

            Item.value = Item.buyPrice(copper: 50);
            Item.rare = ItemRarityID.Gray;

            Item.shoot = ModContent.ProjectileType<Projectiles.Ball>(); // need to define what gets shot, as well as the ammo used.
            Item.useAmmo = ModContent.ItemType<Ammo.BallAmmo>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var batPlayer = Main.LocalPlayer.GetModPlayer<BatPlayer>(); // despite what it looks like, this cannot be a class field. compiler does not like that at all

            if(batPlayer.isFirstShot) // start the power getting loop, but don't shoot
            {
                batPlayer.powerRate = powerMeterRate;
                batPlayer.isFirstShot = false;
                batPlayer.power = 0;
                // power loop handled by the shotPowerMeter - via isCalibratingPower as flag
                batPlayer.isCalibratingPower = true;
            }
            else // power is dialled, now we can shoot
            {
                batPlayer.isFirstShot = true;
                batPlayer.isCalibratingPower = false;
                Vector2 velocityWithPower = new((float)(velocity.X * batPlayer.power * globalVelocityModifier), (float)(velocity.Y * batPlayer.power * globalVelocityModifier));
                Projectile.NewProjectile(source, source.Player.Center, velocityWithPower, type, (int)(damage * batPlayer.power), knockback, source.Player.whoAmI);
                batPlayer.power = 0;
            }
            return false;
        }

        // reflect balls on right click. return true to override default behavior
        public override bool AltFunctionUse(Player player)
        {
            var batPlayer = Main.LocalPlayer.GetModPlayer<BatPlayer>();
            if(batPlayer.canReflect) batPlayer.isReflecting = true;
            if(batPlayer.isReflecting) Item.holdStyle = ItemHoldStyleID.HoldGolfClub; // not that great. consider it a placeholder
            else Item.holdStyle = ItemHoldStyleID.HoldGuitar;
            return false;
        }

        public override bool CanUseItem(Player player)
        {
            var batPlayer = Main.LocalPlayer.GetModPlayer<BatPlayer>();
            if(batPlayer.isReflecting) return false; // can't do anything if we're reflecting
            return true;
        }
    }
}