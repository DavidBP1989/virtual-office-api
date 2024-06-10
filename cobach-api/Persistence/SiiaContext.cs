using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using cobach_api.Persistence.Models;

namespace cobach_api.Persistence;

public partial class SiiaContext : DbContext
{
    public SiiaContext()
    {
    }

    public SiiaContext(DbContextOptions<SiiaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AutorizacionSolicitude> AutorizacionSolicitudes { get; set; }

    public virtual DbSet<CamposDisciplinare> CamposDisciplinares { get; set; }

    public virtual DbSet<CatalogoPermisosLaborale> CatalogoPermisosLaborales { get; set; }

    public virtual DbSet<CatalogoPlazasAdministrativa> CatalogoPlazasAdministrativas { get; set; }

    public virtual DbSet<CentrosDeTrabajo> CentrosDeTrabajos { get; set; }

    public virtual DbSet<CmcatalogoProyecto> CmcatalogoProyectos { get; set; }

    public virtual DbSet<CorteTiempo> CorteTiempos { get; set; }

    public virtual DbSet<Documento> Documentos { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<EntidadFederativa> EntidadFederativas { get; set; }

    public virtual DbSet<EstadoCivil> EstadoCivils { get; set; }

    public virtual DbSet<Faacumulado> Faacumulados { get; set; }

    public virtual DbSet<Fabeneficiario> Fabeneficiarios { get; set; }

    public virtual DbSet<FacuentasBancaria> FacuentasBancarias { get; set; }

    public virtual DbSet<Faprestamo> Faprestamos { get; set; }

    public virtual DbSet<Faregistro> Faregistros { get; set; }

    public virtual DbSet<Fatransaccione> Fatransacciones { get; set; }

    public virtual DbSet<InformacionLaboral> InformacionLaborals { get; set; }

    public virtual DbSet<Localidad> Localidads { get; set; }

    public virtual DbSet<Nacionalidad> Nacionalidads { get; set; }

    public virtual DbSet<NivelEstudio> NivelEstudios { get; set; }

    public virtual DbSet<PermisoEconomico> PermisoEconomicos { get; set; }

    public virtual DbSet<TipoSangre> TipoSangres { get; set; }

    public virtual DbSet<TiposDocumento> TiposDocumentos { get; set; }

    public virtual DbSet<TurnosxCentrosDeTrabajo> TurnosxCentrosDeTrabajos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:siia");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Modern_Spanish_CI_AI");

        modelBuilder.Entity<AutorizacionSolicitude>(entity =>
        {
            entity.HasKey(e => e.IdProyecto);

            entity.Property(e => e.IdProyecto)
                .ValueGeneratedNever()
                .HasColumnName("idProyecto");
            entity.Property(e => e.Autoriza1)
                .HasMaxLength(128)
                .HasDefaultValueSql("(newid())");
            entity.Property(e => e.Autoriza2)
                .HasMaxLength(128)
                .HasDefaultValueSql("(newid())");
            entity.Property(e => e.AutorizaPermiso).HasMaxLength(128);
        });

        modelBuilder.Entity<CamposDisciplinare>(entity =>
        {
            entity.HasKey(e => e.CampoDisciplinarId);

            entity.Property(e => e.Nombre)
                .HasMaxLength(120)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CatalogoPermisosLaborale>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("catalogoPermisosLaborales");

            entity.Property(e => e.Activo).HasColumnName("activo");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.TiempoPermitido).HasColumnName("tiempoPermitido");
        });

