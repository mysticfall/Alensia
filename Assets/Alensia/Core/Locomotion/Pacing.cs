namespace Alensia.Core.Locomotion
{
    public class Pacing
    {
        public readonly string Name;

        public readonly float SpeedModifier;

        public Pacing(string name, float speedModifier)
        {
            Name = name;
            SpeedModifier = speedModifier;
        }

        public static Pacing Walking(float speedModifier = 1)
        {
            return new Pacing("Walking", speedModifier);
        }

        public static Pacing Crawling(float speedModifier = 0.2f)
        {
            return new Pacing("Crawling", speedModifier);
        }

        public static Pacing Crouching(float speedModifier = 0.5f)
        {
            return new Pacing("Crouching", speedModifier);
        }

        public static Pacing Running(float speedModifier = 2)
        {
            return new Pacing("Running", speedModifier);
        }
    }
}