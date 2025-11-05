CREATE OR ALTER PROCEDURE dbo.spInventario_BajoMinimo
AS
BEGIN
  SET NOCOUNT ON;
  SELECT p.IdProducto, p.Nombre, i.Existencia, p.MinimoExistencia
  FROM dbo.Productos p
  LEFT JOIN dbo.Inventario i ON i.IdProducto = p.IdProducto
  WHERE ISNULL(i.Existencia,0) < p.MinimoExistencia
  ORDER BY p.IdProducto DESC;
END