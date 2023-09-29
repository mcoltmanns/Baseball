using Terraria.ID;

namespace Baseball.Content.Projectiles
{
    public class BasicBall : Ball
    {
        public override float BounceRestitution => 0.75f;
        public override int AIStyle => ProjAIStyleID.Arrow;
        public override bool Friendly => true;
        public override bool Hostile => false;
        public override int Penetrate => 5;
        public override bool TileCollide => true;
        public override int AItype => ProjectileID.WoodenArrowFriendly;
    }
}