using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent;
using System.Collections.Generic;
using Baseball.Content.Items.Weapons;
using Baseball.Common.Players;

namespace Baseball.Content.UI
{
    public class ShotPowerMeter : UIState
    {
        private UIElement area; // ui element to put all the other stuff in
        private UIImage meterFrame;
        private Color gradientRed;
        private Color gradientGreen;
        private UIText powerText;

        public override void OnInitialize()
        {
            // put the area just under the player (just under center screen)
            area = new UIElement();
            area.Left.Set(-area.Width.Pixels - 600, 1f); // left and top here are irrelevant since later we follow mouse, but keep them just in case we don't want to follow mouse anymore!
            area.Top.Set(30, 0f);
            area.Width.Set(182, 0f);
            area.Height.Set(60, 0f);

            meterFrame = new UIImage(ModContent.Request<Texture2D>("Baseball/Content/UI/Textures/ExampleResourceFrame")); // placeholder!
            meterFrame.Left.Set(22, 0f);
            meterFrame.Top.Set(0, 0f);
            meterFrame.Width.Set(138, 0f); // width and height here should match up with the png's actual width and height, or at least should be an even ratio
            meterFrame.Height.Set(34, 0f);

            gradientRed = new Color(255, 0, 0); // red
            gradientGreen = new Color(0, 255, 0); // green

            powerText = new UIText("0", 0.8f);
            powerText.Width.Set(138, 0f);
            powerText.Height.Set(34, 0f);
            powerText.Top.Set(40, 0f);
            powerText.Left.Set(0, 0f);

            area.Append(powerText);
            area.Append(meterFrame); // pack the meter frame into the area
            Append(area); // pack the area into this class
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BatPlayer>();

            if(Main.LocalPlayer.HeldItem.ModItem is not Bat || !modPlayer.isInRangedMode) return; // only want to draw the meter if we're holding the bat, or in ranged mode

            base.Draw(spriteBatch);
        }

        // all draw logic goes in here!
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            var modPlayer = Main.LocalPlayer.GetModPlayer<BatPlayer>();

            // get the area within the meter frame to fill
            Rectangle fillArea = meterFrame.GetInnerDimensions().ToRectangle();
            fillArea.X += 12;
            fillArea.Width -= 24;
            fillArea.Y += 8;
            fillArea.Height -= 16;

            // draw gradient in fill area
            int left = fillArea.Left; // left bound
            int right = fillArea.Right; // right bound
            int stopAt = (int)((right - left) * modPlayer.power); // how much to fill, based on power level
            for(int i = 0; i < stopAt; i++){
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left + i, fillArea.Y, 1, fillArea.Height), Color.Lerp(gradientGreen, gradientRed, (float)i / (right - left))); // lerp relative to total fill area, not just area to fill
            }
        }

        public override void Update(GameTime gameTime)
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BatPlayer>();

            if(Main.LocalPlayer.HeldItem.ModItem is not Bat) return; // only need to do anything if we're holding a bat

            if(modPlayer.isInSweetSpot) powerText.TextColor = Color.Red; // set text color based on sweet spot
            else powerText.TextColor = Color.White;

            powerText.SetText(((int)(modPlayer.power * 100)).ToString());

            if(modPlayer.isCalibratingPower) modPlayer.CalibratePower(gameTime); // if we are calibrating power, tell the modplayer to do that. can't do that in the modplayer because there's no Update() there
            // TODO: tell BatPlayer to handle reflection timing logic

            // follow mouse
            area.Left.Set(Main.mouseX - 85, 0f);
            area.Top.Set(Main.mouseY + 34, 0f);
        }

        [Autoload(Side = ModSide.Client)] // only load this item if we are client-side
        internal class BatShotMeterSystem : ModSystem
        {
            private UserInterface BatShotMeterUserInterface;

            internal ShotPowerMeter BatShotMeter;

            public override void Load()
            {
                BatShotMeter = new();
                BatShotMeterUserInterface = new();
                BatShotMeterUserInterface.SetState(BatShotMeter);
            }

            public override void UpdateUI(GameTime gameTime)
            {
                BatShotMeterUserInterface?.Update(gameTime);
            }

            public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
            {
                int batShotMeterIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars")); // find the right layer to draw the bar
                if(batShotMeterIndex != -1){ // if the layer exists (i think?)
                    layers.Insert(batShotMeterIndex, new LegacyGameInterfaceLayer( // add this interface element to the layer
                        "Baseball Mod: Bat Shot Meter",
                        delegate{
                            BatShotMeterUserInterface.Draw(Main.spriteBatch, new GameTime());
                            return true;
                        },
                        InterfaceScaleType.UI)
                    );
                }
            }
        }
    }
}