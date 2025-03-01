using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CasaLannister.Models;
using CasaLannister.ViewModels;
using CasaLannister.Data;

namespace CasaLannister.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ProductoData _productoData;
        private readonly CategoriaData _categoriaData;

        public ProductosController(ProductoData productoData, CategoriaData categoriaData)
        {
            _productoData = productoData;
            _categoriaData = categoriaData;
        }

        // GET: Productos
        public IActionResult Index()
        {
            var productos = _productoData.ObtenerProductos();
            return View(productos);
        }

        // GET: Productos/Details/5
        public IActionResult Details(int id)
        {
            var producto = _productoData.ObtenerProductoPorId(id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Productos/Create
        public IActionResult Create()
        {
            var viewModel = new ProductoViewModel
            {
                Producto = new Producto(),
                Categorias = _categoriaData.ObtenerCategorias().Select(c =>
                    new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Nombre
                    })
            };

            return View(viewModel);
        }

        // POST: Productos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductoViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Verificar que la categoría existe
                    var categoria = _categoriaData.ObtenerCategoriaPorId(viewModel.Producto.IdCategoria);
                    if (categoria == null)
                    {
                        ModelState.AddModelError("Producto.IdCategoria", "La categoría seleccionada no existe.");
                        PrepararCategorias(viewModel);
                        return View(viewModel);
                    }

                    _productoData.AgregarProducto(viewModel.Producto);
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError("", "Error al crear el producto: " + ex.Message);
                }
            }

            PrepararCategorias(viewModel);
            return View(viewModel);
        }

        // GET: Productos/Edit/5
        public IActionResult Edit(int id)
        {
            var producto = _productoData.ObtenerProductoPorId(id);
            if (producto == null)
            {
                return NotFound();
            }

            var viewModel = new ProductoViewModel
            {
                Producto = producto,
                Categorias = _categoriaData.ObtenerCategorias().Select(c =>
                    new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Nombre,
                        Selected = c.Id == producto.IdCategoria
                    })
            };

            return View(viewModel);
        }

        // POST: Productos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ProductoViewModel viewModel)
        {
            if (id != viewModel.Producto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Verificar que la categoría existe
                    var categoria = _categoriaData.ObtenerCategoriaPorId(viewModel.Producto.IdCategoria);
                    if (categoria == null)
                    {
                        ModelState.AddModelError("Producto.IdCategoria", "La categoría seleccionada no existe.");
                        PrepararCategorias(viewModel);
                        return View(viewModel);
                    }

                    _productoData.ActualizarProducto(viewModel.Producto);
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError("", "Error al actualizar el producto: " + ex.Message);
                }
            }

            PrepararCategorias(viewModel);
            return View(viewModel);
        }

        // GET: Productos/Delete/5
        public IActionResult Delete(int id)
        {
            var producto = _productoData.ObtenerProductoPorId(id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _productoData.EliminarProducto(id);
            return RedirectToAction(nameof(Index));
        }

        // Método auxiliar para preparar las categorías en el ViewModel
        private void PrepararCategorias(ProductoViewModel viewModel)
        {
            viewModel.Categorias = _categoriaData.ObtenerCategorias().Select(c =>
                new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Nombre,
                    Selected = viewModel.Producto != null && c.Id == viewModel.Producto.IdCategoria
                });
        }
    }
}