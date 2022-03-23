package DataAccessObj;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;

public class ConnectionToDB {
    private static String USERNAME;
    private static String PASSWORD;
    private static String URL;

    public ConnectionToDB(){
        this.USERNAME = "postgres";
        this.PASSWORD = "Trauma2189";
        this.URL = "jdbc:postgresql://localhost:5432/Competitie";
    }

    public Connection getConnection() throws SQLException {
            Connection connection = DriverManager.getConnection(URL,USERNAME,PASSWORD);
            return connection;
    }
}
