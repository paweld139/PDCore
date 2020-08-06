using PDCore.Extensions;
using PDCore.Helpers.Comparers;
using PDCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Helpers.DataStructures
{
    /// <summary>
    /// Kategorie są posortowane i unikalne. Obiekty nazwane mają unikalne nazwy i są po nich posortowane.
    /// </summary>
    public class CategoryCollection : SortedDictionary<string, SortedSet<NamedObject>>
    {
        public CategoryCollection Add(string categoryName, NamedObject namedObject)
        {
            if (!ContainsKey(categoryName))
            {
                Add(categoryName, new SortedSet<NamedObject>(new NamedObjectComparer())); //Dzięki kontrawariancji (in) można wrzucać bardziej bazowe konwertery
            }

            this[categoryName].Add(namedObject);

            return this;
        }

        public CategoryCollection AddRange(string categoryName, IEnumerable<NamedObject> namedObject)
        {
            namedObject.ThrowIfNull(nameof(namedObject));

            NamedObject firstNamedObject = namedObject.FirstOrDefault();

            if (firstNamedObject == null) //W sekwencji nie ma żadnych elementów
                throw new ArgumentException("Nie podano nazwanych obiektów do dodania", nameof(namedObject));

            Add(categoryName, firstNamedObject); //Istnienie klucza będzie sprawdzane tylko raz

            namedObject.Skip(1).ForEach(x => this[categoryName].Add(x));

            return this;
        }

        public CategoryCollection AddRange(string categoryName, params NamedObject[] namedObject) //Dodawanie wielu obiektów do kategorii
        {
            return AddRange(categoryName, namedObject.AsEnumerable());
        }
    }
}
