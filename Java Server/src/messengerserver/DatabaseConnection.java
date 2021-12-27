package messengerserver;

import javax.naming.SizeLimitExceededException;
import java.sql.*;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;

public class DatabaseConnection 
{

	
	private boolean isOccupied = false;
	private final int debugMask = 2;

	// These are all of the statements that the connection can execute.
	
	private Connection connection;
	private PreparedStatement getUserByName;
	private PreparedStatement getUserByID;
	private PreparedStatement createUser;
	private PreparedStatement deleteUser;
	private PreparedStatement pullFriends;
	private PreparedStatement addFriend;
	private PreparedStatement removeFriend;
	private PreparedStatement checkFriend;

	
	private static final String getUserByNameQuery = "SELECT * FROM RegisteredUsers WHERE UserName = ?";
	private static final String getUserByIDQuery = "SELECT * FROM RegisteredUsers WHERE UserID = ?";
	private static final String createUserQuery ="INSERT INTO RegisteredUsers (Username, PasswordHash, PasswordSalt) VALUES (?, ?, ?)";
	private static final String deleteUserQuery = "DELETE FROM RegisteredUsers WHERE UserID = ?";
	private static final String pullFriendsQuery = "SELECT * FROM FriendPairs WHERE UserID = ?";
	private static final String addFriendQuery = "INSERT INTO FriendPairs (UserID, FriendUserID) VALUES (?, ?)";
	private static final String removeFriendQuery = "DELETE FROM FriendPairs WHERE UserID = ? AND FriendUserID = ?";
	private static final String checkFriendQuery = "SELECT * FROM FriendPairs WHERE UserID = ? AND FriendUserID = ?";
	
	
	public DatabaseConnection(String database, String user, String pass) 
	{
		
		
		// Here we connect to the database.

		Debugger.record("Database Connection being created.", 2);
		try 
		{

			DriverManager.registerDriver(new com.mysql.cj.jdbc.Driver());
			connection = DriverManager.getConnection(database, user, pass);
			Debugger.record("Database connection established", 2);
		} 
		catch (Exception e) 
		{
			Debugger.record("Database connection failed: " + Arrays.toString(e.getStackTrace()), 3);
		}
		
		
		// Here we create all the statements that the connection can execute throughout its life.
		try 
		{	
			getUserByName = connection.prepareStatement(getUserByNameQuery);
			getUserByID = connection.prepareStatement(getUserByIDQuery);
			createUser = connection.prepareStatement(createUserQuery);
			deleteUser = connection.prepareStatement(deleteUserQuery);
			pullFriends = connection.prepareStatement(pullFriendsQuery);
			addFriend = connection.prepareStatement(addFriendQuery);
			removeFriend = connection.prepareStatement(removeFriendQuery);
			checkFriend = connection.prepareStatement(checkFriendQuery);


		}
		catch (SQLException e)
		{
			Debugger.record("Statement creation error: " + e.getMessage(), 3);
		}
		
	}
	
	public boolean open() 
	{
		if (isOccupied == false) 
		{

			isOccupied = true;
			Debugger.record("A database connection has allowed access to a thread.", 2);
			return true;
		}
		else
		{
			Debugger.record("A database connection has denied access to a thread.", 3);
			return false;
		}
	}
	

	public void close() 
	{
		isOccupied = false;
	}
	
	
	public boolean createUser(String username, String passwordHash, String passwordSalt) {

		if (getUser(username).size() > 0) {
			Debugger.record("Duplicate user creation attempt rejected.", debugMask);
			return false;
		}
		try {
			createUser.setString(1, username);
			createUser.setString(2, passwordHash);
			createUser.setString(3, passwordSalt);
		}
		catch(SQLException e) {
			Debugger.record("Error preparing create user statement.", debugMask + 1);
			return false;
		}
		
		try {
			return createUser.execute();
		}
		catch(SQLException e) {
			Debugger.record("Error executing create user statement.", debugMask + 1);
			return false;
		}
	}

