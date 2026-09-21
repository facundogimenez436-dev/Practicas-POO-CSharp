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

        vehiculo.Estado = EstadoVehiculo.Disponible;

        return costoTotal;
    }
}






