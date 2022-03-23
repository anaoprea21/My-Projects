package DataAccessObj;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.*;

public class CompetitorDAO {
    private Connection connection;
    public CompetitorDAO() throws SQLException {
        ConnectionToDB serverToDB = new ConnectionToDB();
        connection= serverToDB.getConnection();
    }
    public Map getScoreBoard() throws SQLException {

        Map <String,Integer> map = new HashMap<>();

        String SQL = "SELECT s.scor AS scor, c.nume AS nume, c.prenume AS prenume FROM scor AS s JOIN competitor AS c \n" +
                "ON s.id_competitor = c.id_competitor";
        PreparedStatement statem = connection.prepareStatement(SQL);
        ResultSet scores = statem.executeQuery();

        while(scores.next()) {
            String name = scores.getString("nume") + " " + scores.getString("prenume");
            String scor = scores.getString("scor");

            if (map.containsKey(name)) {
                Integer aux = Integer.valueOf(map.get(name));
                aux = aux + Integer.valueOf(scor);

                map.replace(name, aux);
            }
            else {
                map.put(name, Integer.valueOf(scor));
            }

        }

        LinkedHashMap<String, Integer> sortedMap = new LinkedHashMap<>();
        map.entrySet().stream().sorted(Map.Entry.comparingByValue(Comparator.reverseOrder()))
                .forEachOrdered(x -> sortedMap.put(x.getKey(), x.getValue()));

        return sortedMap;
    }
    public String insertMember(String name, String surname) throws SQLException {

        String SQL = "INSERT INTO competitor(prenume,nume) VALUES (?,?);";
        PreparedStatement statement = connection.prepareStatement(SQL);
        statement.setString(1,name);
        statement.setString(2,surname);
        statement.executeUpdate();

        return "Inscriere reusita!";
    }

    public String insertScore(String user, Integer score, Integer etapa) throws SQLException {

        String aux = "SELECT id_competitor FROM competitor WHERE nume = (?)";
        PreparedStatement auxStatement = connection.prepareStatement(aux);
        auxStatement.setString(1,user);
        ResultSet set = auxStatement.executeQuery();
        Integer id = null;
        while(set.next()){
            id = set.getInt("id_competitor");
        }

        String SQL = "INSERT INTO scor(id_etapa,id_competitor,scor) VALUES (?,?,?);";
        PreparedStatement statement = connection.prepareStatement(SQL);
        statement.setInt(1,etapa);
        statement.setInt(2,id);
        statement.setInt(3,score);

        statement.executeUpdate();

        return "Inregistrare scor reusita!";
    }

    public Map getPlayerInfoList(Integer id) throws SQLException {

        Map<String, String> map = new HashMap<>();

        String SQL = "SELECT * FROM competitor WHERE id_competitor = (?)";
        PreparedStatement statement = connection.prepareStatement(SQL);
        statement.setInt(1, id);
        ResultSet result = statement.executeQuery();

        while(result.next()){

            String surname = result.getString("nume");
            map.put("nume",surname);
            String name = result.getString("prenume");
            map.put("prenume",name);
        }

        return map;
    }

    public String updatePlayer(String id, String idRequest, String change) throws SQLException {

        if(Integer.valueOf(idRequest) == 0){
            String sql = "UPDATE competitor \n" +
                    "SET nume = (?)\n" +
                    "WHERE id_competitor = (?)";
            PreparedStatement statement = connection.prepareStatement(sql);
            statement.setString(1,change);
            statement.setInt(2, Integer.parseInt(id));
            statement.executeUpdate();

            return "Actualizare nume efectuata";
        }
        else{
            String sql = "UPDATE competitor \n" +
                    "SET prenume = (?)\n" +
                    "WHERE id_competitor = (?)";
            PreparedStatement statement = connection.prepareStatement(sql);
            statement.setString(1,change);
            statement.setInt(2, Integer.parseInt(id));
            statement.executeUpdate();

            return "Actualizare prenume efectuata";
        }
    }

    public String makePlayerDeletion(Integer id) throws SQLException {

        String SQL = "DELETE FROM competitor\n" +
                "WHERE id_competitor =(?)";
        PreparedStatement statement = connection.prepareStatement(SQL);
        statement.setInt(1,id);
        statement.executeUpdate();

        return "Stergere reusita";
    }

    public String addMember(Integer id, Integer idTeam) throws SQLException {

        String sql = "SELECT COUNT (id_echipa)\n" +
                "FROM echipa_competitor\n" +
                "WHERE id_echipa = ?";
        PreparedStatement statem = connection.prepareStatement(sql);
        statem.setInt(1,idTeam);
        ResultSet count = statem.executeQuery();
        Integer number = null;
        while(count.next())
        {
            number = count.getInt(1);
        }

        if(number == 5) {
            String SQL = "INSERT INTO echipa_competitor(id_echipa, id_competitor) VALUES (?,?);";
            PreparedStatement statement = connection.prepareStatement(SQL);
            statement.setInt(1, idTeam);
            statement.setInt(2, id);
            statement.executeUpdate();

            return "Adaugare reusita";
        }

        return "Adaugare imposibila, echipa va trece de numarul maxim de membrii";
    }

    public String eliminateMember(Integer id, Integer idTeam) throws SQLException {

        String sql = "SELECT COUNT (id_echipa)\n" +
                "FROM echipa_competitor\n" +
                "WHERE id_echipa = ?";
        PreparedStatement statem = connection.prepareStatement(sql);
        statem.setInt(1, idTeam);
        ResultSet count = statem.executeQuery();
        Integer number = null;
        while (count.next()) {
            number = count.getInt(1);
        }

        if (number > 2) {
            String SQL = "DELETE FROM echipa_competitor\n" +
                    "WHERE id_competitor = ?";
            PreparedStatement statement = connection.prepareStatement(SQL);
            statement.setInt(1, id);
            statement.executeUpdate();

            return "Eliminare reusita";
        }

        return "Eliminare imposibila, echipa nu va avea numarul minim de membrii";
    }

    public String checkLogin(String user) throws SQLException {
        if(user.equals("admin")) {
            return "Logare reusita";
        }
        else {
            String sql = "SELECT COUNT (id_competitor)\n" +
                    "FROM competitor\n" +
                    "WHERE nume = ?";
            PreparedStatement statem = connection.prepareStatement(sql);
            statem.setString(1, user);
            ResultSet count = statem.executeQuery();
            Integer number = null;
            while (count.next()) {
                number = count.getInt(1);
            }

            if (number == 0) {
                return "Nu exista acest competitor";
            }
            else {
                return "Logare reusita";
            }
        }
    }
}
