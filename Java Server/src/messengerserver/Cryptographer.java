package messengerserver;
import java.security.SecureRandom;
import java.security.NoSuchAlgorithmException;

public class Cryptographer {
	
	private static SecureRandom rand;
	
	static {
		
		try {
			rand = SecureRandom.getInstanceStrong();
		}
		catch (NoSuchAlgorithmException e) {
			
			Debugger.record("Random number generator creation failed.", 17);
		}
	}
	
	public static String generateRandomString(int length) {
		
		String randString = "";
		
		while (randString.length() < length) {
			
			int randInt = rand.nextInt(95);
			char nextChar = (char)(randInt + 32);
			randString += nextChar;
			
		}
		
		return randString;
	}

}
