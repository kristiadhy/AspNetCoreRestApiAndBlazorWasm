using AutoMapper;

namespace Services;
public class DataProcessingExtension
{
    public static List<T1> Create_ListOfModel_FromListOfDTO<T1, T2>(List<T2> lstDto, IMapper mapper)
    {
        List<T1> lstDataModel = new();
        foreach (var row in lstDto)
        {
            var dataModel = mapper.Map<T1>(row);
            lstDataModel.Add(dataModel);
        }
        return lstDataModel;
    }
}
