// Models/Producto.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CasaLannister.Models
{
    public class Producto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio")]
        [Display(Name = "Nombre del Producto")]
        public string Nombre { get; set; }

        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que 0")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Precio { get; set; }

        [Display(Name = "Categoría")]
        public int IdCategoria { get; set; }

        // Propiedad de navegación
        [ForeignKey("IdCategoria")]
        public virtual Categoria Categoria { get; set; }
    }
}