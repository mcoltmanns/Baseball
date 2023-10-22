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
    public class BatPlayer : ModPlayer
    {
        public double power; // how much shot power does this player have stored?
        private const double MIN_POWER = 0;
        private const double MAX_POWER = 1;
        public double powerRate; // how fast does this player's shot power change? (points/sec)
        public bool isFirstShot;
        public bool isCalibratingPower;
        public (double, double) sweetSpotRange; // what is the range on the sweet spot?

        public bool isInRangedMode;
        public bool isInSweetSpot;

        public override void Initialize()
        {
            power = MIN_POWER;
            powerRate = 1;
            isFirstShot = true;
            isCalibratingPower = false;

            isInRangedMode = false;
            isInSweetSpot = false;
        }

        public void CalibratePower(GameTime deltaTime)
        {
            power += powerRate * deltaTime.ElapsedGameTime.TotalSeconds;
            //isInSweetSpot = true;
            // if we are at the end of the meter, reverse the direction of growth. also clamp.
            if(power > MAX_POWER)
            {
                power = 1;
                powerRate = -powerRate;
            }
            else if(power < MIN_POWER)
            {
                power = 0;
                powerRate = -powerRate;
            }
            isInSweetSpot = power >= sweetSpotRange.Item1 && power <= sweetSpotRange.Item2;
        }
    }
}