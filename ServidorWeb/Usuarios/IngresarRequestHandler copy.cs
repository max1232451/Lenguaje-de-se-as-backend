
using MongoDB.Driver;

public static class   IngresarRequestHandler{

    public static IResult Ingresar(Ingresar datos){
      if(string.IsNullOrWhiteSpace(datos.Correo)){
        return Results.BadRequest("El correo es requerido");
      }
      if(string.IsNullOrWhiteSpace(datos.Contraseña)){
        return Results.BadRequest("La contraseña es requerida");
      } 

BaseDatos bd= new BaseDatos();
var coleccion= bd.ObtenerColeccion<Registro>("Usuarios");
if(coleccion==null){
throw new Exception("No existe la coleccion Usuarios");
}
FilterDefinitionBuilder<Registro> filterBuilder = new FilterDefinitionBuilder<Registro>();
var filter = filterBuilder.Eq(x=> x.Correo, datos.Correo);

Registro? usuarioExistente =coleccion.Find(filter).FirstOrDefault();
if(usuarioExistente==null){
    return Results.BadRequest($"No existe un usuario con el correo {datos.Correo}");
}
if(usuarioExistente.Contraseña!=datos.Contraseña){
  return Results.BadRequest("La contraseña es incorrecta");
}

return Results.Ok();
    }
    
}