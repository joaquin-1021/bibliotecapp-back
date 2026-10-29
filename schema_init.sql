-- Esquema de Bibliotecapp (PostgreSQL 17)
BEGIN;

CREATE TABLE tipo_usuario (
    id_tipo      INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    tipo         VARCHAR(50)  NOT NULL UNIQUE,
    descripcion  VARCHAR(255)
);

CREATE TABLE usuario (
    id_usuario     INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    nombre         VARCHAR(100) NOT NULL,
    apellido       VARCHAR(100) NOT NULL,
    email          VARCHAR(255) NOT NULL UNIQUE,
    clave  VARCHAR(255) NOT NULL,
    id_tipo        INT          NOT NULL REFERENCES tipo_usuario (id_tipo),
    created_at     TIMESTAMPTZ  NOT NULL DEFAULT now()
);

CREATE TABLE libro (
    id_libro          INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    isbn_10           CHAR(10) UNIQUE,
    isbn_13           CHAR(13) UNIQUE,
    titulo            VARCHAR(500) NOT NULL,
    subtitulo         VARCHAR(500),
    editorial         VARCHAR(255),
    anio_publicacion  SMALLINT,
    portada_url       TEXT,
    paginas           INT CHECK (paginas > 0),
    created_at        TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE TABLE ejemplar (
    id_ejemplar  INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    id_libro     INT NOT NULL REFERENCES libro (id_libro),
    id_usuario   INT NOT NULL REFERENCES usuario (id_usuario) ON DELETE CASCADE,
    estado       VARCHAR(50),
    notas        TEXT,
    created_at   TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE TABLE contacto (
    id_contacto           INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    id_usuario            INT NOT NULL REFERENCES usuario (id_usuario) ON DELETE CASCADE,
    id_usuario_vinculado  INT REFERENCES usuario (id_usuario) ON DELETE SET NULL,
    nombre                VARCHAR(100) NOT NULL,
    apellido              VARCHAR(100),
    telefono              VARCHAR(50),
    email                 VARCHAR(255)
);

CREATE TABLE prestamo (
    id_prestamo       INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    id_ejemplar       INT  NOT NULL REFERENCES ejemplar (id_ejemplar) ON DELETE CASCADE,
    id_contacto       INT  NOT NULL REFERENCES contacto (id_contacto),
    fecha_prestamo    DATE NOT NULL DEFAULT CURRENT_DATE,
    fecha_devolucion  DATE,
    notas             TEXT,
    CHECK (fecha_devolucion IS NULL OR fecha_devolucion >= fecha_prestamo)
);

-- Un ejemplar no puede tener dos préstamos activos a la vez
CREATE UNIQUE INDEX ux_prestamo_activo ON prestamo (id_ejemplar) WHERE fecha_devolucion IS NULL;

-- Índices para las FKs (Postgres no los crea solo)
CREATE INDEX ix_usuario_tipo        ON usuario  (id_tipo);
CREATE INDEX ix_ejemplar_libro      ON ejemplar (id_libro);
CREATE INDEX ix_ejemplar_usuario    ON ejemplar (id_usuario);
CREATE INDEX ix_contacto_usuario    ON contacto (id_usuario);
CREATE INDEX ix_contacto_vinculado  ON contacto (id_usuario_vinculado);
CREATE INDEX ix_prestamo_contacto   ON prestamo (id_contacto);

INSERT INTO tipo_usuario (tipo, descripcion) VALUES
    ('admin',   'Administrador de la aplicación'),
    ('usuario', 'Usuario estándar');

COMMIT;
