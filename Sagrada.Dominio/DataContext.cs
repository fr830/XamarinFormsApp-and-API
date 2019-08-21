using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sagrada.Dominio {

    public class DataContext : DbContext{

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<TipoUsuario> TipoUsuarios { get; set; }

        public DbSet<Medico> Medicos { get; set; }

        public DbSet<Turno> Turnos { get; set; }

        public DbSet<Especialidad> Especialidades { get; set; }

        public DbSet<CobroMedico> CobroMedicos { get; set; }

        public DbSet<Pago> Pagos { get; set; }

        public DbSet<Tarjeta> Tarjetas { get; set; }

        public DbSet<UsuarioAtendido> UsuarioAtendidos { get; set; }

        public DbSet<UsuarioEnEspera> UsuarioEnEsperas { get; set; }

        public DbSet<ObraSocialUsuario> ObraSocialUsuarios { get; set; }

        public DbSet<ObraSocial> ObraSociales { get; set; }

        public DbSet<ObraSocialMedico> ObraSocialMedicos { get; set; }

        public DbSet<Horario> Horarios { get; set; }

        public DbSet<Banco> Bancos { get; set; }

        public DataContext() : base("DefaultConnection") {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            

            /* modelBuilder.Entity<UsuarioAtendido>()
                 .HasOptional(b => b.EspecialidadMedico)
                 .WithMany(a => a.Medicoes)
                 .HasForeignKey(b => b.IdMedico);

             modelBuilder.Entity<UsuarioAtendido>()
                 .HasOptional(b => b.EspecialidadEspecialidad)
                 .WithMany(a => a.Especialidads)
                 .HasForeignKey(b => b.IdEspecialidad); */
        }

    }
}
