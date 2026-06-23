/* =====================================================================
   PROYECTO: CambioSeguro P2P  -  Base de Datos (SQL Server / T-SQL)
   Curso: Desarrollo de Aplicaciones Web - UESAN
   Motor:  SQL Server  |  Patrón: Database-First (.NET 10 + EF Core)

   Contenido:
     - 11 tablas existentes (reconstruidas desde tus entidades)
     - 4 tablas NUEVAS:  MetodoPago, OfertaMetodoPago,
                         EvidenciaDisputa, Notificacion
     - ReporteAdministrativo: REINCORPORADA y ahora VINCULADA al Usuario
                         (administrador) que la genera, vía FK
                         GeneradoPorUsuarioId  (HU-017 Dashboard Admin)
     - 1 columna nueva:  Oferta.MontoDisponible (para "Disponible" del
                         Marketplace, separado de los límites Min/Máx)
     - Datos semilla: Moneda y MetodoPago

   El script es re-ejecutable: borra las tablas si existen (en orden
   inverso a las FK) y las vuelve a crear.
   ===================================================================== */

-------------------------------------------------------------------------
-- 0) CREAR / USAR LA BASE DE DATOS
-------------------------------------------------------------------------
IF DB_ID(N'CambioSeguroP2PDB') IS NULL
BEGIN
    CREATE DATABASE CambioSeguroP2PDB;   -- (corregido: antes decía ...P2PBD)
END
GO

USE CambioSeguroP2PDB;
GO

-------------------------------------------------------------------------
-- 1) LIMPIEZA (orden inverso a las dependencias de FK)
-------------------------------------------------------------------------
-- ReporteAdministrativo depende de Usuario, por eso se borra ANTES que Usuario:
IF OBJECT_ID(N'dbo.ReporteAdministrativo', N'U') IS NOT NULL DROP TABLE dbo.ReporteAdministrativo;
IF OBJECT_ID(N'dbo.Notificacion',          N'U') IS NOT NULL DROP TABLE dbo.Notificacion;
IF OBJECT_ID(N'dbo.EvidenciaDisputa',      N'U') IS NOT NULL DROP TABLE dbo.EvidenciaDisputa;
IF OBJECT_ID(N'dbo.OfertaMetodoPago',      N'U') IS NOT NULL DROP TABLE dbo.OfertaMetodoPago;
IF OBJECT_ID(N'dbo.Calificacion',          N'U') IS NOT NULL DROP TABLE dbo.Calificacion;
IF OBJECT_ID(N'dbo.Mensaje',               N'U') IS NOT NULL DROP TABLE dbo.Mensaje;
IF OBJECT_ID(N'dbo.Disputa',               N'U') IS NOT NULL DROP TABLE dbo.Disputa;
IF OBJECT_ID(N'dbo.ComprobantePago',       N'U') IS NOT NULL DROP TABLE dbo.ComprobantePago;
IF OBJECT_ID(N'dbo.TemporizadorOperacion', N'U') IS NOT NULL DROP TABLE dbo.TemporizadorOperacion;
IF OBJECT_ID(N'dbo.Operacion',             N'U') IS NOT NULL DROP TABLE dbo.Operacion;
IF OBJECT_ID(N'dbo.Oferta',                N'U') IS NOT NULL DROP TABLE dbo.Oferta;
IF OBJECT_ID(N'dbo.VerificacionIdentidad', N'U') IS NOT NULL DROP TABLE dbo.VerificacionIdentidad;
IF OBJECT_ID(N'dbo.MetodoPago',            N'U') IS NOT NULL DROP TABLE dbo.MetodoPago;
IF OBJECT_ID(N'dbo.Moneda',                N'U') IS NOT NULL DROP TABLE dbo.Moneda;
IF OBJECT_ID(N'dbo.Usuario',               N'U') IS NOT NULL DROP TABLE dbo.Usuario;
GO

-------------------------------------------------------------------------
-- 2) TABLAS BASE (sin dependencias)
-------------------------------------------------------------------------

