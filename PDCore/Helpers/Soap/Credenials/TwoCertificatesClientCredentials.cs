using System;
using System.Configuration;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Security.Tokens;

namespace PDCore.Helpers.Soap.Credentials
{
    /// 
    /// Class that extends Client Credentials so that the certificate for the
    /// Transport layer encryption can be separate
    /// 
    /// <summary>
    /// Klasa, która rozszerza klasę odpowiadającą za zbieranie kwalifikacji klienta celem jego uwierzytelniania, tak by występował osobny certyfikat służący zabezpieczeniu warstwy transportu.
    /// </summary>
    public class TwoCertificatesClientCredentials : ClientCredentials
    {
        /// &lt;summary&gt;
        /// The X509 Certificate that is to be used for https
        /// &lt;/summary&gt;
        /// <summary>
        /// X509, certyfikat TLS, który służy zabezpieczeniu warstwy transportu wiadomości do usługi
        /// </summary>
        public X509Certificate2 TransportCertificate { get; set; }

        /// <summary>
        /// Konstruktor służący do stworzenia instancji klasy na podstawie istniejącego obiektu ClientCredentials. Obiekt zostaje przekazany do bazowego konstruktora.
        /// Służy to temu, by utworzyć nowy obiekt i nie edytować istniejącego, bo nowy będzie rozszerzeniem obecnego.
        /// </summary>
        /// <param name="existingCredentials">Istniejący obiekt ClientCredentials, zawierający informacje o kliencie usługi</param>
        public TwoCertificatesClientCredentials(ClientCredentials existingCredentials)
          : base(existingCredentials)
        {
        }

        /// <summary>
        /// Chroniony konstruktor, który tworzy nowy obiekt tej klasy na podstawie instancji tej klasy przekazanej jako parametr do tego konstruktora. Zostaje także wywołany bazowy konstruktor
        /// celem utworzenia nowego obiektu ClientCredentials na podstawie przekazanego, skopiowania tego obiektu.
        /// </summary>
        /// <param name="other">Istniejący obiekt tej klasy, na podstawie którego podstanie nowy obiekt</param>
        protected TwoCertificatesClientCredentials(TwoCertificatesClientCredentials other) : base(other)
        {
            //Przypisanie certyfikatu TLS do nowej klasy, z tej istniejącej, przekzanej do konstruktora
            TransportCertificate = other.TransportCertificate;
        }

        /// <summary>
        /// Nadpisana, chroniona metoda z klasy bazowej do klonowania instancji tej klasy. Może być wywołana w tej klasie, jak i w klasach pochodnych.
        /// </summary>
        /// <returns>Sklonowany obiekt</returns>
        protected override ClientCredentials CloneCore()
        {
            return new TwoCertificatesClientCredentials(this);
        }

        /// <summary>
        /// Publiczna, nadpisana metoda z klasy bazowej odpowiedzialna za utworznie menedżera tokenów bezpieczeństwa na podstawie instancji tej klasy.
        /// </summary>
        /// <returns>Menedżer tokenów bezpieczeństwa</returns>
        public override SecurityTokenManager CreateSecurityTokenManager()
        {
            return new MyCredentialsSecurityTokenManager(this);
        }

        /// <summary>
        /// Przypisanie do zmiennej certyfikatu TLS na podstawie obiektów zawierających: lokalizację magazynu certyfikatów, 
        /// nazwę magazynu do otworzenia i informacje o certyfikcie na podstawie której ma być on znaleziony.
        /// Certyfikat jest wyszukiwany na podstawie podmiotu, nazwy wyróżniającej certyfikatu.
        /// </summary>
        /// <param name="subjectName">Podmiot, nazwa wyróżniająca certyfikatu</param>
        /// <param name="storeLocation">Lokalizacja magazynu certyfikatów</param>
        /// <param name="storeName">Nazwa magazynu certyfikatów do otworzenia</param>
        public void SetTransportCertificate(string subjectName, StoreLocation storeLocation, StoreName storeName)
        {
            //Wywołanie przeciążonej (overloaded) metody celem znalezienia i przypisania certyfikatu do zmiennej na podstawie zadanych informacji
            SetTransportCertificate(storeLocation, storeName, X509FindType.FindBySubjectDistinguishedName, subjectName);
        }

