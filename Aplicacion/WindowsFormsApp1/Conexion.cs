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
        MySqlConnection cnx = new MySqlConnection();
        static string servidor = "127.0.0.3";
        static string bd= "catalog";
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

    }
}