-- USUARIO  (HU-001, HU-002, HU-013 reputación)
CREATE TABLE dbo.Usuario (
    Id                  INT             IDENTITY(1,1) NOT NULL,
    NombreCompleto      VARCHAR(150)    NOT NULL,
    Correo              VARCHAR(150)    NOT NULL,
    Password            VARCHAR(100)    NOT NULL,   -- guardar SIEMPRE el HASH, nunca texto plano
    Telefono            VARCHAR(20)     NULL,
    Rol                 VARCHAR(50)     NOT NULL,   -- 'Usuario' | 'Administrador'
    EstadoVerificacion  VARCHAR(50)     NOT NULL,   -- 'Pendiente' | 'Verificado' | 'Rechazado'
    Reputacion          DECIMAL(3,2)    NULL DEFAULT (0),
    FechaRegistro       DATETIME        NULL DEFAULT (GETDATE()),
    CONSTRAINT PK_Usuario PRIMARY KEY (Id),
    CONSTRAINT UQ_Usuario_Correo UNIQUE (Correo)
);
GO

-- MONEDA  (HU-005, HU-006, HU-007)
CREATE TABLE dbo.Moneda (
    Id      INT             IDENTITY(1,1) NOT NULL,
    Codigo  VARCHAR(10)     NOT NULL,   -- 'PEN', 'USD', 'EUR', 'BRL', 'CLP'
    Nombre  VARCHAR(100)    NOT NULL,
    CONSTRAINT PK_Moneda PRIMARY KEY (Id),
    CONSTRAINT UQ_Moneda_Codigo UNIQUE (Codigo)
);
GO

-- METODO DE PAGO  [NUEVA]  (HU-005, HU-006, HU-007)
CREATE TABLE dbo.MetodoPago (
    Id      INT             IDENTITY(1,1) NOT NULL,
    Nombre  VARCHAR(100)    NOT NULL,   -- 'Transferencia bancaria', 'Yape', 'Plin', 'PayPal'
    Activo  BIT             NOT NULL DEFAULT (1),
    CONSTRAINT PK_MetodoPago PRIMARY KEY (Id),
    CONSTRAINT UQ_MetodoPago_Nombre UNIQUE (Nombre)
);
GO

-------------------------------------------------------------------------
-- 3) TABLAS QUE DEPENDEN DE USUARIO
-------------------------------------------------------------------------

-- VERIFICACION DE IDENTIDAD  (HU-003)
CREATE TABLE dbo.VerificacionIdentidad (
    Id                  INT           IDENTITY(1,1) NOT NULL,
    UsuarioId           INT           NOT NULL,
    DocumentoIdentidad  VARCHAR(30)   NOT NULL,
    TipoDocumento       VARCHAR(50)   NOT NULL,   -- 'DNI' | 'Pasaporte'
    EstadoVerificacion  VARCHAR(50)   NOT NULL,   -- 'Pendiente' | 'Verificado' | 'Rechazado'
    FechaRegistro       DATETIME      NULL DEFAULT (GETDATE()),
    CONSTRAINT PK_VerificacionIdentidad PRIMARY KEY (Id),
    CONSTRAINT FK_VerificacionIdentidad_Usuario
        FOREIGN KEY (UsuarioId) REFERENCES dbo.Usuario(Id)
);
GO

-- REPORTE ADMINISTRATIVO  [REINCORPORADA + VINCULADA]  (HU-017)
-- Cada reporte queda asociado al Usuario administrador que lo genera.
CREATE TABLE dbo.ReporteAdministrativo (
    Id                   INT       IDENTITY(1,1) NOT NULL,
    GeneradoPorUsuarioId INT       NOT NULL,            -- FK al admin que genera el reporte
    FechaGeneracion      DATETIME  NULL DEFAULT (GETDATE()),
    TotalOperaciones     INT       NOT NULL DEFAULT (0),
    TotalDisputas        INT       NOT NULL DEFAULT (0),
    TotalUsuarios        INT       NOT NULL DEFAULT (0),
    CONSTRAINT PK_ReporteAdministrativo PRIMARY KEY (Id),
    CONSTRAINT FK_ReporteAdministrativo_Usuario
        FOREIGN KEY (GeneradoPorUsuarioId) REFERENCES dbo.Usuario(Id)
);
GO

