using System.Transactions;
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
    public abstract class Bat : ModItem {
        public double globalVelocityModifier = 1;

        //BatPlayer batPlayer;
        // don't forget to override!
        private double powerMeterRate = 1; // how fact does this bat's power meter oscillate (units/sec)

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

            Item.shoot = 10; // 10 is convention. set to 0 for no shooting
            Item.useAmmo = ModContent.ItemType<Ammo.BallAmmo>(); // ammo shot is determined here
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var batPlayer = Main.LocalPlayer.GetModPlayer<BatPlayer>(); // despite what it looks like, this cannot be a class field. compiler does not like that at all

            if(!batPlayer.isInRangedMode) return false; // if not in ranged mode do nothing

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

        // shoot balls on right click. return true to override default behavior
        public override bool AltFunctionUse(Player player)
        {
            var batPlayer = Main.LocalPlayer.GetModPlayer<BatPlayer>();
            batPlayer.isInRangedMode = !batPlayer.isInRangedMode;
            return false;
        }
    }
}