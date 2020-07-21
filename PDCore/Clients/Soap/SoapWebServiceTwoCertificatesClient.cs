using PDCore.Helpers.Soap.Credentials;
using PDCore.Helpers.Soap.ExceptionHandling;
using PDCore.Extensions;
using PDCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Text;

namespace PDCore.Clients.Soap
{
    /// <summary>
    /// Publiczna klasa dziedzicząca po abstrakcyjnej klasie "SoapWebServiceClient" będąca klientem specyficznego typu usługi sieciowej - wykorzystującej
    /// osobny certyfikat do podpisu wiadomości i do jej transportu
    /// </summary>
    /// <typeparam name="TClient">Typ klienta usługi, klasa z definicją operacji usługi. Music dziedziczyć po ClientBase<TIClient></typeparam>
    /// <typeparam name="TIClient">>Interfejs kanału dla klienta usługi. Musi być klasą.</typeparam>
    /// <typeparam name="TOperationErrors">Klasa z metodami i właściwościami potrzebnymi przy obsłudze błędów. Musi implementować interfejs IOperationErrors i posiadać bezparametrowy konstruktor.</typeparam>
    public class SoapWebServiceTwoCertificatesClient<TClient, TIClient, TOperationErrors> : SoapWebServiceClient<TClient, TIClient, TOperationErrors>
        where TClient : ClientBase<TIClient> where TIClient : class where TOperationErrors : IOperationErrors, new()
    {
        //Certyfikat TLS służący zabezpieczeniu transportu wiadomości
        private readonly X509Certificate2 transportCertificate;

        //Certyfikat WSS służący podpisaniu wiadomości
        private readonly X509Certificate2 signCertificate;

        //Certyfikat świadczący o tożsamości usługi sieciowej
        private readonly X509Certificate2 serviceCertificate;

        /// <summary>
        /// Publiczny konstruktor dostępny z poziomu tej klasy, jak i każdej innej. Dodatkowo zostaje wywołany konstruktor z klasy bazowej 
        /// (zostają do niego przekazane: adres usługi, adres DNS będący tożsamością usługi i certyfikat świadczący o tożsamości punktu końcowego.
        /// Niewymagane parametry muszą być na końcu, żeby było wiadomo który parametr jest pomijany, a który nie, nie używając nazw parametrów z dwukropkiem.
        /// </summary>
        /// <param name="endpointAddressUrl">Adres usługi. Jest wymagany.</param>
        /// <param name="transportCertificate">Certyfikat TLS służący zabezpieczeniu transportu wiadomości</param>
        /// <param name="signCertificate">Certyfikat WSS służący podpisaniu wiadomości</param>
        /// <param name="serviceCertificate">Certyfikat świadczący o tożsamości usługi sieciowej</param>
        /// <param name="dNSEndpointIndentity">Tożsamość DNS usługi. Nie jest wymagana.</param>
        /// <param name="certificateEndpointIdentity">Certyfikat świadczący o toższamości usługi. Nie jest wymagany.</param>
        public SoapWebServiceTwoCertificatesClient
            (string endpointAddressUrl, X509Certificate2 transportCertificate, X509Certificate2 signCertificate, X509Certificate2 serviceCertificate,  
            string dNSEndpointIndentity = null, X509Certificate2 certificateEndpointIdentity = null) : 
            base(endpointAddressUrl, dNSEndpointIndentity, certificateEndpointIdentity)
        {
            //Ustawienie zmiennej zawierającej certyfikat do transportu
            this.transportCertificate = transportCertificate;

            //Ustawienie certyfikatu służącego do podpisywania wysyłanej wiadomości
            this.signCertificate = signCertificate;

            //Ustawienie certyfikatu usługi, świadczącego o jej tożsamości.
            this.serviceCertificate = serviceCertificate;
        }

        /// <summary>
        /// Chroniona, nadpisana metoda abstrakcyjna z klasy bazowej służąca do przygotowania obiektu klienta usługi do wykorzystania. Może być ponownie nadpisana w klasie pochodnej.
        /// Jest dostępna z poziomu tej klasy, jak i klas pochodnych. Jest nadpisana nawet, jeśli obiekt tej klasy zostanie przypisany do zmiennej typu klasy bazowej
        /// (w przeciwieństwie do przesłaniania, nawet IDE wskaże wtedy potrzebę użycia słowa kluczowego "new")
        /// </summary>
        /// <param name="client">Obiekt klienta usługi</param>
        protected override void PrepareClient(TClient client)
        {
            //Jest tworzony obiekt listy uwierzytelniającej klienta z osobnym certyfikatem TLS na podstawie istniejącej listy uwierzytelniającej klienta
            TwoCertificatesClientCredentials myCredentials = new TwoCertificatesClientCredentials(client.ClientCredentials)
            {

                //Przypisanie certyfikatu TLS do listy uwierzytelniającej klienta
                TransportCertificate = transportCertificate
            };

            myCredentials.ClientCertificate.Certificate = signCertificate; //Przypisanie certyfikatu WSS do listy uwierzytelniającej klienta

            myCredentials.ServiceCertificate.DefaultCertificate = serviceCertificate; //Przypisanie certyfikatu świadczącego o tożsamości usługi sieciowej do listy uwierzytelniającej klienta

            myCredentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None; //Wyłączanie uwierzytelniania usługi z wykorzystaniem certyfikatu

            //Zastąpienie istniejącej listy uwierzytelniającej klienta, nowo utworzoną listą uwierzytelniającą zawierającą dodatkowo certyfikat TLS
            client.Endpoint.Behaviors.Remove(client.Endpoint.Behaviors.FirstOrDefault(x => x.GetType() == typeof(ClientCredentials)));

            //Usunięcie zbędnej, niepotrzebnej zawartości z wysyłanego komunikatu celem zmniejszenia jego wagi i szybszej komunikacji
            var vs = client.Endpoint.Behaviors.FirstOrDefault((i) => i.GetType().Namespace == "Microsoft.VisualStudio.Diagnostics.ServiceModelSink");

            //Usunięcie znalezionego zachowania punktu końcowego usługi sieciowej z którą łączy się klient
            if (vs != null)
            {
                client.Endpoint.Behaviors.Remove(vs);
            }

            //Dodanie do zachowań punktu końcowego usługi, utworzonej listy uwierzytelniającej zawierającej dodatkowo certyfikat TLS
            client.Endpoint.Behaviors.Add(myCredentials);

            //Zabezpieczeniem umowy z punktem końcowym usługi będzie podpisywanie wiadomości, by umożliwić integralność przekzywanych danych
            client.Endpoint.Contract.ProtectionLevel = ProtectionLevel.Sign;
        }

        /// <summary>
        /// Chroniona, nadpisana metoda, służąca do utworzenia wiązania
        /// </summary>
        /// <returns></returns>
        protected override Binding PrepareBinding()
        {
            //Utworzenie instancji obiektu, będącego niestandardowym wiązaniem z usługą
            CustomBinding binding = new CustomBinding();

            /*
             * Utworzenie elementu wiązania zapewniającego asymetryczne, obustronne bezpieczeństwo, czyli możliwe jest uwierzytelnienie klienta, jak i serwera za pomocą certyfikatów
             * Wspiera bezpieczeństwo kanału z wykorzystaniem asymetrycznego szyfrowania. Element wiązania podpisuje wiadomość z wykorzystaniem tokenu wierzytelniania wysyłającego i
             * szyfruje ją z wykorzystaniem tokenu odbiorcy komunikatu. Zostaje przekazana wersja zabezpieczenia wiadomości. SOAP to protokół komunikacyjny.
             * W tym przypadku wiadomość nie będzie szyfrowana.
             */
            var sec = (AsymmetricSecurityBindingElement)SecurityBindingElement.
                CreateMutualCertificateBindingElement(MessageSecurityVersion.WSSecurity10WSTrust13WSSecureConversation13WSSecurityPolicy12BasicSecurityProfile10);

            //Ustawienie wersji zabezpieczenie wiadomości dla utworzonego elementu wiązania
            sec.MessageSecurityVersion = MessageSecurityVersion.WSSecurity10WSTrust13WSSecureConversation13WSSecurityPolicy12BasicSecurityProfile10;

            sec.IncludeTimestamp = false; //Stemple czasowe nie będa występowały w wiadomościach

            /* 
             * Ustawienie kolejności ochrony wiadomości (podpisywanie i szyfrowanie). W tym przypadku wiadomość
             * będzie najpierw podpisywana, a później szyfrowana. Jako że ustawiono wcześniej, że
             * zabezpieczeniem umowy z punktem końcowym usługi będzie jedynie podpisywanie wiadomości,
             * to jaka opcja będzie tutaj ustawiona nie ma większego znaczenia, bo
             * wiadomość nie będzie szyfrowana.
             */
            sec.MessageProtectionOrder = MessageProtectionOrder.SignBeforeEncrypt; 

            sec.AllowSerializedSigningTokenOnReply = true; //Serializowanie tokena podpisującego w komunikacie odpowiedzi jest dozwolone.

            sec.LocalClientSettings.DetectReplays = false; //Wykrywanie ponownego wysłania wiadomości jest wyłączone w wiadomościach otrzymywanych przez klienta z usługi.

            //Dodawanie utworzonego elementu wiązania do utworzonego niestandardowego wiązania
            binding.Elements.Add(sec);


            //Utworzenie elementu wiązania zawierającego informcje o kodowanie wiadomości tekstowej. Wiadomość będzie w wersji protokołu SOAP 1.1 i będzie kodowania w formacie UTF-8
            var tme = new TextMessageEncodingBindingElement(MessageVersion.Soap11, Encoding.UTF8);

            //Dodawanie utworzonego elementu wiązania do utworzonego niestandardowego wiązania
            binding.Elements.Add(tme);


            //Utworznie elementu wiązania zawierającego informcje o transporcie wiadomości poprzez protokuł HTTPS (Hypertext Transfer Protocol Secure)
            var https = new HttpsTransportBindingElement
            {

                //Wymagane jest uwierzytelnianie SSL klienta z wykorzystaniem certyfikatu
                RequireClientCertificate = true,

                MaxReceivedMessageSize = 1000000 //Maksymalna wielkość otrzymywanej wiadomości w bajtach wynosi 1000000, czyli ok. 1MB
            };

            //Dodawanie utworzonego elementu wiązania do utworzonego niestandardowego wiązania
            binding.Elements.Add(https);

            //Zwrócenie utworzonego niestandardowego wiązania
            return binding;
        }
    }
}
