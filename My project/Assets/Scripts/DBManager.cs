using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;
using UnityEngine.UI;
using TMPro;


public class DBManager : MonoBehaviour
{
    private string dbUri = "URI=file:mydb.sqlite";
    private string SQL_COUNT_ELEMENTS = "SELECT count(*) FROM Jugadores;";
    private string SQL_CREATE_JUGADORES  = "CREATE TABLE IF NOT EXISTS Jugadores (Id INTEGER PRIMARY KEY, Nombre TEXT NOT NULL, Cualidad TEXT NOT NULL, Arma INTEGER REFERENCES Armas(Id), Apodo TEXT);";
    private string SQL_CREATE_ARMAS = "CREATE TABLE IF NOT EXISTS Armas (Id INTEGER PRIMARY KEY, Nombre TEXT NOT NULL, Tipo TEXT NOT NULL DEFAULT 'Metalica');";
    private string[] NOMBRE_JUGADORES = {"Alaric", "Bodil", "Edme", "Hawise", "Orla", "Arthur"};
    private string[] CUALIDADES_JUGADORES = {"Atrevido", "Miedoso", "Arriesgado", "Intrepido", "Despistado", "Cuidadoso"};
    private string[] ARMAS = {"Hacha", "Horca", "Maza", "Lanza", "Espada", "Escudo"};
    private string[] TIPO = {"Metalica", "Madera", "Plastico", "Piedra", "Cristal"};

    private int NUM_JUGADORES = 11;

    public TMP_InputField inputField; 

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        IDbConnection dbConnection = OpenDatabase();
        InitializeDB(dbConnection);
        //string apodo = inputField.text; //coge el dato del usuario, habia anadido el script al inputField y despues asociado el object pero no aceptaba bien la informacion. Tengo el codigo para insertar la info en comentarios
        AddData(dbConnection); //AddData(dbConnection, apodo);
        searchByArma(dbConnection, "Espada");
        UpdateJugador(dbConnection, "Alaric", "Valiente"); //Los personajes que se llamen Alaric quiero que sean valientes
        DeleteJugador(dbConnection, "Arthur"); 

        dbConnection.Close();
        Debug.Log("End");
    }

    private void searchByArma(IDbConnection dbConnection, string arma)
    {
        IDbCommand dbCmd = dbConnection.CreateCommand();
        dbCmd.CommandText = $"SELECT Id FROM ARMAS WHERE Nombre ='{arma}';"; // si pongo * el orden en el que me devuelvan los datos será el de CREATE 
        IDataReader reader = dbCmd.ExecuteReader();
        if (!reader.Read())
        {
            return;
        }
        int id_arma = reader.GetInt32(0);
        reader.Close();

        dbCmd.CommandText = $"SELECT * FROM Jugadores WHERE Arma ='{id_arma}';"; // si pongo * el orden en el que me devuelvan los datos será el de CREATE 
        reader = dbCmd.ExecuteReader();
        string jugadores = "";
        while(reader.Read())
        {
            jugadores += $"{reader.GetInt32(0)}, {reader.GetString(1)}, {reader.GetString(2)}\n";
        }
        Debug.Log(jugadores);
    }
    private IDbConnection OpenDatabase()
    {
        IDbConnection dbConnection = new SqliteConnection(dbUri);

        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = "PRAGMA foreign_keys = ON";
        dbCommand.ExecuteNonQuery();

        dbConnection.Open();
        return dbConnection;
    }

    private void InitializeDB(IDbConnection dbConnection)
    {
        IDbCommand dbCmd = dbConnection.CreateCommand();
        dbCmd.CommandText = SQL_CREATE_JUGADORES+SQL_CREATE_ARMAS;
        dbCmd.ExecuteReader();
    }

    private void AddData(IDbConnection dbConnection) //private void AddData(IDbConnection dbConnection, string apodo)
    {
        if (CountNumberElements(dbConnection) > 0)
        {
            return;
        }
        int num_armas = ARMAS.Length;
        string command = "INSERT INTO Armas (Nombre,Tipo) VALUES";
        System.Random rnd = new System.Random();
        for (int i = 0; i<num_armas; i++)
        {
            string tipo = TIPO[rnd.Next(TIPO.Length)];
            command += $"('{ARMAS[i]}', '{tipo}'),";
        }
        command = command.Remove(command.Length - 1, 1);
        command += ";";
        command += "INSERT INTO Jugadores (Nombre,Cualidad,Arma,Apodo) VALUES";
        for (int i = 0; i<NUM_JUGADORES; i++)
        {
            string nombre = NOMBRE_JUGADORES[rnd.Next(NOMBRE_JUGADORES.Length)];
            string cualidad = CUALIDADES_JUGADORES[rnd.Next(CUALIDADES_JUGADORES.Length)];
            int armas = rnd.Next(num_armas) + 1;
            command += $"('{nombre}','{cualidad}', '{armas}'),"; //command += $"('{nombre}','{cualidad}', '{armas}', '{apodo}'),";
        }
        command = command.Remove(command.Length - 1, 1);
        command += ";";
        //Debug.Log(command);
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = command;
        dbCommand.ExecuteNonQuery();
    }

    private void UpdateJugador(IDbConnection dbConnection, string nombreJugador, string nuevaCualidad)
    {
        IDbCommand dbCmd = dbConnection.CreateCommand();
        dbCmd.CommandText = $"UPDATE Jugadores SET Cualidad = '{nuevaCualidad}' WHERE Nombre = '{nombreJugador}';";
        dbCmd.ExecuteNonQuery();
    }

    private void DeleteJugador(IDbConnection dbConnection, string nombreJugador)
    {
        IDbCommand dbCmd = dbConnection.CreateCommand();
        dbCmd.CommandText = $"DELETE FROM Jugadores WHERE Nombre = '{nombreJugador}';";
        dbCmd.ExecuteNonQuery();
    }

    private int CountNumberElements(IDbConnection dbConnection)
    {
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = SQL_COUNT_ELEMENTS;
        IDataReader reader = dbCommand.ExecuteReader();
        reader.Read();
        return reader.GetInt32(0); //hay todo tipo de gets

    }
    
}


