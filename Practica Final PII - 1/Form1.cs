using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Practica_Final_PII___1
{
    public partial class Form : System.Windows.Forms.Form
    {
        Acciones aBD = new Acciones(@"Data Source=DESKTOP-6IKJ7NT\SQLEXPRESS;Initial Catalog=TUP_PII_2020;Integrated Security=True");

        List<Personas> lP = new List<Personas>();

        enum acciones
        {
            nuevo, editado
        }
        acciones miAccion;

        public Form()
        {
            InitializeComponent();
        }

        private void Abm_Load(object sender, EventArgs e)
        {
            cargarCombo(cboEstadoCivil, "tipo_estado_civil");
            cargarCombo(cboTipoDoc, "tipo_documento");
            cargarLista("personas");
            habilitar(false);
            miAccion = acciones.editado;
        }

        public void cargarCombo(ComboBox combo, string nombreTabla)
        {
            DataTable dt = new DataTable();

            dt = aBD.consultarTabla(nombreTabla);

            combo.DataSource = dt;

            combo.ValueMember = dt.Columns[0].ColumnName;

            combo.DisplayMember = dt.Columns[1].ColumnName;

            combo.DropDownStyle = ComboBoxStyle.DropDownList;

        }

        private void cargarLista(string nombreTabla)
        {
            lP.Clear();

            aBD.leerTabla(nombreTabla);

            while (aBD.Lector.Read())
            {
                Personas p = new Personas();

                if (!aBD.Lector.IsDBNull(0))
                {
                    p.Apellido = aBD.Lector.GetString(0);
                }
                if (!aBD.Lector.IsDBNull(1))
                {
                    p.Nombres = aBD.Lector.GetString(1);
                }
                if (!aBD.Lector.IsDBNull(2))
                {
                    p.Tipo_documento = aBD.Lector.GetInt32(2);
                }
                if (!aBD.Lector.IsDBNull(3))
                {
                    p.Documento = aBD.Lector.GetInt32(3);
                }
                if (!aBD.Lector.IsDBNull(4))
                {
                    p.Tipo_estado_civil = aBD.Lector.GetInt32(4);
                }
                lP.Add(p);
            }
            aBD.Lector.Close();
            aBD.desconectar();
            listBox1.Items.Clear();

            for (int i = 0; i < lP.Count; i++)
            {
                listBox1.Items.Add(lP[i].ToString());
            }

            listBox1.SelectedIndex = 0;         
        }
        private void habilitar(bool x)
        {
            txtApellido.Enabled = x;
            txtNombre.Enabled = x;
            txtDocumento.Enabled = x;
            cboTipoDoc.Enabled = x;
            cboEstadoCivil.Enabled = x;
            listBox1.Enabled = !x;
        }

        private void limpiar()
        {
            txtApellido.Clear();
            txtDocumento.Clear();
            txtNombre.Clear();
            cboEstadoCivil.SelectedIndex = -1;
            cboTipoDoc.SelectedIndex = -1;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarCampos(listBox1.SelectedIndex);
        }

        private void cargarCampos(int posicion)
        {
            txtApellido.Text = lP[posicion].Apellido;
            txtNombre.Text = lP[posicion].Nombres;
            txtDocumento.Text = lP[posicion].Documento.ToString();
            cboEstadoCivil.SelectedValue = lP[posicion].Tipo_estado_civil;
            cboTipoDoc.SelectedValue = lP[posicion].Tipo_documento;
            
        }
        private void btnEditar_Click(object sender, EventArgs e)
        {
            habilitar(true);
            txtDocumento.Enabled = false;
            txtApellido.Focus();

        }
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            habilitar(true);
            limpiar();
            miAccion = acciones.nuevo;

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            
            habilitar(false);
            miAccion = acciones.nuevo;

        }
        private void btnGrabar_Click(object sender, EventArgs e)
        {
            string consultaSQL = "";
            if (validarDatos())
            {
                Personas p = new Personas();
                p.Apellido = txtApellido.Text;
                p.Nombres = txtNombre.Text;
                p.Tipo_estado_civil = (int)cboEstadoCivil.SelectedValue;
                p.Tipo_documento = (int)cboTipoDoc.SelectedValue;
                p.Documento = int.Parse(txtDocumento.Text);

                if (rbtMasculino.Checked)
                {
                    p.Sexo = 1;
                }
                else
                {
                    p.Sexo = 2;
                }

                if (miAccion == acciones.nuevo)
                {
                    consultaSQL = "INSERT INTO personas(apellido,nombres,tipo_documento,documento,tipo_estado_civil,id_sexo)" +
                        "VALUES (@apellido,@nombres,@tipo_documento,@documento,@tipo_estado_civil,@id_sexo)";

                    aBD.actualizarParametros(consultaSQL, p);

                    cargarLista("personas");
                }
                else
                {
                    p.Documento = int.Parse(txtDocumento.Text);
                    consultaSQL = "UPDATE personas SET apellido=@apellido, nombres=@nombres" +
                        " WHERE documento=@documento";

                    aBD.actualizarParametros(consultaSQL, p);

                    cargarLista("personas");
                }
            }
            miAccion = acciones.editado;

            habilitar(false);
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Está seguro de eliminar esta persona?",
                "ELMINAR",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {

                string consultaSQL = "DELETE FROM personas WHERE documento="+lP[listBox1.SelectedIndex].Documento;

                aBD.actualizar(consultaSQL);

                cargarLista("personas");

            }
        }

        private bool validarDatos()
        {
            if (txtApellido.Text == string.Empty)
            {
                MessageBox.Show("Debe ingresar apellido");
                txtApellido.Focus();
                return false;                
            }
            if (txtNombre.Text == string.Empty)
            {
                MessageBox.Show("Debe ingresar nombre");
                txtNombre.Focus();
                return false;
            }
            if (cboTipoDoc.SelectedIndex == -1)
            {
                MessageBox.Show("Debe ingresar tipo de documento");
                cboTipoDoc.Focus();
                return false;
            }
            if (cboEstadoCivil.SelectedIndex == -1)
            {
                MessageBox.Show("Debe ingresar estado civil");
                cboEstadoCivil.Focus();
                return false;
            }
            return true;
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Está seguro de abandonar la aplicacion?",
                "SALIENDO",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        
    }
}
