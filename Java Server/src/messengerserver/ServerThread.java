package messengerserver;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.net.Socket;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;

public class ServerThread implements Runnable 
{
	
	int debugMask = 4; // Indicates the bit mask for Debugger usage. +1 the debugMask to indicate an error message.
	
	private Socket socket;

	private PrintWriter writer;
	private BufferedReader reader;
	
	private boolean connected = false;
	
	
	private String sessionID = "NONE";
	
	
	// Determines how long the hash salts are.
	private static final int SALT_LENGTH = 128;
	
	public ServerThread(Socket newSocket) 
	{
		socket = newSocket;
	}

	/**
	 * This socket-less constructor should only be used for testing.
	 */
	public ServerThread() {}
	
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
			
			writer = new PrintWriter(socket.getOutputStream(), true);
			reader = new BufferedReader((new InputStreamReader(socket.getInputStream())));
			
			
			Debugger.record("Connection with server thread " + getID() + " established.", 4);
			
			while(connected) {
			
				char[] chars = new char[10];
				//while((input = reader.readLine()) != null) 


				while (reader.ready())
				{
					writer.write("writer is printing lollllll");

					//reader.read(chars);
					
					input = Arrays.toString(chars);
					input = reader.readLine();
					Debugger.record("Thread " + getID() + " received: " + input, 4);

					handleMessage(input);

					transmit("SCREAMING INTO THE VOID");
				}
			}
		}
		catch(Exception e) 
		{
			Debugger.record("Thread " + getID() + " has encountered an error: " + e.getMessage(), 5);
		}

		Debugger.record("Thread " + getID() + " is exiting.", debugMask);
		onExit();
		
	}
	
	public void transmit(String message) {
		
		Debugger.record("Transmitting with message: " + message, debugMask);
		writer.print(message);
		writer.print("SCREAMING INTO THE VOID");
		writer.print("SCREAMING INTO THE VOID");
		writer.print("SCREAMING INTO THE VOID");
	}
	
	public long getID()
	{
		return Thread.currentThread().getId();
	}
	
	private void handleMessage(String message) {
		
		HashMap<String, String> inputMap = Parser.parse(message);
		
		if (inputMap.containsKey("Opcode") == true) {
			
			Debugger.record("Entering message handling switch.", debugMask);
			
			String opcode = inputMap.get("Opcode");
			
			switch (opcode) {
			
				case "IR":
					
					register(inputMap);
					return;
					
				case "LR":
					
					transmit(login(inputMap));
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
		
					Debugger.record("Error code returned from Parser. Unable to handle message.", debugMask + 1);
					return;
			}
		}
		else {
			Debugger.record("Server thread received no opcode from parser.", debugMask + 1);
		}
	}
	
	public boolean register(HashMap<String, String> input) {
		
		String username = input.get("UserName");
		String password = input.get("Password");
		
		
		DatabaseConnection connection = DatabasePool.getConnection();
		
		HashMap<String, String> userResults = connection.getUser(username);
		
		if (userResults.containsKey("UserName")) {
			// A user with this name was found in the database.
			transmit("RU" + "The name you are trying to register is already taken.");
			Debugger.record("A user attempted to register the name " + username + ", but was rejected because it is taken.", debugMask + 1);
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
	
	public String login(HashMap<String, String> input) {

		String transmitMessage = "";
		Debugger.record("Login activated by message handler in thread " + getID(), debugMask);
		
		if (input.containsKey("UserName") == false) {
			Debugger.record("UserName field not present in login input.", debugMask + 1);
			transmitMessage = "LU" + "An undefined error has occurred. Please contact the system administrator.";
			return transmitMessage;
		}
		
		String username = input.get("UserName");
		
		if (checkUsernameSyntax(username) == false) {
			transmitMessage = "LU" + "Username was invalid.";
			return transmitMessage;
		}
		
		DatabaseConnection connection = DatabasePool.getConnection();
		
		HashMap<String, String> queryResults = connection.getUser(username);
		
		if (queryResults.containsKey("UserName") == false) {
			Debugger.record("User attempted to login, but the username query returned no results.", debugMask);
			transmitMessage = "LU" + "Username was not found.";
			return transmitMessage;
		}
		else if ((queryResults.containsKey("PasswordHash") == false) || (queryResults.containsKey("PasswordSalt") == false)) {

			Debugger.record("A user has attempted to login, but the query returned partial results.", debugMask);
			transmitMessage = "LU" + "An undefined error has occurred. Please contact the system administrator.";
			return transmitMessage;
		}

		String passwordGiven = input.get("Password");
		String passwordHash = queryResults.get("PasswordHash");
		String passwordSalt = queryResults.get("PasswordSalt");

		// Check the accuracy of the provided password.
		if (Cryptographer.verifyPassword(passwordGiven, passwordHash, passwordSalt) == false) {
			transmitMessage = "LU" + "Password provided did not match.";
			return transmitMessage;
		}

		// All checks passed. Time to create a session.
		// Right now that just means assigning a 32-length string session ID and using it to verify transmissions.

		if (createSession()) {
			Debugger.record("User " + username + " has logged in successfully.", debugMask);
			transmitMessage = "LS" + sessionID + "You have logged in with the username " + username;
			return transmitMessage;
		}

		Debugger.record("Login attempt made it to end of method without returning.", debugMask + 1);
		return "LU" + "An undefined error has occurred. Please contact your system administrator.";
	}

	private boolean createSession() {

		String newSessionID = "";
		do {
			newSessionID = Cryptographer.generateRandomString(32);
		} while (ServerController.addSession(newSessionID) == false);

		sessionID = newSessionID;
		return true;
	}

	/**
	 * Gets a random 256 bit string from the Cryptographer, checks it against the server session
	 * list, and if it's not present assigns it.
	 * @param input
	 * @return
	 */
	
	public boolean pullFriends(HashMap<String, String> input) {
		
		DatabaseConnection connection = DatabasePool.getConnection();

		
		return false;
	}
	
	public boolean addFriend(HashMap<String, String> input) {
		
		DatabaseConnection connection = DatabasePool.getConnection();

		
		return false;
	}
	
	public boolean pullChat(HashMap<String, String> input) {
		
		DatabaseConnection connection = DatabasePool.getConnection();

		
		return false;
	}

	public boolean sendMessage(HashMap<String, String> input) {
		
		DatabaseConnection connection = DatabasePool.getConnection();

		
		return false;
	}
	
	public boolean checkUsernameSyntax(String username) {
		
		if (username.length() < Server.MIN_USERNAME_LENGTH) {
			return false;
		}
		if (username.length() > Server.MAX_PASSWORD_LENGTH) {
			return false;
		}
		
		
		return true;
	}
	
	public boolean checkPasswordSyntax(String password) {
		
		if (password.length() < Server.MIN_PASSWORD_LENGTH) {
			return false;
		}
		if (password.length() > Server.MAX_PASSWORD_LENGTH) {
			return false;
		}
		
		return true;
	}

	// Handles things that have to be handled on closing.

	private void onExit() {

		if (socket != null) {

			try
			{
				socket.close();
			}
			catch(IOException e) {

				Debugger.record("Failure to close a socket on a thread.", debugMask + 1);
			}
		}

		if (sessionID != "NONE") {
			ServerController.removeSession(sessionID);
		}


	}
}
