using Newtonsoft.Json;
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
using SistemaGastos.Models;

namespace SistemaGastos.Views
{
    public partial class FrmUsuario : Form
    {
        public FrmUsuario()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Puedes eliminar si no se usa
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            string correo = txtCorreo.Text.Trim();
            string contrasena = txtContrasena.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(contrasena))
            {
                MessageBox.Show("Por favor complete todos los campos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string rutaArchivo = Path.Combine(Application.StartupPath, @"..\..\Data\usuarios.json");

            List<Usuario> listaUsuarios = new List<Usuario>();

            // Leer si el archivo ya existe
            if (File.Exists(rutaArchivo))
            {
                string jsonExistente = File.ReadAllText(rutaArchivo);
                listaUsuarios = JsonConvert.DeserializeObject<List<Usuario>>(jsonExistente) ?? new List<Usuario>();
            }

            // Crear nuevo usuario con ID incremental
            int nuevoId = (listaUsuarios.Count > 0) ? listaUsuarios.Max(u => u.Id) + 1 : 1;
            Usuario nuevoUsuario = new Usuario(nuevoId, nombre, correo, contrasena);

            listaUsuarios.Add(nuevoUsuario);

            // Guardar la lista actualizada
            string jsonNuevo = JsonConvert.SerializeObject(listaUsuarios, Formatting.Indented);
            File.WriteAllText(rutaArchivo, jsonNuevo);

            MessageBox.Show($"Usuario registrado:\n\nNombre: {nuevoUsuario.Nombre}\nCorreo: {nuevoUsuario.Correo}",
                "Registro exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Limpiar campos
            txtNombre.Text = "";
            txtCorreo.Text = "";
            txtContrasena.Text = "";

            // Actualizar DataGridView
            CargarUsuarios();
        }

        private void CargarUsuarios()
        {
            string rutaArchivo = Path.Combine(Application.StartupPath, @"..\..\Data\usuarios.json");

            if (File.Exists(rutaArchivo))
            {
                string json = File.ReadAllText(rutaArchivo);
                List<Usuario> usuarios = JsonConvert.DeserializeObject<List<Usuario>>(json);
                dgvUsuarios.DataSource = usuarios;
            }
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            string correo = txtCorreo.Text.Trim();
            string contrasena = txtContrasena.Text.Trim();

            if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(contrasena))
            {
                MessageBox.Show("Por favor, ingrese ambos campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string rutaArchivo = Path.Combine(Application.StartupPath, @"..\..\Data\usuarios.json");
            if (!File.Exists(rutaArchivo))
            {
                MessageBox.Show("No se encontró el archivo de usuarios.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string json = File.ReadAllText(rutaArchivo);
            List<Usuario> usuarios = JsonConvert.DeserializeObject<List<Usuario>>(json);

            var usuario = usuarios.FirstOrDefault(u => u.Correo == correo && u.Contrasena == contrasena);
            if (usuario != null)
            {
                MessageBox.Show($"Bienvenido, {usuario.Nombre}", "Acceso concedido", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Abre la pantalla principal
                FrmUsuario frm = new FrmUsuario(); // o FrmPrincipal si ya tenés uno
                frm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Correo o contraseña incorrectos.", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}






