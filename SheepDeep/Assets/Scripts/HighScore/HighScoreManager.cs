using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System;

public class HighScoreManager {

    public struct HighScoreObject {
        public string name;
        public int elapsedtime;
    }

    public enum HighScoreType {
        WOLF,
        SHEPARD
    }

    private string DB_PATH;

    public HighScoreManager() {
        DB_PATH = "URI=file:" + Application.persistentDataPath + "/sheepDeep.db";

        createShema();
    }

    private void createShema() {
        Debug.Log("Creating DB File");

        using(var conn = new SqliteConnection(DB_PATH)) {
            conn.Open();

            using (var cmd = conn.CreateCommand()) {
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "CREATE TABLE IF NOT EXISTS 'high_score_wolf' ( " +
					                  "  'id' INTEGER PRIMARY KEY, " +
					                  "  'name' TEXT NOT NULL, " +
					                  "  'time_s' INTEGER NOT NULL" +
					                  ");";

				var result = cmd.ExecuteNonQuery();
				Debug.Log("create schema wolf: " + result);

                cmd.CommandText = "CREATE TABLE IF NOT EXISTS 'high_score_shepart' ( " +
                                        "  'id' INTEGER PRIMARY KEY, " +
					                    "  'name' TEXT NOT NULL, " +
					                    "  'time_s' INTEGER NOT NULL" +
					                    ");";
                
                result = cmd.ExecuteNonQuery();
                Debug.Log("create schema shepard: " + result);
            }
        }
    }

    public void insertIntoTable(HighScoreObject highScore, HighScoreType type) {
        string tableName;
        if(type == HighScoreType.WOLF){
            tableName = "high_score_wolf";
        }else {
            tableName = "high_score_shepart";
        }

        using(var conn = new SqliteConnection(DB_PATH)) {
            conn.Open();

            SqliteCommand command = new SqliteCommand("INSERT INTO " + tableName + "(name, time_s) VALUES (@name, @time_s)", conn);
            command.Parameters.Add(new SqliteParameter {
						ParameterName = "name",
						Value = highScore.name
					});
            command.Parameters.Add(new SqliteParameter {
						ParameterName = "time_s",
						Value = highScore.elapsedtime
					});

            try {
                command.ExecuteNonQuery();
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }   
        }
    }

    public ArrayList getHighScores(int limit, HighScoreType type) {
        string tableName;
        if(type == HighScoreType.WOLF){
            tableName = "high_score_wolf";
        }else {
            tableName = "high_score_shepart";
        }

        ArrayList scores = new ArrayList();
		using (var conn = new SqliteConnection(DB_PATH)) {
			conn.Open();
			using (var cmd = conn.CreateCommand()) {
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = "SELECT * FROM " + tableName +" ORDER BY time_s ASC LIMIT @Count;";

				cmd.Parameters.Add(new SqliteParameter {
					ParameterName = "Count",
					Value = limit
				});

				Debug.Log("scores (begin)");
				var reader = cmd.ExecuteReader();
				while (reader.Read()) {
					var id = reader.GetInt32(0);
                    HighScoreObject highScore = new HighScoreObject();
                    highScore.name = reader.GetString(1);
                    highScore.elapsedtime = reader.GetInt32(2);
                    scores.Add(highScore);
				}
				Debug.Log("scores (end)");
			}
		}
        Debug.Log("Scores: " + scores.ToString());
        return scores;
	}
}
