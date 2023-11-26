using MongoDB.Bson;

public  class Registro{
    public ObjectId Id {get;set;}
public string Correo {get; set;}= string.Empty;
public string Nombre {get; set;}= string.Empty;
public string Contraseña {get; set;}= string.Empty;
}