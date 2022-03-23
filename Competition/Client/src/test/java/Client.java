import java.io.*;
import java.net.Inet4Address;
import java.net.Socket;
import java.net.UnknownHostException;
import java.util.*;

public class Client {
    private static String IP = "192.168.1.3";
    private static Integer PORT = 8888;
    private static DataOutputStream data;

    public static void main(String[] args) {
        try (Socket clientSocket = new Socket(Inet4Address.getByName(IP), PORT)) {
            communicate(clientSocket);
        }
        catch (UnknownHostException e) {
            e.printStackTrace();
        }
        catch (IOException e) {
            e.printStackTrace();
        } catch (ClassNotFoundException e) {
            e.printStackTrace();
        }
    }
    private static void communicate(Socket clientSocket) throws IOException, ClassNotFoundException {

        System.out.println("Logare in cont");
        System.out.println("Introduceti doar numele: ");
        Scanner login = new Scanner(System.in);
        String user = login.nextLine();
        requestLogin(user, clientSocket);
        InputStream in = clientSocket.getInputStream();
        DataInputStream data = new DataInputStream(in);
        String ok = data.readUTF();
        System.out.println(ok);
        if(ok.equals("Logare reusita")) {
            Integer request;
            do {
                System.out.println("-1 = Iesirea din aplicatie");

                if (user.equals("admin")) {
                    System.out.println("0 = Inscriere competitor");
                    System.out.println("1 = Actualizare detalii competitor");
                    System.out.println("2 = Stergere competitor");
                    System.out.println("3 = Inscriere echipa");
                    System.out.println("4 = Adaugare membru intr-o echipa");
                    System.out.println("5 = Eliminare membru dintr-o echipa");
                    System.out.println("6 = Actualizare nume echipa");
                    System.out.println("7 = Stergere echipa");
                    System.out.println("8 = Creare etapa");
                    System.out.println("9 = Actualizare etapa");
                    System.out.println("10 = Stergere etapa");

                } else {
                    System.out.println("0 = Vizualizare lista echipe");
                    System.out.println("1 = Vizualizare lista membrii echipa");
                    System.out.println("2 = Vizualizare detalii competitor");
                    System.out.println("3 = Inscriere scor pentru etapa actuala");
                    System.out.println("4 = Vizualizare top etapa");
                    System.out.println("5 = Vizualizare top competitori");
                }
                Scanner cin = new Scanner(System.in);
                request = cin.nextInt();

                switch (request) {
                    case 0:
                        if (user.equals("admin")) {
                            requestRegistration(user, request, clientSocket);
                            DataInputStream dataIn = new DataInputStream(in);
                            String message = dataIn.readUTF();
                            System.out.println(message);
                            System.out.println();
                            break;
                        } else {
                            requestTeamList(user, request, clientSocket);
                            ObjectInputStream objIn = new ObjectInputStream(in);
                            Set newSet = (Set) objIn.readObject();
                            System.out.println(newSet);
                            System.out.println();
                            break;
                        }
                    case 1:
                        if (user.equals("admin")) {
                            requestPlayerInfoUpdate(user, request, clientSocket);
                            DataInputStream confirm = new DataInputStream(in);
                            System.out.println(confirm.readUTF());
                            System.out.println();
                            break;
                        } else {
                            requestTeamMembers(user, request, clientSocket);
                            ObjectInputStream listIn = new ObjectInputStream(in);
                            Set list = (Set) listIn.readObject();
                            System.out.println(list);
                            System.out.println();
                            break;
                        }
                    case 2:
                        if (user.equals("admin")) {
                            requestPlayerDeletion(user, request, clientSocket);
                            DataInputStream confirm = new DataInputStream(in);
                            System.out.println(confirm.readUTF());
                            System.out.println();
                            break;
                        } else {
                            requestPlayerDetails(user, request, clientSocket);
                            ObjectInputStream info = new ObjectInputStream(in);
                            Map infoMap = (Map) info.readObject();

                            System.out.print("Nume: ");
                            System.out.println(infoMap.get("nume"));
                            System.out.print("Prenume: ");
                            System.out.println(infoMap.get("prenume"));
                            System.out.println();
                            break;
                        }
                    case 3:
                        if (user.equals("admin")) {
                            requestRegistrationTeam(user, request, clientSocket);
                            DataInputStream mess = new DataInputStream(in);
                            String m = mess.readUTF();
                            System.out.println(m);
                            System.out.println();
                            break;
                        } else {
                            requestScoreRegistration(user, request, clientSocket);
                            DataInputStream inMess = new DataInputStream(in);
                            String inmessage = inMess.readUTF();
                            System.out.println(inmessage);
                            System.out.println();
                            break;
                        }
                    case 4:
                        if (user.equals("admin")) {
                            requestAddingMember(user, request, clientSocket);
                            DataInputStream confirm = new DataInputStream(in);
                            System.out.println(confirm.readUTF());
                            System.out.println();
                            break;
                        } else {
                            requestTopEtapa(user, request, clientSocket);
                            ObjectInputStream obj = new ObjectInputStream(in);
                            Map newObj = (Map) obj.readObject();
                            Integer place = 1;
                            for (Object name : newObj.keySet()) {
                                System.out.print(name);
                                System.out.print(" se afla pe locul ");
                                System.out.print(place);
                                place++;
                                System.out.print(" cu punctajul: ");
                                System.out.println(newObj.get(name));
                            }
                            System.out.println();
                            break;
                        }
                    case 5:
                        if (user.equals("admin")) {
                            requestEliminatingMember(user, request, clientSocket);
                            DataInputStream confirm = new DataInputStream(in);
                            System.out.println(confirm.readUTF());
                            System.out.println();
                            break;
                        } else {
                            requestScoreBoard(user, request, clientSocket);
                            ObjectInputStream scores = new ObjectInputStream(in);
                            Map scoreMap = (Map) scores.readObject();
                            Integer place = 1;
                            for (Object name : scoreMap.keySet()) {
                                System.out.print(name);
                                System.out.print(" se afla pe locul ");
                                System.out.print(place);
                                place++;
                                System.out.print(" cu punctajul: ");
                                System.out.println(scoreMap.get(name));
                            }
                            System.out.println();
                            break;
                        }
                    case 6:
                        if (user.equals("admin")) {
                            requestRenamingTeam(user, request, clientSocket);
                            DataInputStream confirm = new DataInputStream(in);
                            System.out.println(confirm.readUTF());
                            System.out.println();
                            break;
                        }
                    case 7:
                        if (user.equals("admin")) {
                            requestEliminatingTeam(user, request, clientSocket);
                            DataInputStream confirm = new DataInputStream(in);
                            System.out.println(confirm.readUTF());
                            System.out.println();
                            break;
                        }
                    case 8:
                        if (user.equals("admin")) {
                            requestAddingEtapa(user, request, clientSocket);
                            DataInputStream confirm = new DataInputStream(in);
                            System.out.println(confirm.readUTF());
                            System.out.println();
                            break;
                        }
                    case 9:
                        if (user.equals("admin")) {
                            requestRenamingEtapa(user, request, clientSocket);
                            DataInputStream confirm = new DataInputStream(in);
                            System.out.println(confirm.readUTF());
                            System.out.println();
                            break;
                        }
                    case 10:
                        if (user.equals("admin")) {
                            requestEliminatingEtapa(user, request, clientSocket);
                            DataInputStream confirm = new DataInputStream(in);
                            System.out.println(confirm.readUTF());
                            System.out.println();
                            break;
                        }
                }
            } while (request != -1);
        }
    }

