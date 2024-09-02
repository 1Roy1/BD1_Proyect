using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class VentanaVentas : Form
    {
        private MySqlTransaction _transaction;
        private MySqlConnection _connection;
        private const string logSubFolder = "Bitacora";
        private readonly string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, logSubFolder, "transaction_log.txt");
        private bool transaccionactiva = false;
        private int? idClienteCarrito = null;

        public VentanaVentas()
        {
            InitializeComponent();
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, logSubFolder);
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
        }
        private void RegistrarTransaccion(string mensaje)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(logFilePath, true))
                {
                    sw.WriteLine($"{DateTime.Now}: {mensaje}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al escribir en el archivo de log: " + ex.Message);
            }
        }
        private void limpiarcampos()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
        }


        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void moduloVentasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (transaccionactiva == true)
            {
                try
                {
                    using (var cmd = new MySqlCommand("ROLLBACK", _connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    RegistrarTransaccion("Transacción revertida antes de abrir Ventana.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al hacer rollback: " + ex.Message);
                }
            }
            VentanaInventario abrir = new VentanaInventario();
            abrir.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                textBox1.Text = selectedRow.Cells["ID"].Value.ToString();
                textBox2.Text = selectedRow.Cells["Nombre"].Value.ToString();
                textBox3.Text = selectedRow.Cells["Descripcion"].Value.ToString();
                textBox4.Text = selectedRow.Cells["Existencia"].Value.ToString();
                textBox6.Text = selectedRow.Cells["Precio"].Value.ToString();
                textBox7.Text = selectedRow.Cells["Marca"].Value.ToString();
            }
        }

        private DataTable originalDataTable;

        private void VentanaVentas_Load(object sender, EventArgs e)
        {
            IniciarConexion();
            CargarDatos();
            originalDataTable = ((DataTable)dataGridView1.DataSource).Copy();
        }
        private void IniciarConexion()
        {
            string servidor = "localhost";
            string bd = "proyecto";
            string usuario = "root";
            string password = "root1234";
            string puerto = "3306";
            string cadenaConexion = $"server={servidor};port={puerto};user id={usuario};password={password};database={bd};";
            _connection = new MySqlConnection(cadenaConexion);

            try
            {
                _connection.Open();
                using (var cmd = new MySqlCommand("SET autocommit = 0;", _connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir la conexión: " + ex.Message);
            }
        }

        private void IniciarTransaccion()
        {
            try
            {
                using (var cmd = new MySqlCommand("START TRANSACTION", _connection))
                {
                    cmd.ExecuteNonQuery();
                }
                RegistrarTransaccion("Transacción iniciada.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al iniciar la transacción: " + ex.Message);
            }
        }
        private void CargarDatos()
        {
            try
            {
                if (_connection == null || _connection.State != ConnectionState.Open)
                {
                    IniciarConexion();
                }

                string sqlQuery = "SELECT pro.ID, pro.Nombre, pro.Descripcion, pro.Existencia, pro.Precio, MAX(p.Nombre) AS Marca " +
                  "FROM producto pro " +
                  "LEFT JOIN detalle_compras dc ON pro.ID = dc.Producto_ID " +
                  "LEFT JOIN compras c ON dc.Compras_ID = c.ID " +
                  "LEFT JOIN proveedores p ON c.Proveedores_ID = p.ID " +
                  "GROUP BY pro.ID, pro.Nombre, pro.Descripcion, pro.Existencia, pro.Precio";
                DataTable dataTable = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter(sqlQuery, _connection);
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;

                string sqlQuery2 = "SELECT * FROM venta";
                DataTable dataTable2 = new DataTable();
                MySqlDataAdapter adapter2 = new MySqlDataAdapter(sqlQuery2, _connection);
                adapter2.Fill(dataTable2);
                dataGridView2.DataSource = dataTable2;

                string sqlQuery3 = "SELECT id, nombre, apellido FROM clientes";
                DataTable dataTable3 = new DataTable();
                MySqlDataAdapter adapter3 = new MySqlDataAdapter(sqlQuery3, _connection);
                adapter3.Fill(dataTable3);
                dataGridView3.DataSource = dataTable3;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
      

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void menuPrincipalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (transaccionactiva == true)
            {
                try
                {
                    using (var cmd = new MySqlCommand("ROLLBACK", _connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    RegistrarTransaccion("Transacción revertida antes de abrir Ventana.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al hacer rollback: " + ex.Message);
                }
            }
            Form1 abririmenu = new Form1();
            abririmenu.Show();
            this.Hide();
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox1_TextChanged_1(object sender, EventArgs e)
        {
            string filtro = toolStripTextBox1.Text.Trim();

            if (string.IsNullOrEmpty(filtro))
            {
                // Restaurar la fuente de datos original
                dataGridView1.DataSource = originalDataTable;
            }
            else
            {
                // Filtrar las filas basadas en el nombre
                DataTable filteredDataTable = originalDataTable.Clone();
                foreach (DataRow row in originalDataTable.Rows)
                {
                    if (row["Nombre"].ToString().Contains(filtro))
                    {
                        filteredDataTable.ImportRow(row);
                    }
                }

                // Asignar la nueva DataTable al DataGridView
                dataGridView1.DataSource = filteredDataTable;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            try
            {
                if (!int.TryParse(textBox8.Text.Trim(), out int idcliente))
                {
                    MessageBox.Show("Contenido de textBox8: " + textBox8.Text, "Debugging Info");
                    MessageBox.Show("Por favor, ingrese un ID de cliente válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (idClienteCarrito != idcliente)
                {
                    MessageBox.Show("El cliente seleccionado no coincide con el cliente del carrito. Debe continuar con el mismo cliente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                float TotalFinal = 0;
                IniciarTransaccion();  // Iniciar la transacción aquí

                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (row.IsNewRow) continue;

                    int idProducto = Convert.ToInt32(row.Cells["idventa"].Value);
                    int cantidadVendida = Convert.ToInt32(row.Cells["existencias"].Value);
                    float total = Convert.ToSingle(row.Cells["total"].Value);
                    TotalFinal += total;

                    // Verificar la existencia del producto
                    string sqlQuery2 = "SELECT Existencia FROM producto WHERE ID = @ID LIMIT 1";
                    MySqlCommand cmd2 = new MySqlCommand(sqlQuery2, _connection, _transaction);
                    cmd2.Parameters.AddWithValue("@ID", idProducto);
                    object resultado = cmd2.ExecuteScalar();

                    if (resultado == null || resultado == DBNull.Value)
                    {
                        MessageBox.Show("Producto no encontrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw new Exception("Producto no encontrado");
                    }

                    int viejoValor = Convert.ToInt32(resultado);
                    int valorFinal = viejoValor - cantidadVendida;

                    if (valorFinal < 0)
                    {
                        MessageBox.Show("No hay suficientes existencias del producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw new Exception("No hay suficientes existencias del producto");
                    }

                    // Actualizar la existencia del producto
                    string sqlQuery3 = "UPDATE producto SET Existencia = @NuevoValor WHERE ID = @ID";
                    MySqlCommand cmd3 = new MySqlCommand(sqlQuery3, _connection, _transaction);
                    cmd3.Parameters.AddWithValue("@NuevoValor", valorFinal);
                    cmd3.Parameters.AddWithValue("@ID", idProducto);
                    cmd3.ExecuteNonQuery();

                    // Eliminar el producto del carrito
                    string sqlQuery4 = "DELETE FROM venta WHERE idventa = @ID";
                    MySqlCommand cmd4 = new MySqlCommand(sqlQuery4, _connection, _transaction);
                    cmd4.Parameters.AddWithValue("@ID", idProducto);
                    cmd4.ExecuteNonQuery();
                }

                // Insertar la venta en la tabla 'ventas'
                string sqlQuery5 = "INSERT INTO ventas(total, clientes_id) VALUES(@total, @cliente_id)";
                MySqlCommand cmd5 = new MySqlCommand(sqlQuery5, _connection, _transaction);
                cmd5.Parameters.AddWithValue("@total", TotalFinal);
                cmd5.Parameters.AddWithValue("@cliente_id", idcliente);
                int rowsAffected3 = cmd5.ExecuteNonQuery();

                if (rowsAffected3 > 0)
                {
                    using (var cmd = new MySqlCommand("COMMIT", _connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Compra realizada exitosamente!");
                    RegistrarTransaccion("Transacción comiteada.");
                    limpiarcampos();
                    idClienteCarrito = null;
                }
                else
                {

                    using (var cmd = new MySqlCommand("ROLLBACK", _connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    RegistrarTransaccion("Transacción revertida por error: ");
                    limpiarcampos();
                    CargarDatos();
                    throw new Exception("Error al insertar en la tabla ventas");
                    

                }
            }
            catch (Exception ex)
            {
                if (_connection != null && _connection.State == ConnectionState.Open)
                {
                    using (var cmd = new MySqlCommand("ROLLBACK", _connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    RegistrarTransaccion("Transacción revertida por error: " + ex.Message);
                    limpiarcampos();
                    CargarDatos();  
                }
                MessageBox.Show("Error: " + ex.Message);
            }

            CargarDatos();
        }

        private void proveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (transaccionactiva == true)
            {
                try
                {
                    using (var cmd = new MySqlCommand("ROLLBACK", _connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    RegistrarTransaccion("Transacción revertida antes de abrir Ventana.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al hacer rollback: " + ex.Message);
                }
            }
            Proveedores abrir = new Proveedores();
            abrir.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                IniciarTransaccion();
                transaccionactiva = true;
                int id = Convert.ToInt32(textBox1.Text);
                float precio = float.Parse(textBox6.Text);
                int cantidadVenta = int.Parse(textBox5.Text);
                int existenciasActuales = int.Parse(textBox4.Text);

                int idClienteActual = Convert.ToInt32(textBox8.Text);

                if (idClienteCarrito == null)
                {
                    idClienteCarrito = idClienteActual;
                }
                else if (idClienteCarrito != idClienteActual)
                {
                    MessageBox.Show("Debe continuar con el mismo cliente antes de agregar productos de otro cliente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (existenciasActuales <= 0)
                {
                    MessageBox.Show("Se han agotado todas las existencias de este producto.");
                }
                else if (existenciasActuales < cantidadVenta)
                {
                    MessageBox.Show("No se tienen las suficientes existencias de este producto.");
                    RegistrarTransaccion("Transacción revertida");
                }
                else
                {
                    string sqlQuery = $"INSERT INTO venta(idventa, nombre, existencias, precio, marca, total) " +
                                      $"VALUES (@idventa, @nombre, @existencias, @precio, @marca, @total)";

                    MySqlCommand cmd = new MySqlCommand(sqlQuery, _connection, _transaction);
                    cmd.Parameters.AddWithValue("@idventa", id);
                    cmd.Parameters.AddWithValue("@nombre", textBox2.Text);
                    cmd.Parameters.AddWithValue("@existencias", cantidadVenta);
                    cmd.Parameters.AddWithValue("@precio", precio);
                    cmd.Parameters.AddWithValue("@marca", textBox7.Text);
                    cmd.Parameters.AddWithValue("@total", (precio * cantidadVenta));

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Se agregaron los productos al carrito exitosamente!");
                        CargarDatosCarrito();
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (Convert.ToInt32(row.Cells["ID"].Value) == id)
                            {
                                int nuevaExistencia = existenciasActuales - cantidadVenta;
                                row.Cells["Existencia"].Value = nuevaExistencia;
                                break;
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("No se encontró el registro con el ID proporcionado.");
                    }
                }
            }
            catch (Exception ex)
            {
                using (var cmd = new MySqlCommand("ROLLBACK", _connection))
                {
                    cmd.ExecuteNonQuery();
                }
                RegistrarTransaccion("Transacción revertida por error: " + ex.Message);
                MessageBox.Show("Error: " + ex.Message);
                limpiarcampos();
                CargarDatos();
            }


        }
        private void CargarDatosCarrito()
        {
            // Cargar los datos actualizados del carrito en el DataGridView2
            try
            {
                string sqlQuery2 = "SELECT * FROM venta";
                DataTable dataTable2 = new DataTable();
                MySqlDataAdapter adapter2 = new MySqlDataAdapter(sqlQuery2, _connection);
                adapter2.Fill(dataTable2);
                dataGridView2.DataSource = dataTable2;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos del carrito: " + ex.Message);
            }
        }


        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comprasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (transaccionactiva == true)
            {
                try
                {
                    using (var cmd = new MySqlCommand("ROLLBACK", _connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    RegistrarTransaccion("Transacción revertida antes de abrir Ventana.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al hacer rollback: " + ex.Message);
                }
            }

            NuevoProducto abrir = new NuevoProducto();
            abrir.Show();
            this.Hide();
        }

        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (transaccionactiva == true)
            {
                try
                {
                    using (var cmd = new MySqlCommand("ROLLBACK", _connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    RegistrarTransaccion("Transacción revertida antes de abrir Ventana.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al hacer rollback: " + ex.Message);
                }
            }
            Form4 a = new Form4();
            a.Show();
            this.Hide();
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView3_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView3.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView3.SelectedRows[0];

                textBox8.Text = selectedRow.Cells["ID"].Value.ToString();
                textBox9.Text = selectedRow.Cells["Nombre"].Value.ToString();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // Revertir cualquier transacción activa
                using (var cmdRollback = new MySqlCommand("ROLLBACK", _connection))
                {
                    cmdRollback.ExecuteNonQuery();
                }
                RegistrarTransaccion("Transacción revertida");
                MessageBox.Show("Venta devuelta");

                // Iniciar una nueva transacción
                using (var cmdStartTransaction = new MySqlCommand("START TRANSACTION", _connection))
                {
                    cmdStartTransaction.ExecuteNonQuery();
                }

                try
                {
                    using (var cmdDelete = new MySqlCommand("DELETE FROM venta", _connection))
                    {
                        cmdDelete.ExecuteNonQuery();
                    }
                    using (var cmdCommit = new MySqlCommand("COMMIT", _connection))
                    {
                        cmdCommit.ExecuteNonQuery();
                    }

                    RegistrarTransaccion("Todos los registros de venta eliminados.");
                }
                catch (Exception ex)
                {
                    // Revertir si algo falla al eliminar registros
                    using (var cmdRollbackDelete = new MySqlCommand("ROLLBACK", _connection))
                    {
                        cmdRollbackDelete.ExecuteNonQuery();
                    }
                    RegistrarTransaccion("Error al eliminar registros de venta: " + ex.Message);
                    MessageBox.Show("Error al eliminar registros: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores en el ROLLBACK inicial
                using (var cmd = new MySqlCommand("ROLLBACK", _connection))
                {
                    cmd.ExecuteNonQuery();
                }
                RegistrarTransaccion("Transacción revertida por error: " + ex.Message);
                MessageBox.Show("Error: " + ex.Message);
            }

            limpiarcampos();
            CargarDatos();
        }
    }
}
