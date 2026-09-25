using _01_GestionFlota_UrbanMove.Common;
using _01_GestionFlota_UrbanMove.Interfaces;
using _01_GestionFlota_UrbanMove.Models;
using _01_GestionFlota_UrbanMove.Exceptions;

namespace _01_GestionFlota_UrbanMove.Services;

public class ProcesarAlquiler
{
    public void IniciarViaje(Transporte vehiculo)
    {
        if (vehiculo == null)
        {
            throw new ArgumentNullException(nameof(vehiculo));
        }
        if (vehiculo is not IAlquilable)
        {
            throw new InvalidOperationException($"El Transporte: {vehiculo.Id} no es alquilable.");
        }
        if (vehiculo is IRecargable recargable && recargable.NivelBateria <= 15)
        {
            throw new BateriaInsuficienteException($"El vehículo {vehiculo.Id} no tiene suficiente batería ({recargable.NivelBateria}%)."
            );
        }
        if (!vehiculo.EsAptoParaUso())
        {
            throw new VehiculoNoAptoException($"El vehiculo: {vehiculo.Id} no esta apto para usar");
        }
        vehiculo.Estado = EstadoVehiculo.EnUso;
    }
    public decimal FinalizarViaje(Transporte vehiculo,int minutos)
    {
        if (vehiculo == null)
        {
            throw new ArgumentNullException(nameof(vehiculo));
        }
        if (minutos <= 0)
        {
            throw new ArgumentException("El tiempo de viaje debe ser mayor a 0 minutos.");
        }
        if (vehiculo is not IAlquilable alquilable)
        {
            throw new InvalidOperationException($"El Transporte: {vehiculo.Id} no es alquilable.");
        }

        decimal costoTotal = alquilable.CalcularCosto(minutos);

        if (vehiculo is IRecargable recargable)
    {
        // Restamos 1% por cada minuto (ajustando para que no baje de 0)
        int nuevoNivel = Math.Max(0, recargable.NivelBateria - minutos);
        recargable.NivelBateria = nuevoNivel;

        // Si la batería bajó del 15%, pasa a mantenimiento
        if (nuevoNivel <= 15)
        {
            vehiculo.Estado = EstadoVehiculo.EnMantenimiento;
        }
        else
        {
            vehiculo.Estado = EstadoVehiculo.Disponible;
        }
    }
    else
    {
        vehiculo.Estado = EstadoVehiculo.Disponible;
    }

    return costoTotal;
}
}






