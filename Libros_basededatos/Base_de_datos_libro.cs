using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Libros_basededatos
{
    public partial class Base_de_datos_libro : Form
    {
        public Base_de_datos_libro()
        {
            InitializeComponent();
        }
        SqlConnection conexion = new SqlConnection(@"server=DESKTOP-D1QBCII\SQLEXPRESS; database=Libro-Base de Datos; Integrated Security=true");

        private void bntIngresar_Click(object sender, EventArgs e)
        {
                conexion.Open();
                string insertar = "INSERT INTO TablaLibro (Cod_Libro, NombreLibro, Preciocompra, Fechacompra )VALUES(@Cod_Libro, @NombreLibro, @Preciocompra, @Fechacompra )";
                SqlCommand comando = new SqlCommand(insertar, conexion);
                comando.Parameters.Add(new SqlParameter("@Cod_Libro", this.txtCodigo.Text));
                comando.Parameters.Add(new SqlParameter("@NombreLibro", this.txtNombre.Text));
                comando.Parameters.Add(new SqlParameter("@Preciocompra", this.txtPrecio.Text));
                comando.Parameters.Add(new SqlParameter("@Fechacompra", datoFecha.Value));
                comando.ExecuteNonQuery();
                conexion.Close();
                MessageBox.Show("Filas Insertadas Correctamente");
            txtCodigo.Clear();
            txtNombre.Clear();
            txtPrecio.Clear();
            Base_de_datos_libro_Load(sender, e);
        }

        private void Base_de_datos_libro_Load(object sender, EventArgs e)
        {
            DataTable dt = getpersona();
            this.comboLibro.DataSource = dt;
            this.comboLibro.DisplayMember = "NombreLibro";
            this.comboLibro.ValueMember = "Cod_Libro";
        }
        private DataTable getpersona(string codigo = "")
        {
            string sql = "";
            if (codigo == "")
            {
                sql = "select Cod_Libro, NombreLibro, Preciocompra, Fechacompra ";
                sql += "from TablaLibro order by NombreLibro, Cod_Libro";
            }
            else
            {
                sql = "select Cod_Libro, NombreLibro, Preciocompra, Fechacompra ";
                sql += "from TablaLibro where Cod_Libro=@Cod_Libro order by NombreLibro";
            }

            SqlCommand comando = new SqlCommand(sql, conexion);
            if (codigo != "")
            {
                comando.Parameters.Add(new SqlParameter("@Cod_Libro", codigo));
            }
            SqlDataAdapter ad1 = new SqlDataAdapter(comando);

            //pasar los datos del adaptador a un datatable
            DataTable dt = new DataTable();
            ad1.Fill(dt);
            return dt;
        }
        private void Mostrar(object sender, EventArgs e)
        {
            //hola
            DataTable dt = getpersona(this.comboLibro.SelectedValue.ToString());
            //mostrar la informacion
            foreach (DataRow row in dt.Rows)
            {
                this.textVerCodigo.Text = row["Cod_Libro"].ToString();
                this.textVerNombre.Text = row["NombreLibro"].ToString();
                this.textVerPrecio.Text = row["Preciocompra"].ToString();
                this.VerDatoFecha.Value = Convert.ToDateTime(row["Fechacompra"].ToString());

            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿ESTAS SEGURO QUE DESEAS ELIMINAR?", "CONFIRMAR", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                conexion.Open();
                string eliminar = "DELETE FROM TablaLibro WHERE Cod_Libro = @Cod_Libro";
                SqlCommand cmd3 = new SqlCommand(eliminar, conexion);
                cmd3.Parameters.AddWithValue("@Cod_Libro", this.textVerCodigo.Text);
                cmd3.ExecuteNonQuery();
                conexion.Close();
                MessageBox.Show("PERSONA ELIMINADA CON EXITO", "ELIMINO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DataTable dt = getpersona();
                Base_de_datos_libro_Load(sender, e);
                textVerCodigo.Clear();
                textVerNombre.Clear();
                textVerPrecio.Clear();

            }
            else
            {
                MessageBox.Show("NO SE ELIMINO NINGUN DATO", "CANCELACION", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿ESTAS SEGURO QUE DESEAS ACTUALIZAR?", "CONFIRMAR", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                conexion.Open();
                string actualizar = "UPDATE TablaLibro SET NombreLibro=@NombreLibro, Preciocompra=@Preciocompra, Fechacompra=@FechaCompra WHERE Cod_Libro=@Cod_Libro";
                SqlCommand cmd2 = new SqlCommand(actualizar, conexion);
                cmd2.Parameters.AddWithValue("@Cod_Libro", this.textVerCodigo.Text);
                cmd2.Parameters.AddWithValue("@Preciocompra", this.textVerPrecio.Text);
                cmd2.Parameters.AddWithValue("@NombreLibro", this.textVerNombre.Text);
                cmd2.Parameters.AddWithValue("@Fechacompra", VerDatoFecha.Value);
                cmd2.ExecuteNonQuery();
                MessageBox.Show("Los datos han sido actualizados");
                conexion.Close();
                DataTable dt = getpersona();
                Base_de_datos_libro_Load(sender, e);
                textVerCodigo.Clear();
                textVerNombre.Clear();
                textVerPrecio.Clear();
            }
        }
    }
}
