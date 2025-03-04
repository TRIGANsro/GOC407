namespace Lab4
{
    public partial class Form1 : Form
    {
        CancellationTokenSource cancellationTokenSource = new();

        public Form1()
        {
            InitializeComponent();
        }

        private async void BtnAkce_Click(object sender, EventArgs e)
        {
            BtnCancel.Enabled = true;
            BtnAkce.Enabled = false;

            LblText.Text = "Pracuji";

            Progress<(int, string)> progress = new Progress<(int, string)>((x) => { ProgressBarMain.Value = x.Item1; StatusText.Text = x.Item2; });

            CancellationToken cancellationToken = cancellationTokenSource.Token;

            Pracant pracant = new();
            try
            {
                string vysledek = await Task.Run(() => pracant.DlouhaAkce(42, progress, cancellationToken));
                LblText.Text = vysledek;
            }
            catch (OperationCanceledException ex)
            {
                LblText.Text = "Prerusena operace";
                ProgressBarMain.Value = 0;
                StatusText.Text = ex.Message;
            }
            catch (Exception ex)
            {
                LblText.Text = "Neocekavana chyba";
                ProgressBarMain.Value = 0;
                StatusText.Text = ex.Message;
            }

            BtnCancel.Enabled = false;
            BtnAkce.Enabled = true;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            cancellationTokenSource.Cancel();
        }
    }
}
