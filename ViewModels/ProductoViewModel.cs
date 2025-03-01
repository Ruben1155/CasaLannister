// ViewModels/ProductoViewModel.cs
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using CasaLannister.Models;

namespace CasaLannister.ViewModels
{
    public class ProductoViewModel
    {
        public Producto Producto { get; set; }
        public IEnumerable<SelectListItem> Categorias { get; set; }
    }
}