-- OFERTA  (HU-005, HU-006, HU-007)  [+ columna nueva MontoDisponible]
CREATE TABLE dbo.Oferta (
    Id              INT             IDENTITY(1,1) NOT NULL,
    UsuarioId       INT             NOT NULL,   -- vendedor que publica
    MonedaOrigenId  INT             NOT NULL,
    MonedaDestinoId INT             NOT NULL,
    TipoOperacion   VARCHAR(50)     NOT NULL,   -- 'Compra' | 'Venta'
    TasaCambio      DECIMAL(10,4)   NOT NULL,
    MontoMinimo     DECIMAL(10,2)   NOT NULL,   -- límite mínimo por trato
    MontoMaximo     DECIMAL(10,2)   NOT NULL,   -- límite máximo por trato
    MontoDisponible DECIMAL(10,2)   NULL,       -- [NUEVA] "Disponible" del Marketplace
    Estado          VARCHAR(50)     NOT NULL,   -- 'Activa' | 'Pausada' | 'Bloqueada' | 'Finalizada'
    FechaCreacion   DATETIME        NULL DEFAULT (GETDATE()),
    CONSTRAINT PK_Oferta PRIMARY KEY (Id),
    CONSTRAINT FK_Oferta_Usuario
        FOREIGN KEY (UsuarioId)       REFERENCES dbo.Usuario(Id),
    CONSTRAINT FK_Oferta_MonedaOrigen
        FOREIGN KEY (MonedaOrigenId)  REFERENCES dbo.Moneda(Id),
    CONSTRAINT FK_Oferta_MonedaDestino
        FOREIGN KEY (MonedaDestinoId) REFERENCES dbo.Moneda(Id)
);
GO

-- OFERTA - METODO DE PAGO  [NUEVA, tabla puente N:M]  (HU-007)
CREATE TABLE dbo.OfertaMetodoPago (
    Id            INT IDENTITY(1,1) NOT NULL,
    OfertaId      INT NOT NULL,
    MetodoPagoId  INT NOT NULL,
    CONSTRAINT PK_OfertaMetodoPago PRIMARY KEY (Id),
    CONSTRAINT UQ_OfertaMetodoPago UNIQUE (OfertaId, MetodoPagoId),
    CONSTRAINT FK_OfertaMetodoPago_Oferta
        FOREIGN KEY (OfertaId)     REFERENCES dbo.Oferta(Id),
    CONSTRAINT FK_OfertaMetodoPago_MetodoPago
        FOREIGN KEY (MetodoPagoId) REFERENCES dbo.MetodoPago(Id)
);
GO

-------------------------------------------------------------------------
-- 4) OPERACION Y SUS DEPENDIENTES
-------------------------------------------------------------------------

-- OPERACION  (HU-008, HU-011, HU-012)
CREATE TABLE dbo.Operacion (
    Id               INT            IDENTITY(1,1) NOT NULL,
    OfertaId         INT            NOT NULL,
    CompradorId      INT            NOT NULL,
    VendedorId       INT            NOT NULL,
    Monto            DECIMAL(10,2)  NOT NULL,
    Estado           VARCHAR(50)    NOT NULL,   -- 'En proceso' | 'Pago enviado' | 'Completada' | 'Expirada' | 'En disputa'
    CodigoOperacion  VARCHAR(50)    NOT NULL,   -- código único (ej: P2P-B1HYFT)
    FechaInicio      DATETIME       NULL DEFAULT (GETDATE()),
    FechaFin         DATETIME       NULL,
    FechaLiberacion  DATETIME       NULL,
    CONSTRAINT PK_Operacion PRIMARY KEY (Id),
    CONSTRAINT UQ_Operacion_Codigo UNIQUE (CodigoOperacion),
    CONSTRAINT FK_Operacion_Oferta
        FOREIGN KEY (OfertaId)    REFERENCES dbo.Oferta(Id),
    CONSTRAINT FK_Operacion_Comprador
        FOREIGN KEY (CompradorId) REFERENCES dbo.Usuario(Id),
    CONSTRAINT FK_Operacion_Vendedor
        FOREIGN KEY (VendedorId)  REFERENCES dbo.Usuario(Id)
);
GO