        /// <summary>
        /// Przypisanie do zmiennej, certyfikatu TLS na podstawie obiektów zawierających: lokalizację magazynu certyfikatów, nazwę magazynu do otworzenia, 
        /// sposób wyszukiwania certyfikatu i informacje o certyfikacie na podstawie której ma być on znaleziony.
        /// </summary>
        /// <param name="storeLocation">Lokalizacja magazynu certyfikatów</param>
        /// <param name="storeName">Nazwa magazynu certyfikatów do otworzenia</param>
        /// <param name="x509FindType">Informacja o certyfikacie na podstawie której ma być on znaleziony, np. numer seryjny. Jest to sposób wyszukiwania certyfikatu.</param>
        /// <param name="subjectName">Wartość na podstawie której ma być znaleziony</param>
        public void SetTransportCertificate(StoreLocation storeLocation, StoreName storeName, X509FindType x509FindType, string subjectName)
        {
            TransportCertificate = FindCertificate(storeLocation, storeName, x509FindType, subjectName);
        }

        /// <summary>
        /// Prywatna (dostępna tylko z poziomu tej klasy), statyczna (bo nie wykorzystuje właściwości, pól z instancji, niezależna od instancji) metoda, 
        /// która służy do wyszikiwania certyfikatów na podstawie zadanych kryteriów.
        /// </summary>
        /// <param name="location">Lokalizacja magazynu certyfikatów</param>
        /// <param name="name">Nazwa magazynu certyfikatów do otworzenia</param>
        /// <param name="findType">Informacja o certyfikacie na podstawie której ma być on znaleziony, np. numer seryjny. Jest to sposób wyszukiwania certyfikatu.</param>
        /// <param name="findValue">Wartość na podstawie której ma być znaleziony</param>
        /// <returns>Znaleziony certyfikat</returns>
        private static X509Certificate2 FindCertificate(StoreLocation location, StoreName name, X509FindType findType, string findValue)
        {
            //Utworzenie obiektu będacego magazynem certyfikatów X509 na podstawie nazwy magazynu i jego lokalizacji
            X509Store store = new X509Store(name, location);

            //Blok "try" może także występować bez bloku "catch"
            try
            {
                //Otworzenie magazynu certyfikatów tylko do odczytu
                store.Open(OpenFlags.ReadOnly);

                //Zwrócenie kolekcji certyfikatów X.509 na podstawie sposobu wyszukiwaniu, wyszukiwanej wartości i jedynie te certyfikatu, które są poprawne
                X509Certificate2Collection col = store.Certificates.Find(findType, findValue, true);

                //Zwrócenie pierwszego znalezionego ceryfikatu
                return col[0]; // return first certificate found
            }
            finally
            {
                //Na końcu, bez względu na to czy wystąpił wyjątek czy nie, magazyn certyfikatów zostaje zamknięty
                store.Close();
            }
        }

    }

    /// <summary>
    /// Klasa dostępna w bieżącym pakiecie/projekcie, która jest menedżerem tokenów bezpieczeństwa
    /// </summary>
    internal class MyCredentialsSecurityTokenManager : ClientCredentialsSecurityTokenManager
    {
        /// <summary>
        /// Pole zawierające obiekt klasy odpowiadającej za utworzenie listy uwierzytelniającej klienta z osobnyn certyfikatem TLS i WSS
        /// </summary>
        readonly TwoCertificatesClientCredentials credentials;

        /// <summary>
        /// Konstruktor służący do utworzenia instancji tej klasy na podstawie obiektu klasy odpowiadającej za utworzenie listy uwierzytelniającej klienta z osobnyn certyfikatem TLS i WSS.
        /// Zostaje wywołany konstruktor bzowy celem utworzeni menedżera tokenów bezpieczeństwa na podstawie obiektu zawierającego kwalifikacje klienta usługi
        /// </summary>
        /// <param name="credentials">Obiekt klasy odpowiadającej za utworzenie listy uwierzytelniającej klienta z osobnyn certyfikatem TLS i WSS</param>
        public MyCredentialsSecurityTokenManager(TwoCertificatesClientCredentials credentials)
            : base(credentials)
        {
            //Przypisanie do zmiennej obiektu przekazanego poprzez konstruktor
            this.credentials = credentials;
        }