    private static void requestLogin(String user,Socket clientSocket) throws IOException {
        OutputStream out = clientSocket.getOutputStream();
        DataOutputStream data = new DataOutputStream(out);
        data.writeUTF(user);
        data.flush();
    }

    private static void requestEliminatingEtapa(String user, Integer request, Socket clientSocket) throws IOException {
        System.out.println("Introduceti id-ul etapei: ");
        Scanner in = new Scanner(System.in);
        String id = in.nextLine();

        OutputStream out = clientSocket.getOutputStream();
        DataOutputStream data = new DataOutputStream(out);
        data.writeUTF(user);
        data.writeUTF(String.valueOf(request));
        data.writeUTF(id);
        data.flush();
    }

    private static void requestRenamingEtapa(String user, Integer request, Socket clientSocket) throws IOException {
        System.out.println("Introduceti id-ul etapei: ");
        Scanner in = new Scanner(System.in);
        String id = in.nextLine();
        System.out.println("Introduceti noul nume al etapei: ");
        String name = in.nextLine();

        OutputStream out = clientSocket.getOutputStream();
        DataOutputStream data = new DataOutputStream(out);
        data.writeUTF(user);
        data.writeUTF(String.valueOf(request));
        data.writeUTF(id);
        data.writeUTF(name);
        data.flush();
    }

