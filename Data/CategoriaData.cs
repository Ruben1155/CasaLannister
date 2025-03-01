using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CasaLannister.Models;

namespace CasaLannister.Data
{
    public class CategoriaData
    {
        private readonly DbConnection _dbConnection;

        public CategoriaData(DbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public List<Categoria> ObtenerCategorias()
        {
            var categorias = new List<Categoria>();

            using (var connection = _dbConnection.GetConnection())
            {
                var command = new SqlCommand("SELECT Id, Nombre FROM Categorias", connection);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categorias.Add(new Categoria
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nombre = reader["Nombre"].ToString()
                        });
                    }
                }
            }

            return categorias;
        }

        public Categoria ObtenerCategoriaPorId(int id)
        {
            using (var connection = _dbConnection.GetConnection())
            {
                var command = new SqlCommand("SELECT Id, Nombre FROM Categorias WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Categoria
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nombre = reader["Nombre"].ToString()
                        };
                    }
                }
            }

            return null;
        }

        public void AgregarCategoria(Categoria categoria)
        {
            using (var connection = _dbConnection.GetConnection())
            {
                var command = new SqlCommand("INSERT INTO Categorias (Nombre) VALUES (@Nombre); SELECT SCOPE_IDENTITY();", connection);
                command.Parameters.AddWithValue("@Nombre", categoria.Nombre);
                connection.Open();

                // Obtenemos el ID generado
                categoria.Id = Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public void ActualizarCategoria(Categoria categoria)
        {
            using (var connection = _dbConnection.GetConnection())
            {
                var command = new SqlCommand("UPDATE Categorias SET Nombre = @Nombre WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", categoria.Id);
                command.Parameters.AddWithValue("@Nombre", categoria.Nombre);
                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        public bool EliminarCategoria(int id)
        {
            // Primero verificamos si hay productos asociados
            if (TieneProductosAsociados(id))
                return false;

            using (var connection = _dbConnection.GetConnection())
            {
                var command = new SqlCommand("DELETE FROM Categorias WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();

                command.ExecuteNonQuery();
                return true;
            }
        }

        public bool TieneProductosAsociados(int categoriaId)
        {
            using (var connection = _dbConnection.GetConnection())
            {
                var command = new SqlCommand("SELECT COUNT(*) FROM Productos WHERE IdCategoria = @IdCategoria", connection);
                command.Parameters.AddWithValue("@IdCategoria", categoriaId);
                connection.Open();

                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }
    }
}