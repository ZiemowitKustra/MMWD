using System;

/// <summary>
/// Niby interfejs to mial być ale nie jest
/// nie ma zadnej funkcji wiec nie moglem z tego zrobic interfejsu
/// ale potrzebowalem zablokowac zmienne zeby nie mozna bylo sie do nich bezposrednio odwolac
/// tylko poprzez funkcje
/// </summary>
public partial class Food
{
    public double ID { get; set; }
    public string Name { get; set; }
    public double Carbonhydrates { get; set; }
    public double K_Calories { get; set; }
    public double Proteins { get; set; }
    public double Fats { get; set; }
    public double Cost { get; set; }
    ///Constructor of this class
    public Food(double ID, string Name, double K_Calories, double Carbonhydrates, double Proteins,
        double Fats, double Cost)
    {
        this.Name = Name;
        this.ID = ID;
        this.Carbonhydrates = Carbonhydrates;
        this.K_Calories = K_Calories;
        this.Proteins = Proteins;
        this.Fats = Fats;
        this.Cost = Cost;
    }
    public override string ToString()
    {
        return Name;
    }
}