    private static void requestRenamingTeam(String user, Integer request, Socket clientSocket) throws IOException {
        System.out.println("Introduceti id-ul echipei: ");
        Scanner in = new Scanner(System.in);
        String id = in.nextLine();
        System.out.println("Introduceti noul nume al echipei: ");
        String name = in.nextLine();

        OutputStream out = clientSocket.getOutputStream();
        DataOutputStream data = new DataOutputStream(out);
        data.writeUTF(user);
        data.writeUTF(String.valueOf(request));
        data.writeUTF(id);
        data.writeUTF(name);
        data.flush();
    }

    private static void requestAddingEtapa(String user, Integer request, Socket clientSocket) throws IOException {
        System.out.println("Introduceti numele etapei: ");
        Scanner in = new Scanner(System.in);
        String etapa = in.nextLine();

        OutputStream out = clientSocket.getOutputStream();
        DataOutputStream data = new DataOutputStream(out);
        data.writeUTF(user);
        data.writeUTF(String.valueOf(request));
        data.writeUTF(etapa);
        data.flush();
    }

    private static void requestEliminatingTeam(String user, Integer request, Socket clientSocket) throws IOException {
        System.out.println("Introduceti id-ul echipei: ");
        Scanner in = new Scanner(System.in);
        Integer idTeam = in.nextInt();

        OutputStream out = clientSocket.getOutputStream();
        DataOutputStream data = new DataOutputStream(out);
        data.writeUTF(user);
        data.writeUTF(String.valueOf(request));
        data.writeUTF(String.valueOf(idTeam));
        data.flush();
    }

    private static void requestEliminatingMember(String user, Integer request, Socket clientSocket) throws IOException {
        System.out.println("Introduceti id-ul competitorului: ");
        Scanner in = new Scanner(System.in);
        Integer id = in.nextInt();
        System.out.println("Introduceti id-ul echipei: ");
        Integer idTeam = in.nextInt();

        OutputStream out = clientSocket.getOutputStream();
        DataOutputStream data = new DataOutputStream(out);
        data.writeUTF(user);
        data.writeUTF(String.valueOf(request));
        data.writeUTF(String.valueOf(id));
        data.writeUTF(String.valueOf(idTeam));
        data.flush();
    }

    private static void requestAddingMember(String user, Integer request, Socket clientSocket) throws IOException {

        System.out.println("Introduceti id-ul competitorului: ");
        Scanner in = new Scanner(System.in);
        Integer id = in.nextInt();
        System.out.println("Introduceti id-ul echipei: ");
        Integer idTeam = in.nextInt();

        OutputStream out = clientSocket.getOutputStream();
        DataOutputStream data = new DataOutputStream(out);
        data.writeUTF(user);
        data.writeUTF(String.valueOf(request));
        data.writeUTF(String.valueOf(id));
        data.writeUTF(String.valueOf(idTeam));
        data.flush();
    }

    private static void requestPlayerDeletion(String user, Integer request, Socket clientSocket) throws IOException {

        System.out.println("Introduceti id-ul competitorului: ");
        Scanner in = new Scanner(System.in);
        Integer id = in.nextInt();

        OutputStream out = clientSocket.getOutputStream();
        DataOutputStream data = new DataOutputStream(out);
        data.writeUTF(user);
        data.writeUTF(String.valueOf(request));
        data.writeUTF(String.valueOf(id));
        data.flush();
    }

    private static void requestPlayerInfoUpdate(String user, Integer request, Socket clientSocket) throws IOException {

        System.out.println("Introduceti id-ul competitorului: ");
        Scanner in = new Scanner(System.in);
        Integer id = in.nextInt();

        System.out.println("Selectati ce trebuie schimbat: ");
        System.out.println("0 = numele,  1 = prenumele");
        Integer idRequest = in.nextInt();
        String change = in.nextLine();

        if(idRequest == 0) {
            System.out.println("Introduceti noul nume:");
        }
        else{
            System.out.println("Introduceti noul prenume:");
        }
        change = in.nextLine();

        OutputStream out = clientSocket.getOutputStream();
        DataOutputStream data = new DataOutputStream(out);
        data.writeUTF(user);
        data.writeUTF(String.valueOf(request));
        data.writeUTF(String.valueOf(id));
        data.writeUTF(String.valueOf(idRequest));
        data.writeUTF(change);
        data.flush();
    }

    private static void requestPlayerDetails(String user, Integer request, Socket clientSocket) throws IOException {

        System.out.println("Introduceti id-ul competitorului: ");
        Scanner in = new Scanner(System.in);
        Integer id = in.nextInt();

        OutputStream out = clientSocket.getOutputStream();
        DataOutputStream data = new DataOutputStream(out);
        data.writeUTF(user);
        data.writeUTF(String.valueOf(request));
        data.writeUTF(String.valueOf(id));
        data.flush();
    }