        /// <summary>
        /// Publiczna, nadpisana metoda z klasy bazowej służąca do utworzenia dostawcy tokenu bezpieczeństwa na podstawie przekazanych wymagań dotyczących tokenu
        /// </summary>
        /// <param name="requirement">Wymagania dotyczące tokenu bezpieczeństwa</param>
        /// <returns>Dostawca tokenu bezpieczeństwa</returns>
        public override SecurityTokenProvider CreateSecurityTokenProvider(SecurityTokenRequirement requirement)
        {
            SecurityTokenProvider result;

            //Jeżeli wymagany jest token służący do zabezpieczenia warstwy transportu i jest to token powstały z certyfikatu X509
            if (requirement.Properties.ContainsKey(ServiceModelSecurityTokenRequirement.TransportSchemeProperty) && requirement.TokenType == SecurityTokenTypes.X509Certificate)
            {
                //Utworzenie dostawcy tokena na podstawie certyfikatu TLS
                result = new X509SecurityTokenProvider(
                    this.credentials.TransportCertificate);
            }
            //W przeciwnym razie jeżeli klucz przypisany do tokekena ma być użyty w celu wygenerowania cyfrowego podpisu i jest to token powstały z certyfikatu X509
            else if (requirement.KeyUsage == SecurityKeyUsage.Signature && requirement.TokenType == SecurityTokenTypes.X509Certificate)
            {
                //Utworzenia dostawcy tokena na podstawie ceryfikatu WSS (Secure Websocket, certificate signing)
                result = new X509SecurityTokenProvider(
                    this.credentials.ClientCertificate.Certificate);
            }
            //W przeciwnym wypadku utworzony zostanie dostawca tokenu bezpieczeństwa z wykorzystaniem metody z klasy bazowej
            else
            {
                result = base.CreateSecurityTokenProvider(requirement);
            }

            //Zwrócenie dostawcy tokenu bezpieczeństwa
            return result;
        }
    }

    /// <summary>
    /// Element konfiguracyjny (XML) będący klasą pochodną elementu listy uwierzytelniającej klienta
    /// </summary>
    class ClientCredentialsExtensionElement : ClientCredentialsElement
    {
        /// <summary>
        /// Kolekcja właściwości konfiguracyjnych
        /// </summary>
        ConfigurationPropertyCollection properties;

        /// <summary>
        /// Publiczna, nadpisana właściwość określająca typ zachowania tego elementu
        /// </summary>
        public override Type BehaviorType
        {
            get
            {
                return typeof(TwoCertificatesClientCredentials); //Typ klasy odpowiadającej za utworzenie listy uwierzytelniającej klienta z osobnyn certyfikatem TLS i WSS
            }
        }

        /// <summary>
        /// Właściwość konfiguracyjna, która przechowuje certyfikat TLS, służący do identyfikacji klienta korzystającego z usługi sieciowej
        /// </summary>
        [ConfigurationProperty("transportCertificate")]
        public X509InitiatorCertificateClientElement TransportCertificate
        {
            get
            {
                return base["transportCertificate"] as X509InitiatorCertificateClientElement; //Certyfikat pobierany jest z klasy bazowej
            }
        }

        /// <summary>
        /// Chroniona, nadpisana metoda zawierająca kolekcję właściwości konfiguracyjnych tego elementu konfiguracyjnego
        /// </summary>
        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                //Ta właściwość jest tworzona tylko raz w zależności od potrzeby, tzw. lazy initialisation
                if (this.properties == null)
                {
                    //Przypisanie do zmiennej właściwości bazowego elementu
                    ConfigurationPropertyCollection properties = base.Properties;

                    //Dodanie do właściwości, nowej właściwości zawierającej certyfikat TLS
                    properties.Add(new ConfigurationProperty(
                        "transportCertificate",
                        typeof(X509InitiatorCertificateClientElement),
                        null, null, null,
                        ConfigurationPropertyOptions.None));

                    this.properties = properties;
                }

                return this.properties;
            }
        }

        /// <summary>
        /// Chroniona, nadpisana metoda służąca do utworzenia zachowania tego elementu konfiguracyjnego
        /// </summary>
        /// <returns>Utworzone zachowanie tego elementu konfiguracyjnego</returns>
        protected override object CreateBehavior()
        {
            //Jest tworzony obiekt listy uwierzytelniającej klienta z osobnym certyfikatem TLS na podstawie zachowania utworzonego z klasy bazowej będącego bazową listą uwierzytelniającą
            TwoCertificatesClientCredentials creds = new TwoCertificatesClientCredentials(base.CreateBehavior() as ClientCredentials);

            //Przypisanie do zmiennej informacji o właściwościach tego elemetu
            _ = ElementInformation.Properties;

            //Dla utworzonego obiektu listy uwierzytelniającej klienta z osobnym certyfikatem TLS zostaje ustawiony certyfikat na podstawie danych certyfikatu z właściwości tego elementu
            creds.SetTransportCertificate(TransportCertificate.StoreLocation,
                                            TransportCertificate.StoreName,
                                            TransportCertificate.X509FindType,
                                            TransportCertificate.FindValue);

            //Dodanie utworzonego elementu konfiguracyjnego do tego elementu konfiguracyjnego
            base.ApplyConfiguration(creds);

            //Zwrócenie utworzonego elementu konfigurcyjnego/zachowania tego elementu
            return creds;
        }
    }
}