	public boolean deleteUser(int userID) {

		try {
			deleteUser.setInt(1, userID);
		}
		catch(SQLException e) {
			Debugger.record("Error preparing delete user by ID statement.", debugMask + 1);
			return false;
		}

		try {
			deleteUser.execute();

			if (deleteUser.getUpdateCount() > 0) {
				return true;
			}
			return false;
		}
		catch(SQLException e) {
			Debugger.record("Error executing delete user by ID statement.", debugMask + 1);
			return false;
		}
	}

	/**
	 *  This method returns a single user by their UserName.
	 *  @param username the name of the user.
	 *	@return a HashMap containing the user's database entry.
 	 */


	public HashMap<String, String> getUser(String username)
	{
		
		HashMap<String, String> userMap = new HashMap<String, String>();

		try {
			getUserByName.setString(1, username);
		}
		catch(SQLException e) {
			Debugger.record("Error preparing get user by name statement.", debugMask + 1);
			return userMap;
		}
		
		try {
			ResultSet results = getUserByName.executeQuery();
			ArrayList<HashMap<String, String>> resultsList = parseResultSet(results);

			// Only one or zero users should ever be returned by a getUser query.
			if (resultsList.size() > 1)
			{
				userMap.put("ER", "An error has occurred. Please contact the administrator.");
				Debugger.record("Error: A duplicate user has been retrieved when conducting a search by UserName.", 3);
			}
			else if (resultsList.size() == 1)
			{
				userMap = resultsList.get(0);
			}
			else
			{
				userMap = new HashMap<String, String>();
			}
			
		}
		catch(SQLException e) {
			Debugger.record("Error executing get user by name statement.", debugMask + 1);
		}
		
		return userMap;
		
	}

	/**
	 *  This method returns a single user by their UserID.
	 *  @param userID the ID of the user.
	 *	@return a HashMap containing the user's database entry.
	 */
	
	public HashMap<String, String> getUser(int userID)
	{
		HashMap<String, String> userMap = new HashMap<String, String>();
		try
		{
			getUserByID.setInt(1, userID);
		}
		
		catch(SQLException e)
		{
			Debugger.record("Error preparing get user by ID statement.", debugMask + 1);
		}
		
		try
		{
			ResultSet results = getUserByID.executeQuery();
			ArrayList<HashMap<String, String>> resultsList = parseResultSet(results);

			// Only one or zero users should ever be returned by a getUser query.
			if (resultsList.size() > 1)
			{
				Debugger.record("Error: A duplicate user has been retrieved when conducting a search by ID.", 3);
			}
			else if (resultsList.size() == 1)
			{
				userMap = resultsList.get(0);
			}
			else
			{
				userMap = new HashMap<String, String>();
			}
		}
		catch(SQLException e)
		{
			Debugger.record("Error executing get user by ID statement.", debugMask + 1);
		}
		
		return userMap;
	}

	/**
	 * This method gets the list of a user's friends, stored as HashMaps.
	 * @param userID The ID of the user whose friends are being gotten.
	 * @return An ArrayList of HashMaps representing an individual user.
	 */
	public ArrayList<HashMap<String, String>> pullFriends(int userID)
	{
		ArrayList<HashMap<String, String>> friendsMap = new ArrayList<HashMap<String, String>>();

		try
		{
			pullFriends.setInt(1, userID);
		}
		catch(SQLException e)
		{
			Debugger.record("Error preparing get user by name statement.", debugMask + 1);
			return friendsMap;
		}

		try
		{
			ResultSet results = pullFriends.executeQuery();
			friendsMap = parseResultSet(results);
		}
		catch(SQLException e)
		{
			Debugger.record("Error executing get user by nane statement.", debugMask + 1);
		}
		return friendsMap;
	}


