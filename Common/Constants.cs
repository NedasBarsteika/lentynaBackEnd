namespace lentynaBackEnd.Common
{
    public static class Constants
    {
        // Review ratings
        public const int MinReviewRating = 1;
        public const int MaxReviewRating = 5;

        // Pagination
        public const int DefaultPageSize = 10;
        public const int MaxPageSize = 100;
        public const int DefaultPage = 1;

        // File uploads
        public const long MaxFileSize = 5 * 1024 * 1024; // 5MB
        public const string AllowedImageTypes = "image/jpeg,image/png,image/gif";

        // Error messages (Lithuanian)
        public const string NaudotojasNerastas = "Naudotojas nerastas";
        public const string KnygaNerastas = "Knyga nerasta";
        public const string AutoriusNerastas = "Autorius nerastas";
        public const string KomentarasNerastas = "Komentaras nerastas";
        public const string TemaNerastas = "Tema nerasta";
        public const string BalsavimasNerastas = "Balsavimas nerastas";
        public const string ZanrasNerastas = "Žanras nerastas";
        public const string NuotaikaNerastas = "Nuotaika nerasta";
        public const string IrasasNerastas = "Įrašas nerastas";
        public const string NeteisingasSlaptazodis = "Neteisingas slaptažodis";
        public const string NaudotojasJauEgzistuoja = "Naudotojas su šiuo el. paštu jau egzistuoja";
        public const string SlapyvardisJauEgzistuoja = "Naudotojas su šiuo slapyvardžiu jau egzistuoja";
        public const string NeturitePrieigos = "Neturite prieigos atlikti šį veiksmą";
        public const string PrisijungimasPrivalomas = "Prisijungimas privalomas";
        public const string JauBalsavote = "Jūs jau balsavote šiame balsavime";
        public const string BalsavimasUzbaigtas = "Balsavimas jau užbaigtas";
        public const string KnygaJauSarase = "Knyga jau yra jūsų sąraše";
        public const string JauSekateAutoriu = "Jūs jau sekate šį autorių";
    }
}
