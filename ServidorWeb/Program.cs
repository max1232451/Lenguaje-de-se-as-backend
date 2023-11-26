using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
 builder.Services.Configure<JsonOptions>(Options=> 
 Options.SerializerOptions.PropertyNamingPolicy=null); 
builder.Services.AddCors();
var app = builder.Build();
app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.MapGet("/", () => "Hello World!");

app.MapPost("/usuarios/registrar",AlmacenarRequestHandler.Registrar);
app.MapPost("/usuarios/ingresar",IngresarRequestHandler.Ingresar);
app.MapPost("/usuarios/recuperar",VerificarRequestHandler.Verificar);
app.MapPost("/usuarios/categorias",CategoriaRequestHandler.Crear);
app.MapGet("/usuarios/categorias1",CategoriaRequestHandler.Listar);
app.MapGet("/lenguaje/{idCategoria}",LenguajeRequestHandler.ListaRegistros);
app.MapPost("/lenguaje",LenguajeRequestHandler.CrearRegistro);
app.MapDelete("/lenguaje/{id}",LenguajeRequestHandler.Eliminar);
app.MapDelete("/categoria/{id}",CategoriaRequestHandler.Eliminar);
app.MapGet("/lenguaje/buscar",LenguajeRequestHandler.Buscar);
app.Run();
