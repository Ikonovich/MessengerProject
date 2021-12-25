package messengerserver;

import org.junit.Assert;
import org.junit.jupiter.api.*;

import java.util.ArrayList;
import java.util.HashMap;

import static org.junit.jupiter.api.Assertions.*;

class DatabaseConnectionTest {

    private DatabaseConnection connection;

//    public static void setUpBeforeClass() {
//        connection = DatabasePool.getConnection();
//    }

//    public static void tearDownAfterClass() {
//        connection.close();
//    }

    // This test creates and then gets a user. As such, it technically tests two aspects of the system
    // at once, and is not applicable until getUser passes.


    // The next two tests pull a pre-created user from the database.

    @Test
    void getUserByIDShouldReturnPremadeUser()
    {

        connection = DatabasePool.getConnection();

        String userID = "3";
        String username = "testdude";
        String createTimestamp = "2021-12-22 04:47:47";
        String modifiedTimestamp = null;
        String passwordHash = "testpass";
        String passwordSalt = "1bYJiCe,kD:D&RI4p/n,e fE^\\G|HZb>70dx8y/@^Sr ]$%$M_cG}9jQAiFl\"Qv&VA7g$iyr9^&viRs%\\BH-+Z<!N:ujYuS@qp*F`!sD!A>TVOemySL7^.U<bqRL62M";

        HashMap<String, String> user = connection.getUser(Integer.parseInt(userID));

        assertTrue(user.containsKey("UserID"));
        assertTrue(user.containsKey("UserName"));
        assertTrue(user.containsKey("CreateTimestamp"));
        assertTrue(user.containsKey("ModifiedTimestamp"));
        assertTrue(user.containsKey("PasswordHash"));
        assertTrue(user.containsKey("PasswordSalt"));

        assertEquals(user.get("UserID"), userID);
        assertEquals(user.get("UserName"), username);
        assertEquals(user.get("CreateTimestamp"), createTimestamp);
        assertEquals(user.get("ModifiedTimestamp"), modifiedTimestamp);
        assertEquals(user.get("PasswordHash"), passwordHash);
        assertEquals(user.get("PasswordSalt").trim(), passwordSalt);

        connection.close();
    }

    @Test
    void getUserByNameShouldReturnPremadeUser()
    {
        connection = DatabasePool.getConnection();

        String userID = "3";
        String username = "testdude";
        String createTimestamp = "2021-12-22 04:47:47";
        String modifiedTimestamp = null;
        String passwordHash = "testpass";
        String passwordSalt = "1bYJiCe,kD:D&RI4p/n,e fE^\\G|HZb>70dx8y/@^Sr ]$%$M_cG}9jQAiFl\"Qv&VA7g$iyr9^&viRs%\\BH-+Z<!N:ujYuS@qp*F`!sD!A>TVOemySL7^.U<bqRL62M";

        HashMap<String, String> user = connection.getUser(username);

        assertTrue(user.containsKey("UserID"));
        assertTrue(user.containsKey("UserName"));
        assertTrue(user.containsKey("CreateTimestamp"));
        assertTrue(user.containsKey("ModifiedTimestamp"));
        assertTrue(user.containsKey("PasswordHash"));
        assertTrue(user.containsKey("PasswordSalt"));

        assertEquals(user.get("UserID"), userID);
        assertEquals(user.get("UserName"), username);
        assertEquals(user.get("CreateTimestamp"), createTimestamp);
        assertEquals(user.get("ModifiedTimestamp"), modifiedTimestamp);
        assertEquals(user.get("PasswordHash"), passwordHash);
        assertEquals(user.get("PasswordSalt").trim(), passwordSalt);

        connection.close();
    }

    // The next two tests create and then delete a user.

    @Test
    void createUserThenGetUserByNameShouldReturnCreatedUser()
    {

        connection = DatabasePool.getConnection();

        String username = "testname";
        String passwordHash = "testpass";
        String passwordSalt = "1bYJiCe,kD:D&RI4p/n,e fE^\\G|HZb>70dx8y/@^Sr ]$%$M_cG}9jQAiFl\"Qv&VA7g$iyr9^&viRs%\\BH-+Z<!N:ujYuS@qp*F`!sD!A>TVOemySL7^.U<bqRL62M";

        connection.createUser(username, passwordHash, passwordSalt);

        HashMap<String, String>user = connection.getUser(username);

        assertTrue(user.containsKey("UserID"));
        assertTrue(user.containsKey("UserName"));
        assertTrue(user.containsKey("CreateTimestamp"));
        assertTrue(user.containsKey("ModifiedTimestamp"));
        assertTrue(user.containsKey("PasswordHash"));
        assertTrue(user.containsKey("PasswordSalt"));

        assertEquals(username, user.get("UserName"));
        assertEquals(passwordHash, user.get("PasswordHash"));
        assertEquals(passwordSalt, user.get("PasswordSalt"));

        connection.close();
    }


    void deleteUserThenGetUserByNameShouldReturnNoUser()
    {
        String username = "testname";
        connection = DatabasePool.getConnection();
        HashMap<String, String> user = connection.getUser(username);

        int userID = Integer.parseInt(user.get("UserID"));

        assertTrue(connection.deleteUser(userID));


        user = connection.getUser(username);
        assertFalse(user.containsKey("UserID"));

        connection.close();
    }

}