using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Agenda
{
    public partial class Form1 : Form
    {
        private string connectionString = @"Data Source=C:\Users\fgfed\Desktop\Agenda\Agenda\db\listaContactos.db;Version=3;";// Ajusta la cadena de conexión según tu base de datos
                                                                                                                              // Lista para almacenar los contactos
        private List<Contacto> listaContactos = new List<Contacto>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CargarContactosEnTreeView();
        }

        
        //Metodo que carga desde la DB los contactos
        private void CargarContactosEnTreeView()
        {
            // Limpiar el TreeView
            treeViewContactos.Nodes.Clear();
            listaContactos.Clear();  // Limpiar la lista de contactos previamente

            // Conectar a la base de datos SQLite
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Consulta para obtener todos los contactos
                string query = "SELECT Categoria, Nombre, Apellido, Telefono, Correo FROM Contactos ORDER BY Categoria, Nombre";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        // Crear un diccionario para almacenar los contactos por categoría
                        Dictionary<string, TreeNode> categorias = new Dictionary<string, TreeNode>();

                        while (reader.Read())
                        {
                            string categoria = reader["Categoria"].ToString();
                            string nombre = reader["Nombre"].ToString();
                            string apellido = reader["Apellido"].ToString();
                            string telefono = reader["Telefono"].ToString();
                            string correo = reader["Correo"].ToString();

                            // Crear y añadir el contacto a la lista
                            Contacto contacto = new Contacto
                            {
                                Categoria = categoria,
                                Nombre = nombre,
                                Apellido = apellido,
                                Telefono = telefono,
                                Correo = correo
                            };
                            listaContactos.Add(contacto);

                            // Si la categoría no está en el TreeView, agregarla
                            if (!categorias.ContainsKey(categoria))
                            {
                                TreeNode categoriaNode = new TreeNode(categoria);
                                categorias[categoria] = categoriaNode;
                                treeViewContactos.Nodes.Add(categoriaNode);
                            }

                            // Crear un nodo para el contacto con el nombre completo
                            TreeNode contactoNode = new TreeNode($"{nombre} {apellido}");

                            // Añadir subnodos para el teléfono y correo
                            contactoNode.Nodes.Add(new TreeNode($"Teléfono: {telefono}"));
                            contactoNode.Nodes.Add(new TreeNode($"Correo: {correo}"));

                            // Añadir el contacto a la categoría correspondiente
                            categorias[categoria].Nodes.Add(contactoNode);

                            // Actualizar la etiqueta lblResultado con la cantidad de contactos encontrados
                            lblResultado.Text = $"{listaContactos.Count} contactos encontrados.";
                        }
                    }
                }
            }

            
        }
        // Método para buscar contactos por nombre, teléfono o correo
        private List<Contacto> BuscarContactos(string criterio)
        {
            // Convertir el criterio a minúsculas para la comparación
            string criterioLower = criterio.ToLower();

            return listaContactos.Where(c =>
                c.Nombre.ToLower().Contains(criterioLower) ||
                c.Telefono.ToLower().Contains(criterioLower) ||
                c.Correo.ToLower().Contains(criterioLower)).ToList();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string criterio = txtBusqueda.Text;  // Asumiendo que tienes un TextBox para ingresar el criterio de búsqueda
            var resultados = BuscarContactos(criterio);

            if (resultados.Any())
            {
                // Mostrar el conteo de resultados en el Label
                lblResultado.Text = $"{resultados.Count} contactos encontrados.";
                // Llamar al método para cargar los resultados en el TreeView
                CargarContactosEnTreeView(resultados);
            }
            else
            {
                MessageBox.Show("No se encontraron contactos con el criterio especificado.");
                lblResultado.Text = $"{resultados.Count} contactos encontrados.";
            }
        }

        // Método para cargar una lista de contactos en el TreeView
        private void CargarContactosEnTreeView(List<Contacto> contactos)
        {
            treeViewContactos.Nodes.Clear(); // Limpiar el TreeView

            var categorias = contactos.GroupBy(c => c.Categoria);

            foreach (var categoriaGroup in categorias)
            {
                TreeNode categoriaNode = new TreeNode(categoriaGroup.Key);
                treeViewContactos.Nodes.Add(categoriaNode);

                foreach (var contacto in categoriaGroup)
                {
                    TreeNode contactoNode = new TreeNode($"{contacto.Nombre} {contacto.Apellido}");
                    contactoNode.Nodes.Add(new TreeNode($"Teléfono: {contacto.Telefono}"));
                    contactoNode.Nodes.Add(new TreeNode($"Correo: {contacto.Correo}"));
                    categoriaNode.Nodes.Add(contactoNode);
                }
            }
        }



        private void ExportarContactos()
        {
            // Crear un cuadro de diálogo para que el usuario elija el formato
            DialogResult result = MessageBox.Show("¿En qué formato deseas exportar los contactos?\n\nElige 'Sí' para CSV o 'No' para vCard.",
                                                  "Exportar Contactos", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Exportar en formato CSV
                ExportarCSV();
            }
            else if (result == DialogResult.No)
            {
                // Exportar en formato vCard
                ExportarVCard();
            }
        }


        private void ExportarCSV()
        {
            // Crear o asegurar la existencia de la carpeta "Reportes"
            string carpetaReportes = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reportes");
            Directory.CreateDirectory(carpetaReportes);

            // Ruta del archivo en la carpeta "Reportes"
            string filePath = Path.Combine(carpetaReportes, "Contactos.csv");

            using (var writer = new System.IO.StreamWriter(filePath))
            {
                // Escribir encabezados
                writer.WriteLine("Nombre,Apellido,Teléfono,Correo");

                // Escribir los contactos desde la lista
                foreach (var contacto in listaContactos)
                {
                    writer.WriteLine($"{contacto.Nombre},{contacto.Apellido},{contacto.Telefono},{contacto.Correo}");
                }
            }

            MessageBox.Show($"Contactos exportados exitosamente como CSV en: {filePath}");
        }

        private void ExportarVCard()
        {
            // Crear o asegurar la existencia de la carpeta "Reportes"
            string carpetaReportes = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reportes");
            Directory.CreateDirectory(carpetaReportes);

            // Ruta del archivo en la carpeta "Reportes"
            string filePath = Path.Combine(carpetaReportes, "Contactos.vcf");

            using (var writer = new System.IO.StreamWriter(filePath))
            {
                // Escribir los contactos en formato vCard
                foreach (var contacto in listaContactos)
                {
                    writer.WriteLine("BEGIN:VCARD");
                    writer.WriteLine("VERSION:3.0");
                    writer.WriteLine($"FN:{contacto.Nombre} {contacto.Apellido}");
                    writer.WriteLine($"TEL:{contacto.Telefono}");
                    writer.WriteLine($"EMAIL:{contacto.Correo}");
                    writer.WriteLine("END:VCARD");
                }
            }

            MessageBox.Show($"Contactos exportados exitosamente como vCard en: {filePath}");
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            ExportarContactos();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            // Crear una nueva instancia del formulario Modificar
            Modificar frmModificar = new Modificar();

            // Mostrar el formulario Modificar como ventana modal
            frmModificar.Show();
        }
    }
}
