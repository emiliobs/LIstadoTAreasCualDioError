namespace DemoINtroAsync
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void btnEmpezar_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = true;

            //Proceso Lento
            //Thread.Sleep(5000); //Synchronic
            await ProcesamientoLargo();


            pictureBox1.Visible = false;
        }

        public async Task ProcesamientoLargo()
        {
            await Task.Delay(5000); //Asynchrony
            MessageBox.Show("Ya pasaron los 5 Segundos!!");

        }

    }
}