    private static void requestScoreBoard(String user, Integer request, Socket clientSocket) throws IOException {

        System.out.println("Topul competitorilor este: ");

        OutputStream out = clientSocket.getOutputStream();
        DataOutputStream data = new DataOutputStream(out);
        data.writeUTF(user);
        data.writeUTF(String.valueOf(request));
        data.flush();
    }

    private static void requestScoreRegistration(String user, Integer request, Socket clientSocket) throws IOException {

        //System.out.println("Va aflati in etapa 1");//de modificat
        Scanner cin = new Scanner(System.in);

        System.out.println("Introduceti scorul obtinut ");


        Integer score = cin.nextInt();
        cin.nextLine();

        System.out.println("Introduceti etapa ");
        String etapa = cin.nextLine();

        OutputStream out = clientSocket.getOutputStream();
        DataOutputStream data = new DataOutputStream(out);
        data.writeUTF(user);
        data.writeUTF(String.valueOf(request));
        data.writeUTF(String.valueOf(score));
        data.writeUTF(etapa);
        data.flush();
    }

    private static void requestRegistrationTeam(String user, Integer request, Socket clientSocket) throws IOException {

        System.out.println("Introduceti numele echipei ");
        Scanner cin = new Scanner(System.in);
        String name = cin.nextLine();
        Integer index = 0;
        Integer member;
        Set<Integer> members = new HashSet<>();
        System.out.println("Introduceti id-ul a cel putin 2 membrii");
        while(true) {
            member = cin.nextInt();
            if(index >= 2) {
                if (member == 0)
                    break;
                members.add(member);
                index = index + 1;
                if(index == 5) {
                    System.out.println("S-a atins numarul maxim de membrii");
                    break;
                }
                else{
                    System.out.println("Introduceti id-ul unui membru, sau 0 daca nu mai aveti membrii de inscris");
                }
            }
            else {
                members.add(member);
                index = index + 1;
                if(index == 2){
                    System.out.println("Introduceti id-ul unui membru, sau 0 daca nu mai aveti membrii de inscris");
                }
                else {
                    System.out.println("Introduceti id-ul unui membrii");
                }
            }
        }

        OutputStream out = clientSocket.getOutputStream();
        DataOutputStream data = new DataOutputStream(out);
        data.writeUTF(user);
        data.writeUTF(String.valueOf(request));
        data.writeUTF(name);
        data.flush();

        ObjectOutputStream obj = new ObjectOutputStream(out);
        obj.writeObject(members);
        obj.flush();
    }

    private static void requestTeamList(String user, Integer request, Socket clientSocket) throws IOException {

        System.out.print("Echipele sunt: ");
        OutputStream out = clientSocket.getOutputStream();
        DataOutputStream data = new DataOutputStream(out);
        data.writeUTF(user);
        data.writeUTF(String.valueOf(request));
        data.flush();

    }

    private static void requestTopEtapa(String user, Integer request, Socket clientSocket) throws IOException {

        System.out.print("Introduceti etapa: ");
        Scanner cin = new Scanner(System.in);
        Integer etapa = cin.nextInt();

        System.out.print("Topul etapei ");
        System.out.print(etapa);
        System.out.println(" este: ");

        OutputStream out = clientSocket.getOutputStream();
        DataOutputStream data = new DataOutputStream(out);
        data.writeUTF(user);
        data.writeUTF(String.valueOf(request));
        data.writeUTF(String.valueOf(etapa));
        data.flush();
    }

    public static void requestTeamMembers(String user, Integer request, Socket clientSocket) throws IOException {

        System.out.print("Introduceti id-ul echipei: ");
        Scanner keyboard = new Scanner(System.in);
        Integer id = keyboard.nextInt();

        OutputStream out = clientSocket.getOutputStream();
        DataOutputStream data = new DataOutputStream(out);
        data.writeUTF(user);
        data.writeUTF(String.valueOf(request));
        data.writeUTF(String.valueOf(id));
        data.flush();
    }

    public static void requestRegistration(String user, Integer request, Socket clientSocket) throws IOException {

        System.out.println("Nume: ");
        Scanner cin1 = new Scanner(System.in);
        String surname = cin1.nextLine();
        System.out.println("Prenume: ");
        String name = cin1.nextLine();

        OutputStream out = clientSocket.getOutputStream();
        DataOutputStream data = new DataOutputStream(out);
        data.writeUTF(user);

        data.writeUTF(String.valueOf(request));
        data.writeUTF(name);
        data.writeUTF(surname);
        data.flush();
    }
}
