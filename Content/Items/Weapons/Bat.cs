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

        // don't forget to override!
        /// <summary>
        /// Oscillation speed of this bat's power meter in units/second.
        /// </summary>
        public abstract double PowerMeterRate { get; }
        /// <summary>
        /// The ammoId of the ammunition this bat will shoot in ranged mode. Should be provided via ModContent.ItemType<...>();
        /// </summary>
        public abstract int AmmoId { get; }
        /// <summary>
        /// This bat's use cooldown. This bat's animation time is equal to its use time.
        /// </summary>
        public abstract int UseTime { get; }
        /// <summary>
        /// This bat's base damage in melee mode.
        /// </summary>
        public abstract int Damage { get; }
        /// <summary>
        /// This bat's knockback in melee mode. Maximum is 20.
        /// </summary>
        public abstract int Knockback { get; }
        /// <summary>
        /// This bat's base crit chance in melee mode. Remember that the player already has a crit chance of 4!
        /// </summary>
        public abstract int CritChance { get; }
        /// <summary>
        /// This bat's price in copper coins. 1 platinum = 100 gold = 10,000 silver = 1,000,000 copper.
        /// </summary>
        public abstract int Price { get; }
        /// <summary>
        /// Rarity of this bat. Should be set to one of the values in ItemRarityID.
        /// </summary>
        public abstract int Rarity { get; }
        /// <summary>
        /// Sound this bat makes when used. Should return either an existing SoundID entry or a new Terraria.Audio.SoundStyle for custom sounds. 
        /// Return null for default behavior
        /// </summary>
        public abstract Terraria.Audio.SoundStyle UseSoundId { get; }
        /// <summary>
        /// Location of this bat's sweet spot range. Hitting a shot in the sweet spot gives special effects.
        /// First value is start of the range, second value is end of range (both inclusive)
        /// </summary>
        public abstract (double, double) SweetSpotRange { get; }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.holdStyle = ItemHoldStyleID.HoldGuitar; // guitar looks pretty solid
            Item.useTime = UseTime; // cooldown
            Item.useAnimation = UseTime; // animation time - should be equal to cooldown
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Melee;
            Item.damage = Damage;
            Item.knockBack = Knockback;
            Item.crit = CritChance;

            Item.value = Item.buyPrice(copper: Price);
            Item.rare = Rarity;

            Item.shoot = ProjectileID.PurificationPowder; // ProjectileID.PurificationPowder (10) is convention. set to 0 for no shooting
            Item.useAmmo = AmmoId;
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
                    batPlayer.sweetSpotRange = SweetSpotRange;
                    batPlayer.powerRate = PowerMeterRate;
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
                    if(batPlayer.power >= SweetSpotRange.Item1 && batPlayer.power <= SweetSpotRange.Item2)
                    {
                        SweetSpot(source, position, velocity, type, damage, knockback, batPlayer.power);
                    }
                    // missed sweet spot
                    else
                    {
                        Vector2 velocityWithPower = new((float)(velocity.X * batPlayer.power * globalVelocityModifier), (float)(velocity.Y * batPlayer.power * globalVelocityModifier));
                        Projectile.NewProjectile(source, source.Player.Center, velocityWithPower, type, (int)(damage * batPlayer.power), knockback, source.Player.whoAmI);
                    }
                    batPlayer.power = 0;
                    batPlayer.isInSweetSpot = false;
                }
            }
            return false;
        }

        /// <summary>
        /// Can be overriden to implement special behavior on sweet spot hit.
        /// </summary>
        /// <param name="source">Source entity. Do not modify</param>
        /// <param name="position">Origin position</param>
        /// <param name="velocity">Origin velocity, e.g. if the player was moving</param>
        /// <param name="type">Do not modify</param>
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

            if(batPlayer.isInRangedMode) Item.useAmmo = AmmoId; // if in ranged mode, use ammo. if not, don't require ammo
            else Item.useAmmo = AmmoID.None;

            return false; // override default
        }
    }
}