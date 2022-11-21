using System.Net;
using System.Windows.Forms;

namespace TP1
{
    internal partial class Form1 : Form
    {
        Form2 formDeRegistro;
        Form3 formUsuarioLogueado;
        Banco banco;
        public Form1()
        {
            banco = new Banco();
            //formDeRegistro = new Form2(banco, TransfDelegadoForm2);
            InitializeComponent();
        }

        private  void TransfDelegadoForm2(){
            this.Show();
            formDeRegistro.Close();
        
        }

        private void TransfDelegadoForm3()
        {
            this.Show();
            try { 
                //formDeRegistro.Close();
            } catch(Exception ex) { }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtB_dni.Text, "[^0-9]"))
            {
                txtB_dni.Text = txtB_dni.Text.Remove(txtB_dni.Text.Length - 1);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(banco.verificarLogueo(int.Parse(txtB_dni.Text), txtB_contrasena.Text))
            {
                //MessageBox.Show("Logueo correcto");
                formUsuarioLogueado = new Form3(banco, TransfDelegadoForm3);
                formUsuarioLogueado.Show();
                this.Hide();
            } 
            else
            {
                MessageBox.Show("Error de logueo");
            }
        }

        private void lbl_Registrarse_Click_1(object sender, EventArgs e)
        {
            formDeRegistro = new Form2(banco, TransfDelegadoForm2);
            this.Hide();

            formDeRegistro.Show();

        }
    }

}