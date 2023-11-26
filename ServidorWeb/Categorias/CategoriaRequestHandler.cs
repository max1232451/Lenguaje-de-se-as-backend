using MongoDB.Driver;
using MongoDB.Bson;
public static class CategoriaRequestHandler{
   public static IResult Crear(CategoriaDTO dto){

  if(string.IsNullOrWhiteSpace(dto.Nombre)){
         return Results.BadRequest("Se debe ingresar nombre a la categoria");
      }

      if(string.IsNullOrWhiteSpace(dto.UrlIcono)){
         return Results.BadRequest("Se debe ingresar un icono  a la categoria");
      }

    
    
var filterBuilder =new FilterDefinitionBuilder<CategoriaDbMap>();
var filter= filterBuilder.Eq(x=>x.Nombre ,dto.Nombre);

BaseDatos bd = new BaseDatos();
var coleccion =bd.ObtenerColeccion<CategoriaDbMap>("Categorias");
CategoriaDbMap? registro = coleccion.Find(filter).FirstOrDefault();

if(registro != null){
    return Results.BadRequest($"La categoria '{dto.Nombre}'ya existe en la base de datos");

}
registro = new CategoriaDbMap();
registro.Nombre = dto.Nombre;
registro.UrlIcono = dto.UrlIcono;


coleccion!.InsertOne(registro);
string nuevoId = registro.Id.ToString();

return Results.Ok(nuevoId);
   } 
   public static IResult Listar(){
      var filterBuilder=new FilterDefinitionBuilder<CategoriaDbMap>();
      var filter = filterBuilder.Empty;

      BaseDatos bd = new BaseDatos();
      var coleccion = bd.ObtenerColeccion<CategoriaDbMap>("Categorias");
      List<CategoriaDbMap> mongoDbList = coleccion.Find(filter).ToList();
      
      var lista = mongoDbList.Select(x => new {
         Id = x.Id.ToString(),
         Nombre = x.Nombre,
         UrlIcono = x.UrlIcono
      }).ToList();

      return Results.Ok(lista);
   }

   public static IResult Eliminar(string id){


    if(!ObjectId.TryParse(id,out ObjectId IdCategoria)){
        return Results.BadRequest($"El id proporcionar ({id})no es valido");
    }
    BaseDatos bd = new BaseDatos();
    var filterBuilder= new FilterDefinitionBuilder<CategoriaDbMap>();
    var filter = filterBuilder.Eq(x=> x.Id, IdCategoria);
    var coleccion = bd.ObtenerColeccion<CategoriaDbMap>("Categorias");
    coleccion!.DeleteOne(filter);

    return Results.NoContent();
 }
}