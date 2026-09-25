using Spectre.Console;
using _01_GestionFlota_UrbanMove.Common;
using _01_GestionFlota_UrbanMove.Exceptions;
using _01_GestionFlota_UrbanMove.Interfaces;
using _01_GestionFlota_UrbanMove.Models;
using _01_GestionFlota_UrbanMove.Services;

namespace _01_GestionFlota_UrbanMove.UI;

/// <summary>
/// Contiene los métodos que implementan la interfaz gráfica de usuario en consola con Spectre.Console.
/// </summary>
public static class FlotaConsole
{
    public static void RegistrarTransporte(List<Transporte> flota)
    {
        DibujarTitulo();

        var opcion = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("== Seleccione el [bold blue]tipo de vehículo[/] a registrar ==")
                .PageSize(10)
                .AddChoices(new[] {
                    "1. Bicicleta Normal",
                    "2. Monopatín Eléctrico",
                    "3. Camión Logística",
                    "0. Volver"
                }));

        if (opcion == "0. Volver") return;

        AnsiConsole.MarkupLine("\n[bold blue]Datos Generales:[/]");
        AnsiConsole.WriteLine();

        try
        {
            string id = AnsiConsole.Prompt(
                new TextPrompt<string>("Ingrese el [bold yellow]ID/Patente[/]")
                    .Validate(input =>
                    {
                        if (flota.Any(v => v.Id.Equals(input.Trim().Replace(" ", ""), StringComparison.OrdinalIgnoreCase)))
                        {
                            return ValidationResult.Error($"[bold red]El ID '{input}' ya existe. Ingrese uno diferente.[/]");
                        }
                        return ValidationResult.Success();
                    })
            );

            string marca = AnsiConsole.Ask<string>("Ingrese la [bold yellow]Marca[/]");
            string modelo = AnsiConsole.Ask<string>("Ingrese el [bold yellow]Modelo[/]");

            Transporte? nuevoTransporte = opcion switch
            {
                "1. Bicicleta Normal" => new BicicletaNormal(id, marca, modelo),
                "2. Monopatín Eléctrico" => new MonopatinElectrico(id, marca, modelo),
                "3. Camión Logística" => new CamionLogistica(id, marca, modelo),
                _ => null
            };

            if (nuevoTransporte != null)
            {
                flota.Add(nuevoTransporte);
                AnsiConsole.MarkupLine("\n[bold yellow]¡Vehículo Registrado con éxito![/]");
                Esperar();
                DibujarTitulo();

                var panel = new Panel(nuevoTransporte.ToString().EscapeMarkup())
                {
                    Header = new PanelHeader("[bold green] Resumen del vehículo registrado [/]"),
                    Border = BoxBorder.Rounded,
                    Padding = new Padding(1, 0, 1, 0),
                    Expand = true
                };
                AnsiConsole.Write(panel);
            }
        }
        catch (ArgumentOutOfRangeException ex)
        {
            AnsiConsole.MarkupLine($"[bold red]Error de rango en los datos:[/] {ex.Message.EscapeMarkup()}");
        }
        catch (VehiculoNoAptoException ex)
        {
            AnsiConsole.MarkupLine($"[bold red]Error vehículo no apto:[/] {ex.Message.EscapeMarkup()}");
        }
        catch (BateriaInsuficienteException ex)
        {
            AnsiConsole.MarkupLine($"[bold red]Error batería insuficiente:[/] {ex.Message.EscapeMarkup()}");
        }
        catch (ArgumentException ex)
        {
            AnsiConsole.MarkupLine($"[bold red]Error de validación:[/] {ex.Message.EscapeMarkup()}");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[bold red]Error inesperado:[/] {ex.Message.EscapeMarkup()}");
        }

