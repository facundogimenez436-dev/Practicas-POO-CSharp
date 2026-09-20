
namespace _01_GestionFlota_UrbanMove.Common;
/// <summary>
/// Representa los estados posible que puede tener un transporte.
/// </summary>
public enum EstadoVehiculo
{
    /// <summary> El transporte esta libre y listo para usarse</summary>
    Disponible,
    /// <summary>El transporte esta realizando un viaje. </summary>
    EnUso,
/// <summary>El transporte esta en el taller por reparacion o fuera de servicio por falta de bateria.</summary>
    EnMantenimiento
}