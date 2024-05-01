using AutoMapper;
using System.Reflection;

namespace Services;

public static class EntityExtensions<T1, T2, T3>
    where T1 : new()
{

    public static List<T1> AddEntityListfromDTO(List<T2> lstDto, IMapper mapper)
    {
        List<T1> lstMD = new();
        foreach (var entDto in lstDto.ToList())
        {
            var entMD = mapper.Map<T1>(entDto);
            lstMD.Add(entMD);
        }
        return lstMD;
    }

    public static List<T1> AddEntityListfromQP(T3 QP)
    {
        List<T1> lstEntity = new();

        PropertyInfo[] properties = typeof(T3).GetProperties();
        foreach (PropertyInfo property in properties)
        {
            T1 entity = new T1();

            var propName = property.Name;
            var propValue = property.GetValue(QP);

            property.SetValue(entity, propValue);

            if (propValue != null)
            {
                lstEntity.Add(entity);
            }
        }

        return lstEntity;
    }
}
