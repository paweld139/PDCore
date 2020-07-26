using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDCore.Extensions;

namespace PDCore.Helpers.Translation
{
    public static class CustomIdentityErrorDescriber
    {
        /// <summary>
        /// Słowniki są inicjalizowane tylko raz i tylko wtedy, gdy są potrzebne.
        /// </summary>
        static CustomIdentityErrorDescriber()
        {

            customErrors = new Dictionary<string, string> //
            {
                ["Passwords must have at least one non letter or digit character"]
            =
            "Hasło musi zawierać co najmniej jeden znak specjalny",

                ["Passwords must have at least one digit ('0'-'9')"]
            =
            "Hasło musi zawierać co najmniej jedną cyfrę",

                ["Passwords must have at least one uppercase ('A'-'Z')"]
            =
            "Hasło musi zawierać co najmniej jedną wielką literę",

                ["Invalid token."] = "Niepoprawny token"
            };

            customErrorsSub = new Dictionary<string, string>
            {
                ["Name"] = "E-mail",
                ["is"] = "jest",
                ["already"] = "już",
                ["taken"] = "zajęty"
            };
        }

        /// <summary>
        /// Słownik zawierający tłumaczenia zdań
        /// </summary>
        private static readonly Dictionary<string, string> customErrors;

        /// <summary>
        /// Słownik zawierający tłumaczenia wyrazów
        /// </summary>
        private static readonly Dictionary<string, string> customErrorsSub;

        /// <summary>
        /// Określa czy można przetłumaczyć kolekcję zawieającą błędy
        /// </summary>
        /// <param name="errors">Lista zawierająca błędy</param>
        /// <param name="sub">Określa czy kolekcja zawiera wyrazy, w przeciwnym wypadku są to zdania.</param>
        /// <returns>Informacja czy można przetłumaczyć kolekcję zawierającą błędy</returns>
        private static bool CanCustomize(IEnumerable<string> errors, bool sub = false)
        {
            bool result; //Rezultat

            if (!sub) //Jeżeli kolekcja nie zawiera wyrazów, tylko zdania
            {
                result = errors.Any(e => customErrors.ContainsKey(e)); //Określenie czy słownik zawiera tłumaczenie dla jakiegokolwiek zdania
            }
            else //Jeżeli kolekcja nie zawiera zdań, tylko wyrazy
            {
                result =  errors.Any(e => customErrorsSub.ContainsKey(e)); //Określenie czy słownik zawiera tłumaczenie dla jakiegokolwiek wyrazu
            }

            return result; //Zwrócenie rezultatu
        }

        /// <summary>
        /// Przetłumaczenie błędów zawartych w kolekcji
        /// </summary>
        /// <param name="errors">Kolekcja z błędami do przetłumaczenia</param>
        /// <returns>Kolekcja zawierająca przetłumaczone błędy</returns>
        public static IList<string> CustomizeErrors(IList<string> errors)
        {
            if (CanCustomize(errors)) //Jeżeli słownik zawiera tłumaczenie jakiegokolwiek błędu zawartego w kolekcji
            {
                ChangeErrors(errors); //Przetłumaczanie błędów zawartych w kolekcji
            }
            else //Słownik nie zawiera tłumaczenia dla jakiegokolwiek błędu zawartego w kolekcji
            {
                //Założenie, że błędy zwierają wiele zdań. Następuje utworzenie kolekcji pojedynczych zdań ze wszystkich błędów.
                List<string> sentences = errors.GetSentences().ToList();

                if (CanCustomize(sentences)) //Jeżeli słownik zawiera tłumaczenie jakiegokolwiek zdania zawartego w kolekcji
                {
                    errors = sentences; //Błędy do przetłumaczenia zostają ustawione jako lista zdań z błędów

                    ChangeErrors(sentences); //Przetłumaczanie zdań zawartych w kolekcji
                }
            }

            ChangeErrorsSub(errors); //Przetłumaczenie wyrazów w zdaniach

            return errors; //Zwrócenie przekazanej i zmodyfikowanej kolekcji błędów
        }

        /// <summary>
        /// Przetłumaczenie błędów zawartych w kolekcji
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="sub">Określa czy kolekcja zawiera wyrazy, w przeciwnym wypadku są to zdania.</param>
        private static void ChangeErrors(IList<string> errors, bool sub = false)
        {
            if (!sub) //Jeżeli kolekcja nie zawiera wyrazów, tylko zdania
            {
                for (int i = 0; i < errors.Count; i++) //Przejście po wszystkich zdaniach w kolekcji
                {
                    if (customErrors.ContainsKey(errors[i])) //Jeżeli słownik zawiera tłumaczenie danego zdania
                    {
                        errors[i] = customErrors[errors[i]]; //Wstawienie w miejsce danego zdania, przetłumaczonego zdania
                    }
                }
            }
            else //Jeżeli kolekcja nie zawiera zdań, tylko wyrazy
            {
                for (int i = 0; i < errors.Count; i++) //Przejście po wszystkich wyrazach w kolekcji
                {
                    if (customErrorsSub.ContainsKey(errors[i])) //Jeżeli słownik zawiera tłumaczenie danego wyrazu
                    {
                        errors[i] = customErrorsSub[errors[i]]; //Wstawienie w miejsce danego wyrazu, przetłumaczonego wyrazu
                    }
                }
            }
        }

        /// <summary>
        /// Przetłumaczenie wyrazów w zdaniach
        /// </summary>
        /// <param name="errors">Kolekcja błędów do przetłumaczenia</param>
        private static void ChangeErrorsSub(IList<string> errors)
        {
            for (int i = 0; i < errors.Count; i++) //Przejście po wszystkich zdaniach w kolekcji
            {
                errors[i] = CustomizeOneSub(errors[i]); //Wstawienie w miejsce danego zdania, przetłumaczonego zdania
            }
        }

        /// <summary>
        /// Przetłumaczenie błędu
        /// </summary>
        /// <param name="error">Błąd do przetłumaczenia</param>
        /// <returns>Przetłumaczony błąd</returns>
        private static string CustomizeOneSub(string error)
        {
            List<string> words = error.GetWords().ToList(); //Utworzenie kolekcji wyrazów dla danego zdania

            if (CanCustomize(words, true)) //Czy można przetłumaczyć jakikolwiek wyraz w zdaniu
            {               
                ChangeErrors(words, true); //Przetłumaczenie wyrazów w zdaniu

                return string.Join(" ", words); //Połączenie przetłumaczonych wyrazów z powrotem w zdanie i zwrócenie ich 
            }

            return error; //Zwrócenie nieprzetłumaczonego zdania
        }
    }
}