	public boolean checkFriend(int userID, int friendUserID)
	{
		try
		{
			checkFriend.setInt(1, userID);
			checkFriend.setInt(2, friendUserID);
		}
		catch (Exception e)
		{
			Debugger.record("Could not set parameters for checkFriend query", debugMask + 1);
			return false;
		}
		try
		{
			ResultSet results = checkFriend.executeQuery();
			ArrayList<HashMap<String, String>> resultsList = parseResultSet(results);

			if (resultsList.size() == 0)
			{
				return false;
			}
			else if (resultsList.size() == 1)
			{
				return true;
			}
			else if (resultsList.size() > 0)
			{
				throw new SizeLimitExceededException();
			}
		}
		catch (SQLException e)
		{
			Debugger.record("Error executing checkFriend query." + e.getMessage(), debugMask + 1);
		}
		catch (SizeLimitExceededException e)
		{
			Debugger.record("CheckFriend query result was unexpectedly large.", debugMask + 1);
		}

		return false;
	}



	public boolean addFriend(int userID, int friendUserID)
	{
		if (checkFriend(userID, friendUserID) == false)
		{

			try
			{
				addFriend.setInt(1, userID);
				addFriend.setInt(2, friendUserID);
			}
			catch (Exception e)
			{
				Debugger.record("Add friend query failed due to exception.", debugMask + 1);
				return false;
			}

			try
			{
				addFriend.execute();

				if (addFriend.getUpdateCount() > 0)
				{
					return true;
				}
				else {
					Debugger.record("Executed addFriend query, but no friend pair was created.", debugMask + 1);
				}

			}
			catch (SQLException e)
			{
				Debugger.record("Error when executing addFriend query." + e.getMessage(), debugMask + 1);
			}
		}

		return false;
	}



	public boolean removeFriend(int userID, int friendUserID)
	{
		if (checkFriend(userID, friendUserID) == true)
		{
			try
			{
				removeFriend.setInt(1, userID);
				removeFriend.setInt(2, friendUserID);
			}
			catch (Exception e)
			{
				Debugger.record("Remove friend query failed due to exception.", debugMask + 1);
				return false;
			}

			try
			{
				removeFriend.execute();

				if (removeFriend.getUpdateCount() > 0)
				{
					return true;
				}
				else {
					Debugger.record("Executed removeFriend query, but no friend pair was deleted.", debugMask + 1);
				}

			}
			catch (SQLException e)
			{
				Debugger.record("Error when executing removeFriend query." + e.getMessage(), debugMask + 1);
			}
		}

		return false;
	}
	
	
	public boolean getStatus() 
	{
		return isOccupied;
	}

	
	// This method takes a result set and converts it into a list of HashMaps.
	public ArrayList<HashMap<String, String>> parseResultSet(ResultSet results) 
	{
		
		ArrayList<HashMap<String, String>> resultsList = new ArrayList<HashMap<String, String>>();
		ArrayList<String> columns = new ArrayList<String>();

		
		try 
		{
			// Ensure there is at least one entry in the set.

			if (results.next() == false) {
				Debugger.record("Empty result set provided to parser.", debugMask);
				return resultsList;
			}

			// Getting the column names.
			
			ResultSetMetaData metaData = results.getMetaData();
		
			for (int i = 1; i < metaData.getColumnCount() + 1; i++)
			{
				columns.add(metaData.getColumnName(i));
			}
		}
		catch (Exception e) 
		{
			
			Debugger.record("Error while getting result set column names.", debugMask + 1);
			return resultsList;

		}
		
		// Building a List of hashmaps out of the entries.
		try 
		{ 
			do
			{
				HashMap<String, String> resultsMap = new HashMap<String, String>();
				
				for (int i = 0; i < columns.size(); i++)
				{

					String data = results.getString(columns.get(i));

					resultsMap.put(columns.get(i), data);
				}
				resultsList.add(resultsMap);
			} while (results.next() == true);
			
		}
		catch (Exception e) {

			Debugger.record("Error while turning rows into hash maps.", debugMask + 1);
		}

		return resultsList;
		
	}
}
