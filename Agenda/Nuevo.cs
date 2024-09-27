using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Agenda
{
    public partial class Nuevo : Form
    {
        // Cadena de conexión a la base de datos
        private string connectionString = @"Data Source=C:\Users\fgfed\Desktop\Agenda\Agenda\db\listaContactos.db;Version=3;";
        public Nuevo()
        {
            InitializeComponent();
        }

        private void Nuevo_Load(object sender, EventArgs e)
        {

        }

        private void Nuevo_Shown(object sender, EventArgs e)
        {
            CargarCategorias();
        }



       
        
        
        private bool ValidarTelefono(string telefono)
        {
            // Validar el formato del teléfono (ejemplo simple)
            return Regex.IsMatch(telefono, @"^\+\d{1,3} \d{3} \d{3} \d{4}$");
        }

        private bool ValidarCorreo(string correo)
        {
            // Validar el formato del correo (ejemplo simple)
            return Regex.IsMatch(correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }




        private void CargarCategorias()
        {
            // Conectar a la base de datos SQLite
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Consulta para obtener las categorías únicas
                string query = "SELECT DISTINCT Categoria FROM Contactos";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        // Limpiar el ComboBox antes de cargar las categorías
                        cmbCategoria.Items.Clear();

                        // Agregar cada categoría al ComboBox
                        while (reader.Read())
                        {
                            string categoria = reader["Categoria"].ToString();
                            cmbCategoria.Items.Add(categoria);
                        }
                    }
                }
            }
        }

        private void CrearContacto(string nombre, string apellido, string telefono, string correo, string categoria)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Consulta para insertar un nuevo contacto
                string query = "INSERT INTO Contactos (Nombre, Apellido, Telefono, Correo, Categoria) VALUES (@Nombre, @Apellido, @Telefono, @Correo, @Categoria)";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", nombre);
                    command.Parameters.AddWithValue("@Apellido", apellido);
                    command.Parameters.AddWithValue("@Telefono", telefono);
                    command.Parameters.AddWithValue("@Correo", correo);
                    command.Parameters.AddWithValue("@Categoria", categoria);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Contacto creado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimpiarCampos();
                    }
                    else
                    {
                        MessageBox.Show("Error al crear el contacto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        }

        private void ValidarContacto()
        {
            // Obtener los datos de los TextBox y ComboBox
            string nombre = txtNombre.Text.Trim();
            string apellido = txtApellido.Text.Trim();
            string telefono = txtTelefono.Text.Trim();
            string correo = txtCorreo.Text.Trim();
            string categoria = cmbCategoria.SelectedItem?.ToString();

            // Validaciones
            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("El nombre no puede estar vacío.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(apellido))
            {
                MessageBox.Show("El apellido no puede estar vacío.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(telefono) || !ValidarTelefono(telefono))
            {
                MessageBox.Show("El teléfono no puede estar vacío y debe tener un formato válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(correo) || !ValidarCorreo(correo))
            {
                MessageBox.Show("El correo no puede estar vacío y debe tener un formato válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(categoria))
            {
                MessageBox.Show("Debes seleccionar una categoría.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Crear contacto en la base de datos
            CrearContacto(nombre, apellido, telefono, correo, categoria);
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            ValidarContacto();
        }

        private void btnAgenda_Click(object sender, EventArgs e)
        {
            // Crear una nueva instancia del formulario Principal
            Form1 frmPrincipal = new Form1();

            // Ocultar el formulario actual
            this.Hide();

            // Mostrar el formulario
            frmPrincipal.Show();
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtApellido.Clear();
            txtTelefono.Clear();
            txtCorreo.Clear();
            cmbCategoria.SelectedIndex = -1; // Desmarcar el ComboBox
        }
    }
}
