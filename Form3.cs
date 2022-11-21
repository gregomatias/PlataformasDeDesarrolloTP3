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
        private int celda = -1;

        public Form3(Banco banco, TransfDelegadoForm2 transEvento)
        {

            this.transEvento = transEvento;

            this.banco = banco;


            InitializeComponent();

            cargaCajasAhorro();
            cargaPlazoFijo();
            cargaTarjetasDeCredito();
            cargarPagos();
            cargaUsuarios();

            label2.Text = banco.obtenerNombre();

            //Valida Amin
            if (!banco.esAdmin()) { 
                this.tabControl.TabPages.Remove(tabUsuarios);
                this.tabControl.TabPages.Remove(tabTrasladoCajas);
            }

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
            if (banco.crearCajaAhorro())
            {
                cargaCajasAhorro();
                MessageBox.Show("Caja de ahorro creada con éxito");
            }
            else { MessageBox.Show("No se pudo crear la caja de ahorro"); }
        }

        private void cargaCajasAhorro()
        {
            int fila;
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            List<CajaDeAhorro> lista = null;

            lista = banco.MostrarCajasDeAhorro();

            if (lista!=null)
            { 
                foreach (CajaDeAhorro caja in lista)
                {

                    fila = dataGridView1.Rows.Add();
                    dataGridView1.Rows[fila].Cells[0].Value = caja._cbu;
                    dataGridView1.Rows[fila].Cells[1].Value = caja._saldo;

                }
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
                    cargaPlazoFijo();
                    break;
                case 2:
                    // Pagos
                    cargarPagos();
                    break;
                case 3:
                    cargaTarjetasDeCredito();

                    break;
                case 6:
                    // Let's suppose TabPage index 1 is the one for the transactions.
                    // Assuming you have put a DataGridView control so that the transactions can be listed.
                    // currentCustomer.Id can be obtained through the CurrencyManager of your BindingSource object used to data bind your data to your Windows form controls.

                    break;
                case 7:
                    cargaUsuarios();
                    break;
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void comboBoxPlazo_Click(object sender, EventArgs e)
        {

            comboBoxPlazo.Items.Clear();
            comboBoxPlazo.Refresh();
            List<CajaDeAhorro> ca = null;

            ca = banco.MostrarCajasDeAhorro();



            if (ca != null) { 
                foreach (CajaDeAhorro caja in ca)
                {
                    comboBoxPlazo.Items.Add(caja._cbu);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBoxPlazo.Text != "" && textBoxPlazo.Text != "")
            {
                CajaDeAhorro caja;

                DateTime fecha = new DateTime();
                fecha = DateTime.Now;
                float montoPlazo = float.Parse(textBoxPlazo.Text);
                if (montoPlazo >= 1000)
                {
                    if (banco.AltaPlazoFijo(montoPlazo, 7, comboBoxPlazo.SelectedItem.ToString()))
                    {
                        MessageBox.Show("El plazo fijo ha sido creado exitosamente");
                        cargaPlazoFijo();

                    }
                    else
                    {
                        MessageBox.Show("El saldo de la cuenta no es suficiente");

                    }
                }
                else
                {
                    MessageBox.Show("El monto debe ser al menos 1000$");
                }
            }
            else
            {
                MessageBox.Show("Elija una caja de ahorro para realizar el Plazo Fijo y un monto");

            }


        }

        private void cargaPlazoFijo()
        {
            int fila;
            dataGridPlazo.Rows.Clear();
            dataGridPlazo.Refresh();

            List<PlazoFijo> listPf = null;

            if (banco.esAdmin())
            {
                listPf = banco.buscarPlazosFijosAdmin();
            } else { listPf = banco.buscarPlazosFijosUsuario(); }

            if (listPf != null) { 
                foreach (PlazoFijo pf in listPf)
                {

                    fila = dataGridPlazo.Rows.Add();
                    dataGridPlazo.Rows[fila].Cells[0].Value = pf._id_plazoFijo;
                    dataGridPlazo.Rows[fila].Cells[1].Value = pf._monto;
                    dataGridPlazo.Rows[fila].Cells[3].Value = pf._fechaFin;
                    dataGridPlazo.Rows[fila].Cells[4].Value = pf._tasa;

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (banco.bajaPlazoFijo(int.Parse(dataGridPlazo.CurrentCell.Value.ToString())))
                {

                    MessageBox.Show("El Plazo Fijo se ha eliminado");
                    cargaPlazoFijo();
                }
                else
                {
                    MessageBox.Show("El plazo fijo aún se encuentra pendiente de pago, pruebe eliminar el registro en una fecha posterior");

                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        private void cBox_caja_ahorro_Click(object sender, EventArgs e)
        {
            cBox_caja_ahorro.Items.Clear();
            cBox_caja_ahorro.Refresh();

            List<CajaDeAhorro> ca = null;

     
                ca = banco.MostrarCajasDeAhorro();



            if (ca != null)
            {
                foreach (CajaDeAhorro caja in ca)
                {
                    cBox_caja_ahorro.Items.Add(caja._cbu);
                }
            }

        }

        private void cbx_lista_CajasAhorro_Click(object sender, EventArgs e)
        {
            cbx_lista_CajasAhorro.Items.Clear();
            cbx_lista_CajasAhorro.Refresh();

            List<CajaDeAhorro> ca = null;

  
                ca = banco.MostrarCajasDeAhorro();

                if (ca != null)
            {
                foreach (CajaDeAhorro caja in ca)
                {
                    cbx_lista_CajasAhorro.Items.Add(caja._cbu);
                }
            }

        }

        private void btn_Crear_Tarjeta_Click(object sender, EventArgs e)
        {
            if (banco.altaTarjeta())
            {
                MessageBox.Show("Tarjeta dada de alta correctamente");
                cargaTarjetasDeCredito();
            }
            else
            {
                MessageBox.Show("Tarjeta no pudo darse de alta");
            }
        }

        private void cargaTarjetasDeCredito()
        {
            int fila;
            dataGView_Tarjetas.Rows.Clear();
            dataGView_Tarjetas.Refresh();

            List<TarjetaDeCredito> tc = null;


            tc = banco.MostrarTarjetasDeCredito();


            if (tc != null) { 
                foreach (TarjetaDeCredito tarjeta in tc)
                {

                    fila = dataGView_Tarjetas.Rows.Add();
                    dataGView_Tarjetas.Rows[fila].Cells[0].Value = tarjeta._numero;
                    dataGView_Tarjetas.Rows[fila].Cells[1].Value = tarjeta._limite;
                    dataGView_Tarjetas.Rows[fila].Cells[2].Value = tarjeta._consumos;

                }
            }

        }

        private void btn_PagarTarjeta_Click(object sender, EventArgs e)
        {
            try
            {
                if (banco.PagarTarjeta(dataGView_Tarjetas.CurrentCell.Value.ToString(), cbx_lista_CajasAhorro.SelectedItem.ToString()))
                {

                    MessageBox.Show("Se ha cancelado el saldo de su tarjeta");
                    cargaTarjetasDeCredito();
                }
                else
                {
                    MessageBox.Show("No se realizo el pago, verifique su saldo");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Debe seleccionar una tarjeta y una caja de ahorro válida");
            }
        }

        private void comboBox1_Click(object sender, EventArgs e)
        {

            comboBox1.Items.Clear();
            comboBox1.Refresh();

            List<CajaDeAhorro> ca = null;

  
                ca = banco.MostrarCajasDeAhorro();

            if (ca != null)
            {
                foreach (CajaDeAhorro caja in ca)
                {
                    comboBox1.Items.Add(caja._cbu);
                }
            }
            txtb_monto.Enabled = true;

        }

        private void btn_extraer_Click(object sender, EventArgs e)
        {

            try
            {
                if (txtb_monto.Text != "")
                {
                    float monto = float.Parse(txtb_monto.Text);
                    if (monto > 0)
                    {


                        if (banco.retirarSaldo(comboBox1.SelectedItem.ToString(), monto))
                        {
                            MessageBox.Show("Retiro efectuado");
                            txtb_monto.Text = "";
                        }
                        else
                        {
                            MessageBox.Show("El retiro no pudo efectuarse, verifique su saldo");
                        }

                    }
                    else { MessageBox.Show("Monto debe ser mayor a cero"); }


                }
                else { MessageBox.Show("El monto no puede estar vacio"); }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Debe ingresar una cuanta válida y completar el monto a extraer");
            }

        }

        private void btn_depositar_Click(object sender, EventArgs e)
        {
             
            try { 
                if (txtb_monto.Text != "" && comboBox1.SelectedItem.ToString() !="")
                {
                    float monto = float.Parse(txtb_monto.Text);
                    if (monto > 0)
                    {

                        if (banco.depositarSaldo(comboBox1.SelectedItem.ToString(), monto))
                        {
                            MessageBox.Show("Deposito efectuado");
                            txtb_monto.Text = "";
                            // refreshData();
                        }
                        else
                        {
                            MessageBox.Show("Error al realizar deposito");
                        }

                    }
                    else { MessageBox.Show("Monto debe ser mayor a cero"); }


                }
                else { MessageBox.Show("Monto y caja son obligatorios"); }
            }
            catch(Exception ex) {MessageBox.Show(ex.Message); }

        }

        private void comboBox2_Click(object sender, EventArgs e)
        {

            comboBox2.Items.Clear();
            comboBox2.Refresh();

            List<CajaDeAhorro> ca = null;

   
                ca = banco.MostrarCajasDeAhorro();


            if (ca != null)
            {
                foreach (CajaDeAhorro caja in ca)
                {
                    comboBox2.Items.Add(caja._cbu);
                }
            }
        }

        private void btn_transferir_Click(object sender, EventArgs e)
        {



            if (txtb_monto_transferencia.Text != "" && txtb_cbu_destino.Text != "")
            {
                float monto = float.Parse(txtb_monto_transferencia.Text);
                string cbu_destino = txtb_cbu_destino.Text;
                if (monto > 0)
                {


                    if (banco.AltaTransferencia(comboBox2.SelectedItem.ToString(), cbu_destino, monto))
                    {
                        MessageBox.Show("Realizada correctamente");
                        txtb_monto_transferencia.Text = "";

                    }
                    else
                    {
                        MessageBox.Show("La transferencia no pudo efectuarse, verifique su saldo");
                    }

                }
                else { MessageBox.Show("Monto debe ser mayor a cero y debe seleccionar caja de ahorro"); }


            }
            else { MessageBox.Show("El monto no puede estar vacio"); }


        }

        private void cBox_tarjeta_Click(object sender, EventArgs e)
        {
            cBox_tarjeta.Items.Clear();
            cBox_tarjeta.Refresh();

            List<TarjetaDeCredito> tc = null;


            tc = banco.MostrarTarjetasDeCredito();


            if (tc != null) { 
                foreach (TarjetaDeCredito t in tc)
                {
                    cBox_tarjeta.Items.Add(t._numero);
                }
            }

        }

        private void btn_ingresar_pago_Click(object sender, EventArgs e)
        {
            float montoPago = 0;
            try { montoPago = float.Parse(txtb_monto_pago.Text); }
            catch (Exception ex) { MessageBox.Show("Debe ingresar el monto del pago"); }


            if (cBox_tarjeta.Text == "" && cBox_caja_ahorro.Text == "")
            {
                MessageBox.Show("Debe ingresar un método de pago");
            }
            else
            {
                if ((cBox_tarjeta.Text == "" && cBox_caja_ahorro.Text != "") || (cBox_tarjeta.Text != "" && cBox_caja_ahorro.Text == ""))
                {

                    if (cBox_tarjeta.Text != "")
                    {
                        banco.AltaPago(montoPago, "TC", txtb_concepto_pago.Text, Int64.Parse(cBox_tarjeta.Text));
                        MessageBox.Show("Pago ingresado");
                        cargarPagos();
                    }
                    else if (cBox_caja_ahorro.Text != "")
                    {
                        banco.AltaPago(montoPago, "CA", txtb_concepto_pago.Text, Int64.Parse(cBox_caja_ahorro.Text));
                        MessageBox.Show("Pago ingresado");
                        cargarPagos();
                    }
                    else { MessageBox.Show("Tarjeta o Caja de Ahorro deben tener datos"); }
                }
                else
                {
                    MessageBox.Show("Debe ingresar solo un método de pago");
                    cBox_caja_ahorro.Text = "";
                    cBox_tarjeta.Text = "";
                }
            }

        }

        public void cargarPagos()
        {
            this.cargaListaPagos(true);
            this.cargaListaPagos(false);

        }

        public void cargaListaPagos(bool pagado)
        {

            List<Pago> p = null;

            if (banco.esAdmin())
            {
                p = banco.buscarPagosAdmin(pagado);

            }
            else { p = banco.buscarPagosUsuario(pagado); }

            if (!pagado)
            {
                dataGridView3.Rows.Clear();
                dataGridView3.Refresh();

                int fila;
                if (p!=null) { 
                    foreach (Pago pago in p)
                    {

                        fila = dataGridView3.Rows.Add();
                        dataGridView3.Rows[fila].Cells[0].Value = pago._id_pago;
                        dataGridView3.Rows[fila].Cells[1].Value = pago._metodo;
                        dataGridView3.Rows[fila].Cells[2].Value = pago._detalle;
                        dataGridView3.Rows[fila].Cells[3].Value = pago._monto;

                    }
                }
            }
            else
            {

                dataGridView4_pagos_pendientes.Rows.Clear();
                dataGridView4_pagos_pendientes.Refresh();

                int fila;
                if (p!=null)
                {
                    foreach (Pago pago in p)
                    {

                        fila = dataGridView4_pagos_pendientes.Rows.Add();
                        dataGridView4_pagos_pendientes.Rows[fila].Cells[0].Value = pago._id_pago;
                        dataGridView4_pagos_pendientes.Rows[fila].Cells[1].Value = pago._metodo;
                        dataGridView4_pagos_pendientes.Rows[fila].Cells[2].Value = pago._detalle;
                        dataGridView4_pagos_pendientes.Rows[fila].Cells[3].Value = pago._monto;


                    }
                }

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //this.celda >= 0 && 
            if (banco.modificarPago(this.celda))
            {
                MessageBox.Show("El pago se realizo de manera exitosa");
                cargarPagos();
            }
            else
            {
                MessageBox.Show("El pago no pudo realizarse");
            }
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (Banco.IsNumeric(dataGridView3.CurrentCell.Value.ToString()))
            {
                this.celda = int.Parse(dataGridView3.CurrentCell.Value.ToString());
            }
            else
            {
                MessageBox.Show("Debe seleccionar un ID de pago válido");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (banco.eliminarPago(this.celda))
            {
                MessageBox.Show("El pago se elimino de manera exitosa");
                cargaListaPagos(true);
            }
            else
            {
                MessageBox.Show("El pago no pudo eliminarse");
            }

        }

        private void dataGridView4_pagos_pendientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Banco.IsNumeric(dataGridView4_pagos_pendientes.CurrentCell.Value.ToString()))
            {
                this.celda = int.Parse(dataGridView4_pagos_pendientes.CurrentCell.Value.ToString());
            }
            else
            {
                MessageBox.Show("Debe seleccionar un ID de pago válido");
            }

        }

        private void btn_busca_movimiento_Click(object sender, EventArgs e)
        {
            if (comboBox3_movimientos.Text != "")
            {
                float montoFiltro;
                if (txtb_filtro_monto.Text != "")
                {
                    montoFiltro = float.Parse(txtb_filtro_monto.Text);

                }
                else { montoFiltro = 0; }

                List<List<string>> listaMovimientos = new List<List<string>>();

                listaMovimientos = banco.MostrarMovimientos(comboBox3_movimientos.Text, txtb_filtro_detalle.Text, dateTimePicker_filtro.Value, montoFiltro);

                dataGridView_movimiento.Rows.Clear();
                dataGridView_movimiento.Refresh();


                //Nuevo retorna un string con todos los datos
                foreach (List<string> intem in listaMovimientos)
                {
                    dataGridView_movimiento.Rows.Add(intem.ToArray());


                }




            }
            else { MessageBox.Show("La cuenta es obligatoria"); }

        }

        private void comboBox3_movimientos_Click(object sender, EventArgs e)
        {

            comboBox3_movimientos.Items.Clear();
            comboBox3_movimientos.Refresh();

            List<CajaDeAhorro> ca = null;

      
                ca = banco.MostrarCajasDeAhorro();


            if (ca != null)
            {
                foreach (CajaDeAhorro caja in ca)
                {
                    comboBox3_movimientos.Items.Add(caja._cbu);
                }
            }

        }

        private void cargaUsuarios()
        {
            int fila;
            dataGridUsuarios.Rows.Clear();
            dataGridUsuarios.Refresh();

            List<Usuario> lista = banco.listarUsuarios();

            if(lista!=null)
            { 
                foreach (Usuario u in lista)
                {

                    fila = dataGridUsuarios.Rows.Add();
                    dataGridUsuarios.Rows[fila].Cells[0].Value = u._id_usuario;
                    dataGridUsuarios.Rows[fila].Cells[1].Value = u._dni;
                    dataGridUsuarios.Rows[fila].Cells[2].Value = u._nombre;
                    dataGridUsuarios.Rows[fila].Cells[3].Value = u._apellido;

                }
            }
        }

        private void dataGridUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (Banco.IsNumeric(dataGridUsuarios.CurrentCell.Value.ToString()))
            {
                this.celda = int.Parse(dataGridUsuarios.CurrentCell.Value.ToString());
            }
            else
            {
                MessageBox.Show("Debe seleccionar un ID de pago válido");
            }
        }

        private void buttonDesbloquear_Click(object sender, EventArgs e)
        {
            try
            {
                if(banco.desbloquearUsuario(this.celda))
                {
                    MessageBox.Show("Usuario desbloqueado");
                } else
                {
                    MessageBox.Show("Ocurrio un error al desbloquear usuario");
                }
            }
            catch(Exception ex) 
            {
                MessageBox.Show("Ocurrio un error al desbloquear usuario: " + ex.Message);
            }
        }

        private void btn_eliminarUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                if (banco.eliminarUsuario(this.celda))
                {
                    MessageBox.Show("Usuario eliminado");
                    cargaUsuarios();
                }
                else
                {
                    MessageBox.Show("Ocurrio un error al eliminar usuario");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error al eliminar usuario: " + ex.Message);
            }

        }


        private void comBox_cbu_Traslado_Saldo_Click(object sender, EventArgs e)
        {
            comBox_cbu_Traslado_Saldo.Items.Clear();
            comBox_cbu_Traslado_Saldo.Refresh();

            List<CajaDeAhorro> ca = null;

      
                ca = banco.MostrarCajasDeAhorro();


            if (ca != null)
            {
                foreach (CajaDeAhorro caja in ca)
                {
                    comBox_cbu_Traslado_Saldo.Items.Add(caja._cbu);
                }
            }

        }

        //comBox_id_usuario_Traslado
        private void comBox_id_usuario_Traslado_Click(object sender, EventArgs e)
        {
            comBox_id_usuario_Traslado.Items.Clear();
            comBox_id_usuario_Traslado.Refresh();

            foreach (Usuario u in banco.listarUsuarios())
            {
                comBox_id_usuario_Traslado.Items.Add(u._id_usuario);
            }

        }

        private void btn_elimina_Caja_Click(object sender, EventArgs e)
        {
            if (comBox_cbu_Traslado_Saldo.Text != "") { 
                if(comBox_id_usuario_Traslado.Text != "") { 
                    try
                    {
                        if (banco.eliminarTitularCaja(int.Parse(comBox_id_usuario_Traslado.Text), comBox_cbu_Traslado_Saldo.Text)) 
                        {
                            MessageBox.Show("Titular eliminado correctamente");
                        } else { MessageBox.Show("El titular a eliminar debe ser del cbu seleccionado"); }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Ocurrió un error al eliminar titular de la cuenta: " + ex.Message);
                    }
                } else { MessageBox.Show("Debe seleccionar un ID de titular"); }
            } else { MessageBox.Show("Debe seleccionar un CBU"); }
        }

        private void btn_traslada_Caja_Click(object sender, EventArgs e)
        {
            if (comBox_cbu_Traslado_Saldo.Text != "")
            {
                if (comBox_id_usuario_Traslado.Text != "")
                {
                    try
                    {
                        if (banco.agregarTitularCaja(int.Parse(comBox_id_usuario_Traslado.Text), comBox_cbu_Traslado_Saldo.Text))
                        {
                            MessageBox.Show("Titular agregado correctamente");
                        }
                        else { MessageBox.Show("Ocurrió un error al agregar titular a la cuenta"); }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ocurrió un error al agregar titular a la cuenta: " + ex.Message);
                    }
                }
                else { MessageBox.Show("Debe seleccionar un ID de titular"); }
            }
            else { MessageBox.Show("Debe seleccionar un CBU"); }
        }
    }
}

