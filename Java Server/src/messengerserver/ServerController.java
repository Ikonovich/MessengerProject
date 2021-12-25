package messengerserver;


import java.net.ServerSocket;
import java.net.Socket;
import java.util.HashSet;

public class ServerController 
{
	
	private static HashSet<String> sessionMap;


	// The length of session ID strings. 32 UTF-8 chars is equivalent to 256 bits.
	private static final int SESSION_LENGTH = 32;

	static
	{
		sessionMap = new HashSet<String>();
	}
	
	public void start() {
		
		listen();
	}
	
	private static void listen() 
	{
		
		ServerSocket serverSocket;
		
		try 
		{
			
			boolean run = true;
			serverSocket = new ServerSocket(Server.PORT);
			
			while(run == true) 
			{
				
				Socket socket = serverSocket.accept();
				
				ServerThread thread = new ServerThread(socket);
				
				new Thread(thread).start();
				
			}
			
			serverSocket.close();
			
		}
		catch (Exception e) 
		{
			Debugger.print(e.getMessage());
			return;
		}
	}
	
	public static synchronized boolean addSession(String sessionID)
	{
		if (sessionID.length() == SESSION_LENGTH) {
			return sessionMap.add(sessionID);
		}
		return false;

	}
	
	public static synchronized boolean removeSession(String sessionID)
	{
		
		return sessionMap.remove(sessionID);
	}

	/**
	 *
	 * @param sessionID The session ID to check the existence of.
	 * @return A boolean, true if the session ID is found, false otherwise.
	 */
	public static synchronized boolean checkSession(String sessionID)
	{
		return sessionMap.contains(sessionID);
	}

}