-- TEMPORIZADOR DE OPERACION  (HU-009)
CREATE TABLE dbo.TemporizadorOperacion (
    Id           INT          IDENTITY(1,1) NOT NULL,
    OperacionId  INT          NOT NULL,
    FechaInicio  DATETIME     NOT NULL,
    FechaFin     DATETIME     NOT NULL,
    Estado       VARCHAR(50)  NOT NULL,   -- 'Activo' | 'Expirado' | 'Detenido'
    CONSTRAINT PK_TemporizadorOperacion PRIMARY KEY (Id),
    CONSTRAINT FK_TemporizadorOperacion_Operacion
        FOREIGN KEY (OperacionId) REFERENCES dbo.Operacion(Id)
);
GO

-- COMPROBANTE DE PAGO  (HU-010)
CREATE TABLE dbo.ComprobantePago (
    Id           INT           IDENTITY(1,1) NOT NULL,
    OperacionId  INT           NOT NULL,
    RutaArchivo  VARCHAR(300)  NOT NULL,
    FechaSubida  DATETIME      NULL DEFAULT (GETDATE()),
    CONSTRAINT PK_ComprobantePago PRIMARY KEY (Id),
    CONSTRAINT FK_ComprobantePago_Operacion
        FOREIGN KEY (OperacionId) REFERENCES dbo.Operacion(Id)
);
GO

-- DISPUTA  (HU-014, HU-015)
CREATE TABLE dbo.Disputa (
    Id            INT           IDENTITY(1,1) NOT NULL,
    OperacionId   INT           NOT NULL,
    Motivo        VARCHAR(300)  NOT NULL,
    Estado        VARCHAR(50)   NOT NULL,   -- 'En revisión' | 'Resuelta a favor comprador' | 'Resuelta a favor vendedor'
    Resolucion    VARCHAR(300)  NULL,
    FechaRegistro DATETIME      NULL DEFAULT (GETDATE()),
    CONSTRAINT PK_Disputa PRIMARY KEY (Id),
    CONSTRAINT FK_Disputa_Operacion
        FOREIGN KEY (OperacionId) REFERENCES dbo.Operacion(Id)
);
GO

-- EVIDENCIA DE DISPUTA  [NUEVA]  (HU-014: "subir evidencias" en plural)
CREATE TABLE dbo.EvidenciaDisputa (
    Id           INT           IDENTITY(1,1) NOT NULL,
    DisputaId    INT           NOT NULL,
    RutaArchivo  VARCHAR(300)  NOT NULL,
    FechaSubida  DATETIME      NULL DEFAULT (GETDATE()),
    CONSTRAINT PK_EvidenciaDisputa PRIMARY KEY (Id),
    CONSTRAINT FK_EvidenciaDisputa_Disputa
        FOREIGN KEY (DisputaId) REFERENCES dbo.Disputa(Id)
);
GO

-- CALIFICACION  (HU-013)
CREATE TABLE dbo.Calificacion (
    Id                   INT           IDENTITY(1,1) NOT NULL,
    OperacionId          INT           NOT NULL,
    UsuarioCalificadoId  INT           NOT NULL,
    Puntaje              INT           NOT NULL,   -- 1 a 5 estrellas
    Comentario           VARCHAR(300)  NULL,
    FechaRegistro        DATETIME      NULL DEFAULT (GETDATE()),
    CONSTRAINT PK_Calificacion PRIMARY KEY (Id),
    CONSTRAINT CK_Calificacion_Puntaje CHECK (Puntaje BETWEEN 1 AND 5),
    CONSTRAINT FK_Calificacion_Operacion
        FOREIGN KEY (OperacionId)         REFERENCES dbo.Operacion(Id),
    CONSTRAINT FK_Calificacion_Usuario
        FOREIGN KEY (UsuarioCalificadoId) REFERENCES dbo.Usuario(Id)
);
GO

