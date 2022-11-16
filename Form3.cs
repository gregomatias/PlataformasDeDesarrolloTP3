using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace TP1
{
    internal partial class Form3 : Form
    {
        private object[] argumentos;
        private List<List<string>> datos;
        private Banco banco;
        private TransfDelegadoForm2 transEvento;
        private int celda;

        public Form3(Banco banco, TransfDelegadoForm2 transEvento)
        {

            this.transEvento = transEvento;

            this.banco = banco;
            

            InitializeComponent();

            cargaCajasAhorro();

            //Valida Amin
            /*if (!banco.esAdmin()) { 
                this.tabControl.TabPages.Remove(tabUsuarios);
                this.tabControl.TabPages.Remove(tabTrasladoCajas);
           
            }*/

        }
        public Form3(object[] args)
        {
            InitializeComponent();
            argumentos = args;
            label2.Text = (string)args[0];

            datos = new List<List<string>>();
        }


        public delegate void TransfDelegadoForm2();


        private void button1_Click(object sender, EventArgs e)
        {
            refreshData();
        }

        private void refreshData()
        {
            //borro los datos
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();


        }


        private void button4_Click(object sender, EventArgs e)
        {

            this.Close();
            //banco.CerrarSesion();
            this.transEvento();

        }

        private void btn_crearCajaAhorro_Click(object sender, EventArgs e)
        {
            if(banco.crearCajaAhorro())
            {
                cargaCajasAhorro();
                MessageBox.Show("Caja de ahorro creada con éxito");
            } else { MessageBox.Show("No se pudo crear la caja de ahorro"); }
        }

        private void cargaCajasAhorro()
        {
            int fila;
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            foreach (CajaDeAhorro caja in banco.buscarCajasUsuario())
            {

                fila = dataGridView1.Rows.Add();
                dataGridView1.Rows[fila].Cells[0].Value = caja._cbu;
                dataGridView1.Rows[fila].Cells[1].Value = caja._saldo;

            }
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {

            switch ((sender as TabControl).SelectedIndex)
            {
                case 0:
                    // Do nothing here (let's suppose that TabPage index 0 is the address information which is already loaded.

                    cargaCajasAhorro();

                    break;
                case 1:
                    // PLazo Fijo
                    break;
                case 2:
                    // Pagos
                    //cargarPagos();
                    break;
                case 3:

                    //cargaTarjetasDeCredito();

                    break;
                case 6:
                    // Let's suppose TabPage index 1 is the one for the transactions.
                    // Assuming you have put a DataGridView control so that the transactions can be listed.
                    // currentCustomer.Id can be obtained through the CurrencyManager of your BindingSource object used to data bind your data to your Windows form controls.

                    break;
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
    }
}

