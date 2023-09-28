using Baseball.Content.Items.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Baseball.Common.Players
{
    /*
    The engine is a little strange (to me). Instead of looking for instances of objects globally, you have to use the player's fields to store values that you want to be accessible
    across more than one object. See the docs: http://docs.tmodloader.net/docs/stable/class_mod_player.html#details
    As far as I can tell, we would need a new extension of ModPlayer any time we have another class of variables that are important to multiple objects. In this case, I'm using this
    player as a place to put all the cross-class fields needed to make the shot power feature functional.
    Working instance can be got like this: var foo = Main.LocalPlayer.GetModPlayer<ShotPowerMeterPlayer>();
    */
    public class ShotPowerPlayer : ModPlayer
    {
        public double power; // how much shot power does this player have stored?
        private const double MIN_POWER = 0;
        private const double MAX_POWER = 1;
        public double powerRate; // how fast does this player's shot power change? (points/sec)
        public bool isFirstShot;
        public bool isCalibratingPower;

        public override void Initialize()
        {
            power = MIN_POWER;
            powerRate = 1;
            isFirstShot = true;
            isCalibratingPower = false;
        }

        public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if(item.type != ModContent.ItemType<Bat>()) return true; // don't want to modify non-bat shooting behaviors
            // maybe have to do the looping in here somehow? but no way to detect mouse down/up
            if(isFirstShot) // start the power getting loop, but don't shoot
            {
                isFirstShot = false;
                power = MIN_POWER;
                // power loop handled by the shotPowerMeter - via isCalibratingPower as flag
                isCalibratingPower = true;
            }
            else // power is dialled, now we can shoot
            {
                isFirstShot = true;
                isCalibratingPower = false;
                Vector2 velocityWithPower = new Vector2((float)(velocity.X * power), (float)(velocity.Y * power));
                Projectile.NewProjectile(source, source.Player.Center, velocityWithPower, type, (int)(damage * power), knockback, source.Player.whoAmI);
            }
            return false;
        }

        public void CalibratePower(GameTime deltaTime)
        {
            power += powerRate * deltaTime.ElapsedGameTime.TotalSeconds;
            // if we are at the end of the meter, reverse the direction of growth. also clamp. clamp shouldn't be needed, but is good for peace of mind
            if(power > 1)
            {
                power = 1;
                powerRate = -powerRate;
            }
            else if(power < 0){
                power = 0;
                powerRate = -powerRate;
            }
        }
    }
}