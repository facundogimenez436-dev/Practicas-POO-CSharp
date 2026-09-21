using _01_GestionFlota_UrbanMove.Common;
using _01_GestionFlota_UrbanMove.Interfaces;

namespace _01_GestionFlota_UrbanMove.Models;
/// <summary>
/// Transporte tipo bicicleta sin bateria.
/// </summary>
public class BicicletaNormal : Transporte, IAlquilable
{
    public decimal CostoPorMinuto { get; } = 12.0m;

    public BicicletaNormal(string id, string marca, string modelo, EstadoVehiculo estado) : base(id, marca, modelo, estado){}
    public override bool EsAptoParaUso() => Estado == EstadoVehiculo.Disponible;
    
    public decimal CalcularCosto(int minutos)
    {
        return CostoPorMinuto * minutos;
    }
    public override string ToString()
    {
        return $"[{base.ToString}]| Costo/minutos:${CostoPorMinuto}";
    }
}