using _01_GestionFlota_UrbanMove.Common;

namespace _01_GestionFlota_UrbanMove.Interfaces;
/// <summary>
/// Interface para controlar si un transporte se puede usar o no.
/// </summary>
public interface IDisponible
{
    /// <summary>Guarda el estado actual del transporte.</summary>
    public EstadoVehiculo Estado { get; set; }
    /// <summary>Verifica si esta en condiciones para usarse.</summary>
    
    bool EsAptoParaUso();
    
}