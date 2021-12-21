package messengerserver;

import java.sql.*;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;

public class DatabaseConnection 
{

	
	private boolean isOccupied = false;

	// These are all of the statements that the connection can execute.
	
	private Connection connection;
	private PreparedStatement getUserByName;
	private PreparedStatement getUserByID;
	private PreparedStatement createUser;
	
	private static final String getUserByNameQuery = "SELECT * FROM RegisteredUsers WHERE UserName = ?";
	private static final String getUserByIDQuery = "SELECT * FROM RegisteredUsers WHERE UserID = ?";
	private static final String createUserQuery ="INSERT INTO RegisteredUsers (Username, PasswordHash, PasswordSalt) VALUES (?, ?, ?)";
	private static final String pullFriendsQuery = "SELECT FriendUserID FROM FriendPairs WHERE UserID = ?";

	
	
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
		
		
		// Here we create all of the statements that the connection can execute throughout its life.
		try 
		{	
			getUserByName = connection.prepareStatement(getUserByNameQuery);
			getUserByID = connection.prepareStatement(getUserByIDQuery);
			createUser = connection.prepareStatement(createUserQuery);
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
		
		try {
			createUser.setString(1, username);
			createUser.setString(2, passwordHash);
			createUser.setString(3, passwordSalt);
		}
		catch(SQLException e) {
			Debugger.record("Error preparing create user statement.", 3);
			return false;
		}
		
		try {
			
			createUser.execute();
			return true;
		}
		catch(SQLException e) {
			Debugger.record("Error executing create user statement.", 3);
			return false;
		}
	}
	
	public ArrayList<HashMap<String, String>> getUser(String username) 
	{
		
		ArrayList<HashMap<String, String>> userMap = new ArrayList<HashMap<String, String>>();

		try {
			getUserByName.setString(1, username);
		}
		catch(SQLException e) {
			Debugger.record("Error preparing get user by name statement.", 3);
			return userMap;
		}
		
		try {
			ResultSet results = getUserByName.executeQuery();
			
			userMap = parseResultSet(results);
			
		}
		catch(SQLException e) {
			Debugger.record("Error executing get user by nane statement.", 3);
		}
		
		return userMap;
		
	}
	
	public ArrayList<HashMap<String, String>> getUser(int userID) 
	{
		
		ArrayList<HashMap<String, String>> userMap = new ArrayList<HashMap<String, String>>();
		try {
			getUserByID.setInt(1, userID);
		}
		
		catch(SQLException e) {
			Debugger.record("Error preparing get user by ID statement.", 3);
		}
		
		try {
			ResultSet results = getUserByID.executeQuery();
			userMap = parseResultSet(results);
			
		}
		catch(SQLException e) {
			Debugger.record("Error executing get user by ID statement.", 3);
		}
			
		
		return userMap;
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

		// Getting the column names.
		try 
		{
			ResultSetMetaData metaData = results.getMetaData();
		
			for (int i = 0; i < metaData.getColumnCount(); i++) 
			{
				columns.add(metaData.getColumnName(i));
			}
		}
		catch (Exception e) 
		{
			
			Debugger.record("Error while getting result set column names.", 5);
			return resultsList;

		}
		
		// Building a List of hashmaps out of the entries.
		try 
		{ 
			while (results.next() == true) 
			{
				
				HashMap<String, String> resultsMap = new HashMap<String, String>();
				
				for (int i = 0; i < columns.size(); i++) {
					
					resultsMap.put(columns.get(i), results.getString(columns.get(i)));
				}
				resultsList.add(resultsMap);
			}
		}
		catch (Exception e) {
			
			Debugger.record("Error while turning rows into hash maps.", 5);
		}

		
		return resultsList;
		
	}
}
