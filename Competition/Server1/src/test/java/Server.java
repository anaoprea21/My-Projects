import DataAccessObj.CompetitorDAO;

import java.io.*;
import java.net.Inet4Address;
import java.net.ServerSocket;
import java.net.Socket;
import java.sql.SQLException;
import java.util.Map;
import java.util.Scanner;
import java.util.Set;
import java.util.Vector;

public class Server {
    static String IP = "192.168.1.3";
    static Integer PORT = 8888;
    public static void main(String[] args) throws SQLException {
        ServerSocket serverSocket = null;
        try {
            serverSocket=new ServerSocket(PORT,10, Inet4Address.getByName((IP)));
        } catch (IOException e) {
            e.printStackTrace();
        }

        while(true){
            try {
                Socket clientSocket = serverSocket.accept();
                ServeAction action = new ServeAction(clientSocket);
                Thread thread = new Thread(action);
                thread.start();
            } catch (IOException e) {
                e.printStackTrace();
            }
        }
    }
}

class ServeAction implements Runnable{
    private Socket clientSocket;
    private DataInputStream data;
    private DataOutputStream outMess;

    ServeAction(Socket client){
        this.clientSocket = client;
    }

    public void serve() throws IOException, SQLException, ClassNotFoundException {
        ServerToDB obj = new ServerToDB();

        InputStream input = clientSocket.getInputStream();
        DataInputStream dataIn = new DataInputStream(input);
        String userLogin = dataIn.readUTF();
        //login check
        String ok = obj.requestLogin(userLogin);
        OutputStream out = clientSocket.getOutputStream();
        DataOutputStream okMessage = new DataOutputStream(out);
        okMessage.writeUTF(ok);
        okMessage.flush();

        while(true){
            InputStream in = clientSocket.getInputStream();
            DataInputStream data = new DataInputStream(in);
            String user = data.readUTF();
            String requestStr = data.readUTF();
            Integer request = Integer.valueOf(requestStr);

            switch (request) {
                case 0:
                    if (user.equals("admin")) {
                        String name = data.readUTF();
                        String surname = data.readUTF();
                        String message = obj.makeInsertionOfMember(name, surname);
                        DataOutputStream dataOut = new DataOutputStream(out);
                        dataOut.writeUTF(String.valueOf(message));
                        dataOut.flush();
                        break;
                    } else {
                        Set set = obj.makeTeamList();
                        ObjectOutputStream objOutStr = new ObjectOutputStream(out);
                        objOutStr.writeObject(set);
                        objOutStr.flush();
                        break;
                    }
                case 1:
                    if (user.equals("admin")) {
                        String id = data.readUTF();
                        String idRequest = data.readUTF();
                        String change = data.readUTF();
                        if (Integer.valueOf(idRequest) == 0 || Integer.valueOf(idRequest) == 1) {
                            String confirmation = obj.makePlayerInfoUpdate(id, idRequest, change);
                            DataOutputStream confirm = new DataOutputStream(out);
                            confirm.writeUTF(confirmation);
                            confirm.flush();
                        } else {
                            DataOutputStream warning = new DataOutputStream(out);
                            warning.writeUTF("Cerere invalida!");
                            warning.flush();
                        }
                        break;
                    } else {
                        String teamStr = data.readUTF();
                        Integer team = Integer.valueOf(teamStr);
                        Set list = obj.makeTeamMembersList(team);
                        ObjectOutputStream listOut = new ObjectOutputStream(out);
                        listOut.writeObject(list);
                        listOut.flush();
                        break;
                    }
                case 2:
                    if (user.equals("admin")) {
                        String id = data.readUTF();
                        String mess = obj.requestPlayerDeletion(Integer.valueOf(id));
                        DataOutputStream output = new DataOutputStream(out);
                        output.writeUTF(mess);
                        output.flush();
                        break;
                    } else {
                        String idStr = data.readUTF();
                        Map map = obj.makePlayerInfoList(Integer.valueOf(idStr));
                        ObjectOutputStream info = new ObjectOutputStream(out);
                        info.writeObject(map);
                        info.flush();
                        break;
                    }
                case 3:
                    if (user.equals("admin")) {
                        String teamName = data.readUTF();
                        ObjectInputStream inMembers = new ObjectInputStream(in);
                        Set members = (Set) inMembers.readObject();
                        String mess = obj.makeTeamRegistration(members, teamName);
                        DataOutputStream outMess = new DataOutputStream(out);
                        outMess.writeUTF(String.valueOf(mess));
                        outMess.flush();
                        break;
                    } else {
                        String scoreStr = data.readUTF();
                        Integer score = Integer.valueOf(scoreStr);
                        String etapa = data.readUTF();
                        String outMessage = obj.makeScoreRegistration(user, score,Integer.valueOf(etapa));
                        DataOutputStream message = new DataOutputStream(out);
                        message.writeUTF(outMessage);
                        message.flush();
                        break;
                    }
                case 4:
                    if (user.equals("admin")) {
                        String id = data.readUTF();
                        String idTeam = data.readUTF();
                        String mess = obj.requestAddingMember(Integer.valueOf(id), Integer.valueOf(idTeam));
                        DataOutputStream output = new DataOutputStream(out);
                        output.writeUTF(mess);
                        output.flush();
                        break;
                    } else {
                        String etapaStr = data.readUTF();
                        Integer etapa = Integer.valueOf(etapaStr);
                        Map topEtapa = obj.makeTopEtapa(etapa);
                        ObjectOutputStream objOut = new ObjectOutputStream(out);
                        objOut.writeObject(topEtapa);
                        objOut.flush();
                        break;
                    }
                case 5:
                    if (user.equals("admin")) {
                        String id = data.readUTF();
                        String idTeam = data.readUTF();
                        String mess = obj.requestEliminatingMember(Integer.valueOf(id), Integer.valueOf(idTeam));
                        DataOutputStream output = new DataOutputStream(out);
                        output.writeUTF(mess);
                        output.flush();
                        break;
                    } else {
                        Map scores = obj.makeScoreBoard();
                        ObjectOutputStream outScores = new ObjectOutputStream(out);
                        outScores.writeObject(scores);
                        outScores.flush();
                        break;
                    }
                case 6:
                    if (user.equals("admin")) {
                        String idTeam = data.readUTF();
                        String name = data.readUTF();
                        String mess = obj.requestRenamingTeam(name, Integer.valueOf(idTeam));
                        DataOutputStream output = new DataOutputStream(out);
                        output.writeUTF(mess);
                        output.flush();
                        break;
                    }
                case 7:
                    if (user.equals("admin")) {
                        String idTeam = data.readUTF();
                        String mess = obj.requestEliminatingTeam(Integer.valueOf(idTeam));
                        DataOutputStream output = new DataOutputStream(out);
                        output.writeUTF(mess);
                        output.flush();
                        break;
                    }
                case 8:
                    if (user.equals("admin")) {
                        String etapa = data.readUTF();
                        String mess = obj.requestAddingEtapa(etapa);
                        DataOutputStream output = new DataOutputStream(out);
                        output.writeUTF(mess);
                        output.flush();
                        break;
                    }
                case 9:
                    if (user.equals("admin")) {
                        String idEtapa = data.readUTF();
                        String name = data.readUTF();
                        String mess = obj.requestRenamingEtapa(name, Integer.valueOf(idEtapa));
                        DataOutputStream output = new DataOutputStream(out);
                        output.writeUTF(mess);
                        output.flush();
                        break;
                    }
                case 10:
                    if (user.equals("admin")) {
                        String id = data.readUTF();
                        String mess = obj.requestEliminatingEtapa(Integer.valueOf(id));
                        DataOutputStream output = new DataOutputStream(out);
                        output.writeUTF(mess);
                        output.flush();
                        break;
                    }
            }
        }
    }


    @Override
    public void run() {
        try {
            try {
                serve();
            } catch (ClassNotFoundException e) {
                e.printStackTrace();
            }
        } catch (IOException | SQLException e) {
            e.printStackTrace();
        }
        try {
            clientSocket.close();
        }
        catch (IOException e) {
            e.printStackTrace();
        }
    }

}