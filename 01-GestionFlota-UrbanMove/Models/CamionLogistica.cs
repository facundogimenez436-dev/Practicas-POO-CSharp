using _01_GestionFlota_UrbanMove.Common;

namespace _01_GestionFlota_UrbanMove.Models;
/// <summary>
/// Transporte de la empresa que no puede ser alquilado.
/// </summary>
public class CamionLogistica : Transporte
{
    public int CapacidadMaxima { get; } = 5;
    /// <summary>
    /// Lista para almacenar los vehiculos que se cargan dentro del camion.
    /// </summary>
    public List<Transporte> CargaCamion { get; private set; } = new();
    public CamionLogistica(string id, string marca, string modelo) : base(id, marca, modelo){}

    public override bool EsAptoParaUso() => Estado == EstadoVehiculo.Disponible;

    public void CargarCamion(Transporte carga)
    {
        if (carga == null)
        {
            throw new ArgumentNullException(nameof(carga), "El vehiculo a cargar no puede ser nulo.");
        }
        if (CargaCamion.Count >= CapacidadMaxima)
        {
            throw new InvalidOperationException($"El Camion{Id} alcanzo su capacidad maxima {CapacidadMaxima}");
        }
        if (carga is CamionLogistica)
        {
            throw new InvalidOperationException("No se pude cargar un camion dentro de otro camion.");
        }
        CargaCamion.Add(carga);
        carga.Estado = EstadoVehiculo.EnMantenimiento;
    }
    public bool DescargarCamion(Transporte carga)
    {
        if (carga == null)
        {
            throw new ArgumentNullException(nameof(carga), "El vehiculo a descargar no puede ser nulo.");
        }
        carga.Estado = EstadoVehiculo.Disponible;
        return CargaCamion.Remove(carga);

    }
    public override string ToString()
    {
        return $"{base.ToString()}\nCapacidad Maxima: {CapacidadMaxima} Vehiculos";
    }
}