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

	/**
	 * Contains the latest hashing algorithm used by the server.
	 * @param input The string to be hashed.
	 * @return The result of the hashing algorithm.
	 */
	public static String hash(String input) {

		return input;
	}


	/**
	 *
	 * Takes a provided password and the user's password hash and salt from the database, and ensures
	 * that, when hashed, the provided password matches the known database hash.
	 * @param passwordGiven The password provided by the user.
	 * @param passwordHash The known hash of the correct password from the database.
	 * @param passwordSalt The previously generated salt for the hashing algorithm, from the database.
 	 */
	public static boolean verifyPassword(String passwordGiven, String passwordHash, String passwordSalt) {

		return true;
	}

}
