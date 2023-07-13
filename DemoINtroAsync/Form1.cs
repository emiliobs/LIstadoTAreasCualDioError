using System.Diagnostics;

namespace DemoINtroAsync
{
    public partial class Form1 : Form
    {

        HttpClient httpClient = new HttpClient();
        public Form1()
        {
            InitializeComponent();
        }
        //Peligroso: async void debe ser evitado, EXECTO en eventos.
        private async void btnEmpezar_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = true;

            Console.WriteLine("Inicio");
            List<Imagen> imagenes = ObtnerImagenes();

            var sw = new Stopwatch();

            sw.Start();

            //Proceso Lento
            //Thread.Sleep(5000); //Synchronic
            //var nombre = await ProcesamientoLargo();
            //MessageBox.Show($"Saludos, {nombre}");

            //    var tareas = new List<Task>()
            //  {
            //         RealizarProcesamientoLargoA(),
            //         RealizarProcesamientoLargoB(),
            //         RealizarProcesamientoLargoC(),
            //};

            //await Task.WhenAll(tareas);

            var directorioActual = AppDomain.CurrentDomain.BaseDirectory;
            var destinoBaseSecuencial = Path.Combine(directorioActual, @"Imagenes\resultado-secuencial");
            var destinoBaseParalelo = Path.Combine(directorioActual, @"Imagenes\resultado-paralelo");
            PrepararEjecucion(destinoBaseParalelo, destinoBaseSecuencial);

            //Parte Secuencial:
            foreach (var imagen in imagenes)
            {
                await ProcesarImagen(destinoBaseSecuencial, imagen);
            }

            Console.WriteLine($"Secuencial - duración en segundos: {sw.ElapsedMilliseconds / 1000.0}");

            sw.Restart();

            sw.Start();

            var tareaEnumerable = imagenes.Select(async imagen =>
            {
                await ProcesarImagen(destinoBaseParalelo, imagen);
            });

            await Task.WhenAll(tareaEnumerable);

            Console.WriteLine($"Paralelo - duración en segundos: : {sw.ElapsedMilliseconds / 1000.0}");


            sw.Stop();

            //var duracion = $"El progrmama se ejecutó en {sw.ElapsedMilliseconds / 1000.0} segundos.";
            //Console.WriteLine(duracion);

            pictureBox1.Visible = false;
        }

        private async Task ProcesarImagen(string directorio, Imagen imagen)
        {
            var respuesta = await httpClient.GetAsync(imagen.URL);
            var contenido = await respuesta.Content.ReadAsByteArrayAsync();

            Bitmap bitmap;

            using (var ms = new MemoryStream(contenido))
            {
                bitmap = new Bitmap(ms);
            }

            bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
            var destino = Path.Combine(directorio, imagen.Nombre);
            bitmap.Save(destino);
        }

        private static List<Imagen> ObtnerImagenes()
        {
            var imaganes = new List<Imagen>();

            for (int i = 0; i < 10; i++)
            {
                imaganes.Add(new Imagen()
                {
                    Nombre = $"Bolívar{i}.jpg",
                    URL = "https://upload.wikimedia.org/wikipedia/commons/c/c4/Bol%C3%ADvar_en_Carabobo.jpg",
                });

                imaganes.Add(new Imagen()
                {
                    Nombre = $"Parque Arqueológico de San Agustín{i}.jpg",
                    URL = "https://upload.wikimedia.org/wikipedia/commons/4/4c/Parque_Arqueol%C3%B3gico_de_San_Agust%C3%ADn_-_tomb_of_a_deity_with_supporting_warriors.jpg",
                });

                imaganes.Add(new Imagen()
                {
                    Nombre = $"Gold Museum{i}.jpg",
                    URL = "https://upload.wikimedia.org/wikipedia/commons/9/99/Gold_Museum%2C_Bogota_%2836145671394%29.jpg",
                });
            }

            return imaganes;
        }

        private void BorrandoArchivos(string directorio)
        {
            var archivos = Directory.EnumerateFiles(directorio);

            foreach (var archivo in archivos)
            {
                File.Delete(archivo);
            }
        }

        private void PrepararEjecucion(string destinoBaseParalelo, string destinoBaseSecuencial)
        {
            if (!Directory.Exists(destinoBaseParalelo))
            {
                Directory.CreateDirectory(destinoBaseParalelo);
            }

            if (!Directory.Exists(destinoBaseSecuencial))
            {
                Directory.CreateDirectory(destinoBaseSecuencial);
            }

            BorrandoArchivos(destinoBaseSecuencial);
            BorrandoArchivos(destinoBaseParalelo);
        }



        public async Task<string> ProcesamientoLargo()
        {
            await Task.Delay(5000); //Asynchrony

            return "Emilio Barrera";
            //MessageBox.Show("Ya pasaron los 5 Segundos!!");

        }

        private async Task RealizarProcesamientoLargoA()
        {
            await Task.Delay(1000);//Async
            Console.WriteLine("Proceso A ha Finalizado!");
        }

        private async Task RealizarProcesamientoLargoB()
        {
            await Task.Delay(1000);//Async
            Console.WriteLine("Proceso B ha Finalizado!");
        }

        private async Task RealizarProcesamientoLargoC()
        {
            await Task.Delay(1000);//Async
            Console.WriteLine("Proceso C ha Finalizado!");
        }

    }
}