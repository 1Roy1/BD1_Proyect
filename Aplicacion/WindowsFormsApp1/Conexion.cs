using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    class Conexion
    {
        EncryptMD5 encrypt = new EncryptMD5();
        MySqlConnection cnx = new MySqlConnection();
        static string servidor = "localhost";
        static string bd= "proyecto";
        static string usuario= "root";
        static string password= "root123";
        static string puerto= "3306";
        string cadenaConexion = "server="+servidor+";"+"port="+puerto+";"+"user id="+usuario+";"+"password="+password+";"+"database="+bd+";";
        public MySqlConnection establecerconexion() {
            try {
                cnx.ConnectionString = cadenaConexion;
                cnx.Open();
                MessageBox.Show("SE CONECTO CORRECTAMENTE");

            }
            catch (MySqlException e){
                MessageBox.Show("ERROR: " + e.ToString());
               
            }
            return cnx;
        }
        public void InsertarUsuario(string usuario, string contraseña)
        {
            try
            {
                string query = "INSERT INTO usuarios (Usuario, Password) VALUES (@Usuario, @Password)";
                MySqlCommand command = new MySqlCommand(query, cnx);
                command.Parameters.AddWithValue("@Usuario", usuario);
                command.Parameters.AddWithValue("@Password", contraseña);

                
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al insertar usuario: " + ex.Message);
            }
        }
        public (string usuario, string contraseña) BuscarUsuario(string usuario, string contraseña)
        {
            string usuarioEncontrado = null;
            string contraseñaEncontrada = null;

            try
            {
                string query = "SELECT Usuario, Password FROM usuarios WHERE Usuario = @Usuario";
                MySqlCommand command = new MySqlCommand(query, cnx);
                command.Parameters.AddWithValue("@Usuario", usuario);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        usuarioEncontrado = reader.GetString("Usuario");
                        string passwordFromDB = reader.GetString("Password");
                        contraseñaEncontrada = encrypt.Decrypt(passwordFromDB);
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al buscar usuario: " + ex.Message);
            }

            return (usuarioEncontrado, contraseñaEncontrada);
        }

    }
}
