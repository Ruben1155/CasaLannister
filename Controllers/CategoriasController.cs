using Microsoft.AspNetCore.Mvc;
using CasaLannister.Models;
using CasaLannister.Data;
using System.Linq;

namespace CasaLannister.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly CategoriaData _categoriaData;
        private readonly ProductoData _productoData;

        public CategoriasController(CategoriaData categoriaData, ProductoData productoData)
        {
            _categoriaData = categoriaData;
            _productoData = productoData;
        }

        // GET: Categorias
        public IActionResult Index()
        {
            var categorias = _categoriaData.ObtenerCategorias();
            return View(categorias);
        }

        // GET: Categorias/Details/5
        public IActionResult Details(int id)
        {
            var categoria = _categoriaData.ObtenerCategoriaPorId(id);
            if (categoria == null)
            {
                return NotFound();
            }

            // Cargar los productos asociados a esta categoría
            categoria.Productos = _productoData.ObtenerProductosPorCategoria(id);
            return View(categoria);
        }

        // GET: Categorias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categorias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _categoriaData.AgregarCategoria(categoria);
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // GET: Categorias/Edit/5
        public IActionResult Edit(int id)
        {
            var categoria = _categoriaData.ObtenerCategoriaPorId(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        // POST: Categorias/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _categoriaData.ActualizarCategoria(categoria);
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // GET: Categorias/Delete/5
        public IActionResult Delete(int id)
        {
            var categoria = _categoriaData.ObtenerCategoriaPorId(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        // POST: Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            bool eliminacionExitosa = _categoriaData.EliminarCategoria(id);

            if (!eliminacionExitosa)
            {
                // Si no se pudo eliminar (tiene productos asociados)
                TempData["ErrorMessage"] = "No se puede eliminar la categoría porque tiene productos asociados.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}