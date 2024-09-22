using Common.Enums.EnumUniqueValueSettings;

namespace Common.Enums;
//[UniqueEnumValues]
public enum EnumPermission
{
    /// <summary>
    /// After adding permission enum, don't forget to add a migration to the database!!!!!!!!
    /// </summary>

    //SP
    CreateOrUpdateSp = 1,
    DeleteSp = 2,

    //User 
    GetUsers = 3,
    BlockOrUnlockUser = 4,
    DeleteUser = 5,
    CreateOrUpdateUser = 6,

    //Role 
    CreateOrUpdateRole = 7,
    DeleteRole = 8,
    GetRole = 9,


  

}
