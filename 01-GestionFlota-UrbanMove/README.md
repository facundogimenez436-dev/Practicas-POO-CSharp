# UrbanMove - Sistema de Gestión de Flota y Logística

Este proyecto es una aplicación de consola desarrollada en **C# (.NET 10)** creada con el objetivo principal de consolidar y poner en práctica los fundamentos de la **Programación Orientada a Objetos (POO)**. 

El foco del desarrollo estuvo puesto en el diseño de un modelo de dominio sólido, con responsabilidades bien delimitadas, manejo de reglas de negocio y excepciones personalizadas. Como complemento visual, se integró la librería **Spectre.Console** para ofrecer una interfaz interactiva de usuario en la terminal.

---

## 💡 Conceptos de POO Aplicados

En la arquitectura del sistema se implementaron los pilares y buenas prácticas de POO:

- **Abstracción y Herencia:** Clase base abstracta `Transporte` que define la estructura común de los vehículos de la flota, heredada por tipos específicos (`BicicletaNormal`, `MonopatinElectrico`, `CamionLogistica`).
- **Polimorfismo:** Implementación de comportamientos particulares en cada clase derivada para evaluar la aptitud de uso (`EsAptoParaUso()`) y el formateo de datos (`ToString()`).
- **Interfaces y Segregación:** Uso de interfaces específicas (`IAlquilable`, `IRecargable`, `IDisponible`) para definir contratos claros y evitar acoplamiento innecesario.
- **Encapsulamiento:** Validación estricta del estado interno de los objetos mediante propiedades (por ejemplo, rangos de nivel de batería o normalización de identificadores).
- **Manejo de Excepciones de Negocio:** Creación de excepciones personalizadas (`BateriaInsuficienteException`, `VehiculoNoAptoException`) para gestionar flujos alternativos y reglas de negocio.
- **Servicios de Aplicación:** Separación de la lógica operacional (alquileres y costos) en clases de servicio dedicadas (`ProcesarAlquiler`).

---

## 🛠️ Tecnologías y Herramientas

- **Lenguaje:** C# / .NET 10 SDK
- **Interfaz:** CLI con [Spectre.Console](https://spectreconsole.net/)
- **Entorno:** Visual Studio Code en Linux Mint

---

## 🚀 Funcionalidades Principales

1. **Gestión de la Flota:** Registro, edición, listado filtrado y eliminación de vehículos.
2. **Operaciones de Alquiler:** Inicio y finalización de viajes calculando costos según el tipo de transporte.
3. **Logística:** Carga y descarga de vehículos dentro de camiones de transporte respetando límites de capacidad.

---

## 📌 Contexto del Proyecto

Este miniproyecto forma parte de mi proceso de aprendizaje en desarrollo de software, sirviendo como cierre de la etapa de POO previa a continuar con el estudio de bases de datos relacionales (SQL), Entity Framework Core y ASP.NET Core.