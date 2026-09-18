# 01 - Sistema de Gestión de Flota (UrbanMove)

## 🎯 Objetivo
Desarrollar un sistema de consola para la gestión de vehículos de movilidad urbana de corta distancia (Monopatines y Bicis Eléctricas), aplicando principios de Diseño Orientado a Objetos y una arquitectura limpia en C#.

## 🚀 Conceptos Clave Aplicados
* **Herencia y Polimorfismo:** Clase base abstracta `Vehiculo` de la que heredan `MonopatinElectrico` y `Bicicleta`, permitiendo el cálculo dinámico de tarifas según el tipo de transporte.
* **Interfaces:** `IRecargable` para la gestión de carga de batería e `IDisponible` para el control de estados.
* **Excepciones Personalizadas:** Control de reglas de negocio con `BateriaInsuficienteException` y `VehiculoNoDisponibleException`.
* **Interfaz de Consola:** Menú interactivo estructurado con `do-while`, `switch` y refresco visual usando `Console.Clear()`.

## 📚 Objetivo de Aprendizaje
Este mini-proyecto me sirvió para poner en práctica conceptos fundamentales de POO y C#, buscando aplicar buenas prácticas de organización de código mediante carpetas (`Common`, `Exceptions`, `Interfaces`, `Models`, `Services`).

---

## 📐 Diagrama de Clases (ASCII)

```text
                  +-----------------------+       +-----------------------+
                  |     <<Interface>>     |       |     <<Interface>>     |
                  |      IDisponible      |       |      IRecargable      |
                  +-----------------------+       +-----------------------+
                  | + Estado              |       | + NivelBateria        |
                  | + EstaDisponible()    |       | + CargarBateria()     |
                  +-----------------------+       +-----------------------+
                              ^                               ^
                              | (Implementa)                  | (Implementa)
                              |                               |
        +-------------------------------------------+         |
        |               <<Abstract>>                |         |
        |                 Vehiculo                  |         |
        +-------------------------------------------+         |
        | + Id: string                              |         |
        | + Marca: string                           |         |
        | + Modelo: string                          |         |
        | + Estado: EstadoVehiculo                  |         |
        +-------------------------------------------+         |
        | + EstaDisponible(): bool                  |         |
        | + CalcularCostoViaje(min): double [Abstr] |         |
        | + CambiarEstado(nuevoEstado): void        |         |
        +-------------------------------------------+         |
                              ^                               |
                              | (Hereda)                      |
            +-----------------+-----------------+             |
            |                                   |             |
+-----------------------+           +-----------------------+-+
|       Bicicleta       |           |   MonopatinElectrico  |
+-----------------------+           +-----------------------+
| + EnTaller: bool      |           | + NivelBateria: int   |
| + CostoBase: double   |           | + CostoPorMinuto: dbl |
| + CostoMinuto: double |           +-----------------------+
+-----------------------+           | + CargarBateria()     |
| + CalcularCostoViaje()|           | + CalcularCostoViaje()|
+-----------------------+           +-----------------------+

                     --- EXCEPCIONES Y SERVICIOS ---

    +--------------------------------+     +--------------------------------+      
    |  BateriaInsuficienteException  |     |  VehiculoNoDisponibleException |
    +--------------------------------+     +--------------------------------+
    | + NivelActual: int             |     | (Hereda de Exception)          |
    | + NivelMinimoRequerido: int    |     | Lanza error si Estado != Dispo |
    +--------------------------------+     +--------------------------------+
    +-------------------------------------------------+
    |                ProcesarAlquiler                 |
    +-------------------------------------------------+
    | - _disponible: IDisponible                      |
    +-------------------------------------------------+
    | + ProcesarAlquiler(disponible: IDisponible)     |
    | + IniciarViaje(minutos: int): double            |
    +-------------------------------------------------+
