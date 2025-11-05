GO
CREATE OR ALTER PROCEDURE dbo.spProductos_Create
  @Nombre NVARCHAR(120),
  @Descripcion NVARCHAR(500) = NULL,
  @PrecioVenta DECIMAL(18,2),
  @MinimoExistencia INT
AS
BEGIN
  SET NOCOUNT ON;
  INSERT INTO dbo.Productos(Nombre, Descripcion, PrecioVenta, MinimoExistencia)
  VALUES (@Nombre, @Descripcion, @PrecioVenta, @MinimoExistencia);

  DECLARE @Id INT = SCOPE_IDENTITY();

  IF NOT EXISTS (SELECT 1 FROM dbo.Inventario WHERE IdProducto = @Id)
    INSERT INTO dbo.Inventario(IdProducto, Existencia) VALUES (@Id, 0);

  SELECT * FROM dbo.Productos WHERE IdProducto = @Id;
END
GO

CREATE OR ALTER PROCEDURE dbo.spProductos_Update
  @IdProducto INT,
  @Nombre NVARCHAR(120),
  @Descripcion NVARCHAR(500) = NULL,
  @PrecioVenta DECIMAL(18,2),
  @MinimoExistencia INT
AS
BEGIN
  SET NOCOUNT ON;
  UPDATE dbo.Productos
  SET Nombre=@Nombre,
      Descripcion=@Descripcion,
      PrecioVenta=@PrecioVenta,
      MinimoExistencia=@MinimoExistencia
  WHERE IdProducto=@IdProducto;

  SELECT * FROM dbo.Productos WHERE IdProducto=@IdProducto;
END
GO
