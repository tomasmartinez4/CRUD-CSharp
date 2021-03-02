using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Practica_Final_PII___1
{
    class Acciones
    {
        
        public SqlConnection conexion;
        public SqlCommand comando;
        public SqlDataReader lector;
        public string cadenaConexion;
    

        public SqlDataReader Lector { get => lector; set => lector = value; }
        public string CadenaConexion { get => cadenaConexion; set => cadenaConexion = value; }

        public Acciones(string cadenaConexion)
        {
            this.conexion = new SqlConnection();
            this.comando = new SqlCommand();
            this.Lector = null;
            this.CadenaConexion = cadenaConexion;
        }

        public Acciones()
        {
            this.conexion = new SqlConnection();
            this.comando = new SqlCommand();
            this.Lector = null;
            this.CadenaConexion = null;
        }

        public void conectar() 
        {
            conexion.ConnectionString = cadenaConexion;
            conexion.Open();
            comando.Connection = conexion;
            comando.CommandType = CommandType.Text;
        }

        public void desconectar()
        {
            conexion.Close();
            conexion.Dispose();
        }

        public DataTable consultarTabla(string nombreTabla)
        {
            DataTable tabla = new DataTable();
            this.conectar();
            this.comando.CommandText = "SELECT * FROM " + nombreTabla;
            tabla.Load(comando.ExecuteReader());
            this.desconectar();
            return tabla;
        }

        public void leerTabla(string nombreTabla)
        {
            this.conectar();
            this.comando.CommandText = "SELECT * FROM " + nombreTabla;
            this.lector = comando.ExecuteReader();

        }

        public void actualizar(string consultaSQL)
        {
            this.conectar();
            this.comando.CommandText = consultaSQL;
            this.comando.ExecuteNonQuery();
            this.desconectar();
        }

        public void actualizarParametros(string consultaSQL, Personas p)
        {
            this.conectar();
            this.comando.CommandText = consultaSQL;
            comando.Parameters.Clear();

            comando.Parameters.AddWithValue("@apellido",p.Apellido);
            comando.Parameters.AddWithValue("@nombres",p.Nombres);
            comando.Parameters.AddWithValue("@tipo_documento",p.Tipo_documento);
            comando.Parameters.AddWithValue("@documento",p.Documento);
            comando.Parameters.AddWithValue("@tipo_estado_civil",p.Tipo_estado_civil);
            comando.Parameters.AddWithValue("@id_sexo",p.Sexo);

            this.comando.ExecuteNonQuery();
            this.desconectar();
        }
    }
}
