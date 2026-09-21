using _01_GestionFlota_UrbanMove.Common;
using _01_GestionFlota_UrbanMove.Interfaces;

namespace _01_GestionFlota_UrbanMove.Models;
/// <summary>
/// Clase abstracta que heredaran todos los medios de transportes de la flota.
/// </summary>
public abstract class Transporte : IDisponible, IComparable<Transporte>
{
    private string _id = string.Empty;
    public string Id
    {
        get => _id;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("El ID no puede ser nulo o estar vacio.");
            }
            _id = value;
        }
    }
    private string _marca = string.Empty;
    public string Marca
    {
        get => _marca;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("La marca no puede ser nulo o estar vacio.");
            }
            _marca = value;
        }
    }
    private string _modelo = string.Empty;
    public string Modelo
    {
        get => _modelo;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("El modelo no puede ser nulo o estar vacio.");
            }
            _modelo = value;
        }
    }
    public EstadoVehiculo Estado { get; set; }
protected Transporte(string id,string marca,string modelo,EstadoVehiculo estado)
    {
        this.Id = id;
        this.Marca = marca;
        this.Modelo = modelo;
        this.Estado = estado;
    }
    public abstract bool EsAptoParaUso();
    /// <summary>
    /// Compara este vehículo con otro usando el ID para poder ordenarlos
    /// </summary>
    /// <param name="otro"></param>
    /// <returns></returns>
    public int CompareTo(Transporte? otro)
    {
        if (otro == null) return 1;

        return this.Id.CompareTo(otro.Id);
    }
    /// <summary>
    /// Devuelve los datos principales del vehículo formateados como texto
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"[{Id}] {Marca} {Modelo} | Estado: {Estado}";
    }
}