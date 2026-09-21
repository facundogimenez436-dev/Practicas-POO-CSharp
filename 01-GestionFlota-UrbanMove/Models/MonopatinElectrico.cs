using _01_GestionFlota_UrbanMove.Common;
using _01_GestionFlota_UrbanMove.Interfaces;

namespace _01_GestionFlota_UrbanMove.Models;
/// <summary>
/// Transporte de tipo electrico que necesita bateria.
/// </summary>
public class MonopatinElectrico : Transporte,IAlquilable,IRecargable
{
    private int _nivelBateria;
    public int NivelBateria
    {
        get => _nivelBateria;
        set
        {
            if (value < 0 || value > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "El nivel de bateria debe estar entre 0 y 100.");
            }
            _nivelBateria = value;
        }
    }
    public decimal CostoBase { get; } = 50.0m;
    public decimal CostoPorMinuto { get; } = 15.0m;

    public MonopatinElectrico(string id, string marca, string modelo) : base(id,marca,modelo)
    {
        this.NivelBateria = 100;
    }
    public override bool EsAptoParaUso()=> Estado == EstadoVehiculo.Disponible && NivelBateria > 15;
    public decimal CalcularCosto(int minutos)
    {
        return CostoBase + (CostoPorMinuto * minutos);
    }

    public void CargarBateria()
    {
        NivelBateria = 100;
        if (Estado == EstadoVehiculo.EnMantenimiento)
        {
            Estado = EstadoVehiculo.Disponible;
        }
    }
    public override string ToString()
    {
        return $"{base.ToString()}\nBateria:{NivelBateria}%\nCosto Base:${CostoBase}\nCosto/minutos:${CostoPorMinuto}";
    }
}  