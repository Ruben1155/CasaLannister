using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CasaLannister.Models;

namespace CasaLannister.Data
{
    public class ProductoData
    {
        private readonly DbConnection _dbConnection;

        public ProductoData(DbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public List<Producto> ObtenerProductos()
        {
            var productos = new List<Producto>();

            using (var connection = _dbConnection.GetConnection())
            {
                var command = new SqlCommand(@"
                    SELECT p.Id, p.Nombre, p.Descripcion, p.Precio, p.IdCategoria, c.Nombre AS CategoriaNombre
                    FROM Productos p
                    INNER JOIN Categorias c ON p.IdCategoria = c.Id", connection);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var producto = new Producto
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nombre = reader["Nombre"].ToString(),
                            Descripcion = reader["Descripcion"].ToString(),
                            Precio = Convert.ToDecimal(reader["Precio"]),
                            IdCategoria = Convert.ToInt32(reader["IdCategoria"]),
                            Categoria = new Categoria
                            {
                                Id = Convert.ToInt32(reader["IdCategoria"]),
                                Nombre = reader["CategoriaNombre"].ToString()
                            }
                        };

                        productos.Add(producto);
                    }
                }
            }

            return productos;
        }

        public List<Producto> ObtenerProductosPorCategoria(int categoriaId)
        {
            var productos = new List<Producto>();

            using (var connection = _dbConnection.GetConnection())
            {
                var command = new SqlCommand("EXEC sp_ObtenerProductosPorCategoria @IdCategoria", connection);
                command.Parameters.AddWithValue("@IdCategoria", categoriaId);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        productos.Add(new Producto
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nombre = reader["Nombre"].ToString(),
                            Descripcion = reader["Descripcion"].ToString(),
                            Precio = Convert.ToDecimal(reader["Precio"]),
                            IdCategoria = categoriaId,
                            Categoria = new Categoria
                            {
                                Id = categoriaId,
                                Nombre = reader["CategoriaNombre"].ToString()
                            }
                        });
                    }
                }
            }

            return productos;
        }

        public Producto ObtenerProductoPorId(int id)
        {
            using (var connection = _dbConnection.GetConnection())
            {
                var command = new SqlCommand(@"
                    SELECT p.Id, p.Nombre, p.Descripcion, p.Precio, p.IdCategoria, c.Nombre as CategoriaNombre
                    FROM Productos p
                    INNER JOIN Categorias c ON p.IdCategoria = c.Id
                    WHERE p.Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Producto
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nombre = reader["Nombre"].ToString(),
                            Descripcion = reader["Descripcion"].ToString(),
                            Precio = Convert.ToDecimal(reader["Precio"]),
                            IdCategoria = Convert.ToInt32(reader["IdCategoria"]),
                            Categoria = new Categoria
                            {
                                Id = Convert.ToInt32(reader["IdCategoria"]),
                                Nombre = reader["CategoriaNombre"].ToString()
                            }
                        };
                    }
                }
            }

            return null;
        }

        public void AgregarProducto(Producto producto)
        {
            using (var connection = _dbConnection.GetConnection())
            {
                var command = new SqlCommand("EXEC sp_InsertarProducto @Nombre, @Descripcion, @Precio, @IdCategoria, @Id OUTPUT", connection);

                command.Parameters.AddWithValue("@Nombre", producto.Nombre);
                command.Parameters.AddWithValue("@Descripcion", producto.Descripcion ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Precio", producto.Precio);
                command.Parameters.AddWithValue("@IdCategoria", producto.IdCategoria);

                SqlParameter idParam = new SqlParameter("@Id", SqlDbType.Int);
                idParam.Direction = ParameterDirection.Output;
                command.Parameters.Add(idParam);

                connection.Open();
                command.ExecuteNonQuery();

                // Obtenemos el ID generado
                producto.Id = Convert.ToInt32(idParam.Value);
            }
        }

        public void ActualizarProducto(Producto producto)
        {
            using (var connection = _dbConnection.GetConnection())
            {
                var command = new SqlCommand("EXEC sp_ActualizarProducto @Id, @Nombre, @Descripcion, @Precio, @IdCategoria", connection);

                command.Parameters.AddWithValue("@Id", producto.Id);
                command.Parameters.AddWithValue("@Nombre", producto.Nombre);
                command.Parameters.AddWithValue("@Descripcion", producto.Descripcion ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Precio", producto.Precio);
                command.Parameters.AddWithValue("@IdCategoria", producto.IdCategoria);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void EliminarProducto(int id)
        {
            using (var connection = _dbConnection.GetConnection())
            {
                var command = new SqlCommand("EXEC sp_EliminarProducto @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}