using PDCore.Helpers.Soap.ExceptionHandling;
using System;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace PDCore.Clients.Soap
{
    /// <summary>
    /// Publiczna, generyczna, abstrakcyjna klasa, która może być dziedziczona przez inne klasy będące klientami usługi sieciowej WCF. Po tej klasie można dziedziczyć.
    /// </summary>
    /// <typeparam name="TClient">Klient usługi, klasa z definicją operacji usługi. Music dziedziczyć po ClientBase<TIClient></typeparam>
    /// <typeparam name="TIClient">Interfejs kanału dla klienta usługi. Musi być klasą</typeparam>
    /// <typeparam name="TOperationErrors">Klasa z metodami i właściwościami potrzebnymi przy obsłudze błędów. Musi implementować interfejs IOperationErrors i posiadać bezparametrowy konstruktor.</typeparam>
    public abstract class SoapWebServiceClient<TClient, TIClient, TOperationErrors>
        where TClient : ClientBase<TIClient> where TIClient : class where TOperationErrors : IOperationErrors, new()
    {
        /// <summary>
        /// Punkt końcowy usługi, który specyfikuje unikalny adres używany do komunikacji z punktem końcowym usługi. Dostępny tylko w tej klsie.
        /// </summary>
        private EndpointAddress endpointAddress;

        /// <summary>
        /// Wiązanie, które zawiera protokoły, sposób transportu i kodowanie wiadomości używane do kumunikacji między klientami a usługami. Dostępne tylko w tej klasie.
        /// </summary>
        private Binding binding;

        /// <summary>
        /// Konstruktor dostępny jedynie w tej klasie
        /// </summary>
        private SoapWebServiceClient()
        {
            //Ustawienie protokołów bezpieczeństwa. Może to być TLS 1.2 lub TLS 1.0 lub SSL 3.0. Nie wiadomo jakiego rodzaju protokół zabezpieczjący transport wykorzystuje usługa.
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072 | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
        }

        /// <summary>
        /// Konstruktor dostępny w tej klasie, jak i wklasach pochodnych, dziedziczących po niej. Zostaje dodatkowo wywołany bezparametrowy konstruktor.
        /// </summary>
        /// <param name="endpointAddressUrl">Adres usługi. Jest wymagany.</param>
        /// <param name="dNSEndpointIndentity">Tożsamość DNS usługi. Nie jest wymagana.</param>
        /// <param name="certificateEndpointIdentity">Certyfikat świadczący o toższamości usługi. Nie jest wymagany.</param>
        protected SoapWebServiceClient(string endpointAddressUrl, string dNSEndpointIndentity = null, X509Certificate2 certificateEndpointIdentity = null) : this()
        {
            //Ustawienie punktu końcowego usługi, a także wiązania.
            Initialise(endpointAddressUrl, dNSEndpointIndentity, certificateEndpointIdentity);
        }

        /// <summary>
        /// Ustawienie punktu końcowego usługi, a także wiązania.
        /// </summary>
        /// <param name="endpointAddressUrl">Adres usługi. Jest wymagany.</param>
        /// <param name="dNSEndpointIndentity">Tożsamość DNS usługi. Nie jest wymagana.</param>
        /// <param name="certificateEndpointIdentity">Certyfikat świadczący o toższamości usługi. Nie jest wymagany.</param>
        private void Initialise(string endpointAddressUrl, string dNSEndpointIndentity = null, X509Certificate2 certificateEndpointIdentity = null)
        {
            //Ustawienie punktu końcowego
            PrepareEndpoint(endpointAddressUrl, dNSEndpointIndentity, certificateEndpointIdentity);

            //Ustawienie wiązania
            SetBinding();
        }

        /// <summary>
        /// Ustawienie punktu końcowego. Nazwa DNS i certyfikat nie są wymagane, bo mozliwe jest użycie jednej z tych tożsamości, obu lub też żadnej. Posiadają wartości domyślne "null"
        /// </summary>
        /// <param name="endpointAddressUrl">Adres usługi. Jest wymagany.</param>
        /// <param name="dNSEndpointIndentity">Tożsamość DNS usługi. Nie jest wymagana.</param>
        /// <param name="certificateEndpointIdentity">Certyfikat świadczący o toższamości usługi. Nie jest wymagany.</param>
        private void PrepareEndpoint(string endpointAddressUrl, string dNSEndpointIndentity = null, X509Certificate2 certificateEndpointIdentity = null)
        {
            /*
             * Najpierw jest tworzony obiekt z pustą referencją. Obiekt znajduje się na stercie, a referencje do obiektu na stosie. Obiekt znajduje się w pamięci.
             * Sterta jest strukturą bez określonego ładu. Obiekty znajdują się w pamięci dłużej, niż w przypadku stosu.
             * Poszczególne obiekty są ustawiane na stosie kolejno jeden na drugim. W tym przypadku tworzona jest pusta referencja, czyli referencja, która nie referuje do żadnego obiektu.
             */
            EndpointIdentity endpointIdentity = null;

            //Jeśli przekazana została niepusta referencja, czyli tożsamość DNS punktu końcowego posiada wartość.
            if (dNSEndpointIndentity != null)
            {
                //Utworzenie tożsamości DNS na podstawie nazwy DNS i przypisanie jej do zmiennej.
                endpointIdentity = EndpointIdentity.CreateDnsIdentity(dNSEndpointIndentity);
            }
            /*
             * Jeśli certyfikat świadczący o tożsamości punktu końcowego został przekazany/posiada wartość. "Else if", bo 
             * to że nazwa DNS nie została przekazana, nie oznacza że certyfikat został przekazany
             */
            else if (certificateEndpointIdentity != null)
            {
                //Utworzenie tożsamości punktu końcowego z wykorzystaniem certyfikatu i przypisanie jej do zmiennej.
                endpointIdentity = EndpointIdentity.CreateX509CertificateIdentity(certificateEndpointIdentity);
            }

            //Jeśli tożsamość punktu końcowego usługi nie została ustawiona.
            if (endpointIdentity == null)
            {
                //Przypisanie do zmiennej adresu punktu końcowego na postawie przekazanego adresu Uri
                endpointAddress = new EndpointAddress(endpointAddressUrl);
            }
            /*
             *W tym przypadku tożsamość punktu końcowego usługi została ustawiona, więc wymagane będzie użycie innego konstruktora. Przekazanie do niego jako parametru wartości null mogłoby 
             * spowodować wyjątek, stąd został wymorzystany warunek.
             */
            else
            {
                //Utworzenie obiektu Uri na podstawie łańcucha znaków zawierającego Uri
                Uri endpointAddressUri = new Uri(endpointAddressUrl);

                //Przypisanie do zmiennej adresu punktu końcowego na podstawie utworzonego obiektu Uri i tożsamości punktu końcowego
                endpointAddress = new EndpointAddress(endpointAddressUri, endpointIdentity);
            }
        }

        /// <summary>
        /// Ustawienie wiązania
        /// </summary>
        private void SetBinding()
        {
            //Utworzenie wiązania za pomocą absrakcyjnej metody i przypisanie do zmiennej
            binding = PrepareBinding();

            //Ustawienie timeoutu wysyłania dla wiązania
            binding.SendTimeout = SendTimeout;
        }

        /// <summary>
        /// Abstrakcyjna, chroniona metoda służąca do utworzenie wiązania. Za jej działanie odpowiadają klasy pochodne, które będą posiadały specyficzne sposoby tworzenia wiązania.
        /// Metoda jest dostępna w tej klasie, jak i w klasach pochodnych.
        /// Metoda musi zostać nadpisana w klasie pochodnej.
        /// </summary>
        /// <returns>Utworzone wiązanie</returns>
        protected abstract Binding PrepareBinding();

        /// <summary>
        /// Wirtualna, chroniona właściwość tylko do odczytu. Przechowuje timeout wysyłania dla wiązania z usługą. Może zostać nadpisana w klasie pochodnej, ale nie musi.
        /// Domyślnie timeout wysyłania do 60 sekund. Po tym czasie przy wysyłaniu lub odbieraniu komunikatu, zostanie wyrzucony (throw) wyjątek.
        /// Właściwość jest dostępna w tej klasie, jak i w klasie pochodnej.
        /// </summary>
        protected virtual TimeSpan SendTimeout
        {
            get
            {
                return new TimeSpan(0, 0, 60);
            }
        }

        /// <summary>
        /// Publiczna metoda odpowiadająca za wysłania żądania do usługi.
        /// </summary>
        /// <typeparam name="TResponse">Typ odpowiedzi</typeparam>
        /// <param name="operation">Zhermetyzowana metoda, która przyjmuje jako parametr obiekt klienta usługi i zwraca odpowiedź.</param>
        /// <param name="handleTimeoutException">Flaga określająca czy wyjątek dotyczący timeouta ma zostać obsłużony. Domyślnie jest obsługiwany i wtedy nie trzeba przekazywać tego parametru.</param>
        /// <returns>Odpowiedź otrzymana od usługi</returns>
        public TResponse SendRequest<TResponse>(Func<TClient, TResponse> operation, bool handleTimeoutException = true)
        {
            //Utworzenie obiektu odpowiedzi z domyślną wartością
            TResponse response = default(TResponse);

            //Utworzenie klienta usługi. Żeby szybciej sie pisało i kod był czystszy, wykorzystano "var". Poza tym i tak nie wiemy jakiego typu jest klient usługi
            var client = GetClient();

            /*
             * W tym fragmencie kodu może zdarzyć się wyjątkowa, nieprzewidziana sytuacja. Użycie bloku "try" nie jest zbyt wydajne, ale 
             * lepsze to niż oglądanie przez klienta errorów lub co gorszaie oglądanie ich i frustracja.
             */ 
            try
            {
                //Wywołanie operacji z pomocą klienta i możliwe otrzymanie obiektu będącego odpowiedzią.
                response = operation(client);

                //Zamknięcie połączenia klienta z usługą
                client.Close();
            }
            //Ten blok zostanie wywołany w przypadku, gdy pojawi się jakikolwiek wyjątek.
            catch (Exception ex)
            {
                /*
                 * Natychmiastowo zostanie zamknięte połączenie klienta z usługą. W przypadku metody "Close" zostaje wysłane powiadomienie do usługi, przez 
                 * co mógłby pojawić się wyjątek związany z komunikacją. W przypadku "Abort" połącznie jest gwałtownie, natychmiastowo zamykane, bez możliwości odczytania odpowiedzi.
                 */
                client.Abort();

                /*
                 * Jeżeli wyjątek nie jest związany z timeoutem lub należy obsłużyć timeout, to zostaje obsłużony wyjątek. W przeciwnym wypadku wyjątek zostaje wyrzucony ponownie i
                 * może zostać obsłużony przez motodę wywołującą (jeżeli jest timeout i nie ma być obsługiwany tutaj). 
                 * Jest używane "throw", by nie zaburzyć CallStacka (nie jest tworzony nowy obiekt wyjątku).
                 */
                if (!(ex is TimeoutException) || handleTimeoutException)
                    HandleError(ex);
                else
                    throw;
            }

            //Zwrócenie otrzymanej odpowiedzi tylko w przypadku, jeżeli nie wystąpił żaden wyjątek.
            return response;
        }

        /// <summary>
        /// Utworznie obiektu klienta usługi
        /// </summary>
        /// <returns></returns>
        private TClient GetClient()
        {
            /*
             * Utworznie instancji klienta usługi. Typ jest określony przez podanie typu dla tej klasy, która jest klasą generyczną, przez klasę potomną.
             * Nie jest znany konstruktur tego typu, więc zostaje wykorzystana klasa "Activator", której metoda przyjmuje tablicę z parametrami, a także type obiektu.
             * Na końcu następuje jawne (explicit) rzutowanie na typ klienta usługi.
             */
            TClient client = (TClient)Activator.CreateInstance(typeof(TClient), new object[] { binding, endpointAddress });

            //Przygotowanie klienta do wykorzystania
            PrepareClient(client);

            //Zwrócenie utworzonego klienta
            return client;
        }

        /// <summary>
        /// Chroniona, abstrakcyjna metoda, która przetwarza/przygotowuje/modyfikuje obiekt klienta usługi celem jego późniejszego wykorzystania.
        /// Metoda jest dostępna w tej klasie jak i w klasach pochodnych. Musi zostać nadpisana w klasie pochodnej. Tworzenie obiektu klienta jest często specyficzne dla
        /// różnych usług sieciowych / różnych typów usług sieciowych
        /// </summary>
        /// <param name="client">Obiekt klienta usługi</param>
        protected abstract void PrepareClient(TClient client);

        /// <summary>
        /// Chroniona, wirtualna metoda służąca wyświetleniu błędu. Może być nadpisana przez klasę pochodną, ale nie musi. Dostępne w tej klasie, jak i w klasie pochodnej. Nic nie zwraca.
        /// </summary>
        /// <param name="error">Łańcych znaków zawierający błąd</param>
        protected virtual void ShowError(string error)
        {
            //Wyświetlenie błędu (jako nowej linii) w oknie "Output", terminalu wyświetlanym podczas debugowania aplikacji.
            Trace.WriteLine(error);
        }

        /// <summary>
        /// Chroniona, wrtualna metoda służąca wyświetleniu użytkownikowi pytania, pobraniu odpowiedzi, a następnie zwróceniu jej. Jest to odpowiedź typu tak/nie (obiekt typu bool).
        /// Metoda może być nadpisana przez klasę pochodną, ale nie musi. Metoda jest dostępne w tej klasie i w klasie pochodnej.
        /// </summary>
        /// <param name="content">Łańcuch znaków zawierający pytanie kierowane do użytkownika systemu</param>
        /// <returns>Odpowiedź "tak" lub "nie"</returns>
        protected virtual bool ShowQuestion(string content)
        {
            //Wyświetlenie w konsoli łańcucha znaków zawierającego pytanie jako nową linię
            Console.WriteLine(content);

            Console.WriteLine("Naciśnij \"t\", jeśli się zgadzasz.");

            //Pobranie przycisku naciśniętego przez użytkownika (następuje oczekiwanie aż użytkownik wciśnie jakikolwiek przycisk)
            ConsoleKeyInfo key = Console.ReadKey();

            //Jeżeli użytkownik wcisnął na klawiaturze klawisz "T", czyli odpowiedź "tak"
            if(key.Key == ConsoleKey.T)
            {
                //Odpowiedź twierdząca
                return true;
            }

            //Odpowiedź przecząca
            return false;
        }

        /// <summary>
        /// Obsługa błędu, który może się pojawić przy wysyłaniu komunikatu SOAP do usługi
        /// </summary>
        /// <param name="ex">Wyjątek do obsłużenia</param>
        private void HandleError(Exception ex)
        {
            //Utworzenie instancji klasy służącej do obsługi wyjątków
            TOperationErrors errors = new TOperationErrors();

            //Obsłużenie wyjątku za pomocą wyżej wymienionej klasy.
            errors.HandleException(ex);

            //Ustawienie tekstu błędu/komunikat, który został wygenerowany z wykorzystaniem wyżej wymienionej klasy
            string error = errors.ToString();

            //Pokazanie błędu
            ShowError(error);
        }
    }
}
