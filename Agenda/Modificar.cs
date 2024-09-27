using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Agenda
{
    public partial class Modificar : Form
    {
        private string connectionString = @"Data Source=C:\Users\fgfed\Desktop\Agenda\Agenda\db\listaContactos.db;Version=3;"; // Ajusta la ruta de tu base de datos

        public Modificar()
        {
            InitializeComponent();
        }

        private void Modificar_Load(object sender, EventArgs e)
        {
            //// Configurar el DataGridView
            //ConfigurarDataGridView();

            //// Cargar los contactos en la grilla
            //CargarContactosEnGrilla();
        }

        private void ConfigurarDataGridView()
        {
            // Crear columnas para la grilla
            dataGridView1.ColumnCount = 4;
            dataGridView1.Columns[0].Name = "Nombre";
            dataGridView1.Columns[1].Name = "Apellido";
            dataGridView1.Columns[2].Name = "Teléfono";
            dataGridView1.Columns[3].Name = "Correo";

            // Añadir columna de botón "Editar"
            DataGridViewButtonColumn editarButton = new DataGridViewButtonColumn();
            editarButton.Name = "Editar";
            editarButton.Text = "Editar";
            editarButton.UseColumnTextForButtonValue = true; // Usar el texto del botón como valor
            dataGridView1.Columns.Add(editarButton);

            // Añadir columna de botón "Eliminar"
            DataGridViewButtonColumn eliminarButton = new DataGridViewButtonColumn();
            eliminarButton.Name = "Eliminar";
            eliminarButton.Text = "Eliminar";
            eliminarButton.UseColumnTextForButtonValue = true; // Usar el texto del botón como valor
            dataGridView1.Columns.Add(eliminarButton);
        }

        private void CargarContactosEnGrilla()
        {
            // Limpiar las filas existentes en el DataGridView
            dataGridView1.Rows.Clear();

            // Conectar a la base de datos SQLite
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Consulta para obtener todos los contactos
                string query = "SELECT Nombre, Apellido, Telefono, Correo FROM Contactos";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        // Agregar filas a la grilla con los datos de la base de datos
                        while (reader.Read())
                        {
                            string nombre = reader["Nombre"].ToString();
                            string apellido = reader["Apellido"].ToString();
                            string telefono = reader["Telefono"].ToString();
                            string correo = reader["Correo"].ToString();

                            // Agregar fila con los datos del contacto
                            dataGridView1.Rows.Add(nombre, apellido, telefono, correo);
                        }
                    }
                }
            }
        }

       



        private void EliminarContacto(string correo)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Consulta para eliminar un contacto por correo
                string query = "DELETE FROM Contactos WHERE Correo = @Correo";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Correo", correo);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show($"El contacto con correo {correo} ha sido eliminado.");
                    }
                    else
                    {
                        MessageBox.Show("No se encontró el contacto para eliminar.");
                    }
                }
            }

            // Recargar la grilla después de eliminar el contacto
            CargarContactosEnGrilla();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            // Crear una nueva instancia del formulario Principal
            Form1 frmPrincipal = new Form1();

            // Ocultar el formulario actual
            this.Hide();

            // Mostrar el formulario
            frmPrincipal.Show();

        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            // Si se hizo clic en el botón "Eliminar"
            if (e.ColumnIndex == dataGridView1.Columns["Eliminar"].Index && e.RowIndex >= 0)
            {
                // Obtener el correo del contacto a eliminar
                string correo = dataGridView1.Rows[e.RowIndex].Cells["Correo"].Value.ToString();

                // Mensaje de advertencia para confirmar la eliminación
                var result = MessageBox.Show($"¿Estás seguro de que deseas eliminar el contacto con correo {correo}?",
                                              "Advertencia",
                                              MessageBoxButtons.YesNo,
                                              MessageBoxIcon.Warning);

                // Si el usuario confirma la eliminación
                if (result == DialogResult.Yes)
                {
                    // Llamar al método para eliminar el contacto
                    EliminarContacto(correo);
                }
            }

            // Si se hizo clic en el botón "Editar"
            if (e.ColumnIndex == dataGridView1.Columns["Editar"].Index && e.RowIndex >= 0)
            {
                // Obtener el nombre del contacto a editar
                string nombre = dataGridView1.Rows[e.RowIndex].Cells["Nombre"].Value.ToString();
                MessageBox.Show($"Se editó el contacto: {nombre}");
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            // Crear una nueva instancia del formulario Principal
            Nuevo frmNuevo = new Nuevo();

            // Ocultar el formulario actual
            this.Hide();

            // Mostrar el formulario
            frmNuevo.Show();
        }

        private void Modificar_Shown(object sender, EventArgs e)
        {
            // Configurar el DataGridView
            ConfigurarDataGridView();

            // Cargar los contactos en la grilla
            CargarContactosEnGrilla();
        }
    }
}
