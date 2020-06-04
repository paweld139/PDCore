using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Attributes
{
    /// <summary>
    /// Atrybut do zastosowania na właściwościach w celu określenia porządku tych właściwości. Nie może być dziedziczony przez klasy pochodne 
    /// (modyfikator dostępu - access modifier - "sealed" i "Inherited = false"), może być tylko jeden.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class OrderAttribute : Attribute
    {
        /// <summary>
        /// Konstruktor z domyślną wartością "order"
        /// </summary>
        /// <param name="order">Liczba określająca miejsce właściwości, jej kolejność.</param>
        public OrderAttribute(int order = 0)
        {
            Order = order;
        }
        
        /// <summary>
        /// Można ustawić tylko poprzez konstruktor, ale można pobrać z każdego miejsca
        /// </summary>
        public int Order { get; private set; }
    }
}
