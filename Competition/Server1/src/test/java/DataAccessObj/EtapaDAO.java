package DataAccessObj;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.*;

public class EtapaDAO {
    private Connection connection;

    public EtapaDAO() throws SQLException {
        ConnectionToDB serverToDB = new ConnectionToDB();
        connection= serverToDB.getConnection();
    }

    public Map getTopEtapa(Integer idEtapaActuala) throws SQLException {

        Map<String, Integer> map = new HashMap<>();

        String SQL = "SELECT c.nume, s.scor FROM scor AS s JOIN competitor AS c ON c.id_competitor = s.id_competitor WHERE s.id_etapa=(?)";
        PreparedStatement statement = connection.prepareStatement(SQL);
        statement.setInt(1,idEtapaActuala);
        ResultSet set = statement.executeQuery();

        while(set.next()) {
            String name = set.getString("nume");
            Integer score = set.getInt("scor");
            map.put(name, score);
        }

        LinkedHashMap<String, Integer> sortedMap = new LinkedHashMap<>();
        map.entrySet().stream().sorted(Map.Entry.comparingByValue(Comparator.reverseOrder()))
                .forEachOrdered(x -> sortedMap.put(x.getKey(), x.getValue()));

        return sortedMap;
    }

    public String addEtapa(String etapa) throws SQLException {
        String sql = "INSERT INTO etapa(nume) VALUES (?);";

        PreparedStatement statement = connection.prepareStatement(sql);
        statement.setString(1, etapa);
        statement.executeUpdate();

        return "Creare de etapa reusita";
    }

    public String renameEtapa(String name, Integer id) throws SQLException {
        String sql = "UPDATE etapa \n" +
                "SET nume = (?)\n" +
                "WHERE id_etapa = (?)";
        PreparedStatement statement = connection.prepareStatement(sql);
        statement.setString(1,name);
        statement.setInt(2, id);
        statement.executeUpdate();

        return "Redenumire etapa reusita";
    }

    public String eliminateEtapa(Integer id) throws SQLException {
        String SQL = "DELETE FROM etapa\n" +
                "WHERE id_etapa = ?";
        PreparedStatement statement = connection.prepareStatement(SQL);
        statement.setInt(1, id);
        statement.executeUpdate();

        return "Eliminare reusita";
    }
}
