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
        static string servidor = "192.168.106.167";
        static string bd= "proyecto";
        static string usuario= "rootdos";
        static string password= "root";
        static string puerto= "3306";
        string cadenaConexion = "server="+servidor+";"+"port="+puerto+";"+"user id="+usuario+";"+"password="+password+";"+"database="+bd+";";
        public MySqlConnection establecerconexion() {
            try {
                cnx.ConnectionString = cadenaConexion;
                cnx.Open();
                

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

                command.ExecuteNonQuery();
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
        public bool BuscarUsuario2(string usuario)
        {
            bool usuarioEncontrado = false;

            try
            {
                // Abrir la conexión a la base de datos
                establecerconexion();

                // Consulta SQL para buscar el usuario por su nombre de usuario
                string query = "SELECT COUNT(*) FROM usuarios WHERE Usuario = @Usuario";
                MySqlCommand command = new MySqlCommand(query, cnx);
                command.Parameters.AddWithValue("@Usuario", usuario);

                // Ejecutar la consulta y obtener el resultado
                int count = Convert.ToInt32(command.ExecuteScalar());

                // Verificar si se encontró el usuario
                if (count > 0)
                {
                    usuarioEncontrado = true;
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al buscar usuario: " + ex.Message);
            }
            finally
            {
                // Cerrar la conexión a la base de datos
                cnx.Close();
            }

            return usuarioEncontrado;
        }
        public bool ActualizarContrasenaUsuario(string usuario, string nuevaContraseña)
        {
            try
            {
                // Abrir la conexión a la base de datos
                establecerconexion();

                // Cifrar la nueva contraseña
                string contraseñaCifrada = encrypt.Encrypt(nuevaContraseña);

                // Actualizar la contraseña en la base de datos
                string query = "UPDATE usuarios SET Password = @Password WHERE Usuario = @Usuario";
                MySqlCommand command = new MySqlCommand(query, cnx);
                command.Parameters.AddWithValue("@Password", contraseñaCifrada);
                command.Parameters.AddWithValue("@Usuario", usuario);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return true; // Contraseña actualizada con éxito
                }
                else
                {
                    return false; // No se pudo actualizar la contraseña
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al actualizar contraseña: " + ex.Message);
                return false; // Error al actualizar la contraseña
            }
            finally
            {
                // Cerrar la conexión a la base de datos
                cnx.Close();
            }
        }

    }


}
