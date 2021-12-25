package messengerserver;

import java.io.FileOutputStream;
import java.util.Properties;

public class Server {
	
	public static final int PORT = 3000;

	public static final int MIN_USERNAME_LENGTH = 8;
	public static final int MAX_USERNAME_LENGTH = 32;
	
	public static final int MIN_PASSWORD_LENGTH = 8;
	public static final int MAX_PASSWORD_LENGTH = 128;


	public static void main(String[] args) 
	{

		System.out.println("Starting server.");

		ServerController serverController = new ServerController();
		serverController.start();
		
		Properties props = new Properties();
		props.setProperty("Port", "3000");
		props.setProperty("Minimum Username Length", "32");
		props.setProperty("Maximum Username Length", "32");
		props.setProperty("Minimum Username Length", "32");
		props.setProperty("Maximum Password Length", "128");

		try 
		{
			props.storeToXML(new FileOutputStream("settings.xml"), "");
		}
		catch (Exception e) {
			System.out.println("It's a real bugapalooza!");
		}
	}
}
