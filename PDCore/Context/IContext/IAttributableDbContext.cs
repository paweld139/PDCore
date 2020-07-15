using FTCore.CoreLibrary.AttributeApi;
using FTCore.CoreLibrary.SQLLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace PDCore.Context.IContext
{
    /// <summary>
    /// Interfejs, który posiada potrzebne metody dla kontekstu bazy danych z wykorzystaniem biblioteki FTCore, czyli obiektów atrybutowalnych
    /// </summary>
    public interface IAttributableDbContext : ISqlHelper, IDbContext
    {
        /// <summary>
        /// Zapisanie danych w postaci listy do bazy danych (utworzenie lub edycja w zależności od przekazywanego id).
        /// </summary>
        /// <typeparam name="T">Typ zapisywanego obiektu. Musi dziedziczyć po Attributable, czyli musi być atrybutowalny i posiadać bezparametrowy konstruktor</typeparam>
        /// <param name="list">Lista obiektów do zapisania</param>
        void SaveChanges<T>(IEnumerable<T> list) where T : Attributable, new();

        /// <summary>
        /// Zapisanie danych w obiektu do bazy danych (utworzenie lub edycja w zależności od przekazywanego id).
        /// </summary>
        /// <typeparam name="T">Typ zapisywanego obiektu. Musi dziedziczyć po Attributable, czyli musi być atrybutowalny i posiadać bezparametrowy konstruktor</typeparam>
        /// <param name="obj">Obiekt do zapisania</param>
        void SaveChanges<T>(T obj) where T : Attributable, new();

        /// <summary>
        /// Pobranie tabeli z danymi z bazy danych na podstawie kwerendy. Metoda przesłania metodę z interfejsu ISqlHelper.
        /// </summary>
        /// <param name="query">Kwerenda, na podstawie której silnik bazodanowy zwróci odpowiednie dane</param>
        /// <returns>Tabela z danymi</returns>
        new DataTable GetDataTable(string query);

        /// <summary>
        /// Pobranie tabeli z danymi z bazy danych na podstawie typu poszukiwanego obiektu i warunków selekcji. Metoda przesłania metodę z interfejsu ISqlHelper.
        /// </summary>
        /// <typeparam name="T">Typ pobieranego obiektu. Musi dziedziczyć po Attributable, czyli musi być atrybutowalny i posiadać bezparametrowy konstruktor</typeparam>
        /// <param name="o">Instancja pobieranego typu obiektu</param>
        /// <param name="where">Warunki selekcji, które zostaną dodane do kwerendy</param>
        /// <returns>Tabela z danymi</returns>
        DataTable GetDataTable<T>(string where) where T : Attributable, new();

        /// <summary>
        /// Wywołanie kwerendy SQL
        /// </summary>
        /// <param name="query">Kwerenda do wywołania</param>
        /// <returns>Rezultat wywołania kwerendy</returns>
        new int ExecuteSQLQuery(string query);

        /// <summary>
        /// Pobranie obiektu z bazy danych na podstawie jego id
        /// </summary>
        /// <typeparam name="T">Typ pobieranego obiektu. Musi dziedziczyć po Attributable, czyli musi być atrybutowalny i posiadać bezparametrowy konstruktor</typeparam>
        /// <param name="id">Id obiektu do zwrócenia</param>
        /// <returns>Obiekt pobrany z bazy danych</returns>
        T Load<T>(int id) where T : Attributable, new();

        /// <summary>
        /// Pobranie listy obiektów z bazy danych na podstawie warunków selekcji
        /// </summary>
        /// <typeparam name="T">Typ pobierych obiektów. Musi dziedziczyć po Attributable, czyli musi być atrybutowalny i posiadać bezparametrowy konstruktor</typeparam>
        /// <param name="where">Warunki selekcji, które zostaną dodane do kwerendy</param>
        /// <returns>Lista obiektów pobranych z bazy danych</returns>
        List<T> LoadByWhere<T>(string where) where T : Attributable, new();

        List<T> LoadByQuery<T>(string query) where T : Attributable, new();

        string GetQuery<T>(string where) where T : Attributable, new();

        /// <summary>
        /// Usunięcie zadanego obiektu z bazy danych
        /// </summary>
        /// <typeparam name="T">Typ obiektu do usunięcia. Musi dziedziczyć po Attributable, czyli musi być atrybutowalny i posiadać bezparametrowy konstruktor</typeparam>
        /// <param name="obj">Obiekt do usunięcia</param>
        void Delete<T>(T obj) where T : Attributable, new();

        /// <summary>
        /// Usunięcie zadanych obiektów o zadanym typie z bazy danych
        /// </summary>
        /// <typeparam name="T">Typ obiektów do usunięcia. Muszą dziedziczyć po Attributable, czyli musi być atrybutowalny i posiadać bezparametrowy konstruktor</typeparam>
        /// <param name="list">Obiekty do usunięcia</param>
        void Delete<T>(IEnumerable<T> list) where T : Attributable, new();

        int GetCountByWhere<T>(string where) where T : Attributable, new();
    }
}
