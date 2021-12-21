package messengerserver;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.net.Socket;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;

public class ServerThread implements Runnable 
{
	
	
	private Socket socket; 
	
	private boolean connected = false;
	
	
	private String sessionID;
	
	
	// Determines how long the hash salts are.
	private static final int SALT_LENGTH = 128;
	
	public ServerThread(Socket newSocket) 
	{
		
		socket = newSocket;
	}
	
	public void run() 
	{
		
		connected = true;
		listen();
		
	}
	
	private void listen() 
	{
		
		String output = "";
		String input = "";
		
		try 
		{
			
			PrintWriter writer = new PrintWriter(socket.getOutputStream(), true);
			BufferedReader reader = new BufferedReader((new InputStreamReader(socket.getInputStream())));
			
			
			Debugger.record("Connection with server thread " + getID() + " established.", 4);
			
			while(connected) {
			
				char[] chars = new char[10];
				//while((input = reader.readLine()) != null) 
				
				while (reader.ready())
				{
					
					//reader.read(chars);
					
					input = Arrays.toString(chars);
					input = reader.readLine();
					Debugger.record("Thread " + getID() + " received: " + input, 4);

					handleMessage(input);
				}
			}
				
				System.out.println("Exiting");
		}
		catch(Exception e) 
		{
			
			Debugger.record("Thread " + getID() + " has encountered an error: " + e.getMessage(), 5);
			
		}
		
		
	}
	
	private void transmit(String message) {
		
		
		
	}
	
	public long getID()
	{
		
		return Thread.currentThread().getId();
	}
	
	public void handleMessage(String message) {
		
		HashMap<String, String> inputMap = Parser.parse(message);
		
		if (inputMap.containsKey("Opcode") == true) {
			
			String opcode = inputMap.get("Opcode");
			
			switch (opcode) {
			
				case "IR":
					
					register(inputMap);
					return;
					
				case "LR":
					
					login(inputMap);
					return;
					
				case "PF":
					
					pullFriends(inputMap);
					return;
					
				case "AF":
					
					addFriend(inputMap);
					return;
					
				case "PC":
					
					pullChat(inputMap);
					return;
					
				case "SM":
					
					sendMessage(inputMap);
					return;
					
				default:
		
					Debugger.record("Error code returned from Parser. Unable to handle message.", 5);
					return;
			}
				
		}
		
	}
	
	private boolean register(HashMap<String, String> input) {
		
		String username = input.get("UserName");
		String password = input.get("Password");
		
		
		DatabaseConnection connection = DatabasePool.getConnection();
		
		ArrayList<HashMap<String, String>> userResults = connection.getUser(username);
		
		if (userResults.size() > 0) {
			// A user with this name was found in the database.
			transmit("RU" + "The name you are trying to register is already taken.");
			Debugger.record("A user attempted to register the name " + username + ", but was rejected because it is taken.", 5);
			return false;
		}
		
		String passwordSalt = Cryptographer.generateRandomString(SALT_LENGTH);

		
		boolean createStatus = connection.createUser(username, password, passwordSalt);
		
		connection.close();
		
		if (createStatus == true) {
			
			transmit("RS" + "You have successfully registered with the username " + username);
			Debugger.record("A user has registered with the name " + username, 4);
		}
		
		return createStatus;
	
	}
	
	private boolean login(HashMap<String, String> input) {
		
		DatabaseConnection connection = DatabasePool.getConnection();
		
		

		
		return false;
	}
	
	private boolean pullFriends(HashMap<String, String> input) {
		
		DatabaseConnection connection = DatabasePool.getConnection();

		
		return false;
	}
	
	private boolean addFriend(HashMap<String, String> input) {
		
		DatabaseConnection connection = DatabasePool.getConnection();

		
		return false;
	}
	
	private boolean pullChat(HashMap<String, String> input) {
		
		DatabaseConnection connection = DatabasePool.getConnection();

		
		return false;
	}

	private boolean sendMessage(HashMap<String, String> input) {
		
		DatabaseConnection connection = DatabasePool.getConnection();

		
		return false;
	}
}
