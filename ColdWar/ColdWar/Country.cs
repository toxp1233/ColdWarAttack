using System;
using System.Collections.Generic;

namespace ColdWar
{
    public abstract class Country
    {
        public string Name { get; private set; }
        public decimal Income { get; set; }
        public int Population { get; private set; }
        public string CurrentLeader { get; private set; }
        public int WarheadsCapacity { get; private set; }

        protected Country(string name, decimal income, int population, string currentLeader, int warheadsCapacity)
        {
            Name = name;
            Income = income;
            Population = population;
            CurrentLeader = currentLeader;
            WarheadsCapacity = warheadsCapacity;
        }

        public abstract decimal CalculateIncome();

        // Method to modify Population
        public void AdjustPopulation(int amount)
        {
            Population = Math.Max(0, Population + amount);
        }

        // Method to modify WarheadsCapacity
        public void AdjustWarheadsCapacity(int amount)
        {
            WarheadsCapacity = Math.Max(0, WarheadsCapacity + amount);
        }

        public void LaunchAttack(Country targetCountry, int warheadsToLaunch)
        {
            if (WarheadsCapacity < warheadsToLaunch)
            {
                Console.WriteLine($"{Name} does not have enough warheads to launch {warheadsToLaunch} rockets at {targetCountry.Name}.");
                return;
            }

            AdjustWarheadsCapacity(-warheadsToLaunch);
            Console.WriteLine($"{Name} launched {warheadsToLaunch} rockets at {targetCountry.Name}. {Name} has {WarheadsCapacity} warheads remaining.");

            targetCountry.OnAttackReceived(warheadsToLaunch);
        }

        protected virtual void OnAttackReceived(int incomingWarheads)
        {
            int populationImpact = incomingWarheads * 1000;
            AdjustPopulation(-populationImpact);

            Console.WriteLine($"{Name} is under attack! {incomingWarheads} rockets have hit, reducing the population by {populationImpact}. Current Population: {Population}");
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"Country: {Name}");
            Console.WriteLine($"Income: {Income:C}");
            Console.WriteLine($"Population: {Population}");
            Console.WriteLine($"Current Leader: {CurrentLeader}");
            Console.WriteLine($"Warheads Capacity: {WarheadsCapacity}");
        }
    }
}
