using System;
using System.Collections.Generic;

namespace Boiler.DTOs;

public static class UserId
{
    private static int id = 0;

    public static void SetId(int _id){
        id = _id;
    }

    public static int GetId(){
        return id;
    }
}
