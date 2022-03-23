import DataAccessObj.CompetitorDAO;
import DataAccessObj.EchipaDAO;
import DataAccessObj.EtapaDAO;

import java.sql.SQLException;
import java.util.Map;
import java.util.Set;

public class ServerToDB {
    private CompetitorDAO competitor;
    private EchipaDAO echipa;
    private EtapaDAO etapa;

    public ServerToDB() throws SQLException {
        competitor = new CompetitorDAO();
        etapa = new EtapaDAO();
        echipa = new EchipaDAO();
    }

    public Map makeTopEtapa(Integer nrEtapa) throws SQLException {

        return etapa.getTopEtapa(nrEtapa);
    }

    public String requestAddingEtapa(String etapa) throws SQLException {
        return this.etapa.addEtapa(etapa);
    }

    public Set makeTeamList() throws SQLException {

        return echipa.getTeamList();
    }

    public Set makeTeamMembersList(Integer team) throws SQLException {
        return echipa.getTeamMembers(team);
    }

    public String makeInsertionOfMember(String name, String surname) throws SQLException {
        return competitor.insertMember(name, surname);
    }

    public String makeTeamRegistration(Set members, String name) throws SQLException {
        return echipa.insertTeam(members, name);
    }

    public String makeScoreRegistration(String user, Integer score, Integer etapa) throws SQLException {
        return competitor.insertScore(user, score, etapa);
    }

    public Map makeScoreBoard() throws SQLException {
        return competitor.getScoreBoard();
    }

    public Map makePlayerInfoList(Integer id) throws SQLException {
        return competitor.getPlayerInfoList(id);
    }

    public String makePlayerInfoUpdate(String id, String idRequest, String change) throws SQLException {
        return competitor.updatePlayer(id, idRequest, change);
    }

    public String requestPlayerDeletion(Integer id) throws SQLException {
        return competitor.makePlayerDeletion(id);
    }

    public String requestAddingMember(Integer id, Integer idTeam) throws SQLException {
        return competitor.addMember(id, idTeam);
    }

    public String requestEliminatingMember(Integer id, Integer idTeam) throws SQLException {
        return competitor.eliminateMember(id, idTeam);
    }

    public String requestEliminatingTeam(Integer idTeam) throws SQLException {
        return echipa.eliminateTeam(idTeam);
    }

    public String requestRenamingTeam(String name, Integer id) throws SQLException {
        return echipa.renameTeam(name,id);
    }

    public String requestRenamingEtapa(String name, Integer id) throws SQLException {
        return etapa.renameEtapa(name,id);
    }

    public String requestEliminatingEtapa(Integer id) throws SQLException {
        return etapa.eliminateEtapa(id);
    }

    public String requestLogin(String user) throws SQLException {
        return competitor.checkLogin(user);
    }
}
