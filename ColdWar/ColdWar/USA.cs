using System;

namespace ColdWar
{
    public class USA : Country
    {
        public string President { get; private set; }

        public USA(int population, List<string> cities, int warheadsCapacity, string president)
            : base("USA", 0, population, president, warheadsCapacity)
        {
            President = president;
        }

        public override decimal CalculateIncome()
        {
            return (Population / 1000) * 10;
        }
    }
}
