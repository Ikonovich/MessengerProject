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
import com.google.gson.Gson;

public class ServerThread implements Runnable 
{
	
	int debugMask = 4; // Indicates the bit mask for Debugger usage. +1 the debugMask to indicate an error message.
	
	private Socket socket;

	private PrintWriter writer;
	private BufferedReader reader;
	
	private boolean connected = false;

	// Keeps track of data unique to this session and user.

	private String username = "NONE";
	private int userID = -1;
	private String sessionID = "NONE";

	// Stores the most recent transmission
	private String lastTransmission = "";

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
		String input = "";
		
		try 
		{
			
			writer = new PrintWriter(socket.getOutputStream(), true);
			reader = new BufferedReader((new InputStreamReader(socket.getInputStream())));
			
			
			Debugger.record("Connection with server thread " + getID() + " established.", 4);
			
			while(connected) {

				while (reader.ready())
				{
					input = reader.readLine();
					Debugger.record("Thread " + getID() + " received: " + input, 4);

					handleMessage(input);
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
	
	public void transmit(String message)
	{
		
		Debugger.record("Transmitting with message: " + message, debugMask);
		writer.print(message);
		writer.flush();

		lastTransmission = message;

	}

	public String getLastTransmission()
	{
		return lastTransmission;
	}
	
	public long getID()
	{
		return Thread.currentThread().getId();
	}
	
	private void handleMessage(String message) {
		
		HashMap<String, String> inputMap = Parser.parse(message);
		
		if (inputMap.containsKey("Opcode") == true) {
			
			String opcode = inputMap.get("Opcode");

			if (verifyMessageIntegrity(inputMap) == false)
			{
				Debugger.record("Server thread could not verify the integrity of a message. Provided User ID: " + inputMap.get("UserID") + "\nProvided Session ID: " + inputMap.get("SessionID") + "\nStored User ID: " + this.userID + "\nStored Session ID: " + this.sessionID, debugMask + 1);
				return;
			}

			Debugger.record("Entering message handling switch with opcode " + opcode, debugMask);

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
					pullUserChatPairs(inputMap);
					return;

				case "PM":
					pullMessages(inputMap);
					return;
				case "SM":
					sendMessage(inputMap);
					return;
				case "ER":
					Debugger.record("Error code returned from Parser. Unable to handle message.", debugMask + 1);
					return;
				default:
					Debugger.record("Unrecognized opcode returned from Parser. Unable to handle message.", debugMask + 1);
					return;
			}
		}
		else {
			Debugger.record("Server thread received no opcode from parser.", debugMask + 1);
		}
	}

	/**
	 * This method verifies the integrity of a received message depending on the opcode.
	 *
	 * @param input The hashmap containing the parsed message.
	 * @return A boolean indicating whether or not the message could be verified successfully.
	 */

	public boolean verifyMessageIntegrity(HashMap<String, String> input)
	{
		String opcode = input.get("Opcode");

		if (opcode.equals("IR") || opcode.equals("LR"))
		{
			// No session or userID assigned, can't be verified normally right now, just return true.
			return true;
		}
		else
		{
			// Input maps with any other opcode should contain a sessionID and userID that can be verified.

			if (input.containsKey("UserID") && input.containsKey("SessionID"))
			{
				String userID = input.get("UserID");
				String sessionID = input.get("SessionID");

				return ((userID.equals(Integer.toString(this.userID))) && (sessionID .equals(this.sessionID)));
			}
			else
			{
				Debugger.record("Input to verify message did not contain expected keys for opcode: " + opcode, debugMask + 1);
			}

		}

		return false;

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
			Debugger.record("A user has registered with the name " + username, debugMask);
		}
		
		return createStatus;
	
	}
	
	public String login(HashMap<String, String> input) {

		String transmitMessage = "";
		Debugger.record("Login activated by message handler in thread " + getID(), debugMask);
		
		if (input.containsKey("UserName") == false) {
			Debugger.record("UserName field not present in login input.", debugMask + 1);
			transmitMessage = "LU" + "An undefined error has occurred while logging in. Please contact the system administrator.";
			return transmitMessage;
		}
		
		String username = input.get("UserName");
		
		if (checkUsernameSyntax(username) == false) {
			transmitMessage = "LU" + "Username " + username + " was invalid.";
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
			transmitMessage = "LU" + "An undefined error has occurred while logging in. Please contact the system administrator.";
			return transmitMessage;
		}

		String userID = queryResults.get("UserID");
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

		if (createSession())
		{

			try
			{
				this.username = username;
				this.userID = Integer.parseInt(userID);
			}
			catch (Exception e)
			{
				transmitMessage = "LU" + "An undefined error has occurred while logging in. Please contact the system administrator.";
				return transmitMessage;

			}
			Debugger.record("User " + username + " has logged in successfully.", debugMask);
			transmitMessage = "LS" + Parser.pack(userID, 32) + Parser.pack(username, 32) + sessionID + "You have logged in with the username " + username;

			return transmitMessage;
		}

		Debugger.record("Login attempt made it to end of method without returning.", debugMask + 1);
		return "LU" + "An undefined error has occurred while logging in. Please contact the system administrator.";

	}

	private boolean createSession()
	{

		String newSessionID = "";
		do
		{
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

		int userID = 0;
		try
		{
			userID = Integer.parseInt(input.get("UserID"));
		}
		catch (Exception e) {
			Debugger.record("User ID could not be parsed, or was not in the input to pulLFriends", debugMask + 1);
			return false;
		}

		ArrayList<HashMap<String, String>> friends = connection.pullFriends(userID);

		Gson json = new Gson();
		String friendsJson = json.toJson(friends);

		transmit("FP" + Parser.pack(input.get("UserID"), 32) + sessionID + friendsJson);

		return false;
	}
	
	public boolean addFriend(HashMap<String, String> input) {
		
		DatabaseConnection connection = DatabasePool.getConnection();

		int userOneID = 0;
		int userTwoID = 0;
		try
		{
			String userOne = input.get("UserID");
			String userTwo = input.get("UserName");

			userOneID = Integer.parseInt(userOne);
			userTwoID = Integer.parseInt(userTwo);
		}
		catch (Exception e)
		{
			Debugger.record("addFriend function failed to get or parse provided userIDs: " + e.getMessage(), debugMask + 1);

			String message = "ER" + Parser.pack(Integer.toString(this.userID), 32) + sessionID + "Unable to send or approve friend request.";
			transmit(message);
			return false;
		}

		if (connection.checkFriendRequest(userTwoID, userOneID) == true) {

			if (connection.addFriend(userOneID, userTwoID) && (connection.addFriend(userTwoID, userOneID)))
			{
				HashMap<String, String> userMap= new HashMap<>();
				userMap.put("UserID", input.get("UserID"));
				return (pullFriends(userMap));
			}
			else
			{
				return false;
			}
		}
		else {
			return connection.addFriendRequest(userOneID, userTwoID);
		}
	}

	public boolean removeFriend(HashMap<String, String> input)
	{
		DatabaseConnection connection = DatabasePool.getConnection();

		int userOneID = 0;
		int userTwoID = 0;

		try
		{
			String userOne = input.get("UserID");
			String userTwo = input.get("UserName");

			userOneID = Integer.parseInt(userOne);
			userTwoID = Integer.parseInt(userTwo);
		}
		catch (Exception e)
		{
			Debugger.record("RemoveFriend function failed to parse provided userIDs: " + e.getMessage(), debugMask + 1);
			return false;
		}

		return (connection.removeFriend(userOneID, userTwoID) && (connection.removeFriend(userTwoID, userOneID)));
	}

	public boolean pullUserChatPairs(HashMap<String, String> input) {

		DatabaseConnection connection = DatabasePool.getConnection();

		int userID = 0;
		try
		{
			userID = Integer.parseInt(input.get("UserID"));
		}
		catch (Exception e)
		{
			Debugger.record("PullUserChatPairs failed to parse userID from input.", debugMask + 1);
			return false;
		}

		ArrayList<HashMap<String, String>> chatPairs = connection.pullUserChats(userID);

		Gson json = new Gson();
		String chatPairsJson = json.toJson(chatPairs);

		String transmitMessage = "CP" + userID + sessionID + chatPairsJson;
		transmit(transmitMessage);

		return false;
	}

	public boolean pullMessages(HashMap<String, String> input) {

		DatabaseConnection connection = DatabasePool.getConnection();

		int chatID;
		try
		{
			chatID = Integer.parseInt(input.get("ChatID"));
		}
		catch (Exception e)
		{
			Debugger.record("PullMessages failed to get chatID.", debugMask + 1);
			return false;
		}
		try
		{
			ArrayList<HashMap<String, String>> messages = connection.pullMessagesFromChat(chatID);

			if (messages.size() > 0) {
				Gson json = new Gson();
				String messagesJson = json.toJson(messages);

				String transmitMessage = "MP" + Parser.pack(userID, 32) + sessionID + Parser.pack(chatID, 32) + messagesJson;
				transmit(transmitMessage);
			}
			else
			{
				Debugger.record("Message push called but found no results to send.", debugMask);
			}
			return true;
		}
		catch (Exception e)
		{
			Debugger.record("PullMessages failed to get messages.", debugMask + 1);
		}

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
