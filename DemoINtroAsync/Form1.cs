using System.Diagnostics;

namespace DemoINtroAsync
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //Peligroso: async void debe ser evitado, EXECTO en eventos.
        private async void btnEmpezar_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = true;

            var sw = new Stopwatch();

            sw.Start();

            //Proceso Lento
            //Thread.Sleep(5000); //Synchronic
            //var nombre = await ProcesamientoLargo();
            //MessageBox.Show($"Saludos, {nombre}");

            await RealizarProcesamientoLargoA();
            await RealizarProcesamientoLargoB();
            await RealizarProcesamientoLargoC();

            sw.Stop();

            var duracion = $"El progrmama se ejecutó en {sw.ElapsedMilliseconds / 1000.0} segundos.";
            Console.WriteLine(duracion);

            pictureBox1.Visible = false;
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