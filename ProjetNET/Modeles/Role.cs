namespace ProjetNET.Modeles
{
    public static class Role
    {
        public const string Pharmacien = "pharmacien";
        public const string Admin = "admin";
        public const string Medecin = "medecin";



        public static IEnumerable<string> AllRoles()
        {
            yield return Pharmacien;
            yield return Admin;
            yield return Medecin;

        }
    }



}

