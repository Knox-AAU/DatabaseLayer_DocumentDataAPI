namespace DocumentDataAPI.Models;

public class SearchResponseModel
{
    public DocumentModel DocumentModel { get; }
    public double Relevance { get; }

    public SearchResponseModel(DocumentModel documentModel, double relevance)
    {
        DocumentModel = documentModel;
        Relevance = relevance;
    }
}