        Pausar();
    }

    public static void ListarFlota(List<Transporte> flota)
    {
        if (ValidarFlotaVacia(flota)) return;

        var opcion = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Seleccione el [bold blue]Filtro de vehículo[/]")
                .PageSize(10)
                .AddChoices(new[] {
                    "1. Todos los Vehículos",
                    "2. Solo Alquilables",
                    "3. Solo Disponibles",
                    "4. Solo En Uso",
                    "5. Solo En Mantenimiento",
                    "0. Volver"
                }));

        if (opcion == "0. Volver") return;

        IEnumerable<Transporte> filtrados = opcion switch
        {
            "1. Todos los Vehículos" => flota,
            "2. Solo Alquilables" => flota.Where(v => v is IAlquilable),
            "3. Solo Disponibles" => flota.Where(v => v.Estado == EstadoVehiculo.Disponible),
            "4. Solo En Uso" => flota.Where(v => v.Estado == EstadoVehiculo.EnUso),
            "5. Solo En Mantenimiento" => flota.Where(v => v.Estado == EstadoVehiculo.EnMantenimiento),
            _ => Enumerable.Empty<Transporte>()
        };

        var listaFinal = filtrados.ToList();

        if (!listaFinal.Any())
        {
            AnsiConsole.MarkupLine("\n[bold yellow]No hay vehículos que coincidan con este filtro.[/]");
            Pausar();
            return;
        }

        listaFinal.Sort();
        MostrarTablaVehiculos("Flota de Vehículos Registrados", listaFinal);
        Pausar();
    }

    public static void IniciarViajeUI(List<Transporte> flota, ProcesarAlquiler alquilerService)
    {
        if (ValidarFlotaVacia(flota)) return;

        var alquilablesDisponibles = flota
            .Where(v => v.Estado == EstadoVehiculo.Disponible && v is IAlquilable)
            .ToList();

        if (!alquilablesDisponibles.Any())
        {
            AnsiConsole.MarkupLine("[bold red]No hay vehículos disponibles para alquilar en este momento.[/]");
            Pausar();
            return;
        }

        MostrarTablaVehiculos("Vehículos Disponibles para Alquilar", alquilablesDisponibles);

        string idIngresado = AnsiConsole.Ask<string>("Ingrese el [bold yellow]ID/Patente[/] del vehículo a utilizar:")
            .Trim()
            .Replace(" ", "")
            .ToUpper();

        var vehiculoSeleccionado = alquilablesDisponibles.FirstOrDefault(v => v.Id == idIngresado);

        if (vehiculoSeleccionado == null)
        {
            AnsiConsole.MarkupLine("\n[bold red]Error: No se encontró el ID seleccionado entre los disponibles.[/]");
            Pausar();
            return;
        }

        try
        {
            alquilerService.IniciarViaje(vehiculoSeleccionado);
            AnsiConsole.MarkupLine($"\n[bold green]¡Viaje iniciado con éxito en el vehículo {vehiculoSeleccionado.Id}![/]");
        }
        catch (BateriaInsuficienteException ex)
        {
            AnsiConsole.MarkupLine($"\n[bold red]Error de Batería:[/] {ex.Message.EscapeMarkup()}");
        }
        catch (VehiculoNoAptoException ex)
        {
            AnsiConsole.MarkupLine($"\n[bold red]Error de Aptitud:[/] {ex.Message.EscapeMarkup()}");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[bold red]Error al iniciar el viaje:[/] {ex.Message.EscapeMarkup()}");
        }

        Pausar();
    }

    public static void FinalizarViajeUI(List<Transporte> flota, ProcesarAlquiler alquilerService)
    {
        if (ValidarFlotaVacia(flota)) return;

        var enUso = flota.Where(v => v.Estado == EstadoVehiculo.EnUso).ToList();

        if (!enUso.Any())
        {
            AnsiConsole.MarkupLine("[bold yellow]No hay ningún vehículo en uso actualmente.[/]");
            Pausar();
            return;
        }

        MostrarTablaVehiculos("Vehículos Actualmente en Uso", enUso);

        string idIngresado = AnsiConsole.Ask<string>("Ingrese el [bold yellow]ID/Patente[/] a finalizar:")
            .Trim()
            .Replace(" ", "")
            .ToUpper();

        var vehiculoSeleccionado = enUso.FirstOrDefault(v => v.Id == idIngresado);

        if (vehiculoSeleccionado == null)
        {
            AnsiConsole.MarkupLine("\n[bold red]Error: El ID ingresado no corresponde a un vehículo actualmente en uso.[/]");
            Pausar();
            return;
        }

        int minutos = AnsiConsole.Ask<int>("Ingrese la [bold yellow]duración del viaje en minutos[/]:");

        try
        {
            decimal costoTotal = alquilerService.FinalizarViaje(vehiculoSeleccionado, minutos);
            AnsiConsole.MarkupLine("\n[bold green]Viaje finalizado con éxito.[/]");
            AnsiConsole.MarkupLine($"[bold yellow]Costo Total del Alquiler:[/] [bold white]${costoTotal:N2}[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[bold red]Error al finalizar el viaje:[/] {ex.Message.EscapeMarkup()}");
        }

        Pausar();
    }

    public static void CargarCamionUI(List<Transporte> flota)
    {
        if (ValidarFlotaVacia(flota)) return;

        var camiones = flota.OfType<CamionLogistica>().ToList();
        if (!camiones.Any())
        {
            AnsiConsole.MarkupLine("[bold red]No hay camiones de logística registrados en la flota.[/]");
            Pausar();
            return;
        }

        var opcionesCamion = camiones.Select(c => $"{c.Id} - {c.Marca} {c.Modelo} (Carga actual: {c.CargaCamion.Count}/{c.CapacidadMaxima})").ToList();
        opcionesCamion.Add("0. Volver");

        var seleccionCamion = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Seleccione el [bold blue]Camión de Logística[/] de destino:")
                .AddChoices(opcionesCamion));

        if (seleccionCamion == "0. Volver") return;

        string idCamion = seleccionCamion.Split(" - ")[0];
        var camion = camiones.First(c => c.Id == idCamion);

        var vehiculosDisponibles = flota
            .Where(v => v is not CamionLogistica && v.Estado == EstadoVehiculo.Disponible)
            .ToList();

        if (!vehiculosDisponibles.Any())
        {
            AnsiConsole.MarkupLine("[bold yellow]No hay vehículos disponibles para cargar en el camión.[/]");
            Pausar();
            return;
        }

        var opcionesVehiculos = vehiculosDisponibles.Select(v => $"{v.Id} - {v.GetType().Name} ({v.Marca} {v.Modelo})").ToList();
        opcionesVehiculos.Add("0. Volver");

        var seleccionVehiculo = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Seleccione el [bold blue]Vehículo[/] a cargar:")
                .AddChoices(opcionesVehiculos));

        if (seleccionVehiculo == "0. Volver") return;

        string idVehiculo = seleccionVehiculo.Split(" - ")[0];
        var vehiculoACargar = vehiculosDisponibles.First(v => v.Id == idVehiculo);

        try
        {
            camion.CargarCamion(vehiculoACargar);
            AnsiConsole.MarkupLine($"\n[bold green]Vehículo {vehiculoACargar.Id} cargado con éxito en el camión {camion.Id}.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[bold red]Error al cargar camión:[/] {ex.Message.EscapeMarkup()}");
        }

        Pausar();
    }

    public static void DescargarCamionUI(List<Transporte> flota)
    {
        if (ValidarFlotaVacia(flota)) return;

        var camionesConCarga = flota.OfType<CamionLogistica>()
            .Where(c => c.CargaCamion.Any())
            .ToList();

        if (!camionesConCarga.Any())
        {
            AnsiConsole.MarkupLine("[bold yellow]No hay camiones con carga para descargar actualmente.[/]");
            Pausar();
            return;
        }

        var opcionesCamion = camionesConCarga.Select(c => $"{c.Id} - {c.Marca} {c.Modelo} (Carga: {c.CargaCamion.Count})").ToList();
        opcionesCamion.Add("0. Volver");

        var seleccionCamion = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Seleccione el [bold blue]Camión[/] a descargar:")
                .AddChoices(opcionesCamion));

        if (seleccionCamion == "0. Volver") return;

        string idCamion = seleccionCamion.Split(" - ")[0];
        var camion = camionesConCarga.First(c => c.Id == idCamion);

        var opcionesCarga = camion.CargaCamion.Select(v => $"{v.Id} - {v.GetType().Name} ({v.Marca} {v.Modelo})").ToList();
        opcionesCarga.Add("0. Volver");

        var seleccionVehiculo = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($"Seleccione el [bold blue]Vehículo[/] a descargar del camión {camion.Id}:")
                .AddChoices(opcionesCarga));

        if (seleccionVehiculo == "0. Volver") return;

        string idVehiculo = seleccionVehiculo.Split(" - ")[0];
        var vehiculoADescargar = camion.CargaCamion.First(v => v.Id == idVehiculo);

        try
        {
            if (camion.DescargarCamion(vehiculoADescargar))
            {
                AnsiConsole.MarkupLine($"\n[bold green]Vehículo {vehiculoADescargar.Id} descargado con éxito del camión {camion.Id}.[/]");
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[bold red]Error al descargar vehículo:[/] {ex.Message.EscapeMarkup()}");
        }

        Pausar();
    }

    public static void EditarTransporte(List<Transporte> flota)
    {
        if (ValidarFlotaVacia(flota)) return;

        var opciones = flota.Select(v => $"{v.Id.EscapeMarkup()} - {v.GetType().Name.EscapeMarkup()} ({v.Marca.EscapeMarkup()} {v.Modelo.EscapeMarkup()})").ToList();
        opciones.Add("0. Volver");

        var seleccion = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Seleccione el vehículo a [bold blue]editar[/]")
                .AddChoices(opciones));

        if (seleccion == "0. Volver") return;

        string idElegido = seleccion.Split(" - ")[0];
        var vehiculo = flota.First(v => v.Id == idElegido);

        var campo = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($"Modificando: [bold blue]{vehiculo.Id.EscapeMarkup()}[/]")
                .AddChoices(new[] {
                    "1. ID / Patente",
                    "2. Marca",
                    "3. Modelo",
                    "0. Volver"
                }));

        if (campo == "0. Volver") return;

        try
        {
            switch (campo)
            {
                case "1. ID / Patente":
                    string nuevoId = AnsiConsole.Ask<string>("Nuevo ID/Patente:");
                    string idFormateado = nuevoId.Trim().Replace(" ", "").ToUpper();

                    if (flota.Any(v => v != vehiculo && v.Id == idFormateado))
                    {
                        AnsiConsole.MarkupLine("[bold red]Error:[/] Ese ID ya pertenece a otro vehículo.");
                    }
                    else
                    {
                        vehiculo.Id = nuevoId;
                        AnsiConsole.MarkupLine("[bold green]¡ID actualizado con éxito![/]");
                    }
                    break;

                case "2. Marca":
                    vehiculo.Marca = AnsiConsole.Ask<string>("Nueva Marca:");
                    AnsiConsole.MarkupLine("[bold green]¡Marca actualizada con éxito![/]");
                    break;

                case "3. Modelo":
                    vehiculo.Modelo = AnsiConsole.Ask<string>("Nuevo Modelo:");
                    AnsiConsole.MarkupLine("[bold green]¡Modelo actualizado con éxito![/]");
                    break;
            }
        }
        catch (ArgumentException ex)
        {
            AnsiConsole.MarkupLine($"[bold red]Error:[/] {ex.Message.EscapeMarkup()}");
        }

        Pausar();
    }

    public static void EliminarTransporte(List<Transporte> flota)
    {
        if (ValidarFlotaVacia(flota)) return;

        var opciones = flota.Select(v => $"{v.Id.EscapeMarkup()} - {v.GetType().Name.EscapeMarkup()} ({v.Marca.EscapeMarkup()} {v.Modelo.EscapeMarkup()})").ToList();
        opciones.Add("0. Volver");

        var seleccion = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Seleccione el vehículo a [bold red]eliminar[/]")
                .AddChoices(opciones));

        if (seleccion == "0. Volver") return;

        string idElegido = seleccion.Split(" - ")[0];
        var vehiculoEliminar = flota.FirstOrDefault(v => v.Id == idElegido);

        if (vehiculoEliminar == null) return;

        bool confirmar = AnsiConsole.Prompt(
            new ConfirmationPrompt($"¿Está seguro que desea eliminar el vehículo ID: [bold red]{vehiculoEliminar.Id.EscapeMarkup()}[/]?")
            {
                Yes = 'S',
                No = 'N',
                DefaultValue = false,
                ShowDefaultValue = false
            });

        if (confirmar)
        {
            flota.Remove(vehiculoEliminar);
            AnsiConsole.MarkupLine("\n[bold green]¡El vehículo fue eliminado con éxito de la flota![/]");
        }
        else
        {
            AnsiConsole.MarkupLine("\n[bold yellow]Operación cancelada. El vehículo no fue eliminado.[/]");
        }

        Pausar();
    }
    public static void RecargarBateriaUI(List<Transporte> flota)
{
    if (ValidarFlotaVacia(flota)) return;

    
    var recargables = flota.OfType<IRecargable>()
        .Cast<Transporte>()
        .ToList();

    if (!recargables.Any())
    {
        AnsiConsole.MarkupLine("[bold yellow]No hay vehículos eléctricos registrados en la flota.[/]");
        Pausar();
        return;
    }

    var opciones = recargables.Select(v => 
        $"{v.Id.EscapeMarkup()} - {v.GetType().Name.EscapeMarkup()} (Batería actual: {((IRecargable)v).NivelBateria}%)"
    ).ToList();
    opciones.Add("0. Volver");

    var seleccion = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Seleccione el vehículo eléctrico a [bold blue]recargar[/]:")
            .AddChoices(opciones));

    if (seleccion == "0. Volver") return;

    string idSeleccionado = seleccion.Split(" - ")[0];
    var vehiculo = recargables.First(v => v.Id == idSeleccionado);

    if (vehiculo is IRecargable recargable)
    {
        recargable.CargarBateria();
        AnsiConsole.MarkupLine($"\n[bold green]¡Batería del vehículo {vehiculo.Id.EscapeMarkup()} cargada al 100%! Estado actualizado a Disponible.[/]");
    }

    Pausar();
}

    // --- MÉTODOS REUTILIZABLES ---

    private static bool ValidarFlotaVacia(List<Transporte> flota)
    {
        if (flota == null || flota.Count == 0)
        {
            AnsiConsole.MarkupLine("[bold yellow]No hay vehículos registrados en la flota todavía.[/]\n");
            Pausar();
            return true;
        }
        return false;
    }

    private static void MostrarTablaVehiculos(string titulo, IEnumerable<Transporte> lista)
    {
        var table = new Table().Border(TableBorder.Rounded);
        table.Title($"[bold blue]{titulo}[/]");
        table.AddColumn(new TableColumn("[bold]Tipo[/]").Centered());
        table.AddColumn(new TableColumn("[bold]ID/Patente[/]").Centered());
        table.AddColumn(new TableColumn("[bold]Marca[/]").Centered());
        table.AddColumn(new TableColumn("[bold]Modelo[/]").Centered());
        table.AddColumn(new TableColumn("[bold]Estado[/]").Centered());
        table.AddColumn(new TableColumn("[bold]Batería[/]").Centered());

        foreach (var v in lista)
        {
            string estadoFormateado = v.Estado switch
            {
                EstadoVehiculo.Disponible => $"[green]{v.Estado}[/]",
                EstadoVehiculo.EnUso => $"[yellow]{v.Estado}[/]",
                EstadoVehiculo.EnMantenimiento => $"[red]{v.Estado}[/]",
                _ => v.Estado.ToString()
            };

            string bateria = v is IRecargable recargable ? $"{recargable.NivelBateria}%" : "-";

            table.AddRow(
                v.GetType().Name.EscapeMarkup(),
                v.Id.EscapeMarkup(),
                v.Marca.EscapeMarkup(),
                v.Modelo.EscapeMarkup(),
                estadoFormateado,
                bateria
            );
        }

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
    }

    public static void Pausar()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[dim]Presione cualquier tecla para continuar...[/]");
        Console.ReadKey(true);
    }

    public static void Esperar()
    {
        Thread.Sleep(1500);
        AnsiConsole.Clear();
    }

    public static void DibujarTitulo()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(
            new FigletText("UrbanMove")
                .Color(Color.Blue));
        AnsiConsole.Write(new Rule("[yellow]Sistema de Gestión de Flota & Logística[/]"));
        AnsiConsole.WriteLine();
    }
}
