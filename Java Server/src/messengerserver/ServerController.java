package messengerserver;


import java.net.ServerSocket;
import java.net.Socket;

public class ServerController 
{
	
	private static int port = 3000;
	
	public void start() {
		
		listen();
	}
	
	private static void listen() 
	{
		
		ServerSocket serverSocket;
		
		try 
		{
			
			boolean run = true;
			serverSocket = new ServerSocket(port);
			
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

}
