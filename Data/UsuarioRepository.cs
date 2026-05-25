using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TPPreenchedor.Data.Models;

namespace TPPreenchedor.Data
{
    public class UsuarioRepository : IDisposable
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRepository()
        {
            _context = new ApplicationDbContext();
            _context.Database.EnsureCreated();
            ImportarBancoLegadoSeNecessario();
        }

        public void AdicionarUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
        }

        public List<Usuario> ObterTodosUsuarios()
        {
            return _context.Usuarios.ToList();
        }

        public Dictionary<string, string> ObterSenhasPorLogin(IEnumerable<string> logins)
        {
            var loginsLista = logins.ToList();

            return _context.Usuarios
                .Where(usuario => loginsLista.Contains(usuario.Login))
                .ToDictionary(usuario => usuario.Login, usuario => usuario.Senha);
        }

        public Usuario ObterUsuarioPorId(int id)
        {
            return _context.Usuarios.FirstOrDefault(u => u.Id == id);
        }

        public Usuario ObterUsuarioPorLogin(string login)
        {
            return _context.Usuarios.FirstOrDefault(u => u.Login == login);
        }

        public void AtualizarUsuario(Usuario usuario)
        {
            var usuarioExistente = usuario.Id > 0
                ? _context.Usuarios.FirstOrDefault(u => u.Id == usuario.Id)
                : _context.Usuarios.FirstOrDefault(u => u.Login == usuario.Login);

            if (usuarioExistente == null)
            {
                _context.Usuarios.Add(new Usuario
                {
                    Login = usuario.Login,
                    Senha = usuario.Senha ?? string.Empty
                });
            }
            else
            {
                usuarioExistente.Login = usuario.Login;
                usuarioExistente.Senha = usuario.Senha ?? string.Empty;
            }

            _context.SaveChanges();
        }

        public void SalvarUsuarios(IEnumerable<Usuario> usuarios)
        {
            foreach (var usuario in usuarios)
            {
                var usuarioExistente = _context.Usuarios.FirstOrDefault(u => u.Login == usuario.Login);

                if (usuarioExistente == null)
                {
                    _context.Usuarios.Add(new Usuario
                    {
                        Login = usuario.Login,
                        Senha = usuario.Senha ?? string.Empty
                    });
                }
                else
                {
                    usuarioExistente.Senha = usuario.Senha ?? string.Empty;
                }
            }

            _context.SaveChanges();
        }

        public void RemoverUsuario(int id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                _context.SaveChanges();
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private void ImportarBancoLegadoSeNecessario()
        {
            if (_context.Usuarios.Any())
            {
                return;
            }

            foreach (var caminhoBanco in ObterCaminhosBancoLegado())
            {
                if (!File.Exists(caminhoBanco) || CaminhosIguais(caminhoBanco, ApplicationDbContext.DatabasePath))
                {
                    continue;
                }

                if (ImportarBancoLegado(caminhoBanco))
                {
                    return;
                }
            }
        }

        private bool ImportarBancoLegado(string caminhoBanco)
        {
            try
            {
                var usuarios = new List<Usuario>();
                var connectionString = new SqliteConnectionStringBuilder
                {
                    DataSource = caminhoBanco
                }.ToString();

                using (var connection = new SqliteConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = "SELECT Login, Senha FROM Usuarios";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuarios.Add(new Usuario
                            {
                                Login = reader.GetString(0),
                                Senha = reader.GetString(1)
                            });
                        }
                    }
                }

                if (!usuarios.Any())
                {
                    return false;
                }

                _context.Usuarios.AddRange(usuarios);
                _context.SaveChanges();
                return true;
            }
            catch (SqliteException)
            {
                return false;
            }
        }

        private IEnumerable<string> ObterCaminhosBancoLegado()
        {
            var caminhos = new List<string>();
            AdicionarCaminhoBancoLegado(caminhos, AppDomain.CurrentDomain.BaseDirectory);
            AdicionarCaminhoBancoLegado(caminhos, Environment.CurrentDirectory);

            var diretorio = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

            for (var i = 0; i < 4 && diretorio != null; i++)
            {
                AdicionarCaminhoBancoLegado(caminhos, diretorio.FullName);
                diretorio = diretorio.Parent;
            }

            return caminhos.Distinct(StringComparer.OrdinalIgnoreCase);
        }

        private void AdicionarCaminhoBancoLegado(List<string> caminhos, string diretorio)
        {
            if (!string.IsNullOrWhiteSpace(diretorio))
            {
                caminhos.Add(Path.Combine(diretorio, "TpPreenchedor.db"));
            }
        }

        private bool CaminhosIguais(string caminho1, string caminho2)
        {
            return string.Equals(
                Path.GetFullPath(caminho1),
                Path.GetFullPath(caminho2),
                StringComparison.OrdinalIgnoreCase);
        }
    }
}
