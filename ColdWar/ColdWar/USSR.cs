using System;

namespace ColdWar
{
    public class USSR : Country
    {
        public string Leader { get; private set; }

        public USSR(int population, List<string> cities, int warheadsCapacity, string leader)
            : base("USSR", 0, population, leader, warheadsCapacity)
        {
            Leader = leader;
        }

        public override decimal CalculateIncome()
        {
            return (Population / 1000) * 10;
        }
    }
}
