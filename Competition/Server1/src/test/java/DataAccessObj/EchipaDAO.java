package DataAccessObj;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.HashSet;
import java.util.Set;

public class EchipaDAO {
    private Connection connection;
    public EchipaDAO() throws SQLException {
        ConnectionToDB serverToDB = new ConnectionToDB();
        connection= serverToDB.getConnection();
    }
    public Set getTeamList() throws SQLException {

        Set<String> list = new HashSet<>();//set is unordered

        String SQL = "SELECT nume FROM echipa";
        PreparedStatement statement = connection.prepareStatement(SQL);
        ResultSet set = statement.executeQuery();
        while(set.next()){
            String name = set.getString("nume");
            list.add(name);
        }
        return list;
    }
    public Set getTeamMembers(Integer team) throws SQLException {


        Set<String> list = new HashSet<>();//set is unordered
        String SQL = "SELECT c.nume AS nume,c.prenume AS prenume FROM competitor AS c\n" +
                "JOIN echipa_competitor AS ec ON ec.id_competitor = c.id_competitor\n" +
                "JOIN echipa AS e ON e.id_echipa = ec.id_echipa \n" +
                "WHERE e.id_echipa = (?)";
        PreparedStatement statement = connection.prepareStatement(SQL);

        statement.setInt(1,team);
        ResultSet set = statement.executeQuery();
        while(set.next()){
            String surname = set.getString("nume");
            String name = set.getString("prenume");
            list.add(surname+" "+name);
        }
        return list;
    }
    public String insertTeam(Set members, String name) throws SQLException {

        String SQL = "INSERT INTO echipa(nume) VALUES (?);";
        PreparedStatement statement = connection.prepareStatement(SQL);
        statement.setString(1,name);
        statement.executeUpdate();

        String SQL2 = "SELECT id_echipa FROM echipa WHERE nume = (?)";
        PreparedStatement stat = connection.prepareStatement(SQL2);
        stat.setString(1,name);
        ResultSet setId = stat.executeQuery();
        Integer id = null;
        while(setId.next()){
            id = setId.getInt("id_echipa");
        }

        for(Object member : members) {
            String SQL3 = "INSERT INTO echipa_competitor(id_echipa, id_competitor) VALUES (?,?);";
            PreparedStatement statem = connection.prepareStatement(SQL3);
            statem.setInt(1,id);
            statem.setInt(2, (Integer) member);
            statem.executeUpdate();
        }

        return "Inregistrare echipa efectuata";
    }

    public String eliminateTeam(Integer idTeam) throws SQLException {
        String SQL = "DELETE FROM echipa\n" +
                "WHERE id_echipa = ?";
        PreparedStatement statement = connection.prepareStatement(SQL);
        statement.setInt(1, idTeam);
        statement.executeUpdate();

        return "Eliminare reusita";
    }

    public String renameTeam(String name, Integer id) throws SQLException {

        String sql = "UPDATE echipa \n" +
                "SET nume = (?)\n" +
                "WHERE id_echipa = (?)";
        PreparedStatement statement = connection.prepareStatement(sql);
        statement.setString(1,name);
        statement.setInt(2, id);
        statement.executeUpdate();

        return "Redenumire echipa reusita";
    }
}
