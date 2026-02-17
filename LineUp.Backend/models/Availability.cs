public class Availability
{
    public String ID = "";
    public TimeOnly[][] availability; //See https://github.com/user-attachments/assets/37a07d97-902a-4195-b558-985008aa8912 for a visual on how this works.

    public String schedule_ID;

    public String userName; //NOT a "username" in the traditional sense. This holds the real name of the user.

    public String userEmail;

    public Object[] preferences;
}
