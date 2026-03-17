using System;
using System.Collections.Generic;

namespace VehicleRentalSystem
{
    public abstract class Vehicle
    {
        private string make;
        private string model;
        private int year;
        private double dailyRate;

        public string Make
        {
            get { return make; }
            set { make = value; }
        }

        public string Model
        {
            get { return model; }
            set { model = value; }
        }

        public int Year
        {
            get { return year; }
            set { year = value; }
        }

        public double DailyRate
        {
            get { return dailyRate; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Daily rate must be greater than zero");
                dailyRate = value;
            }
        }

        public Vehicle(string make, string model, int year, double dailyRate)
        {
            this.make = make;
            this.model = model;
            this.year = year;
            DailyRate = dailyRate;
        }

        public abstract double CalculateRentalCost(int days);

        public virtual string GetDescription()
        {
            return $"{year} {make} {model} – RM {dailyRate:F2}/day";
        }
    }

    public class Car : Vehicle
    {
        private int numPassengers;

        public int NumPassengers
        {
            get { return numPassengers; }
            set { numPassengers = value; }
        }

        public Car(string make, string model, int year, double dailyRate, int numPassengers)
            : base(make, model, year, dailyRate)
        {
            this.numPassengers = numPassengers;
        }

        public override double CalculateRentalCost(int days)
        {
            return DailyRate * days;
        }

        public override string GetDescription()
        {
            return $"{Year} {Make} {Model} ({numPassengers} passengers) - RM {DailyRate:F2}/day";
        }
    }

    public class Motorcycle : Vehicle
    {
        private bool hasSidecar;

        public bool HasSidecar
        {
            get { return hasSidecar; }
            set { hasSidecar = value; }
        }

        public Motorcycle(string make, string model, int year, double dailyRate, bool hasSidecar)
            : base(make, model, year, dailyRate)
        {
            this.hasSidecar = hasSidecar;
        }

        public override double CalculateRentalCost(int days)
        {
            double baseCost = DailyRate * days;
            if (!hasSidecar)
                return baseCost * 0.9;
            return baseCost;
        }

        public override string GetDescription()
        {
            string sidecarInfo = hasSidecar ? "with sidecar" : "no sidecar";
            return ($"{Year} {Make} {Model} ({sidecarInfo}) - RM {DailyRate:F2}/day");
        }
    }

    public class Truck : Vehicle
    {
        private double payloadTons;

        public double PayloadTons
        {
            get { return payloadTons; }
            set { payloadTons = value; }
        }

        public Truck(string make, string model, int year, double dailyRate, double payloadTons)
            : base(make, model, year, dailyRate)
        {
            this.payloadTons = payloadTons;
        }

        public override double CalculateRentalCost(int days)
        {
            return (DailyRate + (30 * payloadTons)) * days;
        }

        public override string GetDescription()
        {
            return $"{Year} {Make} {Model} ({payloadTons:F1} tons) - RM {DailyRate:F2}/day";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Vehicle> vehicles = new List<Vehicle>
            {
                new Car("Toyota", "Camry", 2022, 150.00, 5),
                new Motorcycle("Honda", "CBR", 2021, 80.00, false),
                new Truck("Volvo", "FH", 2020, 200.00, 8.5)
            };

            int days = 5;
            double maxCost = 0;
            Vehicle mostExpensive = null;

            foreach (Vehicle v in vehicles)
            {
                double cost = v.CalculateRentalCost(days);
                Console.WriteLine(v.GetDescription());
                Console.WriteLine($"  5-day rental cost: RM {cost:F2}\n");

                if (cost > maxCost)
                {
                    maxCost = cost;
                    mostExpensive = v;
                }
            }

            if (mostExpensive != null)
            {
                string desc = mostExpensive.GetDescription();
                int dashIndex = desc.IndexOf(" - RM");
                string vehicleInfo = dashIndex > 0 ? desc.Substring(0, dashIndex) : desc;
                Console.WriteLine($"Most expensive: {vehicleInfo} - RM {maxCost:F2}");
            }
        }
    }
}