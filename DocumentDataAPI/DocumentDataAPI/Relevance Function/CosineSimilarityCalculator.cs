﻿namespace DocumentDataAPI.Relevance_Function;

public static class CosineSimilarityCalculator
{
    
    public static double CalculateCosineSimilarity(Dictionary<string, int> document, IEnumerable<string> query)
    {
        double dotProduct = 0;
        double documentVectorLengthHelper = 0;
        
        foreach (KeyValuePair<string, int> wordcount in document)
        {
            // Since the query can be interpreted as a vector containing 1 and 0,
            // with a 1 for words in the query and 0 otherwise, the dot product can be
            // calculated by summing the amount of the words that occur in both the query and the document
            if (query.Contains(wordcount.Key))
            {
                dotProduct += wordcount.Value;
            }
            //Squaring and summing the values, so that the length of the document vector can be found later
            documentVectorLengthHelper += Math.Pow(wordcount.Value, 2);
        }

        double documentVectorLength = Math.Sqrt(documentVectorLengthHelper);
        
        //The length of the query vector can be found simply be using count(),
        //since the query can be interpreted as a vector with entries 0 and 1.
        //Only the entries with 1 are represented in the list and therefore count() suffices 
        double queryVectorLength = Math.Sqrt(query.Distinct().Count());
        
        return dotProduct/(documentVectorLength * queryVectorLength);
    }
}
