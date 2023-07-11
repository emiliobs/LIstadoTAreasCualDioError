// See https://aka.ms/new-console-template for more information
Console.WriteLine("Inicio");

var tareas = new List<Task>() { Tarea1(), Tarea2(), Tarea3() };

try
{
    await Task.WhenAll(tareas);
}
catch (Exception)
{

    var tareasQueFallaron = tareas.Where(t => t.IsFaulted).ToList();

    foreach (var tarea in tareasQueFallaron)
    {
        var excepcion = tarea.Exception?.InnerException!;
        var nombreMetodo = excepcion?.Data["nombre_metodo"];
        Console.WriteLine($"Ha fallado el método {nombreMetodo}");
    }
}

Console.WriteLine("Fin");


async Task Tarea1()
{
    try
    {
        await Task.Delay(1000);

        //throw new Exception();
    }
    catch (Exception ex)
    {

        ex.Data["nombre_metodo"] = nameof(Tarea1);
        throw;
    }
}

async Task Tarea2()
{
    try
    {
        await Task.Delay(1000);

        throw new Exception();
    }
    catch (Exception ex)
    {

        ex.Data["nombre_metodo"] = nameof(Tarea2);
        throw;
    }
}

async Task Tarea3()
{
    try
    {
        await Task.Delay(1000);

        throw new Exception();
    }
    catch (Exception ex)
    {

        ex.Data["nombre_metodo"] = nameof(Tarea3);
        throw;
    }
}

