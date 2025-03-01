// Models/Categoria.cs
using LannisterApp.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CasaLannister.Models
{
    public class Categoria
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la categoría es obligatorio")]
        [Display(Name = "Nombre de la Categoría")]
        public string Nombre { get; set; }

        // Propiedad de navegación (para la relación con Productos)
        public virtual ICollection<Producto> Productos { get; set; }
    }
}