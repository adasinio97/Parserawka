using System.Collections.Generic;

namespace ParserawkaCore.Interfaces
{
    interface IQueryProcessor
    {
        // Przetwarzanie zapytań będzie wymagało utworzenia modelu tworzącego drzewa dla danych zapytań.
        // Tymczasowo rolę drzewa dla danego zapytania pełni lista obiektów typu string, przetworzona w metodzie ProcessQuery(string query).

        List<string> ProcessQuery(string query);
        string EvaluateQuery(List<string> processedQuery);
        string EvaluteQuery(IProgramKnowledgeBase programKnowledgeBase);
        void ProjectQuery(string queryResults);
    }
}
