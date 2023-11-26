using MongoDB.Driver;
using MongoDB.Bson;
using System.Text.RegularExpressions;
public class LenguajeRequestHandler{
public static IResult ListaRegistros(string IdCategoria){
        var filterBuilder=new FilterDefinitionBuilder<LenguajeDbMap>();
        var filter = filterBuilder.Eq(x=>x.IdCategoria, IdCategoria);
   
   BaseDatos bd=new BaseDatos();
   var coleccion=bd.ObtenerColeccion<LenguajeDbMap>("Lenguaje");
   var lista=coleccion.Find(filter).ToList();

return Results.Ok(lista.Select(x => new {
    Id=x.Id.ToString(),
    IdCategoria=x.IdCategoria,
    Titulo=x.Titulo,
    Descripcion=x.Descripcion,
    EsVideo=x.EsVideo,
    Url=x.Url
}).ToList());
       }
    

 public static IResult Eliminar(string id){


    if(!ObjectId.TryParse(id,out ObjectId idLenguaje)){
        return Results.BadRequest($"El id proporcionar ({id})no es valido");
    }
    BaseDatos bd = new BaseDatos();
    var filterBuilder= new FilterDefinitionBuilder<LenguajeDbMap>();
    var filter = filterBuilder.Eq(x=> x.Id, idLenguaje);
    var coleccion = bd.ObtenerColeccion<LenguajeDbMap>("Lenguaje");
    coleccion!.DeleteOne(filter);

    return Results.NoContent();
 }
public static IResult CrearRegistro(LenguajeDTO dto){
    if(string.IsNullOrWhiteSpace(dto.IdCategoria)){
        return Results.BadRequest("Escriba la id de la categoria");
    }else
    if(dto.IdCategoria.Length!=24){
        return Results.BadRequest("El formato de la id es incorrecto");
        
    }
    if(!ObjectId.TryParse(dto.IdCategoria,out ObjectId IdCategoria)){
        return Results.BadRequest($"El id de la categoria ({dto.IdCategoria}) no es válido");

    }
    BaseDatos bd = new BaseDatos();
    var filterBuilderCategorias=new FilterDefinitionBuilder<CategoriaDbMap>();
    var filterCategoria=filterBuilderCategorias.Eq(x=> x.Id, IdCategoria);
    var coleccionCategoria=bd.ObtenerColeccion<CategoriaDbMap>("Categorias");
    var categoria= coleccionCategoria.Find(filterCategoria).FirstOrDefault();
     
     if(categoria == null ){
        return Results.NotFound($"No existe una categoria con ID]='{dto.IdCategoria}'");
     }
    if(string.IsNullOrWhiteSpace(dto.Titulo)){
        return Results.BadRequest("El titulo no existe");
    
    }       
if(string.IsNullOrWhiteSpace(dto.Descripcion)){
        return Results.BadRequest("La descripcion no existe");
    
 } if(string.IsNullOrWhiteSpace(dto.Url)){
    
        return Results.BadRequest("La Url de la categoria no existe");
    } 
    LenguajeDbMap registro=new LenguajeDbMap();
    registro.Titulo=dto.Titulo;
    registro.EsVideo=dto.EsVideo;
    registro.Descripcion=dto.Descripcion;
     registro.Url=dto.Url;
     registro.IdCategoria=dto.IdCategoria;
     
     var colecconLenguaje= bd.ObtenerColeccion<LenguajeDbMap>("Lenguaje");
     colecconLenguaje!.InsertOne(registro);

     return Results.Ok(registro.Id.ToString());
    }
    public static IResult Buscar(string texto){
        var queryExpr= new BsonRegularExpression(new Regex(texto, RegexOptions.IgnoreCase));
        var filterBuilder = new FilterDefinitionBuilder<LenguajeDbMap>();
        var filter = filterBuilder.Regex("Titulo", queryExpr);
        filterBuilder.Regex("Descripcion", queryExpr);

        BaseDatos bd = new BaseDatos();
        var coleccion = bd.ObtenerColeccion<LenguajeDbMap>("Lenguaje");
        var lista = coleccion.Find(filter).ToList();

        return Results.Ok(lista.Select(x => new {
            id = x.Id.ToString(),
            IdCategoria=x.IdCategoria,
            Titulo=x.Titulo,
            Descripcion= x.Descripcion,
            EsVideo=x.EsVideo,
            Url=x.Url
        }).ToList());
    }
}

