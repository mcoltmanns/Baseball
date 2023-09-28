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

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var shotPowerPlayer = Main.LocalPlayer.GetModPlayer<ShotPowerPlayer>();
            if(shotPowerPlayer.isFirstShot) // start the power getting loop, but don't shoot
            {
                shotPowerPlayer.isFirstShot = false;
                shotPowerPlayer.power = 0;
                // power loop handled by the shotPowerMeter - via isCalibratingPower as flag
                shotPowerPlayer.isCalibratingPower = true;
            }
            else // power is dialled, now we can shoot
            {
                shotPowerPlayer.isFirstShot = true;
                shotPowerPlayer.isCalibratingPower = false;
                Vector2 velocityWithPower = new Vector2((float)(velocity.X * shotPowerPlayer.power * globalVelocityModifier), (float)(velocity.Y * shotPowerPlayer.power * globalVelocityModifier));
                Projectile.NewProjectile(source, source.Player.Center, velocityWithPower, type, (int)(damage * shotPowerPlayer.power), knockback, source.Player.whoAmI);
            }
            return false;
        }

        // Override me!
        public override void SetDefaults()
        {
            // width/height?

            Item.useStyle = ItemUseStyleID.Swing;
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
    }
}