-- MENSAJE / CHAT  (HU-016)
CREATE TABLE dbo.Mensaje (
    Id            INT           IDENTITY(1,1) NOT NULL,
    RemitenteId   INT           NOT NULL,
    DestinatarioId INT          NOT NULL,
    OperacionId   INT           NULL,   -- el chat vive dentro de una operación
    Contenido     VARCHAR(500)  NOT NULL,
    FechaEnvio    DATETIME      NULL DEFAULT (GETDATE()),
    CONSTRAINT PK_Mensaje PRIMARY KEY (Id),
    CONSTRAINT FK_Mensaje_Remitente
        FOREIGN KEY (RemitenteId)   REFERENCES dbo.Usuario(Id),
    CONSTRAINT FK_Mensaje_Destinatario
        FOREIGN KEY (DestinatarioId) REFERENCES dbo.Usuario(Id),
    CONSTRAINT FK_Mensaje_Operacion
        FOREIGN KEY (OperacionId)   REFERENCES dbo.Operacion(Id)
);
GO

-- NOTIFICACION  [NUEVA]  (HU-008 y HU-010: "ambas partes reciben notificación")
CREATE TABLE dbo.Notificacion (
    Id            INT           IDENTITY(1,1) NOT NULL,
    UsuarioId     INT           NOT NULL,
    Titulo        VARCHAR(150)  NULL,
    Mensaje       VARCHAR(500)  NOT NULL,
    Leida         BIT           NOT NULL DEFAULT (0),
    OperacionId   INT           NULL,   -- operación que originó la notificación (opcional)
    FechaCreacion DATETIME      NULL DEFAULT (GETDATE()),
    CONSTRAINT PK_Notificacion PRIMARY KEY (Id),
    CONSTRAINT FK_Notificacion_Usuario
        FOREIGN KEY (UsuarioId)   REFERENCES dbo.Usuario(Id),
    CONSTRAINT FK_Notificacion_Operacion
        FOREIGN KEY (OperacionId) REFERENCES dbo.Operacion(Id)
);
GO

-------------------------------------------------------------------------
-- 5) ÍNDICES ÚTILES (mejoran filtros del Marketplace / consultas)
-------------------------------------------------------------------------
CREATE INDEX IX_Oferta_Estado                  ON dbo.Oferta(Estado);
CREATE INDEX IX_Oferta_Monedas                 ON dbo.Oferta(MonedaOrigenId, MonedaDestinoId);
CREATE INDEX IX_Operacion_Estado               ON dbo.Operacion(Estado);
CREATE INDEX IX_Notificacion_Usuario           ON dbo.Notificacion(UsuarioId, Leida);
CREATE INDEX IX_Mensaje_Operacion              ON dbo.Mensaje(OperacionId);
CREATE INDEX IX_ReporteAdministrativo_Usuario  ON dbo.ReporteAdministrativo(GeneradoPorUsuarioId);
GO

-------------------------------------------------------------------------
-- 6) DATOS SEMILLA
-------------------------------------------------------------------------

-- Monedas (las que aparecen en tus prototipos)
INSERT INTO dbo.Moneda (Codigo, Nombre) VALUES
    ('PEN', 'Sol Peruano'),
    ('USD', 'Dólar Estadounidense'),
    ('EUR', 'Euro'),
    ('BRL', 'Real Brasileño'),
    ('CLP', 'Peso Chileno');
GO

-- Métodos de pago (los de "Publicar oferta" y "Marketplace")
INSERT INTO dbo.MetodoPago (Nombre) VALUES
    ('Transferencia bancaria'),
    ('Yape'),
    ('Plin'),
    ('PayPal');
GO

PRINT 'Base de datos CambioSeguroP2P creada correctamente.';
GO