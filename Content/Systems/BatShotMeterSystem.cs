using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace Baseball.Content.Systems
{
    public class BatShotMeterSystem : ModSystem
    {
        internal UserInterface BatShotMeterInterface;
        internal UI.BatShotMeterUI UI;

        private GameTime _lastUpdateUiGameTime;

        public override void Load()
        {
            if(!Main.dedServ) // only activate ui if we aren't a dedicated server
            {
                BatShotMeterInterface = new UserInterface();

                UI = new UI.BatShotMeterUI();
                UI.Activate();
            }
        }
    }
}