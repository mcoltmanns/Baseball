using System;
using Baseball.Common.Players;
using Baseball.Content.Items.Ammo;
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
    For an example implementation, see Weapons/BasicBat.cs
    */
    public abstract class Bat : ModItem {
        public double globalVelocityModifier = 1;

        // don't forget to override!
        /// <summary>
        /// Oscillation speed of this bat's power meter in units/second.
        /// </summary>
        public double powerMeterRate = 1;
        /// <summary>
        /// Location of this bat's sweet spot range. Hitting a shot in the sweet spot gives special effects.
        /// First value is start of the range, second value is end of range (both inclusive)
        /// </summary>
        public (double, double) sweetSpot = (0.75, 1);
        /// <summary>
        /// "Wobble factor" for this bat. 0 is no wobble. Wobble scales with hit power.
        /// </summary>
        public double wobble = 0.25;
        /// <summary>
        /// Ammo that this bat shoots.
        /// </summary>
        public int ammoID = 0;

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.holdStyle = ItemHoldStyleID.HoldGuitar; // guitar looks pretty solid
            Item.useTime = 20; // cooldown
            Item.useAnimation = 20; // animation time - should be equal to cooldown
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Melee;
            Item.damage = 50;
            Item.knockBack = 6;
            Item.crit = 0;

            Item.value = Item.buyPrice(copper: 10000);
            Item.rare = ItemRarityID.Green;

            Item.shoot = ProjectileID.PurificationPowder; // ProjectileID.PurificationPowder (10) is convention. set to 0 for no shooting
            ammoID = ModContent.ItemType<BasicBall>();
            Item.useAmmo = ammoID;
        }

        public override void OnConsumeAmmo(Item ammo, Player player)
        {
            var batPlayer = Main.LocalPlayer.GetModPlayer<BatPlayer>(); // despite what it looks like, this cannot be a class field. compiler does not like that at all
            // only consume on second shot, when in ranged mode
            if(!batPlayer.isFirstShot && batPlayer.isInRangedMode) base.OnConsumeAmmo(ammo, player);
            else ammo.stack++; // otherwise increment ammo stack by 1 (keeps ammo the same)
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if(Main.myPlayer == player.whoAmI) // multiplayer safety. only the owner of the projectile should spawn it. still unclear on how this works! (projectiles aren't synced until after spawn maybe?)
            // if you want to spawn a projectile from an npc, check if(Main.netMode != NetmodeID.MultiplayerClient) - spawns the projectile only on the server's instance
            {
                var batPlayer = Main.LocalPlayer.GetModPlayer<BatPlayer>(); // despite what it looks like, this cannot be a class field. compiler does not like that at all

                if(!batPlayer.isInRangedMode) return false; // if not in ranged mode do nothing

                if(batPlayer.isFirstShot) // start the power getting loop, but don't shoot
                {
                    batPlayer.sweetSpotRange = sweetSpot;
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
                    // in sweet spot
                    if(batPlayer.power >= Math.Min(sweetSpot.Item1, sweetSpot.Item2) && batPlayer.power <= Math.Max(sweetSpot.Item1, sweetSpot.Item2)) // the min/max ideally wouldn't be needed but is safer - in case sweet spot limits ever get mixed up
                    {
                        SweetSpot(source, position, velocity, type, damage, knockback, batPlayer.power);
                    }
                    // missed sweet spot
                    else
                    {
                        Vector2 velocityWithPower = new((float)(velocity.X * batPlayer.power * globalVelocityModifier), (float)(velocity.Y * batPlayer.power * globalVelocityModifier));
                        Projectile.NewProjectile(source, source.Player.Center, velocityWithPower.RotatedByRandom(wobble * batPlayer.power), type, (int)(damage * batPlayer.power), knockback, source.Player.whoAmI, ai0:0); // factor in wobble! - scale based on hit power
                    }
                    batPlayer.power = 0;
                    batPlayer.isInSweetSpot = false;
                }
            }
            return false;
        }

        /// <summary>
        /// Can be overriden to implement special behavior on sweet spot hit.
        /// Default fires a ball without wobble factor
        /// </summary>
        /// <param name="source">Source entity. Do not modify</param>
        /// <param name="position">Origin position</param>
        /// <param name="velocity">Origin velocity, e.g. if the player was moving</param>
        /// <param name="type">Projectile type. Shouldn't need to be modified</param>
        /// <param name="damage">Base damage. Set by the bat, can be modified.</param>
        /// <param name="knockback">Base knockback. Set by the bat, can be modified.</param>
        /// <param name="hitPower">Hit power, determined by the power meter</param>
        public virtual void SweetSpot(EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, double hitPower)
        {
            Vector2 velocityWithPower = new((float)(velocity.X * hitPower * globalVelocityModifier), (float)(velocity.Y * hitPower * globalVelocityModifier));
            Projectile.NewProjectile(source, source.Player.Center, velocityWithPower, type, (int)(damage * hitPower), knockback, source.Player.whoAmI);
        }

        // switch to ranged mode on right click
        public override bool AltFunctionUse(Player player)
        {
            var batPlayer = Main.LocalPlayer.GetModPlayer<BatPlayer>();

            batPlayer.isInRangedMode = !batPlayer.isInRangedMode; // toggle mode

            Item.autoReuse = !batPlayer.isInRangedMode; // don't want auto reuse if in ranged mode, want it in melee mode

            if(batPlayer.isInRangedMode) Item.useAmmo = ammoID; // if in ranged mode, use ammo. if not, don't require ammo
            else Item.useAmmo = AmmoID.None;

            return false; // override default
        }
    }
}