        modelBuilder.Entity<CatalogoPlazasAdministrativa>(entity =>
        {
            entity.HasKey(e => e.CatalogoPlazasAdministrativasId);

            entity.ToTable("catalogoPlazasAdministrativas");

            entity.Property(e => e.CatalogoPlazasAdministrativasId)
                .ValueGeneratedOnAdd()
                .HasColumnName("catalogoPlazasAdministrativasID");
            entity.Property(e => e.CategoriaPlaza)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("categoriaPlaza");
            entity.Property(e => e.ClavePlazaAdministrativa)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("clavePlazaAdministrativa");
            entity.Property(e => e.DescripcionPlazaAdministrativa)
                .HasMaxLength(130)
                .IsUnicode(false)
                .HasColumnName("descripcionPlazaAdministrativa");
            entity.Property(e => e.Ejercicio)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("ejercicio");
            entity.Property(e => e.NivelAdministrativo)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("nivelAdministrativo");
            entity.Property(e => e.SiglasPlazaAdministrativa)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("siglasPlazaAdministrativa");
            entity.Property(e => e.SueldoFederal).HasColumnName("sueldoFederal");
            entity.Property(e => e.SueldoMensualPlaza).HasColumnName("sueldoMensualPlaza");
            entity.Property(e => e.TipoAdministrativo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("tipoAdministrativo");
        });

        modelBuilder.Entity<CentrosDeTrabajo>(entity =>
        {
            entity.HasKey(e => e.CentroDeTrabajoId);

            entity.ToTable("CentrosDeTrabajo");

            entity.Property(e => e.Clave)
                .HasMaxLength(16)
                .IsUnicode(false);
            entity.Property(e => e.ContactoCorreoElectronico)
                .HasMaxLength(120)
                .IsUnicode(false)
                .HasColumnName("Contacto_CorreoElectronico");
            entity.Property(e => e.ContactoTelefonoCelular)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Contacto_TelefonoCelular");
            entity.Property(e => e.ContactoTelefonoLocal)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Contacto_TelefonoLocal");
            entity.Property(e => e.DomicilioCalle)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Domicilio_Calle");
            entity.Property(e => e.DomicilioCodigoPostal)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("Domicilio_CodigoPostal");
            entity.Property(e => e.DomicilioColonia)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("Domicilio_Colonia");
            entity.Property(e => e.DomicilioEntidadFederativa)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("Domicilio_EntidadFederativa");
            entity.Property(e => e.DomicilioLocalidad)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("Domicilio_Localidad");
            entity.Property(e => e.DomicilioNumeroExterior)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("Domicilio_NumeroExterior");
            entity.Property(e => e.DomicilioNumeroInterior)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("Domicilio_NumeroInterior");
            entity.Property(e => e.Nombre)
                .HasMaxLength(120)
                .IsUnicode(false);
            entity.Property(e => e.NombreAdministrador)
                .HasMaxLength(120)
                .IsUnicode(false);
            entity.Property(e => e.NombreDirector)
                .HasMaxLength(120)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CmcatalogoProyecto>(entity =>
        {
            entity.HasKey(e => e.IdCatalogoProyecto);

            entity.ToTable("CMCatalogoProyectos");

            entity.Property(e => e.IdCatalogoProyecto).HasColumnName("idCatalogoProyecto");
            entity.Property(e => e.Activo)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasColumnName("activo");
            entity.Property(e => e.ClaveProyecto)
                .HasMaxLength(6)
                .HasColumnName("claveProyecto");
            entity.Property(e => e.DescripcionProyecto)
                .HasMaxLength(100)
                .HasColumnName("descripcionProyecto");
            entity.Property(e => e.Responsable)
                .HasMaxLength(60)
                .HasColumnName("responsable");
        });

        modelBuilder.Entity<CorteTiempo>(entity =>
        {
            entity.ToTable("CorteTiempo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CentroDeTrabajoId).HasColumnName("centroDeTrabajoId");
            entity.Property(e => e.Comentario).HasColumnName("comentario");
            entity.Property(e => e.Comprobo).HasColumnName("comprobo");
            entity.Property(e => e.EmpleadoId)
                .HasMaxLength(128)
                .HasColumnName("empleadoId");
            entity.Property(e => e.Estatus).HasColumnName("estatus");
            entity.Property(e => e.EstatusFirma)
                .HasMaxLength(128)
                .HasColumnName("estatusFirma");
            entity.Property(e => e.EstatusPermiso).HasColumnName("estatusPermiso");
            entity.Property(e => e.FechaRegisto)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegisto");
            entity.Property(e => e.FechaSolicitud)
                .HasColumnType("smalldatetime")
                .HasColumnName("fechaSolicitud");
            entity.Property(e => e.HoraSalida)
                .HasColumnType("datetime")
                .HasColumnName("horaSalida");
            entity.Property(e => e.PermisoLaboralId).HasColumnName("permisoLaboralId");
            entity.Property(e => e.TiempoEstimado).HasColumnName("tiempoEstimado");
            entity.Property(e => e.TiempoReal).HasColumnName("tiempoReal");
            entity.Property(e => e.TurnoCentroTrabajoId).HasColumnName("turnoCentroTrabajoId");
        });

        modelBuilder.Entity<Documento>(entity =>
        {
            entity.HasIndex(e => e.EmpleadoId, "IX_FK_EmpleadoDocumento");

            entity.HasIndex(e => e.TipoDocumentoId, "IX_FK_TipoDocumentoDocumento");

            entity.Property(e => e.DocumentoId)
                .HasMaxLength(128)
                .HasDefaultValueSql("(newid())");
            entity.Property(e => e.EmpleadoId).HasMaxLength(128);
            entity.Property(e => e.FechaDocumento).HasColumnType("datetime");
            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            entity.Property(e => e.Metadatos)
                .HasMaxLength(3000)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(260)
                .IsUnicode(false);
            entity.Property(e => e.NombreFisico)
                .HasMaxLength(180)
                .IsUnicode(false);

            entity.HasOne(d => d.Empleado).WithMany(p => p.Documentos)
                .HasForeignKey(d => d.EmpleadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Documentos_Empleados");

            entity.HasOne(d => d.TipoDocumento).WithMany(p => p.Documentos)
                .HasForeignKey(d => d.TipoDocumentoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TipoDocumentoDocumento");
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.EmpleadoId).HasName("PK_Empleados_1");

            entity.HasIndex(e => e.DocumentacionCurp, "IX_Empleados_UniqueCurp").IsUnique();

            entity.HasIndex(e => new { e.PrimerApellido, e.SegundoApellido, e.Nombres, e.DocumentacionCurp }, "IX_Empleados_UniqueEmplado").IsUnique();

            entity.HasIndex(e => e.NumeroEmpleado, "IX_Empleados_UniqueNumeroEmpleado").IsUnique();

            entity.Property(e => e.EmpleadoId)
                .HasMaxLength(128)
                .HasDefaultValueSql("(newid())");
            entity.Property(e => e.ClaveUsuario)
                .HasMaxLength(18)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ContactoCorreoElectronico)
                .HasMaxLength(120)
                .IsUnicode(false)
                .HasColumnName("Contacto_CorreoElectronico");
            entity.Property(e => e.ContactoTelefonoCelular)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Contacto_TelefonoCelular");
            entity.Property(e => e.ContactoTelefonoLocal)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Contacto_TelefonoLocal");
            entity.Property(e => e.DocumentacionCurp)
                .HasMaxLength(18)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Documentacion_Curp");
            entity.Property(e => e.DocumentacionFechaAltaSs)
                .HasColumnType("datetime")
                .HasColumnName("Documentacion_FechaAltaSS");
            entity.Property(e => e.DocumentacionNss)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Documentacion_Nss");
            entity.Property(e => e.DocumentacionRfc)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("Documentacion_Rfc");
            entity.Property(e => e.DomicilioCalle)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Domicilio_Calle");
            entity.Property(e => e.DomicilioCodigoPostal)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("Domicilio_CodigoPostal");
            entity.Property(e => e.DomicilioColonia)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("Domicilio_Colonia");
            entity.Property(e => e.DomicilioEntidadFederativa).HasColumnName("Domicilio_EntidadFederativa");
            entity.Property(e => e.DomicilioLocalidad).HasColumnName("Domicilio_Localidad");
            entity.Property(e => e.DomicilioNumeroExterior)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("Domicilio_NumeroExterior");
            entity.Property(e => e.DomicilioNumeroInterior)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("Domicilio_NumeroInterior");
            entity.Property(e => e.EscolaridadNivelEstudios).HasColumnName("Escolaridad_NivelEstudios");
            entity.Property(e => e.EscolaridadPosgrado)
                .HasMaxLength(120)
                .IsUnicode(false)
                .HasColumnName("Escolaridad_Posgrado");
            entity.Property(e => e.EscolaridadProfesion)
                .HasMaxLength(120)
                .IsUnicode(false)
                .HasColumnName("Escolaridad_Profesion");
            entity.Property(e => e.EscolaridadTitulado)
                .HasComment("Campo para indicar si el empleado cuenta con titulo, se manejara con numeros simulando un booleano")
                .HasColumnName("Escolaridad_Titulado");
            entity.Property(e => e.FechaNacimiento).HasColumnType("datetime");
            entity.Property(e => e.Nombres)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.NumeroEmpleado).HasMaxLength(4);
            entity.Property(e => e.Padecimiento)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("padecimiento");
            entity.Property(e => e.PrimerApellido)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.SegundoApellido)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Usuario)
                .HasMaxLength(18)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<EntidadFederativa>(entity =>
        {
            entity.HasKey(e => e.EntidadId).HasName("PK_entidadFederativa");

            entity.ToTable("EntidadFederativa");

            entity.Property(e => e.EntidadId)
                .ValueGeneratedOnAdd()
                .HasColumnName("entidadId");
            entity.Property(e => e.ClaveEntidad)
                .HasMaxLength(2)
                .HasColumnName("claveEntidad");
            entity.Property(e => e.NombreEntidad)
                .HasMaxLength(19)
                .IsUnicode(false)
                .HasColumnName("nombreEntidad");
        });

        modelBuilder.Entity<EstadoCivil>(entity =>
        {
            entity.HasKey(e => e.EstadoCivilId).HasName("PK_estadoCivil");

            entity.ToTable("EstadoCivil");

            entity.Property(e => e.EstadoCivilId)
                .ValueGeneratedOnAdd()
                .HasColumnName("estadoCivilId");
            entity.Property(e => e.EstadoCivil1)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("estadoCivil");
        });

        modelBuilder.Entity<Faacumulado>(entity =>
        {
            entity.HasKey(e => e.IdFaacumulado);

            entity.ToTable("FAAcumulados");

            entity.Property(e => e.IdFaacumulado).HasColumnName("idFAAcumulado");
            entity.Property(e => e.Ejercicio).HasColumnName("ejercicio");
            entity.Property(e => e.Factor).HasColumnName("factor");
            entity.Property(e => e.IdFaregistroDistribucion).HasColumnName("idFARegistroDistribucion");
            entity.Property(e => e.IdRegistroFa).HasColumnName("idRegistroFA");
            entity.Property(e => e.Interes).HasColumnName("interes");
            entity.Property(e => e.Monto).HasColumnName("monto");
            entity.Property(e => e.Quincena).HasColumnName("quincena");

            entity.HasOne(d => d.IdRegistroFaNavigation).WithMany(p => p.Faacumulados)
                .HasForeignKey(d => d.IdRegistroFa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FAAcumulados_FARegistro");
        });

        modelBuilder.Entity<Fabeneficiario>(entity =>
        {
            entity.HasKey(e => e.IdBeneficiario);

            entity.ToTable("FABeneficiarios");

            entity.Property(e => e.IdBeneficiario).HasColumnName("idBeneficiario");
            entity.Property(e => e.IdRegistroFa).HasColumnName("idRegistroFA");
            entity.Property(e => e.NombreBeneficiario)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("nombreBeneficiario");
            entity.Property(e => e.Parentesco)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("parentesco");
            entity.Property(e => e.Porcentaje).HasColumnName("porcentaje");

            entity.HasOne(d => d.IdRegistroFaNavigation).WithMany(p => p.Fabeneficiarios)
                .HasForeignKey(d => d.IdRegistroFa)
                .HasConstraintName("FK_FABeneficiarios_FARegistro");
        });

        modelBuilder.Entity<FacuentasBancaria>(entity =>
        {
            entity.HasKey(e => e.IdCuentasBancarias);

            entity.ToTable("FACuentasBancarias");

            entity.Property(e => e.IdCuentasBancarias).HasColumnName("idCuentasBancarias");
            entity.Property(e => e.Banco)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Clabe)
                .HasMaxLength(18)
                .IsUnicode(false);
            entity.Property(e => e.EmpleadoId).HasMaxLength(128);
            entity.Property(e => e.NumCuenta)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NumTarjeta)
                .HasMaxLength(16)
                .IsUnicode(false);

            entity.HasOne(d => d.Empleado).WithMany(p => p.FacuentasBancaria)
                .HasForeignKey(d => d.EmpleadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FACuentasBancarias_Empleados");
        });

        modelBuilder.Entity<Faprestamo>(entity =>
        {
            entity.HasKey(e => e.IdPrestamo);

            entity.ToTable("FAPrestamos");

            entity.Property(e => e.IdPrestamo).HasColumnName("idPrestamo");
            entity.Property(e => e.Amortizacion).HasColumnName("amortizacion");
            entity.Property(e => e.Aval)
                .HasMaxLength(128)
                .HasColumnName("aval");
            entity.Property(e => e.ComisionBanco).HasColumnName("comisionBanco");
            entity.Property(e => e.DescuentoInicial).HasColumnName("descuentoInicial");
            entity.Property(e => e.DescuentoQuincenal).HasColumnName("descuentoQuincenal");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.FechaPrestamo)
                .HasColumnType("datetime")
                .HasColumnName("fechaPrestamo");
            entity.Property(e => e.IdPrestamoSaldado).HasColumnName("idPrestamoSaldado");
            entity.Property(e => e.IdRegistroFa).HasColumnName("idRegistroFA");
            entity.Property(e => e.ImporteSolicitado).HasColumnName("importeSolicitado");
            entity.Property(e => e.ImporteTransferencia).HasColumnName("importeTransferencia");
            entity.Property(e => e.Intereses).HasColumnName("intereses");
            entity.Property(e => e.Plazo).HasColumnName("plazo");
            entity.Property(e => e.Quincena).HasColumnName("quincena");
            entity.Property(e => e.QuincenaDescuento).HasColumnName("quincenaDescuento");
            entity.Property(e => e.ResumenDescuentos).HasColumnName("resumenDescuentos");
            entity.Property(e => e.ResumenSaldo).HasColumnName("resumenSaldo");

            entity.HasOne(d => d.AvalNavigation).WithMany(p => p.Faprestamos)
                .HasForeignKey(d => d.Aval)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FAPrestamos_Empleados");

            entity.HasOne(d => d.IdPrestamoSaldadoNavigation).WithMany(p => p.InverseIdPrestamoSaldadoNavigation)
                .HasForeignKey(d => d.IdPrestamoSaldado)
                .HasConstraintName("FK_FAPrestamos_FAPrestamos");

            entity.HasOne(d => d.IdRegistroFaNavigation).WithMany(p => p.Faprestamos)
                .HasForeignKey(d => d.IdRegistroFa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FAPrestamos_FARegistro");
        });

        modelBuilder.Entity<Faregistro>(entity =>
        {
            entity.HasKey(e => e.IdRegistroFa);

            entity.ToTable("FARegistro");

            entity.Property(e => e.IdRegistroFa).HasColumnName("idRegistroFA");
            entity.Property(e => e.Aportacion).HasColumnName("aportacion");
            entity.Property(e => e.EmpleadoId).HasMaxLength(128);
            entity.Property(e => e.Estado)
                .HasDefaultValueSql("((1))")
                .HasColumnName("estado");
            entity.Property(e => e.FechaIngreso)
                .HasColumnType("datetime")
                .HasColumnName("fechaIngreso");
            entity.Property(e => e.Socio).HasColumnName("socio");
            entity.Property(e => e.TipoAportacion)
                .HasComment("Fijo o Variable")
                .HasColumnName("tipoAportacion");

            entity.HasOne(d => d.Empleado).WithMany(p => p.Faregistros)
                .HasForeignKey(d => d.EmpleadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FARegistro_Empleados");
        });

        modelBuilder.Entity<Fatransaccione>(entity =>
        {
            entity.HasKey(e => e.IdTransaccion);

            entity.ToTable("FATransacciones");

            entity.Property(e => e.IdTransaccion).HasColumnName("idTransaccion");
            entity.Property(e => e.FechaTransaccion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IdCatalogoConcepto).HasColumnName("idCatalogoConcepto");
            entity.Property(e => e.IdPrestamo).HasColumnName("idPrestamo");
            entity.Property(e => e.IdRegistroFa).HasColumnName("idRegistroFA");
            entity.Property(e => e.Quincena).HasColumnName("quincena");

            entity.HasOne(d => d.IdPrestamoNavigation).WithMany(p => p.Fatransacciones)
                .HasForeignKey(d => d.IdPrestamo)
                .HasConstraintName("FK_FATransacciones_FAPrestamos");

            entity.HasOne(d => d.IdRegistroFaNavigation).WithMany(p => p.Fatransacciones)
                .HasForeignKey(d => d.IdRegistroFa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FATransacciones_FARegistro");
        });

        modelBuilder.Entity<InformacionLaboral>(entity =>
        {
            entity.ToTable("InformacionLaboral");

            entity.Property(e => e.InformacionLaboralId)
                .HasMaxLength(128)
                .HasDefaultValueSql("(newid())");
            entity.Property(e => e.Caracter).HasComment("1.-Base\r\n2.-Confianza\r\n3.-Interino");
            entity.Property(e => e.Categoria)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("categoria");
            entity.Property(e => e.DenominacionPlaza)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("denominacionPlaza");
            entity.Property(e => e.EmpleadoId).HasMaxLength(128);
            entity.Property(e => e.FechaBaja).HasColumnType("datetime");
            entity.Property(e => e.FechaIngreso).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacionNss).HasColumnType("datetime");
            entity.Property(e => e.FechaRegistroNss).HasColumnType("datetime");
            entity.Property(e => e.IdCatalogoProyecto).HasColumnName("idCatalogoProyecto");
            entity.Property(e => e.IdPlaza).HasColumnName("idPlaza");
            entity.Property(e => e.MotivoBaja).HasMaxLength(500);
            entity.Property(e => e.TipoEmpleado).HasComment("1.-Administrativo\r\n2.-Docente");

            entity.HasOne(d => d.Empleado).WithMany(p => p.InformacionLaborals)
                .HasForeignKey(d => d.EmpleadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InformacionLaboral_Empleados");

            entity.HasOne(d => d.TurnoxCentroDeTrabajo).WithMany(p => p.InformacionLaborals)
                .HasForeignKey(d => d.TurnoxCentroDeTrabajoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InformacionLaboral_TurnosxCentrosDeTrabajo");
        });

        modelBuilder.Entity<Localidad>(entity =>
        {
            entity.ToTable("Localidad");

            entity.Property(e => e.LocalidadId)
                .ValueGeneratedOnAdd()
                .HasColumnName("localidadId");
            entity.Property(e => e.Localidad1)
                .HasMaxLength(19)
                .IsUnicode(false)
                .HasColumnName("Localidad");
        });

        modelBuilder.Entity<Nacionalidad>(entity =>
        {
            entity.ToTable("Nacionalidad");

            entity.Property(e => e.NacionalidadId)
                .ValueGeneratedOnAdd()
                .HasColumnName("nacionalidadId");
            entity.Property(e => e.Nacionalidad1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nacionalidad");
        });

        modelBuilder.Entity<NivelEstudio>(entity =>
        {
            entity.HasKey(e => e.EscolaridadId);

            entity.ToTable("NivelEstudio");

            entity.Property(e => e.EscolaridadId)
                .ValueGeneratedOnAdd()
                .HasColumnName("escolaridadId");
            entity.Property(e => e.Escolaridad)
                .HasMaxLength(12)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PermisoEconomico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PermisoE__3213E83FF1779EB4");

            entity.ToTable("PermisoEconomico");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CentroDeTrabajoId).HasColumnName("centroDeTrabajoId");
            entity.Property(e => e.Comentario).HasColumnName("comentario");
            entity.Property(e => e.ConGoceSueldo).HasColumnName("conGoceSueldo");
            entity.Property(e => e.EmpleadoId)
                .HasMaxLength(128)
                .HasColumnName("empleadoId");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.FechaSolicitudFinal)
                .HasColumnType("smalldatetime")
                .HasColumnName("fechaSolicitud_final");
            entity.Property(e => e.FechaSolicitudInicio)
                .HasColumnType("smalldatetime")
                .HasColumnName("fechaSolicitud_inicio");
            entity.Property(e => e.LapsoPermisoDiasHabiles).HasColumnName("lapsoPermisoDiasHabiles");
            entity.Property(e => e.PermisoLaboralId).HasColumnName("permisoLaboralId");
            entity.Property(e => e.TurnoCentroTrabajoId).HasColumnName("turnoCentroTrabajoId");
        });

        modelBuilder.Entity<TipoSangre>(entity =>
        {
            entity.HasKey(e => e.TipoSangreId).HasName("PK_tipoSangre");

            entity.ToTable("TipoSangre");

            entity.Property(e => e.TipoSangreId)
                .ValueGeneratedOnAdd()
                .HasColumnName("tipoSangreId");
            entity.Property(e => e.GrupoSanguineo)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("grupoSanguineo");
        });

        modelBuilder.Entity<TiposDocumento>(entity =>
        {
            entity.HasKey(e => e.TipoDocumentoId);

            entity.ToTable("TiposDocumento");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(256)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TurnosxCentrosDeTrabajo>(entity =>
        {
            entity.HasKey(e => e.TurnoxCentroDeTrabajoId);

            entity.ToTable("TurnosxCentrosDeTrabajo");

            entity.HasIndex(e => e.CentroDeTrabajoId, "IX_FK_CentroDeTrabajoTurnoxCentroDeTrabajo");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
